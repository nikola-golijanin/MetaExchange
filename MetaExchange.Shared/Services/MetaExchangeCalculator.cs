using MetaExchange.Shared.Data;
using MetaExchange.Shared.Model;

namespace MetaExchange.Shared.Services;

public class MetaExchangeCalculator : IMetaExchangeCalculator
{
    private readonly IEnumerable<OrderBook> _orderBooks;
    public MetaExchangeCalculator(IOrderBookRepository orderBookRepository)
    {
        _orderBooks = orderBookRepository.GetOrderBooks();
    }
    public IEnumerable<Order> GetBestAsks(double amount) => GetBestOrders(_orderBooks, "buy", amount);

    public IEnumerable<Order> GetBestBids(double amount) => GetBestOrders(_orderBooks, "sell", amount);

    private static List<Order> GetBestOrders(IEnumerable<OrderBook> orderBooks, string orderType, double amount)
    {
        var result = new List<Order>();

        if (orderType == "buy")
        {
            var askQueue = new PriorityQueue<(OrderBook orderBook, Order order), double>();
            foreach (var orderBook in orderBooks)
            {
                foreach (var ask in orderBook.Asks.Select(a => a.Order))
                {
                    askQueue.Enqueue((orderBook, ask), ask.Price);
                }
            }

            while (amount > 0 && askQueue.Count > 0)
            {
                var (orderBook, ask) = askQueue.Dequeue();
                double availableAmount = Math.Min(ask.Amount, amount);
                double cost = availableAmount * ask.Price;

                if (orderBook.EurBtcBalance.EUR >= cost)
                {
                    result.Add(new Order { Type = "buy", Amount = availableAmount, Price = ask.Price });
                    orderBook.EurBtcBalance.EUR -= cost;
                    amount -= availableAmount;
                }
            }
        }
        else if (orderType == "sell")
        {
            var bidQueue = new PriorityQueue<(OrderBook orderBook, Order order), double>(Comparer<double>.Create((x, y) => y.CompareTo(x)));
            foreach (var orderBook in orderBooks)
            {
                foreach (var bid in orderBook.Bids.Select(b => b.Order))
                {
                    bidQueue.Enqueue((orderBook, bid), bid.Price);
                }
            }

            while (amount > 0 && bidQueue.Count > 0)
            {
                var (orderBook, bid) = bidQueue.Dequeue();
                double availableAmount = Math.Min(bid.Amount, amount);

                if (orderBook.EurBtcBalance.BTC >= availableAmount)
                {
                    result.Add(new Order { Type = "sell", Amount = availableAmount, Price = bid.Price });
                    orderBook.EurBtcBalance.BTC -= availableAmount;
                    amount -= availableAmount;
                }
            }
        }

        return result;
    }
}