using System;
using MetaExchange.Shared.Model;

namespace MetaExchange.Shared.Services;

public interface IMetaExchangeCalculator
{
    IEnumerable<ExecutionPlan> GetBestExecutionPlanOrderByExchange(OrderType orderType, double amount);
    IEnumerable<Order> GetBestExecutionPlan(OrderType orderType, double amount);
}
