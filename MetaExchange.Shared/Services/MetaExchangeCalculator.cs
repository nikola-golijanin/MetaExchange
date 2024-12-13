using MetaExchange.Shared.Data;
using MetaExchange.Shared.Model;

namespace MetaExchange.Shared.Services;

public class MetaExchangeCalculator : IMetaExchangeCalculator
{
    private readonly IEnumerable<Exchange> _exchanges;
    public MetaExchangeCalculator(IExchangeRepository exchangeRepository)
    {
        _exchanges = exchangeRepository.GetExchanges();
    }
    public IEnumerable<ExecutionPlan> GetBestExecutionPlanOrderByExchange(OrderType orderType, double amount)
    {
        var orders = GetBestExecutionPlan(orderType, amount);
        var executionPlanOrderByExchange = orders.GroupBy(o => o.ExchangeName)
                          .Select(grouping => new ExecutionPlan
                          {
                              ExchangeName = grouping.Key,
                              Orders = [.. grouping]
                          });
        return executionPlanOrderByExchange;
    }

    public IEnumerable<Order> GetBestExecutionPlan(OrderType orderType, double amount) =>
        orderType switch
        {
            OrderType.Buy => CalculateBestAsks(_exchanges, amount),
            OrderType.Sell => CalculateBestBids(_exchanges, amount),
            _ => throw new ArgumentException("Invalid order type")
        };
    
    // TODO: Question for interviewer: What if the amount we want to buy/sell is bigger than the current Exchange Balance?
    // Should we handle error in a manner of an unsuccessfully response or return maximum that is allowed by balance? 
    private static IEnumerable<Order> CalculateBestAsks(IEnumerable<Exchange> orderBooks, double amount)
    {
        var result = new List<Order>();
        var askQueue = new PriorityQueue<(Exchange orderBook, Order order), double>();

        foreach (var exchange in orderBooks)
            foreach (var ask in exchange.Asks.Select(a => a.Order))
                askQueue.Enqueue((exchange, ask), ask.Price);

        while (amount > 0 && askQueue.Count > 0)
        {
            var (orderBook, ask) = askQueue.Dequeue();
            double availableAmount = Math.Min(ask.Amount, amount);
            double cost = availableAmount * ask.Price;

            if (orderBook.EurBtcBalance.EUR >= cost)
            {
                result.Add(new Order { Type = OrderType.Buy, Amount = availableAmount, Price = ask.Price, ExchangeName = orderBook.ExchangeName });
                orderBook.EurBtcBalance.EUR -= cost;
                amount -= availableAmount;
            }
        }
        return result;
    }

    private static IEnumerable<Order> CalculateBestBids(IEnumerable<Exchange> orderBooks, double amount)
    {
        var result = new List<Order>();
        var bidQueue = new PriorityQueue<(Exchange orderBook, Order order), double>(Comparer<double>.Create((x, y) => y.CompareTo(x)));

        foreach (var orderBook in orderBooks)
            foreach (var bid in orderBook.Bids.Select(b => b.Order))
                bidQueue.Enqueue((orderBook, bid), bid.Price);

        while (amount > 0 && bidQueue.Count > 0)
        {
            var (orderBook, bid) = bidQueue.Dequeue();
            double availableAmount = Math.Min(bid.Amount, amount);

            if (orderBook.EurBtcBalance.BTC >= availableAmount)
            {
                result.Add(new Order { Type = OrderType.Sell, Amount = availableAmount, Price = bid.Price, ExchangeName = orderBook.ExchangeName });
                orderBook.EurBtcBalance.BTC -= availableAmount;
                amount -= availableAmount;
            }
        }
        return result;
    }


}
