using MetaExchange.Shared.Data;
using MetaExchange.Shared.Model;
using MetaExchange.Shared.Services;
using Moq;

namespace MetaExchange.Tests;

public class MetaExchangeCalculatorTests
{
    [Fact]
    public void GetBestExecutionPlanOrderByExchange_ReturnsCorrectExecutionPlan()
    {
        //Arrange
        var mockRepository = new Mock<IExchangeRepository>();
        mockRepository.Setup(repo => repo.GetExchanges()).Returns(GetSampleExchanges());
        var calculator = new MetaExchangeCalculator(mockRepository.Object);

        //Act
        var result = calculator.GetBestExecutionPlanOrderByExchange(OrderType.Buy, 10)
            .ToList();

        //Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }
    
    [Fact]
    public void GetBestExecutionPlanOrderByExchange_ReturnsCorrectExecutionPlan_BalanceForeachExchangeIsNotExceeded()
    {
        //Arrange
        var mockRepository = new Mock<IExchangeRepository>();
        mockRepository.Setup(repo => repo.GetExchanges()).Returns(GetSampleExchanges());
        var calculator = new MetaExchangeCalculator(mockRepository.Object);

        //Act
        var result = calculator.GetBestExecutionPlanOrderByExchange(OrderType.Buy, 10);

        //Assert
        Assert.NotNull(result);
        var executionPlans = result.ToList();
        executionPlans.Zip(
            GetSampleExchanges()
                .OrderBy(e=>e.ExchangeName), (executionPlan, exchange) => (executionPlan, exchange))
                .ToList()
                .ForEach(pair =>
                {
                    var (executionPlan, exchange) = pair;
                    Assert.Equal(executionPlan.ExchangeName, exchange.ExchangeName);
                    Assert.True(executionPlan.Orders.Sum(o => o.Amount * o.Price) <= exchange.EurBtcBalance.EUR);
                });
    }

    [Fact]
    public void GetBestExecutionPlan_ReturnsCorrectOrdersForSell()
    {
        var mockRepository = new Mock<IExchangeRepository>();
        mockRepository.Setup(repo => repo.GetExchanges()).Returns(GetSampleExchanges());
        var calculator = new MetaExchangeCalculator(mockRepository.Object);

        var result = calculator
            .GetBestExecutionPlan(OrderType.Sell, 10)
            .ToList();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal(5, result.First().Amount);
    }

    private IEnumerable<Exchange> GetSampleExchanges()
    {
        return new List<Exchange>
        {
            new Exchange
            {
                ExchangeName = "Exchange1",
                Asks = [new Ask { Order = new Order { Amount = 5, Price = 100 } }],
                Bids = [new Bid { Order = new Order { Amount = 5, Price = 90 } }],
                EurBtcBalance = new EurBtcBalance { EUR = 1000, BTC = 10 }
            },
            new Exchange
            {
                ExchangeName = "Exchange2",
                Asks = [new Ask { Order = new Order { Amount = 5, Price = 110 } }],
                Bids = [new Bid { Order = new Order { Amount = 5, Price = 85 } }],
                EurBtcBalance = new EurBtcBalance { EUR = 1000, BTC = 10 }
            }
        };
    }
}