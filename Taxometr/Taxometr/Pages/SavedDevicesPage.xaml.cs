using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private DevicesTabbedPage _deviceTabbedPage;

        public SavedDevicesPage(DevicesTabbedPage tabbedPage)
        {
            InitializeComponent();
            Prefabs.CollectionChanged += OnPrefabs_CollectionChanged;
            AppData.BLEAdapter.DeviceConnected += OnBLEAdapter_DeviceConnected; ;
            AppData.BLEAdapter.DeviceDisconnected += OnBLEAdapter_DeviceDisconnected;
        }

        private void OnBLEAdapter_DeviceConnected(object sender, Plugin.BLE.Abstractions.EventArgs.DeviceEventArgs e)
        {
            AppData.Initialize();
        }

        private void OnBLEAdapter_DeviceDisconnected(object sender, Plugin.BLE.Abstractions.EventArgs.DeviceEventArgs e)
        {
            //
        }

        public async Task<int> Initialize()
        {
            Prefabs = new ObservableCollection<DevicePrefab>();
            _bindings.Clear();
            ListOfDevices.ItemsSource = _bindings;
            return await FillDevices();
        }

        private async Task<int> FillDevices()
        {
            Prefabs = new ObservableCollection<DevicePrefab>();
            List<DevicePrefab> list = new List<DevicePrefab>();


            list.AddRange(await AppData.TaxometrDB.DevicePrefabs.GetPrefabsAsync());
            try
            {
                if (list != null && list.Count > 0)
                {
                    Prefabs = new ObservableCollection<DevicePrefab>(list);
                    FillViews();
                    return Prefabs.Count;
                }
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("Exception");
                return 0;
            }
            return 0;
        }

        private async void OnPrefabs_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            await Initialize();
        }

        private void FillViews()
        {
            AppData.Debug.WriteLine("FillViews");
            _bindings.Clear();
            if (Prefabs != null && Prefabs.Count > 0)
            {
                foreach (var p in Prefabs)
                {
                    AppData.Debug.WriteLine($"{p.DeviceId} {p.CustomName} {p.SerialNumber} {p.BLEPassword} {p.UserPassword}");
                    try
                    {
                        var binding = new DeviceViewCell.DeviceViewCellBinding(p.DeviceId, p.SerialNumber, p.BLEPassword, p.UserPassword, p.CustomName, p.AutoConnect);
                        _bindings.Add(binding);
                        binding.AutoconnectionChange += OnBinding_AutoconnectionChange;
                        binding.Deleted += Binding_Deleted;
                        binding.Updated += Binding_Updated;
                    }
                    catch (Exception ex)
                    {
                        AppData.Debug.WriteLine($"{ex.Message} {ex.Source}");
                    }
                }
            }
        }

        private async void Binding_Updated(DeviceViewCell.DeviceViewCellBinding obj)
        {
            await Initialize();
        }

        private async void Binding_Deleted(DeviceViewCell.DeviceViewCellBinding binding)
        {
            _bindings.Remove(binding);
            var device = await AppData.TaxometrDB.DevicePrefabs.GetByIdAsync(binding.Device.Id);
            Guid delDevId = device.DeviceId;
            await AppData.TaxometrDB.DevicePrefabs.DeleteAsync(device);
            await Task.Delay(200);
            if (delDevId == AppData.AutoconnectDeviceID)
            {
                AppData.ClearAutoconnectDevice();
            }
            await AppData.SpecialDisconnect();
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