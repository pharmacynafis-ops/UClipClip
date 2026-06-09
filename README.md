<div align="center">

<pre>
╔══════════════════════════════════════════════════════════════════════════════╗
║                                                                              ║
║    ██╗   ██╗ ██████╗██╗     ██╗██████╗  ██████╗██╗     ██╗██████╗            ║
║    ██║   ██║██╔════╝██║     ██║██╔══██╗██╔════╝██║     ██║██╔══██╗           ║
║    ██║   ██║██║     ██║     ██║██████╔╝██║     ██║     ██║██████╔╝           ║
║    ██║   ██║██║     ██║     ██║██╔═══╝ ██║     ██║     ██║██╔═══╝            ║
║    ╚██████╔╝╚██████╗███████╗██║██║     ╚██████╗███████╗██║██║                ║
║     ╚═════╝  ╚═════╝╚══════╝╚═╝╚═╝      ╚═════╝╚══════╝╚═╝╚═╝                ║
║                                                                              ║
║                 Your clipboard, supercharged. 📋                             ║
║                                                                              ║
╚══════════════════════════════════════════════════════════════════════════════╝
</pre>

**A lightweight, modern Windows clipboard manager that quietly tracks everything you copy — text, files, and images — and keeps it one click away.**

<br>

[![Platform](https://img.shields.io/badge/Platform-Windows%2010%2F11-0078D4?style=for-the-badge&logo=windows&logoColor=white)](https://www.microsoft.com/windows)
[![Framework](https://img.shields.io/badge/.NET-9.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![UI](https://img.shields.io/badge/UI-WPF-0078D4?style=for-the-badge)](https://github.com/dotnet/wpf)
[![License](https://img.shields.io/badge/License-MIT-22C55E?style=for-the-badge)](LICENSE.txt)
[![Release](https://img.shields.io/github/v/release/pharmacynafis-ops/UClipClip?style=for-the-badge&color=blue)](https://github.com/pharmacynafis-ops/UClipClip/releases)

<br>

[**📥 Download**](https://github.com/pharmacynafis-ops/UClipClip/releases) · [**🐛 Report Bug**](https://github.com/pharmacynafis-ops/UClipClip/issues) · [**💡 Request Feature**](https://github.com/pharmacynafis-ops/UClipClip/issues)

</div>

---

## 📑 Table of Contents

- [Overview](#-overview)
- [Features](#-features)
- [Screenshots](#-screenshots)
- [Tech Stack](#-tech-stack)
- [Getting Started](#-getting-started)
- [Project Structure](#-project-structure)
- [How It Works](#-how-it-works)
- [Contributing](#-contributing)
- [Bug Reports & Feature Requests](#-bug-reports--feature-requests)
- [License](#-license)
- [Author](#-author)
- [Acknowledgments](#-acknowledgments)

---

## ✨ Overview

**UClipClip** is a productivity-focused clipboard manager for Windows, built with C# and WPF on .NET 9. It runs quietly in the background, monitors your system clipboard in real time, and organizes every copied item into three neat tabs — **Text**, **Files**, and **Images** — so nothing you copy ever gets lost.

Designed with a sleek, borderless, topmost UI that pops up the moment you copy (when enabled), UClipClip stays out of your way until you need it, then disappears just as fast.

---

## 🎯 Features

### 📝 Text History
- Captures **every piece of text** you copy, automatically.
- Items are grouped by day with a clean, collapsible "Today / Yesterday / …" view.
- One-click **copy back to clipboard** for instant reuse.
- **Pin** important snippets so they are never pushed out of history.
- Side-by-side preview of long content with a smart **80-character collapsed preview**.
- Configurable history size (10 – 500 items).

### 📁 File History
- Tracks file and folder copy operations from **File Explorer** and any app.
- When only image files are copied, they are **automatically routed to the Images tab** for a unified view.
- Shows filenames with a **"(+N more)"** indicator for multi-file copies.
- Configurable history size (10 – 300 items).

### 🖼️ Image History
- Captures **bitmap** and **PNG** clipboard data (screenshots, snips, copied images).
- **Live thumbnails** with native pixel dimensions shown.
- Originals are re-encoded as PNG so you can paste them back into any app.
- Smart de-duplication via **MD5 hash** — the same screenshot won't be saved twice in a row.

### 🎨 Theming
Three hand-tuned themes out of the box, switchable live from Settings:

| Theme   | Vibe                                                     |
| ------- | -------------------------------------------------------- |
| ☀️ Light  | Clean and minimal — easy on the eyes during the day.     |
| 🌙 Dark   | Low-glare — perfect for late-night work.                 |
| 👑 King   | Bold accent colors — a touch of personality.             |

### ⚙️ Customization
- **Appear on copy** — auto-show the window when something new is copied.
- **Run on startup** — register a Run-key entry so UClipClip boots with Windows.
- **Sound on copy** — optional audible confirmation (system beep).
- **Show timestamps** — display copy time on every card.
- **Resizable window** — width and height clamp-safe between 200×300 and 800×1000.
- **Settings persist** between sessions in a local JSON file.

### 🪟 Window Behavior
- **Borderless, topmost, always-above** UI.
- **Drag from the title bar** to move; **minimize** and **close (hide)** buttons.
- Sits out of the way on the edge of your screen until you need it.

### 📦 Easy Install
- Single-click **Inno Setup** installer (`UClipClip_Setup.exe`).
- Installs to `%ProgramFiles%\UClipClip`.
- Adds **Start Menu** and **Startup** shortcuts.
- Clean uninstaller that removes everything it added.

---

## 📸 Screenshots

<table align="center">
  <tr>
    <th>Light Theme</th>
  </tr>
  <tr>
    <td align="center"><img src="https://github.com/user-attachments/assets/fb02a241-b84a-4c10-a1c1-dd851e0ee5f3" width="720" alt="UClipClip Light Theme"></td>
  </tr>
  <tr>
    <th>Dark Theme</th>
  </tr>
  <tr>
    <td align="center"><img src="https://github.com/user-attachments/assets/0291da1c-4c72-4e66-b7bf-4d08f5a75305" width="720" alt="UClipClip Dark Theme"></td>
  </tr>
  <tr>
    <th>King Theme</th>
  </tr>
  <tr>
    <td align="center"><img src="https://github.com/user-attachments/assets/e6bc23e2-7098-4ee3-ad2b-5cba0378c311" width="720" alt="UClipClip King Theme"></td>
  </tr>
</table>

---

## 🛠️ Tech Stack

| Layer           | Technology                          |
| --------------- | ----------------------------------- |
| Language        | C# 12                               |
| UI Framework    | WPF (.NET 9.0-windows)              |
| Serialization   | `System.Text.Json` 10.0.8           |
| Clipboard Hooks | `System.Windows.Forms`              |
| Packaging       | Inno Setup 6                        |
| Build System    | `dotnet` SDK                        |

---

## 🚀 Getting Started

### 📥 Option 1 — Install the Prebuilt Release (Recommended)

1. Go to the [**Releases**](https://github.com/pharmacynafis-ops/UClipClip/releases) page.
2. Download the latest **`UClipClip_Setup.exe`**.
3. Run the installer — it takes about 5 seconds.
4. Launch UClipClip from the **Start Menu** or your **Startup** folder.
5. Start copying. UClipClip does the rest.

### 🧑‍💻 Option 2 — Build From Source

**Prerequisites**
- **Windows 10 / 11**
- **.NET 9 SDK** — [download here](https://dotnet.microsoft.com/download/dotnet/9.0)
- *(Optional)* **Inno Setup 6** — only required if you want to rebuild the installer.

**Clone & Build**
```bash
git clone https://github.com/pharmacynafis-ops/UClipClip.git
cd UClipClip
dotnet build -c Release
```

**Run**
```bash
dotnet run -c Release
```

The compiled `UClipClip.exe` will be in `bin\Release\net9.0-windows\`.

**Build the Installer**
From the project root, run the Inno Setup compiler:
```bash
"C:\Program Files (x86)\Inno Setup 6\ISCC.exe" UClipClip.iss
```
The resulting installer — **`UClipClip_Setup.exe`** — is created in the project root.

---

## 📁 Project Structure

```
UClipClip/
├── App.xaml / App.xaml.cs            # App entry, global WPF styles
├── MainWindow.xaml / .cs             # Main borderless window, tab logic
├── DayGroup.cs                       # Generic day-grouped collection
├── UClipClip.csproj                  # .NET 9 WPF project file
├── UClipClip.iss                     # Inno Setup installer script
├── LICENSE.txt                       # MIT license text
│
├── Assets/
│   └── ico.ico                       # Application icon
│
├── Models/
│   ├── AppSettings.cs                # User-configurable settings model
│   ├── SettingsManager.cs            # Load/save settings, Run-on-startup
│   ├── ClipboardMonitor.cs           # Background clipboard watcher (timer)
│   ├── TextClip.cs                   # Text clip data model
│   ├── FileClip.cs                   # File clip data model
│   └── Imageclip.cs                  # Image clip data model
│
├── Themes/
│   ├── LightTheme.xaml               # Light theme resource dictionary
│   ├── DarkTheme.xaml                # Dark theme resource dictionary
│   └── KingTheme.xaml                # King theme resource dictionary
│
└── UserControls/
    ├── TextPage.xaml / .cs           # Text history tab
    ├── FilesPage.xaml / .cs          # File history tab
    ├── ImagesPage.xaml / .cs         # Image history tab
    ├── SettingsPage.xaml / .cs       # Settings overlay
    ├── AboutPage.xaml / .cs          # About overlay
    └── DeveloperInfoPage.xaml / .cs  # Developer info overlay
```

---

## ⚙️ How It Works

1. **`ClipboardMonitor`** spins up a `System.Windows.Forms.Timer` on a **400 ms interval** when the app starts.
2. Each tick, it inspects the clipboard in this priority order:
   1. **Text** → fires `TextCopied` if the text changed.
   2. **File drop list** → fires `FilesCopied` if the list changed.
   3. **Image** → extracts PNG bytes (preferring native PNG over bitmap conversion), hashes them, and fires `ImageCopied` only if the hash differs from the last seen image.
3. **`MainWindow`** subscribes to all three events and dispatches updates to the appropriate page.
4. **Settings** are loaded from / saved to a JSON file in the user's local app data folder.

---

## 🤝 Contributing

Contributions are what make the open-source community such an amazing place to learn, inspire, and create. Any contribution you make is **greatly appreciated**.

1. **Fork** the project.
2. Create your feature branch:
   ```bash
   git checkout -b feature/AmazingFeature
   ```
3. Commit your changes:
   ```bash
   git commit -m "Add some AmazingFeature"
   ```
4. Push to the branch:
   ```bash
   git push origin feature/AmazingFeature
   ```
5. Open a **Pull Request**.

### 💡 Ideas for Contributions
- 🌐 Localization (i18n) support
- 🔍 Global hotkey to summon the window
- 📌 "Always-on-top pin" for individual clips
- ☁️ Optional cloud sync
- 🧠 Fuzzy search across history
- 🪟 Multi-monitor edge snapping
- 📦 MSIX packaging in addition to Inno Setup

---

## 🐛 Bug Reports & Feature Requests

Found a bug or have an idea? Please open an issue:
👉 [**github.com/pharmacynafis-ops/UClipClip/issues**](https://github.com/pharmacynafis-ops/UClipClip/issues)

When reporting a bug, please include:
- Your **Windows version** (e.g. Windows 11 23H2)
- Your **.NET runtime version** (`dotnet --version`)
- Steps to reproduce
- Screenshots or screen recordings if applicable

---

## 📜 License

Distributed under the **MIT License**. See [`LICENSE.txt`](LICENSE.txt) for the full text.

```
MIT License

Copyright (c) 2024-2026 UClipClip Contributors

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
```

---

## 👤 Author

**UClipClip** is maintained with care by the UClipClip contributors.

- 🐙 GitHub: [@pharmacynafis-ops](https://github.com/pharmacynafis-ops)
- 📦 Repository: [pharmacynafis-ops/UClipClip](https://github.com/pharmacynafis-ops/UClipClip)

See the in-app **Settings → Developer Info** page for additional details.

---

## 🙏 Acknowledgments

- The **WPF team** at Microsoft for an excellent desktop UI framework.
- **Inno Setup** by Jordan Russell for the legendary installer compiler.
- The **.NET community** for countless open-source inspirations.
- **You** for using UClipClip. 💙

---

<div align="center">

⭐ **If UClipClip makes your day a little easier, consider giving it a star on GitHub.** ⭐

<br>

Made with ❤️ and ☕ in Cairo, Egypt.

</div>
