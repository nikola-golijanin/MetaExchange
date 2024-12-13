namespace MetaExchange.Shared.Model;

public class Exchange
{
    public DateTime AcqTime { get; set; }
    
    //For the sake of simplicity ExchangeName is used as some exchange unique identifier
    public string ExchangeName { get; set; }
    public List<Bid> Bids { get; set; }
    public List<Ask> Asks { get; set; }
    public EurBtcBalance EurBtcBalance { get; set; }
}
