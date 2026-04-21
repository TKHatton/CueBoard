namespace Loupedeck.CueBoardPlugin.Actions.Page2
{
    using System;
    using System.Threading;
    using Loupedeck.CueBoardPlugin.Services;

    public class AdmitCommand : CueBoardCommand
    {
        public AdmitCommand()
            : base("Admit All", "Admit all from waiting room (Alt+U, Tab, Enter)", "Operator Mode")
        {
        }

        protected override void RunCommand(String actionParameter)
        {
            if (this.Keyboard == null)
            {
                return;
            }

            // Strategy: Alt+U to open participants, then Tab to "Admit All" button, then Enter
            // This attempts to admit everyone without requiring mouse clicks

            // Step 1: Open participants panel
            this.Keyboard.SendAltKey(KeyboardService.KEY_U);
            Thread.Sleep(300); // Wait for panel to open

            // Step 2: Tab to the Admit All button (usually first or second interactive element)
            this.Keyboard.SendTab();
            Thread.Sleep(100);

            // Step 3: Press Enter to activate "Admit All"
            this.Keyboard.SendEnter();

            this.CueBoard?.Toast?.ShowToast("✅", "Admitted from waiting room", 2000);
            PluginLog.Info("Attempted to admit all from waiting room (Alt+U, Tab, Enter)");
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            return this.DrawIcon(imageSize, "admit.png");
        }
    }
}
