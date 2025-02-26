using System.Diagnostics;
using TaxometrMauiMvvm.Models.Cells;

namespace TaxometrMauiMvvm.Data.CustomElements;

public partial class TabBar : Grid
{
	private TabBarViewModel _viewModel;
    public TabBar()
    {
        InitializeComponent();
    }

    public void Initialize()
    {
        InitializeComponent();
    }

    public void Inject(TabBarViewModel viewModel)
    {
        _viewModel = viewModel;
        BindingContext = _viewModel;
        _viewModel.OnRemoteBtnChanged += OnRemoteBtnChanged;
        _viewModel.OnDriveBtnChanged += OnDriveBtnChanged;
        _viewModel.OnPrintBtnChanged += OnPrintBtnChanged;
    }

    private double _toY = 0;
    private double _fromY = 20;
    private double _scaleTo = 1.2d;
    private double _scaleFrom = 1;
    private uint _time = 300;
    private Easing _easing = Easing.SinInOut;


    private async void OnPrintBtnChanged(bool enable)
    {
        switch (enable)
        {
            case true:
                Debug.WriteLine("Transit to print");
                PrinteBtn.TranslateTo(0, _toY, _time, _easing);
                await PrinteBtn.ScaleTo(_scaleTo, _time, _easing);
                break;
            case false:
                Debug.WriteLine("Transit from print");
                PrinteBtn.TranslateTo(0, _fromY, _time, _easing);
                await PrinteBtn.ScaleTo(_scaleFrom, _time, _easing);
                break;
        }
    }

    private async void OnDriveBtnChanged(bool enable)
    {
        switch (enable)
        {
            case true:
                Debug.WriteLine("Transit to drive");
                DriveBtn.TranslateTo(0, _toY, _time, _easing);
                await DriveBtn.ScaleTo(_scaleTo, _time, _easing);
                break;
            case false:
                Debug.WriteLine("Transit from drive");
                DriveBtn.TranslateTo(0, _fromY, _time, _easing);
                await DriveBtn.ScaleTo(_scaleFrom, _time, _easing);
                break;
        }
    }

    private async void OnRemoteBtnChanged(bool enable)
    {
        switch (enable)
        {
            case true:
                Debug.WriteLine("Transit to remote");
                RemoteBtn.TranslateTo(0, _toY, _time, _easing);
                await RemoteBtn.ScaleTo(_scaleTo, _time, _easing);
                break;
            case false:
                Debug.WriteLine("Transit from remote");
                RemoteBtn.TranslateTo(0, _fromY, _time, _easing);
                await RemoteBtn.ScaleTo(_scaleFrom, _time, _easing);
                break;
        }
    }
}