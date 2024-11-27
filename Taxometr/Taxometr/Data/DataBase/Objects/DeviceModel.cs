using SQLite;
using System;

namespace Taxometr.Data.DataBase.Objects
{
    [Table("Devices")]
    public class DeviceModel
    {
        [PrimaryKey, AutoIncrement, Column("ID")]
        public int Id { get; set; }
        [Column("Name")]
        public string Name { get; set; }
        [Column("UUID")]
        public Guid DeviceId { get; set; }
        [Column("Channel UUID")]
        public Guid ChannelID { get; set; }

        public DeviceModel() { }

        public DeviceModel(string name, Guid deviceId, Guid channelID)
        {
            Name = name;
            DeviceId = deviceId;
            ChannelID = channelID;
        }
    }
}
