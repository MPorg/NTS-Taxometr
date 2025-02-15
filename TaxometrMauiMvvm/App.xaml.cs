using TaxometrMauiMvvm.Data;
using TaxometrMauiMvvm.Interfaces;

namespace TaxometrMauiMvvm
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            MainMenu menu = new MainMenu();
            AppData.MainMenu = menu;
            return new Window(menu);
        }
    }
}