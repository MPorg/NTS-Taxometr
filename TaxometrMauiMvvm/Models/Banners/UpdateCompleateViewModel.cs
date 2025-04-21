using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            _version = AppInfo.VersionString;
            FillCheckList();
        }

        private void FillCheckList()
        {
            CheckList = "Добавлена автоматическая проверка обновлений";
        }
    }
}
