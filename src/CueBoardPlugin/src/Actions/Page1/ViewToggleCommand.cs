namespace Loupedeck.CueBoardPlugin.Actions.Page1
{
    using System;
    using Loupedeck.CueBoardPlugin.Services;

    public class ViewToggleCommand : CueBoardCommand
    {
        public ViewToggleCommand()
            : base("View Toggle", "Switch Gallery/Speaker view", "Live Controls")
        {
        }

        protected override void RunCommand(String actionParameter)
        {
            if (this.State == null)
            {
                return;
            }

            this.State.IsGalleryView = !this.State.IsGalleryView;

            // Alt+F1 = Speaker view, Alt+F2 = Gallery view
            if (this.State.IsGalleryView)
            {
                this.Keyboard.SendAltF2();
            }
            else
            {
                this.Keyboard.SendAltF1();
            }

            this.ActionImageChanged();
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            return this.State?.IsGalleryView == true
                ? this.DrawIcon(imageSize, "view-gallery.png")
                : this.DrawIcon(imageSize, "view-speaker.png");
        }
    }
}
