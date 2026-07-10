using System.Collections.Generic;

namespace CalculatorLibrary;

internal class History
{
    public int TotalOperationsPerformed { get; set; }
    public List<Operation> LatestOperations { get; set; } = [];
}