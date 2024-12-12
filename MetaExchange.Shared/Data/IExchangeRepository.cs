using MetaExchange.Shared.Model;

namespace MetaExchange.Shared.Data;

public interface IExchangeRepository
{
    IEnumerable<Exchange> GetExchanges();
}
