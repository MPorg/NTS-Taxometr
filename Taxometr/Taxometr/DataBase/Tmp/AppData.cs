using Plugin.BluetoothClassic.Abstractions;

namespace Taxometr.DataBase.Tmp
{
    public static class AppData
    {
        public static IBluetoothConnection Connection
        {
            get => bluetoothConnection;
            set => bluetoothConnection = value;
        }
        private static IBluetoothConnection bluetoothConnection { get; set; }

        public static IBluetoothAdapter Adapter
        {
            get => bluetoothAdapter;
            set => bluetoothAdapter = value;
        }

        private static IBluetoothAdapter bluetoothAdapter { get; set; }

        public static BluetoothDeviceModel ConnectedDevice
        {
            get => bluetoothDeviceModel;
            set => bluetoothDeviceModel = value;
        }

        private static BluetoothDeviceModel bluetoothDeviceModel { get; set; }
    }
}
