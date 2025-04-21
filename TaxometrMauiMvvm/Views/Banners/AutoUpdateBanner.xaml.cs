namespace TaxometrMauiMvvm.Views.Banners;

public partial class AutoUpdateBanner : ContentPage
{
    public event Action<bool> AnswerCompleate;

	public AutoUpdateBanner()
	{
		InitializeComponent();
	}

    private void UpdateButton_Clicked(object sender, EventArgs e)
    {
        AnswerCompleate?.Invoke(true);
    }

    private void CancelButton_Clicked(object sender, EventArgs e)
    {
        AnswerCompleate?.Invoke(false);
    }
}