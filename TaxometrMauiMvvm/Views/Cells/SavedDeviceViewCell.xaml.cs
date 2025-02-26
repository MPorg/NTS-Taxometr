using System.Diagnostics;
using TaxometrMauiMvvm.Models.Cells;

namespace TaxometrMauiMvvm.Views.Cells;

public partial class SavedDeviceViewCell : Grid
{
    public SavedDeviceViewCell()
	{
		InitializeComponent();
    }

    private void TouchBehavior_LongPressCompleted(object sender, CommunityToolkit.Maui.Core.LongPressCompletedEventArgs e)
    {
        if (BindingContext is SavedDeviceViewModel viewModel)
        {
            viewModel.LongPressedCommand.Execute(this);
        }
    }
}