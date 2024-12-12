using System.Text.Json;
using System.Text.Json.Serialization;
using MetaExchange.Shared.Model;

namespace MetaExchange.Shared.Data;

public class OrderBookRepository : IOrderBookRepository
{
    public IEnumerable<OrderBook> GetOrderBooks()
    {
        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string jsonFilePath = Path.Combine(baseDirectory, "Data\\data.json");

        string jsonString = File.ReadAllText(jsonFilePath);
        return JsonSerializer.Deserialize<IEnumerable<OrderBook>>(jsonString, new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() }
        }) ?? [];
    }
}