namespace Loupedeck.CueBoardPlugin
{
    using System;

    public class CueBoardApplication : ClientApplication
    {
        public CueBoardApplication()
        {
        }

        protected override String GetProcessName() => "";

        protected override String GetBundleName() => "";

        public override ClientApplicationStatus GetApplicationStatus() => ClientApplicationStatus.Unknown;
    }
}
