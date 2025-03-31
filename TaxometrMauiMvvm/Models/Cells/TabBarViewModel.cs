using Android.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaxometrMauiMvvm.Data;

namespace TaxometrMauiMvvm.Models.Cells;

public partial class TabBarViewModel : ObservableObject
{
    [ObservableProperty]
    private Color _remoteBtnColor;
    [ObservableProperty]
    private Color _driveBtnColor;
    [ObservableProperty]
    private Color _printBtnColor;

    private Color _selectedColor;
    private Color _deselectedColor;

    public TabBarViewModel()
    {
        AppData.TabBarViewModel = this;

        GetColors();

        RemoteBtnColor = _selectedColor;
        DriveBtnColor = _deselectedColor;
        PrintBtnColor = _deselectedColor;
    }

    private void GetColors()
    {

        if (Application.Current.Resources.TryGetValue("TextLink", out var color))
        {
            if (color is Color c) _selectedColor = c;
        }

        if (Application.Current.PlatformAppTheme == AppTheme.Dark)
        {
            if (Application.Current.Resources.TryGetValue("TextDark", out var color1))
            {
                if (color1 is Color c) _deselectedColor = c;
            }
        }
        else
        {
            if (Application.Current.Resources.TryGetValue("TextDark", out var color1))
            {
                if (color1 is Color c) _deselectedColor = c;
            }
        }
    }

    [RelayCommand]
    private void OpenRemote()
    {
        AppData.MainMenu.GoTo("//Remote");
    }
    [RelayCommand]
    private void OpenDrive()
    {
        AppData.MainMenu.GoTo("//Drive");
    }
    [RelayCommand]
    private void OpenPrint()
    {
        AppData.MainMenu.GoTo("//Print");
    }

    public enum Transition
    {
        None,
        Remote,
        Drive,
        Print
    }

    public event Action<bool> OnRemoteBtnChanged;
    public event Action<bool> OnDriveBtnChanged;
    public event Action<bool> OnPrintBtnChanged;

    public void Transit(Transition from = Transition.None, Transition to = Transition.None)
    {
        if (from == to) return;

        switch (from)
        {
            case Transition.Remote:
                OnRemoteBtnChanged(false);
                RemoteBtnColor = _deselectedColor;
                break;
            case Transition.Drive:
                OnDriveBtnChanged(false);
                DriveBtnColor = _deselectedColor;
                break;
            case Transition.Print:
                OnPrintBtnChanged(false);
                PrintBtnColor = _deselectedColor;
                break;
        }

        switch (to)
        {
            case Transition.Remote:
                OnRemoteBtnChanged(true);
                RemoteBtnColor = _selectedColor;
                break;
            case Transition.Drive:
                OnDriveBtnChanged(true);
                DriveBtnColor = _selectedColor;
                break;
            case Transition.Print:
                OnPrintBtnChanged(true);
                PrintBtnColor = _selectedColor;
                break;
        }
    }
}
