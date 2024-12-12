namespace MetaExchange.Shared.Model;

public class Order
{
    public DateTime Time { get; set; }
    public OrderType Type { get; set; }
    public double Amount { get; set; }
    public double Price { get; set; }
    public string ExchangeName { get; set; }
}
