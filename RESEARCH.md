# CueBoard — Build Research

> Everything needed to build the Logitech Actions SDK plugin for the MX Creative Console.
> Compiled 2026-03-22.

---

## 1. Logitech Actions SDK — How It Works

### Architecture
- Plugins are loaded by the **Logi Plugin Service (LPS)**, a background process alongside **Logi Options+**
- LPS brokers between your plugin code, the Options+ UI, and the physical hardware
- **C# plugins** load as .NET assemblies (DLLs) directly into the Plugin Service process
- **Node.js plugins** run as separate processes via IPC (Windows-only currently)
- **No hardware simulator/emulator exists** — you need the physical device to test

### Supported Languages
| Language | Platform | Notes |
|----------|----------|-------|
| C# (.NET 8) | Windows + macOS | Full feature access, recommended |
| Node.js/TypeScript | Windows only | Simpler, fewer features |

**Recommendation: Use C#/.NET 8** — full feature access, better debugging, matches your existing plan.

### Plugin Communication Flow
```
Physical Device ←→ Logi Plugin Service ←→ Your Plugin (DLL)
                         ↕
                    Logi Options+ (UI)
```
Your plugin never talks to hardware directly. LPS handles all USB/Bluetooth communication.

---

## 2. Plugin Project Structure

### Scaffold via LogiPluginTool CLI
```
CueBoard/
├── metadata/
│   └── loupedeckPackage.yaml    # Manifest
├── Actions/
│   ├── MuteAction.cs            # Button actions
│   ├── CameraAction.cs
│   └── ...
├── Commands/
│   └── ...
├── Resources/                    # Icons (80x80 PNG)
│   ├── mute-on.png
│   ├── mute-off.png
│   └── ...
├── CueBoardPlugin.cs            # Main plugin class
└── CueBoard.csproj
```

### Manifest Format (`metadata/loupedeckPackage.yaml`)
```yaml
type: plugin4
name: CueBoard
displayName: CueBoard - Meeting Control Surface
version: 1.0
author: Tyler
copyright: 2026 Tyler
supportedDevices:
  - LoupedeckLive              # ← This is the MX Creative Console identifier
pluginFileName: CueBoard.dll
pluginFolderWin: bin/win/
pluginFolderMac: bin/mac/
```

**Critical:** The MX Creative Console uses `LoupedeckLive` as its device identifier (shared hardware lineage with Loupedeck Live).

---

## 3. SDK Classes You Need

### For LCD Key Buttons: `PluginDynamicCommand`
```csharp
public class MuteAction : PluginDynamicCommand
{
    private bool _isMuted = false;

    public MuteAction()
    {
        DisplayName = "Mute/Unmute";
        Description = "Toggle Zoom microphone";
    }

    protected override void RunCommand(string actionParameter)
    {
        // Send Alt+A to Zoom
        _isMuted = !_isMuted;
        ActionImageChanged(); // Redraw the button
    }

    protected override BitmapImage GetCommandImage(string actionParameter, PluginImageSize imageSize)
    {
        using var builder = new BitmapBuilder(imageSize);
        var icon = _isMuted
            ? EmbeddedResources.ReadBinaryFile("mute-on.png")
            : EmbeddedResources.ReadBinaryFile("mute-off.png");
        builder.SetBackgroundImage(icon);
        builder.DrawText(_isMuted ? "MUTED" : "LIVE");
        return builder.ToImage();
    }
}
```

### For Dial Rotation: `PluginDynamicAdjustment`
```csharp
public class ReactionDialAction : PluginDynamicAdjustment
{
    private int _selectedReaction = 0;
    private readonly string[] _reactions = { "👍", "👏", "❤️", "😂", "🎉" };

    protected override void ApplyAdjustment(string actionParameter, int diff)
    {
        // diff is positive (clockwise) or negative (counter-clockwise)
        _selectedReaction = Math.Clamp(_selectedReaction + diff, 0, _reactions.Length - 1);
        AdjustmentValueChanged(); // Update dial display
    }

    protected override string GetAdjustmentValue(string actionParameter)
    {
        return _reactions[_selectedReaction]; // Shown on dial display
    }

    protected override void RunCommand(string actionParameter)
    {
        // Dial press — send the selected reaction
        SendReaction(_selectedReaction);
    }
}
```

### For Multi-Page Layout: `PluginDynamicFolder`
```csharp
public class MeetingFolder : PluginDynamicFolder
{
    protected override IEnumerable<string> GetButtonPressActionNames()
    {
        // Return the 9 action names for the current page
        return new[] { "mute", "camera", "record", "share", "chat",
                       "reaction", "hand", "view", "end" };
    }

    protected override IEnumerable<string> GetEncoderRotateActionNames()
    {
        return new[] { "reaction-dial" };
    }
}
```

### For Configurable Actions: `ActionEditorCommand`
Used when an action needs user-configurable settings (e.g., selecting which timer duration).

---

## 4. LCD Key Images

- **Size:** 80x80 pixels, PNG format
- **Dynamic rendering:** Use `BitmapBuilder` with `SetBackgroundImage()`, `FillRectangle()`, `DrawText()`
- **Do NOT set explicit font sizes** — Plugin Service auto-sizes for each device
- **Redraw trigger:** Call `ActionImageChanged()` (or `ActionImageChanged(null)` for all buttons)
- **Animated GIFs:** Supported via `BitmapImage.FromArray(byte[])`
- **Resource loading:** `EmbeddedResources.FindFile()` and `EmbeddedResources.ReadBinaryFile()`

---

## 5. Development Setup

### Prerequisites
1. **.NET 8.0 SDK** — [download](https://dotnet.microsoft.com/download/dotnet/8.0)
2. **Logi Options+** — installed on Windows (comes with the MX Creative Console)
3. **LogiPluginTool CLI** — for scaffolding and packaging
4. **IDE:** Visual Studio 2022, VS Code, or Rider
5. **Physical MX Creative Console** — connected via USB-C or Bluetooth

### Build & Debug Workflow
1. Scaffold: `logiplugintool create CueBoard`
2. Build: `dotnet build`
3. Link to Plugin Service: Creates a `.link` file or symlink from output to LPS plugin folder
4. Debug: Visual Studio → Debug → launches LPS with your plugin loaded, full breakpoints

### Packaging & Distribution
- Package: `logiplugintool pack ./bin/Release/ ./CueBoard.lplug4`
- `.lplug4` = ZIP archive containing manifest + compiled DLLs + resources
- Install: Double-click `.lplug4` (registered with Plugin Service)
- Distribute: Via Logitech Marketplace (approval required)

---

## 6. Zoom Keyboard Shortcuts — Complete Reference

### Global-Capable Shortcuts (work even when Zoom is not focused)
| Action | Shortcut | Notes |
|--------|----------|-------|
| Mute/Unmute | `Alt+A` | Must enable "Global Shortcut" in Zoom settings |
| Camera On/Off | `Alt+V` | Must enable "Global Shortcut" |
| Start/Stop Recording | `Alt+R` | Must enable "Global Shortcut" |
| Pause Recording | `Alt+P` | Must enable "Global Shortcut" |
| Screen Share | `Alt+S` | Must enable "Global Shortcut" |
| Chat Panel | `Alt+H` | Must enable "Global Shortcut" |
| Raise Hand | `Alt+Y` | Must enable "Global Shortcut" |
| Mute All (host) | `Alt+M` | Must enable "Global Shortcut" |
| Speaker View | `Alt+F1` | Must enable "Global Shortcut" |
| Gallery View | `Alt+F2` | Must enable "Global Shortcut" |
| Fullscreen | `Alt+F` | Must enable "Global Shortcut" |

### Zoom-Focused-Only Shortcuts
| Action | Shortcut |
|--------|----------|
| End Meeting | `Alt+Q` |
| Participants Panel | `Alt+U` |
| Switch Camera | `Alt+N` |
| Open Invite | `Alt+I` |

### Reaction Shortcuts (require Zoom focus)
| Reaction | Shortcut |
|----------|----------|
| Clap | `Alt+Shift+4` |
| Thumbs Up | `Alt+Shift+5` |
| Heart | `Alt+Shift+6` |
| Joy/Laugh | `Alt+Shift+7` |
| Surprised | `Alt+Shift+8` |
| Tada/Party | `Alt+Shift+9` |
| Open Reactions Panel | `Ctrl+Shift+Y` |

### Actions WITH NO KEYBOARD SHORTCUT
| Action | Workaround |
|--------|-----------|
| Spotlight Speaker | Right-click participant → Spotlight. No shortcut exists. |
| Admit from Waiting Room | Click "Admit" button. No shortcut exists. |
| Lock/Unlock Meeting | Security menu → Lock. No shortcut exists. |
| Remove Participant | Right-click participant → Remove. No shortcut exists. |
| Toggle Captions | Click "Show Captions" toolbar button. No shortcut exists. |
| Pin Video | Right-click participant → Pin. No shortcut exists. |

**Impact on CueBoard:** Page 2 "Operator Mode" actions (Spotlight, Admit, Lock, Remove) cannot be triggered via keyboard. These will need to be either:
1. Simulated/demoed only (for hackathon)
2. Implemented via UI Automation (fragile, version-dependent)
3. Left as "coming soon" features

---

## 7. Keyboard Simulation in C# (.NET 8)

### Recommended: H.InputSimulator NuGet Package
```
dotnet add package H.InputSimulator
```

```csharp
using H.InputSimulator;
using H.InputSimulator.Native;

var sim = new InputSimulator();

// Toggle mute (Alt+A)
sim.Keyboard.ModifiedKeyStroke(VirtualKeyCode.MENU, VirtualKeyCode.VK_A);

// Toggle camera (Alt+V)
sim.Keyboard.ModifiedKeyStroke(VirtualKeyCode.MENU, VirtualKeyCode.VK_V);

// Send reaction: Thumbs Up (Alt+Shift+5)
sim.Keyboard.ModifiedKeyStroke(
    new[] { VirtualKeyCode.MENU, VirtualKeyCode.SHIFT },
    VirtualKeyCode.VK_5
);

// Chain actions with delays
sim.Keyboard
    .ModifiedKeyStroke(VirtualKeyCode.MENU, VirtualKeyCode.VK_A)
    .Sleep(100)
    .ModifiedKeyStroke(VirtualKeyCode.MENU, VirtualKeyCode.VK_V);
```

### Alternative: Raw P/Invoke (zero dependencies)
```csharp
[DllImport("user32.dll", SetLastError = true)]
static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

// Send Alt+Key combination
public static void SendAltKey(ushort vkCode)
{
    var inputs = new INPUT[4];
    inputs[0] = KeyDown(VK_MENU);   // Alt down
    inputs[1] = KeyDown(vkCode);     // Key down
    inputs[2] = KeyUp(vkCode);       // Key up
    inputs[3] = KeyUp(VK_MENU);      // Alt up
    SendInput(4, inputs, Marshal.SizeOf<INPUT>());
}
```

### Zoom Process Detection
```csharp
public static bool IsZoomRunning()
    => Process.GetProcessesByName("Zoom").Length > 0;

public static bool IsInMeeting()
{
    // Check for "Zoom Meeting" window title
    // Uses EnumWindows + GetWindowText
}
```

### Key Best Practices
- **Use Zoom's global shortcuts** — then `SendInput` works without window focusing
- **Thread safety** — serialize all `SendInput` calls to avoid modifier key corruption
- **Timing** — no delay needed within a single `SendInput` array; 50-100ms between separate shortcuts
- **UIPI** — Zoom runs at medium integrity, same as your plugin. No admin needed.
- **Do NOT use PostMessage/SendMessage** — these do not work reliably for keyboard simulation

---

## 8. Current Project State

### What Exists
- `index.html` — Polished landing page (692 lines, fully working)
- 7 markdown documentation files — SPEC, DECISIONS, ROADMAP, DEMO-SCRIPT, PITCH, KNOWN-ISSUES, ZOOM-SHORTCUTS
- Zero source code, zero C# files, zero project files

### What Needs to Be Built
1. .NET 8 solution & project structure
2. Logitech Actions SDK integration (plugin scaffold, manifest)
3. Core services (keyboard simulation, Zoom detection, state management)
4. Page 1 actions (Mute, Camera, Record, Share, Chat, Reaction, Hand, View, End)
5. Page 2 actions (Mute All, Timer, and simulated host controls)
6. Page 3 actions (Flag, Clear, Export)
7. Dial handlers (reaction selection, timer adjustment, flag type selection)
8. LCD key icons (80x80 PNG per state per button)
9. Build, test, package as `.lplug4`

---

## 9. Key Reference Links

| Resource | URL |
|----------|-----|
| Logi Actions SDK Docs | https://logitech.github.io/actions-sdk-docs/ |
| SDK GitHub (C# DemoPlugin) | https://github.com/Logitech/actions-sdk |
| Home Assistant Plugin (best reference) | https://github.com/Logitech/cto-HomeAssistantPlugin-OptionsPlus |
| H.InputSimulator NuGet | https://www.nuget.org/packages/H.InputSimulator |
| Zoom Keyboard Shortcuts | https://support.zoom.com/hc/en/article?id=zm_kb&sysparm_article=KB0067050 |
| .NET 8 SDK Download | https://dotnet.microsoft.com/download/dotnet/8.0 |
| Logitech Marketplace Developer Portal | https://www.logitech.com/en-us/software/marketplace/developer.html |
| Codecademy Logi Plugin Course | https://www.codecademy.com/learn/build-plugins-for-logitech-devices |
| DevStudio 2026 Hackathon | https://devstudiologitech2026.devpost.com/ |
| Logi Developer Discord | https://discord.gg/etJCPZytHg |

---

## 10. Build Order Recommendation

### Phase 1: Foundation (get something running on the device)
1. Install .NET 8 SDK
2. Install Logi Options+ and connect MX Creative Console
3. Scaffold plugin with LogiPluginTool
4. Create one button (Mute toggle) with state-aware LCD icon
5. Wire up H.InputSimulator to send `Alt+A`
6. Verify it works on the physical device

### Phase 2: Page 1 Complete
7. Add remaining Page 1 buttons (Camera, Record, Share, Chat, Hand, View, End)
8. Implement reaction dial (rotate to select, press to send)
9. Create all LCD key icons (on/off states)

### Phase 3: Page 2 (Operator Mode)
10. Mute All button (`Alt+M`)
11. Internal timer (dial sets duration, button starts/pauses)
12. Simulated host actions (Spotlight, Lock, Admit, Remove) — demo-only

### Phase 4: Page 3 (Meeting Intelligence)
13. Flag Moment button + flag type dial
14. Clear Last Flag button
15. Export Summary button (Markdown generation)

### Phase 5: Polish & Package
16. Refine all LCD key icons
17. Test end-to-end with live Zoom meetings
18. Package as `.lplug4`
19. Record demo video for hackathon submission
