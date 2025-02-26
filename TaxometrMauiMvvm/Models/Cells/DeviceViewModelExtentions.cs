using Plugin.BLE.Abstractions;
namespace TaxometrMauiMvvm.Models.Cells;

internal class DeviceViewModelExtentions
{
    internal static readonly Dictionary<string, string> ConnectionStateRU = new Dictionary<string, string>()
    {
        { "Disconnected", "Не подключено" },
        { "Connecting", "Подключение" },
        { "Connected", "Подключено" },
        { "Limited", "Занято" }
    };
}
