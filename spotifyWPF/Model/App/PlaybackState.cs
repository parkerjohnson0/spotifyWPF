using System.ComponentModel;
using System.Runtime.CompilerServices;
using spotifyWPF.Model.Player;

namespace spotifyWPF.Model.App;

public enum RepeatState
{
    off,
    context,
    track
}

public class PlaybackState : INotifyPropertyChanged
{
    public Device Device { get; set; }

    private bool _isPlaying;

    public bool IsPlaying
    {
        get { return _isPlaying; }
        set
        {
            _isPlaying = value;
            NotifyPropertyChanged();
        }
    }

    private bool _shuffleState;

    public bool ShuffleState
    {
        get { return _shuffleState; }
        set
        {
            _shuffleState = value;
            NotifyPropertyChanged();
        }
    }

    private RepeatState _repeatState = App.RepeatState.off;

    public RepeatState RepeatState
    {
        get { return _repeatState; }
        set
        {
            _repeatState = value;
            NotifyPropertyChanged();
        }
    }

    private string _progess;

    public string Progress
    {
        get
        {
            long seconds = ProgressMS / 1000;
            long minutesLength = seconds / 60;
            long secondsLength = seconds % 60;
            string pad = secondsLength < 10 ? "0" : "";
            return $"{minutesLength}:{pad}{secondsLength}";
        }

    }

    private long _progressMS;

    public long ProgressMS
    {
        get { return _progressMS; }
        set
        {
            _progressMS = value;
            NotifyPropertyChanged();
        }
    }

    public Track Track { get; set; }
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void NotifyPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}