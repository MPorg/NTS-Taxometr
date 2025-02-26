using SQLite;
using System;

namespace TaxometrMauiMvvm.Data.DataBase.Objects
{
    [Table("Prfabs")]
    public class DevicePrefab
    {
        [PrimaryKey, AutoIncrement, Column("ID")]
        public int Id { get; set; }
        [Column("Device ID")]
        public Guid DeviceId { get; set; }
        [Column("Custom name")]
        public string CustomName { get; set; }
        [Column("BLE Password")]
        public string BLEPassword { get; set; }
        [Column("Serial number")]
        public string SerialNumber { get; set; }
        [Column("User Password")]
        public string UserPassword { get; set; }
        [Column("Automatic connection")]
        public bool AutoConnect { get; set; }

        public DevicePrefab() { }

        public DevicePrefab(Guid deviceId, string serialNumber, string bLEPassword, string userPassword, string customName = "", bool autoConnect = false)
        {
            DeviceId = deviceId;
            SerialNumber = serialNumber;
            BLEPassword = bLEPassword;
            UserPassword = userPassword;
            CustomName = customName;
            AutoConnect = autoConnect;
        }

        public DevicePrefab(DeviceModel device, string serialNumber, string bLEPassword, string userPassword, string customName = "", bool autoConnect = false)
        {
            DeviceId = device.DeviceId;
            SerialNumber = serialNumber;
            BLEPassword = bLEPassword;
            UserPassword = userPassword;
            CustomName = customName;
        }
    }
}
