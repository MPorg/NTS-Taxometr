namespace TaxometrMauiMvvm.Interfaces
{
    public interface ISettingsManager
    {
        bool BluetoothIsEnable();
        bool LockationIsEnable();

        void ShowLocationSettings();
        void ShowBluetoothSettings();
        void ShowAppSettings();
    }
}
