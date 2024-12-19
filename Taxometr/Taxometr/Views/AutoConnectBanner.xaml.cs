using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Taxometr.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AutoConnectBanner : ContentPage
    {
        public AutoConnectBanner()
        {
            InitializeComponent();
            Color bg = BackgroundColor;
            BackgroundColor = new Color(bg.R, bg.G, bg.B, 0);
        }
    }
}