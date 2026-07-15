namespace CalculatorLibrary;

using System.Collections.Generic;

internal class History
{
    private const int HistorySize = 6;
    public int TotalOperationsPerformed { get; set; }
    public List<Operation> LatestOperations { get; } = [];
    
    internal void Update(Operation operation)
    {
        TotalOperationsPerformed++;
        AddOperation(operation);
    }

    private void AddOperation(Operation operation)
    {
        LatestOperations.Insert(0, operation);
        
        if (LatestOperations.Count > HistorySize)
        {
            LatestOperations.RemoveAt(HistorySize);
        }
    }
}