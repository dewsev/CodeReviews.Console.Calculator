using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CalculatorLibrary;

public class Operation
{
    public double Operand1 { get; init; }
    public double Operand2 { get; init; }
    [JsonConverter(typeof(StringEnumConverter))]
    public OperationType OperationType { get; init; }
    public double Result { get; init; }
}
