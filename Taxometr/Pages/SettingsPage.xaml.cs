using CommunityToolkit.Maui.Core;
using Microsoft.Maui.Controls;
using System.Diagnostics;
using System.Reflection.Metadata;
using Taxometr.DataBase;
using Taxometr.DataBase.Objects;

namespace Taxometr.Pages;

public partial class SettingsPage : ContentPage
{
	private readonly TaxometerLocalDB taxometerLocalDB;

    public SettingsPage(TaxometerLocalDB taxometerLocalDB)
    {
        InitializeComponent();
        this.taxometerLocalDB = taxometerLocalDB;
        LoadChangesAsync();
    }

    private void SetDefaultValues()
    {
		ThemePicker.SelectedIndex = 0;
        SetTheme();
    }

    private void SwitchButtonsTitle()
    {
        Cancel.Text = "Отмена";
        Save.IsEnabled = true;
    }

    private async Task LoadChangesAsync()
	{
        List<Property> properties = await taxometerLocalDB.GetPropertiesAsync();
		if (properties.Count == 0)
        {
            SetDefaultValues();
            await taxometerLocalDB.CreatePropertyAsync(new Property("App theme", ThemePicker.SelectedIndex.ToString()));
		}

        Property appTheme = await taxometerLocalDB.GetPropertyByNameAsync("App theme");
        int.TryParse(appTheme.PropertyValue, out int intValue);
        ThemePicker.SelectedIndex = intValue;

        Cancel.Text = "Назад";
        Save.IsEnabled = false;
    }

    private async Task SaveChangesAsync()
    {
        Property appTheme = await taxometerLocalDB.GetPropertyByNameAsync("App theme");
        appTheme.PropertyValue = ThemePicker.SelectedIndex.ToString();
        await taxometerLocalDB.UpdatePropertyAsync(appTheme);
    }

    private void ThemePicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        SwitchButtonsTitle();
        SetTheme();
    }

	private void SetTheme()
	{
        Picker picker = ThemePicker;
        int i = picker.SelectedIndex;
#pragma warning disable CS8602 // Разыменование вероятной пустой ссылки.
#pragma warning disable CA1416 // Проверка совместимости платформы
        switch (i)
        {
            case 0: Application.Current.UserAppTheme = AppTheme.Unspecified; break;
            case 1:
                Application.Current.UserAppTheme = AppTheme.Light;
                CommunityToolkit.Maui.Core.Platform.StatusBar.SetStyle(StatusBarStyle.LightContent);
                break;
            case 2:
                Application.Current.UserAppTheme = AppTheme.Dark;
                CommunityToolkit.Maui.Core.Platform.StatusBar.SetStyle(StatusBarStyle.DarkContent);
                break;
        }
#pragma warning restore CS8602 // Разыменование вероятной пустой ссылки.
#pragma warning restore CA1416 // Проверка совместимости платформы
    }

    private async void Clancel_Clicked(object sender, EventArgs e)
    {
        await LoadChangesAsync();
        await Navigation.PopModalAsync();
    }

    private async void Save_Clicked(object sender, EventArgs e)
    {
        await SaveChangesAsync();
        await Navigation.PopModalAsync();
    }
}