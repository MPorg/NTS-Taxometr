using CommunityToolkit.Mvvm.ComponentModel;

namespace TaxometrMauiMvvm.Models.Banners
{
    public partial class UpdateCompleateViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _checkList;
        [ObservableProperty]
        private string _version;

        public UpdateCompleateViewModel()
        {
            _version = "Версия: " + AppInfo.VersionString;
            FillCheckList();
        }

        private void FillCheckList()
        {
            CheckList = "Добавлена возможность оплаты картой по NFC-модулю через приложение BPC NFC-POS";
        }
    }
}
