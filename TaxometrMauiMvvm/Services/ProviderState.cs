namespace TaxometrMauiMvvm.Services
{
    public enum ProviderState
    {
        Idle,
        SentFLC_0,
        SentData_0,
        ReciveFLC_0,
        SentFLC_1,
        SentFR,
        ReciveFLC_1,
        SentFLC_2
    }

    public class Timer
    {
        private bool _stopped = true;

        private int _maxMillis;
        public int MaxMillis => _maxMillis;
        private int _currentTime;
        public int CurrentTime => _currentTime;

        public event Action OnStop;
        public event Action OnStart;
        public event Action OnTimeout;
        public event Action OnRestart;

        public Timer(int millis)
        {
            _maxMillis = millis;
            _currentTime = 0;
        }

        public void SetMaxMillis(int maxMillis)
        {
            _maxMillis = maxMillis;
        }

        public async void Start()
        {
            if (!_stopped) throw new Exception("Can't start worked timer, use Restart(); or Stop(); Start();");

            _stopped = false;
            _currentTime = 0;
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                Dispatcher.GetForCurrentThread()?.StartTimer(TimeSpan.FromMilliseconds(1), new Func<bool>(() =>
                {
                    _currentTime++;
                    if (_currentTime >= _maxMillis)
                    {
                        _stopped = true;
                        OnTimeout?.Invoke();
                    }
                    return !_stopped;
                }));
            });
            OnStart?.Invoke();
        }

        public void Stop()
        {
            _stopped = true;
            OnStop?.Invoke();
        }

        public void Restart()
        {
            if (!_stopped)
            {
                Stop();
            }
            Start();
            OnRestart?.Invoke();
        }
    }
}
