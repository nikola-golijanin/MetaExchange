using System.Text.Json;
using System.Text.Json.Serialization;
using MetaExchange.Shared.Model;

namespace MetaExchange.Shared.Data;

public class ExchangeRepository : IExchangeRepository
{
    public IEnumerable<Exchange> GetExchanges()
    {
        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string jsonFilePath = Path.Combine(baseDirectory, "Data/data.json");

        string jsonString = File.ReadAllText(jsonFilePath);
        return JsonSerializer.Deserialize<IEnumerable<Exchange>>(jsonString, new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters = { new JsonStringEnumConverter() }
        }) ?? [];
    }
}