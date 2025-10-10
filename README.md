# Journaler

A Markdown-based journaling system that organizes articles by category and subject, generating dynamic Tables of Contents automatically. 

---

## ⚠️ Work in Progress

Currently under development, it **does not do anything important yet**.  

---

## Planned Features

- Automatically scan journal entries
- Extract categories and subjects from folder names
- Generate a per-year timeline (`Timeline.md`)
- Generate a table of contents for all entries
- Generate and use metadata into markdown files to detect updates (Publish & UpdateDate)
- Maintain kebab-case file naming

---

## Notes

- I may add configuration maybe even a way to make it flexible but note that I might not either, depends on if I feel like it or not

### Compiling the program:
1. Go in the project folder (where `journaler.sln` is)
2. Open bash or a command prompt (In the folder)
3. `dotnet publish -c Release -r win-x64  -p:PublishAot=true`
    > Choose your platform to compile it for: `win-x64`, `linux-x64`, `linux-x86`, `win-x86` etc...
4. Have fun experimenting around, this is more like... a quick prototype than an actual full working app
