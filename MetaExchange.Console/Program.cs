using System.Globalization;
using MetaExchange.Shared.Data;
using MetaExchange.Shared.Model;
using MetaExchange.Shared.Services;

IExchangeRepository exchangeRepository = new ExchangeRepository();
IMetaExchangeCalculator metaExchangeCalculator = new MetaExchangeCalculator(exchangeRepository);


Console.WriteLine("Welcome to MetaExchange!");
Console.WriteLine("Please choose an option:");
Console.WriteLine("1. Buy BTC");
Console.WriteLine("2. Sell BTC");
Console.WriteLine("3. Exit");

var choice = Console.ReadLine();

if (choice == "3")
    return;


if (choice is not "1" && choice is not "2")
{
    Console.WriteLine("Invalid choice. Please enter 1, 2, or 3.");
    return;
}

Console.Write("Enter the amount of BTC: ");
if (!double.TryParse(Console.ReadLine(), NumberStyles.Any, CultureInfo.InvariantCulture, out double amount) || amount <= 0)
{
    Console.WriteLine("Invalid amount. Please enter a positive number.");
    return;
}

OrderType orderType = choice == "1" ? OrderType.Buy : OrderType.Sell;

var orders = orderType switch
{
    OrderType.Buy => metaExchangeCalculator.GetBestExecutionPlan(OrderType.Buy, amount),
    OrderType.Sell => metaExchangeCalculator.GetBestExecutionPlan(OrderType.Sell, amount),
    _ => throw new InvalidOperationException("Invalid choice")
};

Console.WriteLine("----------------------------------- Orders -----------------------------------");
foreach (var order in orders)
{
    Console.WriteLine($"Type: {order.Type}, Amount: {order.Amount}, Price: {order.Price}€, From exchagne: {order.ExchangeName}\n");
}
Console.WriteLine($"Total amount of BTC: {orders.Sum(o => o.Amount)}, Total price in EUR: {orders.Sum(o => o.Amount * o.Price)}€");
Console.WriteLine("-------------------------------------------------------------------------------");


