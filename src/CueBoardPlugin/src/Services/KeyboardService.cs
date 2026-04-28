namespace Loupedeck.CueBoardPlugin.Services
{
    using System;
    using System.Runtime.InteropServices;
    using System.Threading;

    public class KeyboardService
    {
        private readonly Object _lock = new Object();

        public Boolean SimulationMode { get; set; } = false;

        // Virtual key codes
        private const Byte VK_MENU = 0x12;     // Alt
        private const Byte VK_CONTROL = 0x11;  // Ctrl
        private const Byte VK_SHIFT = 0x10;    // Shift
        private const Byte VK_LWIN = 0x5B;     // Left Windows key
        private const Byte VK_F1 = 0x70;
        private const Byte VK_F2 = 0x71;
        private const Byte VK_UP = 0x26;       // Arrow Up
        private const Byte VK_DOWN = 0x28;     // Arrow Down
        private const Byte VK_TAB = 0x09;      // Tab
        private const Byte VK_RETURN = 0x0D;   // Enter

        // Key codes for letters (A=0x41, etc.) and numbers (0=0x30, etc.)
        public const UInt16 KEY_A = 0x41;
        public const UInt16 KEY_B = 0x42;
        public const UInt16 KEY_C = 0x43;
        public const UInt16 KEY_F = 0x46;
        public const UInt16 KEY_H = 0x48;
        public const UInt16 KEY_I = 0x49;
        public const UInt16 KEY_L = 0x4C;
        public const UInt16 KEY_M = 0x4D;
        public const UInt16 KEY_N = 0x4E;
        public const UInt16 KEY_Q = 0x51;
        public const UInt16 KEY_R = 0x52;
        public const UInt16 KEY_S = 0x53;
        public const UInt16 KEY_T = 0x54;
        public const UInt16 KEY_V = 0x56;
        public const UInt16 KEY_U = 0x55;
        public const UInt16 KEY_Y = 0x59;
        public const UInt16 KEY_4 = 0x34;
        public const UInt16 KEY_5 = 0x35;
        public const UInt16 KEY_6 = 0x36;
        public const UInt16 KEY_7 = 0x37;
        public const UInt16 KEY_9 = 0x39;

        public void SendAltKey(UInt16 vkCode)
        {
            this.SendModifiedKey(new Byte[] { VK_MENU }, (Byte)vkCode);
        }

        public void SendCtrlKey(UInt16 vkCode)
        {
            this.SendModifiedKey(new Byte[] { VK_CONTROL }, (Byte)vkCode);
        }

        public void SendAltShiftKey(UInt16 vkCode)
        {
            this.SendModifiedKey(new Byte[] { VK_MENU, VK_SHIFT }, (Byte)vkCode);
        }

        public void SendAltF1()
        {
            this.SendModifiedKey(new Byte[] { VK_MENU }, VK_F1);
        }

        public void SendAltF2()
        {
            this.SendModifiedKey(new Byte[] { VK_MENU }, VK_F2);
        }

        public void SendWinDown()
        {
            this.SendModifiedKey(new Byte[] { VK_LWIN }, VK_DOWN);
        }

        public void SendWinUp()
        {
            this.SendModifiedKey(new Byte[] { VK_LWIN }, VK_UP);
        }

        public void SendTab()
        {
            this.SendModifiedKey(new Byte[] { }, VK_TAB);
        }

        public void SendEnter()
        {
            this.SendModifiedKey(new Byte[] { }, VK_RETURN);
        }

        private void SendModifiedKey(Byte[] modifiers, Byte key)
        {
            lock (this._lock)
            {
                var modNames = new String[modifiers.Length + 1];
                for (var i = 0; i < modifiers.Length; i++)
                {
                    modNames[i] = GetKeyName(modifiers[i]);
                }
                modNames[modifiers.Length] = GetKeyName(key);
                var desc = String.Join("+", modNames);

                if (this.SimulationMode)
                {
                    PluginLog.Info($"[SIMULATION] Sending {desc}");
                    return;
                }

                try
                {
                    PluginLog.Info($"[KEYBOARD] Sending {desc}");

                    // Press modifiers
                    foreach (var mod in modifiers)
                    {
                        keybd_event(mod, 0, 0, UIntPtr.Zero);
                    }

                    // Press and release key
                    keybd_event(key, 0, 0, UIntPtr.Zero);
                    keybd_event(key, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);

                    // Release modifiers in reverse
                    for (var i = modifiers.Length - 1; i >= 0; i--)
                    {
                        keybd_event(modifiers[i], 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
                    }

                    Thread.Sleep(50);
                    PluginLog.Info($"[KEYBOARD] Sent {desc} successfully");
                }
                catch (Exception ex)
                {
                    PluginLog.Error(ex, $"Failed to send {desc}");
                }
            }
        }

        private static String GetKeyName(Byte vk)
        {
            if (vk == VK_MENU) return "Alt";
            if (vk == VK_CONTROL) return "Ctrl";
            if (vk == VK_SHIFT) return "Shift";
            if (vk >= 0x41 && vk <= 0x5A) return ((Char)vk).ToString();
            if (vk >= 0x30 && vk <= 0x39) return ((Char)vk).ToString();
            if (vk == VK_F1) return "F1";
            if (vk == VK_F2) return "F2";
            if (vk == VK_LWIN) return "Win";
            if (vk == VK_UP) return "Up";
            if (vk == VK_DOWN) return "Down";
            if (vk == VK_TAB) return "Tab";
            if (vk == VK_RETURN) return "Enter";
            return $"0x{vk:X2}";
        }

        /// <summary>
        /// Brings the Zoom meeting window to the foreground (if running) and sends Alt+key.
        /// Use this for shortcuts that ONLY work when Zoom has focus (Lock Meeting, Rename, etc.)
        /// </summary>
        public void SendAltKeyToZoom(UInt16 vkCode)
        {
            FocusZoom();
            Thread.Sleep(80); // small delay so the foreground change settles before keystroke
            this.SendAltKey(vkCode);
        }

        /// <summary>
        /// Toggles the Zoom window between minimized and restored state using Win32 ShowWindow,
        /// which is reliable regardless of which app currently has focus.
        /// </summary>
        public Boolean ToggleZoomMinimize()
        {
            var hWnd = FindZoomMainWindow();
            if (hWnd == IntPtr.Zero)
            {
                PluginLog.Warning("Zoom window not found for min/max toggle");
                return false;
            }

            if (IsIconic(hWnd))
            {
                ShowWindow(hWnd, SW_RESTORE);
                SetForegroundWindow(hWnd);
                PluginLog.Info("Zoom window restored");
                return true;
            }
            else
            {
                ShowWindow(hWnd, SW_MINIMIZE);
                PluginLog.Info("Zoom window minimized");
                return false;
            }
        }

        public static void FocusZoom()
        {
            var hWnd = FindZoomMainWindow();
            if (hWnd == IntPtr.Zero)
            {
                return;
            }
            if (IsIconic(hWnd))
            {
                ShowWindow(hWnd, SW_RESTORE);
            }
            SetForegroundWindow(hWnd);
        }

        private static IntPtr FindZoomMainWindow()
        {
            try
            {
                // Multiple Zoom-family processes exist (Zoom.exe, ZoomWorkplace.exe, helpers).
                // We want the one with a visible window — filter by non-empty title first.
                var procs = System.Diagnostics.Process.GetProcesses();
                var zoomProcs = new System.Collections.Generic.List<System.Diagnostics.Process>();
                foreach (var p in procs)
                {
                    try
                    {
                        if (p.ProcessName.IndexOf("Zoom", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            zoomProcs.Add(p);
                        }
                    }
                    catch
                    {
                    }
                }

                // Prefer the meeting window (has "Zoom Meeting" in the title)
                foreach (var p in zoomProcs)
                {
                    try
                    {
                        if (p.MainWindowHandle != IntPtr.Zero
                            && !String.IsNullOrEmpty(p.MainWindowTitle)
                            && p.MainWindowTitle.IndexOf("Meeting", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            return p.MainWindowHandle;
                        }
                    }
                    catch
                    {
                    }
                }

                // Otherwise any visible Zoom window with a non-empty title
                foreach (var p in zoomProcs)
                {
                    try
                    {
                        if (p.MainWindowHandle != IntPtr.Zero && !String.IsNullOrEmpty(p.MainWindowTitle))
                        {
                            PluginLog.Info($"Zoom window picked: {p.ProcessName} — '{p.MainWindowTitle}'");
                            return p.MainWindowHandle;
                        }
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
            }
            PluginLog.Warning("No visible Zoom window found");
            return IntPtr.Zero;
        }

        // P/Invoke - using keybd_event which works from plugin service context
        private const UInt32 KEYEVENTF_KEYUP = 0x0002;
        private const Int32 SW_MINIMIZE = 6;
        private const Int32 SW_RESTORE = 9;

        [DllImport("user32.dll")]
        private static extern void keybd_event(Byte bVk, Byte bScan, UInt32 dwFlags, UIntPtr dwExtraInfo);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern Boolean SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern Boolean ShowWindow(IntPtr hWnd, Int32 nCmdShow);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern Boolean IsIconic(IntPtr hWnd);
    }
}
