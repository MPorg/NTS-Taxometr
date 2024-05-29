using Microsoft.Extensions.DependencyInjection;

namespace Taxometr
{
    public partial class App : Application
    {
        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            var mainPage = serviceProvider.GetRequiredService<MainPage>();
            NavigationPage navigationPage = new NavigationPage(mainPage);
            this.MainPage = navigationPage;
        }
    }
}
