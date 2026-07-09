using Newtonsoft.Json.Converters;

namespace CalculatorLibrary;

internal class Operation
{
    public double Operand1 { get; init; }
    
    public double Operand2 { get; init; }
    
    [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))] 
    public OperationType OperationType { get; init; }
    
    public double Result { get; init; }
}

internal enum OperationType { Addition, Subtraction, Multiplication, Division }