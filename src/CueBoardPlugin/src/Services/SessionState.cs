namespace Loupedeck.CueBoardPlugin.Services
{
    using System;
    using Loupedeck.CueBoardPlugin.Models;

    public class SessionState
    {
        // Page 1 toggles
        public Boolean IsMuted { get; set; } = true;
        public Boolean CameraOn { get; set; } = false;
        public Boolean IsRecording { get; set; } = false;
        public Boolean IsSharing { get; set; } = false;
        public Boolean HandRaised { get; set; } = false;
        public Boolean IsGalleryView { get; set; } = false;

        // Page 2 toggles
        public Boolean MeetingLocked { get; set; } = false;
        public Boolean CaptionsOn { get; set; } = false;
        public Boolean SpotlightOn { get; set; } = false;

        // Dial states
        public Int32 SelectedReactionIndex { get; set; } = 0;
        public FlagType SelectedFlagType { get; set; } = FlagType.ActionItem;

        // Meeting tracking
        public DateTime MeetingStartTime { get; set; } = DateTime.Now;

        public event Action StateChanged;

        public void NotifyStateChanged() => this.StateChanged?.Invoke();

        public void ResetForNewMeeting()
        {
            this.IsMuted = true;
            this.CameraOn = false;
            this.IsRecording = false;
            this.IsSharing = false;
            this.HandRaised = false;
            this.IsGalleryView = false;
            this.MeetingLocked = false;
            this.CaptionsOn = false;
            this.SpotlightOn = false;
            this.SelectedReactionIndex = 0;
            this.SelectedFlagType = FlagType.ActionItem;
            this.MeetingStartTime = DateTime.Now;
            this.NotifyStateChanged();
        }
    }
}
