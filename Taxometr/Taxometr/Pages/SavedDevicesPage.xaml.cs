using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Taxometr.Data;
using Taxometr.Data.DataBase.Objects;
using Taxometr.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Taxometr.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SavedDevicesPage : ContentPage, IDisposable
    {
        private readonly ObservableCollection<DeviceViewCell.DeviceViewCellBinding> _bindings = new ObservableCollection<DeviceViewCell.DeviceViewCellBinding>();
        public ObservableCollection<DevicePrefab> Prefabs { get; private set; } = new ObservableCollection<DevicePrefab>();

        public SavedDevicesPage()
        {
            InitializeComponent();
            Prefabs.CollectionChanged += OnPrefabs_CollectionChanged;
        }

        public async Task<int> Initialize()
        {
            ListOfDevices.ItemsSource = _bindings;
            return await FillDevices();
        }

        private async Task<int> FillDevices()
        {
            Prefabs = new ObservableCollection<DevicePrefab>(await AppData.TaxometrDB.DevicePrefabs.GetPrefabsAsync());
            FillViews();
            return Prefabs.Count;
        }

        private void OnPrefabs_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            FillViews();
        }

        private void FillViews()
        {
            AppData.Debug.WriteLine("FillViews");
            _bindings.Clear();
            if (Prefabs.Count > 0)
            {
                foreach (var p in Prefabs)
                {
                    AppData.Debug.WriteLine($"{p.DeviceId} {p.CustomName} {p.SerialNumber} {p.BLEPassword} {p.UserPassword}");
                    try
                    {
                        var binding = new DeviceViewCell.DeviceViewCellBinding(p.DeviceId, p.SerialNumber, p.BLEPassword, p.UserPassword, p.CustomName, p.AutoConnect);
                        _bindings.Add(binding);
                        binding.AutoconnectionChange += OnBinding_AutoconnectionChange;
                    }
                    catch (Exception ex)
                    {
                        AppData.Debug.WriteLine($"{ex.Message} {ex.Source}");
                    }
                }
            }
        }

        private async void OnBinding_AutoconnectionChange(DeviceViewCell.DeviceViewCellBinding binding)
        {
            AppData.AutoconnectDeviceID = binding.DeviceId;
            var pref = await AppData.TaxometrDB.DevicePrefabs.GetByIdAsync(binding.DeviceId);
            if (pref != null)
            {
                pref.AutoConnect = binding.AutoConnect;
                await AppData.TaxometrDB.DevicePrefabs.UpdateAsync(pref);
            }
        }

        private void OnListOfDevicesItemTapped(object sender, ItemTappedEventArgs e)
        {
            var b = e.Item as DeviceViewCell.DeviceViewCellBinding;
            if (b != null)
            {
                b.OpenContextMenu.Execute(b);
            }
        }

        public void Dispose()
        {
            Prefabs.CollectionChanged -= OnPrefabs_CollectionChanged;
        }
    }
}