// See https://aka.ms/new-console-template for more information
using MetaExchange.Shared.Data;
using MetaExchange.Shared.Services;


double amount = 1.5; // Amount of BTC to buy or sell


var asks = new MetaExchangeCalculator(new OrderBookRepository()).GetBestAsks(amount);
var bids = new MetaExchangeCalculator(new OrderBookRepository()).GetBestBids(amount);

Console.WriteLine("----------------------------------- Asks  -----------------------------------");
foreach (var order in asks)
{
    Console.WriteLine($"Type: {order.Type}, Amount: {order.Amount}, Price: {order.Price}");
}

Console.WriteLine("----------------------------------- Bids  -----------------------------------");
foreach (var order in bids)
{
    Console.WriteLine($"Type: {order.Type}, Amount: {order.Amount}, Price: {order.Price}");
}
