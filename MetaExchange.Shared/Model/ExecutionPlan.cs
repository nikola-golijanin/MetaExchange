using System;

namespace MetaExchange.Shared.Model;

public class ExecutionPlan
{
    public string ExchangeName { get; set; }
    public IEnumerable<Order> Orders { get; set; }

    public double TotalSumEur { get => Orders.Sum(o => o.Price * o.Amount); }
    public double TotalAmountOfBtc { get => Orders.Sum(o => o.Amount); }
}
