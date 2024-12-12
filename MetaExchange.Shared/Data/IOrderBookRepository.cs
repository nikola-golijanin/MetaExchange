using MetaExchange.Shared.Model;

namespace MetaExchange.Shared.Data;

public interface IOrderBookRepository
{
    IEnumerable<Exchange> GetOrderBooks();
}
