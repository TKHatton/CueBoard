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
        private const Byte VK_SHIFT = 0x10;    // Shift
        private const Byte VK_LWIN = 0x5B;     // Left Windows key
        private const Byte VK_F1 = 0x70;
        private const Byte VK_F2 = 0x71;
        private const Byte VK_UP = 0x26;       // Arrow Up
        private const Byte VK_DOWN = 0x28;     // Arrow Down

        // Key codes for letters (A=0x41, etc.) and numbers (0=0x30, etc.)
        public const UInt16 KEY_A = 0x41;
        public const UInt16 KEY_B = 0x42;
        public const UInt16 KEY_C = 0x43;
        public const UInt16 KEY_F = 0x46;
        public const UInt16 KEY_H = 0x48;
        public const UInt16 KEY_I = 0x49;
        public const UInt16 KEY_L = 0x4C;
        public const UInt16 KEY_M = 0x4D;
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
            if (vk == VK_SHIFT) return "Shift";
            if (vk >= 0x41 && vk <= 0x5A) return ((Char)vk).ToString();
            if (vk >= 0x30 && vk <= 0x39) return ((Char)vk).ToString();
            if (vk == VK_F1) return "F1";
            if (vk == VK_F2) return "F2";
            if (vk == VK_LWIN) return "Win";
            if (vk == VK_UP) return "Up";
            if (vk == VK_DOWN) return "Down";
            return $"0x{vk:X2}";
        }

        // P/Invoke - using keybd_event which works from plugin service context
        private const UInt32 KEYEVENTF_KEYUP = 0x0002;

        [DllImport("user32.dll")]
        private static extern void keybd_event(Byte bVk, Byte bScan, UInt32 dwFlags, UIntPtr dwExtraInfo);
    }
}
