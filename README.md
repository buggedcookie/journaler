# Journaler

A Markdown-based journaling system that organizes entries by category and subject, generating dynamic Tables of Contents automatically.

---

## ⚠️ Work in Progress

Seriously, don't look at it wrong, it might break due to spaghetti mess.

---


## Compiling the Program

1. Navigate to the project folder (where `journaler.sln` is located)
2. Open Bash or Command Prompt in that folder
3. Run:

```bash
dotnet publish -c Release -r win-x64 -p:PublishAot=true
```

> Replace `win-x64` with your target platform: `linux-x64`, `linux-x86`, `win-x86`, etc.

