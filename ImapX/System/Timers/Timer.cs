using System.ComponentModel;
using System.Windows.Threading;

namespace System.Timers
{
    public class Timer : DispatcherTimer
    {
        private bool _elapsedRaised;

        public Timer(int milliseconds)
        {
            Interval = new TimeSpan(0, 0, 0, 0, milliseconds);
            Tick += Timer_Tick;
        }

        [DefaultValue(true)]
        public bool AutoReset { get; set; }

        public bool Enabled
        {
            get { return IsEnabled; }
            set
            {
                if (value != IsEnabled)
                    _elapsedRaised = false;

                if (value && !IsEnabled)
                    Start();
                else if (!value && IsEnabled)
                    Stop();
            }
        }

        public void Close()
        {
            if (IsEnabled)
                Stop();
            _elapsedRaised = false;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (Elapsed == null || (!AutoReset && _elapsedRaised)) return;
            Elapsed(sender, e);
            _elapsedRaised = true;
        }

        public event EventHandler Elapsed;
    }
}