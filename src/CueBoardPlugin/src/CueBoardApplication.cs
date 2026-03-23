namespace Loupedeck.CueBoardPlugin
{
    using System;

    public class CueBoardApplication : ClientApplication
    {
        public CueBoardApplication()
        {
        }

        protected override String GetProcessName() => "Zoom";

        protected override String GetBundleName() => "us.zoom.xos";

        public override ClientApplicationStatus GetApplicationStatus() => ClientApplicationStatus.Unknown;
    }
}
