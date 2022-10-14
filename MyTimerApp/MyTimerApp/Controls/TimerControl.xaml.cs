using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MyTimerApp.Controls;
/// <summary>
/// Interaction logic for TimerControl.xaml
/// </summary>
public partial class TimerControl : UserControl, INotifyPropertyChanged
{
    public string CurrentElapsedTime
    {
        get { return _currentElapsedTime; }
        set {
            _currentElapsedTime = value;
            NotifyPropertyChanged();
        }
    }
    private string _currentElapsedTime = String.Empty;

    private DispatcherTimer _dispatcherTimer;
    private const int RefreshMillis = 1000;

    private long _ElapsedSeconds = 0;


    public TimerControl()
    {
        InitializeComponent();
        this.DataContext = this;
        _dispatcherTimer = new DispatcherTimer(DispatcherPriority.Background, Application.Current.Dispatcher);
        _dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, RefreshMillis);
        _dispatcherTimer.Tick += _dispatcherTimer_Tick;
        UpdateTimer();
        UpdateButtons();
    }

    private void _dispatcherTimer_Tick( object? sender, EventArgs e )
    {
        _ElapsedSeconds++;
        UpdateTimer();
    }

    private void UpdateTimer()
    {
        TimeSpan time = TimeSpan.FromSeconds(_ElapsedSeconds);
        CurrentElapsedTime = String.Format("{0:D2}:{1:D2}:{2:D2}",
                time.Hours,
                time.Minutes,
                time.Seconds);
    }
    private void UpdateButtons()
    {
        ButtonStart.IsEnabled = (!_dispatcherTimer.IsEnabled);
        ButtonPause.IsEnabled = (_dispatcherTimer.IsEnabled);
        ButtonStop.IsEnabled = (_dispatcherTimer.IsEnabled);
    }

    private void ButtonReset_Click( object sender, RoutedEventArgs e )
    {
        _ElapsedSeconds = 0;
        UpdateTimer();
    }

    private void ButtonStart_Click( object sender, RoutedEventArgs e )
    {
        if (!_dispatcherTimer.IsEnabled)
        {
            _dispatcherTimer.Start();
        }
        UpdateTimer();
        UpdateButtons();
    }

    private void ButtonPause_Click( object sender, RoutedEventArgs e )
    {
        if (_dispatcherTimer.IsEnabled)
        {
            _dispatcherTimer.Stop();
        }
        UpdateTimer();
        UpdateButtons();
    }

    private void ButtonStop_Click( object sender, RoutedEventArgs e )
    {
        if (_dispatcherTimer.IsEnabled)
        {
            _dispatcherTimer.Stop();
        }
        _ElapsedSeconds = 0;
        UpdateTimer();
        UpdateButtons();
    }


    #region INotifyPropertyChanged implementation

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void NotifyPropertyChanged( [CallerMemberName] string? name = null )
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    #endregion
}
