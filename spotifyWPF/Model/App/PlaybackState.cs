using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
using spotifyWPF.Model.Player;

namespace spotifyWPF.Model.App;

public enum RepeatState
{
    off,
    context,
    track
}
public enum ContextType 
{
   none, 
   playlist,
   artist,
   album
}

public class PlaybackState : INotifyPropertyChanged
{
    private DispatcherTimer _playbackTick { get; set; }
    public EventHandler OnSongEnd;
    public PlaybackState()
    {
        _playbackTick = new DispatcherTimer();
        _playbackTick.Tick += PlaybackTickOnTick; 
        _playbackTick.Interval = new TimeSpan(0, 0, 1); 
    }

    private void PlaybackTickOnTick(object? sender, EventArgs e)
    {
        ProgressMS += 1000;
        if (_progressMS >= Track.DurationMS)
        {
            _playbackTick.Stop();
           OnSongEnd?.Invoke(this, EventArgs.Empty);

        }
    }

    public Device Device { get; set; }

    private bool _isPlaying;

    public bool IsPlaying
    {
        get { return _isPlaying; }
        set
        {
            _isPlaying = value;
            NotifyPropertyChanged();
            if (_isPlaying)
            {
                _playbackTick.Start();
            }
            else
            {
                _playbackTick.Stop();
            }
        }
    }

    private ContextType _contextType;

    public ContextType ContextType
    {
        get { return _contextType; }
        set { _contextType = value; NotifyPropertyChanged(); }
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
            NotifyPropertyChanged(nameof(Progress));
        }
    }

    private Track _track;

    public Track Track
    {
        get { return _track; }
        set { _track = value;NotifyPropertyChanged(); }
    }
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void NotifyPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}