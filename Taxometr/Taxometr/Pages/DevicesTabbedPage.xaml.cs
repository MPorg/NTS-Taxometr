using System.Threading.Tasks;
using Taxometr.Fonts.IconFont;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Taxometr.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DevicesTabbedPage : TabbedPage
    {
        public DevicesTabbedPage ()
        {
            InitializeComponent();
        }

        DevicesPage _devicePage;
        SavedDevicesPage _savedPage;

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (_devicePage == null) _devicePage = new DevicesPage();

            if (_savedPage == null) _savedPage = new SavedDevicesPage(this);


            FontImageSource sourceDevices = new FontImageSource();
            sourceDevices.FontFamily = "IconsSolid";
            sourceDevices.Glyph = Icons.IconSearch;
            _devicePage.IconImageSource = sourceDevices;
            _devicePage.Title = "Поиск";

            FontImageSource sourceSaved = new FontImageSource();
            sourceSaved.FontFamily = "IconsSolid";
            sourceSaved.Glyph = Icons.IconStar;
            _savedPage.IconImageSource = sourceSaved;
            _savedPage.Title = "Сохранённые";

            Children.Clear();

            Children.Add(_devicePage);
            Children.Add(_savedPage);

            if (await _savedPage.Initialize() > 0) CurrentPage = _savedPage;
        }

        public async Task LoadSearchAsync()
        {
            _savedPage = new SavedDevicesPage(this);

            FontImageSource sourceSaved = new FontImageSource();
            sourceSaved.FontFamily = "IconsSolid";
            sourceSaved.Glyph = Icons.IconStar;
            _savedPage.IconImageSource = sourceSaved;
            _savedPage.Title = "Сохранённые";

            if (await _savedPage.Initialize() > 0) CurrentPage = _savedPage;
        }
    }
}