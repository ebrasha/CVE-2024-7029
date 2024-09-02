# PoC tool for exploiting CVE-2024-7029 in AvTech devices

## 🎤 README Translation
- [English](README.md)
- [فارسی](README.fa.md)

## 📸 Screenshot

<p align="center"><img src="scr.png?raw=true"></p>

 
## 💎 Introduction
CVE-2024-7029 highlights a severe security issue in AVTech devices, where attackers can bypass authentication and execute arbitrary commands remotely. This vulnerability poses significant risks to the integrity and security of affected networks. 

## 💀 Vulnerability Overview
CVE-2024-7029 is a critical security flaw in AVTech devices that allows unauthorized access through authentication bypass, potentially leading to remote code execution. This vulnerability is particularly concerning because it can be exploited remotely, making networks with these devices highly vulnerable.
 

## 🛠️ Development Environment Setup
- **.NET 8**
 

## 🔥 Requirements
There are no specific prerequisites needed to run this PoC.

## 📥 Download
To download the executable versions of this PoC, please visit the official Releases page on GitHub. This will allow you to obtain the compiled version ready for use:

- [Download Executable PoC Versions from GitHub](https://github.com/ebrasha/CVE-2024-7029/releases/latest)

 

## 📦 Setup and Usage
To use this PoC, run the program and either provide the target URL or a file containing a list of URLs. You can also set the number of threads for scanning. The program will then check for vulnerabilities and allow you to interact with the vulnerable system through a shell.

## 😎 Expected Output
The expected output will include messages indicating whether the target is vulnerable or not. If vulnerable, the program will display command outputs after interacting with the system, allowing you to see the results of executed commands.

## ✅ Mitigation
To mitigate CVE-2024-7029, it is strongly recommended to decommission affected AVTech devices, especially if patches are unavailable. Additionally, actively monitor network traffic for unusual activity and apply any security updates as soon as they become available to reduce the risk of exploitation.


## 🎖️ Credit
- **Bug Founder**: Aline Eliovich  Security Researcher at Akamai
- [Aline Eliovich](https://www.linkedin.com/in/aline-eliovich/)

## ❤️ Donation
If you find this project helpful and would like to support further development, please consider making a donation:
- [Donate Here](https://ebrasha.com/abdal-donation)

## 🤵 Programmer
Handcrafted with Passion by **Ebrahim Shafiei (EbraSha)**
- **E-Mail**: Prof.Shafiei@Gmail.com
- **Telegram**: [@ProfShafiei](https://t.me/ProfShafiei)

## ☠️ Reporting Issues
If you encounter any issues or have configuration problems, please reach out via email at Prof.Shafiei@Gmail.com. You can also report issues on GitLab or GitHub.

## ⚠️  Legal Disclaimer
This Proof of Concept (PoC) is provided for educational purposes only. Unauthorized use of this code on systems you do not own or have explicit permission to test is illegal and unethical. By using this PoC, you agree to take full responsibility for any misuse or damage that may result. The author disclaims all liability for actions taken based on the information provided in this repository. Always ensure you have proper authorization before conducting any security testing.