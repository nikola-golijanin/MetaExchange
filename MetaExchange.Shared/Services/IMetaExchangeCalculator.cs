using System;
using MetaExchange.Shared.Model;

namespace MetaExchange.Shared.Services;

public interface IMetaExchangeCalculator
{
    IEnumerable<Order> GetBestAsks(double amount);

    IEnumerable<Order> GetBestBids(double amount);
}
