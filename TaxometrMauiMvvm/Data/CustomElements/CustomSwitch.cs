namespace TaxometrMauiMvvm.Data.CustomElements
{
    public class CustomSwitch : Switch
    {
        public CustomSwitch()
        {
            VisualStateManager.GoToState(this, this.IsToggled ? "On" : "Off");
        }
    }
}
