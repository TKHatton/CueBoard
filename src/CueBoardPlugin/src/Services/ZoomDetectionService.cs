namespace Loupedeck.CueBoardPlugin.Services
{
    using System;
    using System.Diagnostics;

    public class ZoomDetectionService
    {
        public Boolean OverrideMode { get; set; } = false;

        public Boolean IsZoomRunning
        {
            get
            {
                if (this.OverrideMode)
                {
                    return true;
                }

                try
                {
                    return Process.GetProcessesByName("Zoom").Length > 0;
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}
