using System.Runtime.Serialization;

namespace MetaExchange.Shared.Model;

public enum OrderType
{
    [EnumMember(Value = "Buy")]
    Buy,

    [EnumMember(Value = "Sell")]
    Sell
}