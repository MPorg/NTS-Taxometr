using KotlinX.Serialization;
using TaxometrMauiMvvm.Data;
using TaxometrMauiMvvm.Models.Banners;

namespace TaxometrMauiMvvm.Views.Banners;

public partial class CreateDeviceBanner : ContentPage
{
    CreateDeviceViewModel _viewModel;
    public bool Result;
    public CreateDeviceBanner(CreateDeviceViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
        _viewModel = viewModel;
        VisualStateManager.GoToState(_autoConnectSwitch, viewModel.Autoconnect ? "On" : "Off");
        Loaded += CreateDeviceBanner_Loaded;
        viewModel.Result += OnResult;
    }

    private async void OnResult(bool res)
    {
        Result = res;
        await Navigation.PopModalAsync();
    }

    private async void CreateDeviceBanner_Loaded(object? sender, EventArgs e)
    {
        await Task.Delay(250);
        NameEntry.Focus();
        NameEntry.CursorPosition = 0;
        NameEntry.SelectionLength = NameEntry.Text.Length;
    }

    private void Switch_Toggled(object sender, ToggledEventArgs e)
    {
        VisualStateManager.GoToState(_autoConnectSwitch, _viewModel.Autoconnect ? "On" : "Off");
        if (NameEntry.IsFocused) NameEntry.Unfocus();
        if (SerNumEntry.IsFocused) SerNumEntry.Unfocus();
        if (BLEPassEntry.IsFocused) BLEPassEntry.Unfocus();
        if (AdminPassEntry.IsFocused) AdminPassEntry.Unfocus();
    }

    private void OnSerNumEntryTextChanged(object sender, TextChangedEventArgs e)
    {
        bool retry = false;
        string newValue = e.NewTextValue;
        if (newValue.Length == 8)
        {
            _viewModel.SerialNumber = newValue;
            BLEPassEntry.Focus();
        }
        else if (newValue.Length > 8)
        {
            newValue = newValue.Remove(8);
            retry = true;
        }
        if (!int.TryParse(newValue, out int _))
        {
            for (int i = 0; i < newValue.Length; i++)
            {
                if (!int.TryParse(newValue.ToCharArray()[i].ToString(), out _))
                {
                    retry = true;
                    newValue = newValue.Remove(i, 1);
                    break;
                }
            }
        }
        if (retry)
        {
            _viewModel.SerialNumber = newValue;
        }
    }

    private void OnBLEPassEntryTextChanged(object sender, TextChangedEventArgs e)
    {
        bool retry = false;
        string newValue = e.NewTextValue;
        if (newValue.Length == 6)
        {
            _viewModel.BlePassword = newValue;
            AdminPassEntry.Focus();
        }
        else if (newValue.Length > 6)
        {
            newValue = newValue.Remove(6);
            retry = true;
        }
        if (!int.TryParse(newValue, out int _))
        {
            for (int i = 0; i < newValue.Length; i++)
            {
                if (!int.TryParse(newValue.ToCharArray()[i].ToString(), out _))
                {
                    retry = true;
                    newValue = newValue.Remove(i, 1);
                    break;
                }
            }
        }
        if (retry)
        {
            _viewModel.BlePassword = newValue;
        }
    }

    private void OnAdminPassEntryTextChanged(object sender, TextChangedEventArgs e)
    {
        bool retry = false;
        string newValue = e.NewTextValue;
        if (newValue.Length == 6)
        {
            _viewModel.AdminPassword = newValue;
            AdminPassEntry.Unfocus();
        }
        else if (newValue.Length > 6)
        {
            newValue = newValue.Remove(6);
            retry = true;
        }
        if (!int.TryParse(newValue, out int _))
        {
            for (int i = 0; i < newValue.Length; i++)
            {
                if (!int.TryParse(newValue.ToCharArray()[i].ToString(), out _))
                {
                    retry = true;
                    newValue = newValue.Remove(i, 1);
                    break;
                }
            }
        }
        if (retry)
        {
            _viewModel.AdminPassword = newValue;
        }
    }


    private void NameEntry_Completed(object sender, EventArgs e)
    {
        SerNumEntry.Focus();
    }

    private void SerNumEntry_Completed(object sender, EventArgs e)
    {
        BLEPassEntry.Focus();
    }

    private void BLEPassEntry_Completed(object sender, EventArgs e)
    {
        AdminPassEntry.Focus();
    }

    private void AdminPassEntry_Completed(object sender, EventArgs e)
    {
        AdminPassEntry.Unfocus();
    }

    private void SerNumEntry_Unfocused(object sender, FocusEventArgs e)
    {
        string result = _viewModel.SerialNumber;
        if (!String.IsNullOrEmpty(result))
        {
            if (result.Length < 8)
            {
                string start = "";
                for (int i = 0; i < 8 - result.Length; i++)
                {
                    start += "0";
                }
                result = start + result;
                _viewModel.SerialNumber = result;
            }
        }
    }

    private void BLEPassEntry_Unfocused(object sender, FocusEventArgs e)
    {
        string result = _viewModel.BlePassword;
        if (!String.IsNullOrEmpty(result))
        {
            if (result.Length < 6)
            {
                string end = "";
                for (int i = 0; i < 6 - result.Length; i++)
                {
                    end += "0";
                }
                result = result + end;
                _viewModel.BlePassword = result;
            }
        }
    }

    private void AdminPassEntry_Unfocused(object sender, FocusEventArgs e)
    {
        string result = _viewModel.AdminPassword;
        if (!String.IsNullOrEmpty(result))
        {
            if (result.Length < 6)
            {
                string start = "";
                for (int i = 0; i < 6 - result.Length; i++)
                {
                    start += "0";
                }
                result = start + result;
                _viewModel.AdminPassword = result;
            }
        }
        AppData.HideKeyboard();
    }
    protected override bool OnBackButtonPressed()
    {
        _viewModel.CancelCommand.Execute(null);
        return true;
    }

}