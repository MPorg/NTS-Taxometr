namespace Taxometr.Bloetooth
{
    public class DeviceModel
    {
        public string Name { get; private set; }
        public Guid Id { get; private set; }

        public bool IsConnectable { get; private set; }

        public DeviceModel(string name, Guid id, bool isConnectable)
        {
            Name = name;
            Id = id;
            IsConnectable = isConnectable;
        }
    }
}
