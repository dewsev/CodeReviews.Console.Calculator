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
        
        LatestOperations.Insert(0, operation);
        
        if (LatestOperations.Count > HistorySize)
        {
            LatestOperations.RemoveAt(HistorySize);
        }
    }

    public void Clear()
    {
        TotalOperationsPerformed = 0;
        LatestOperations.Clear();
    }
}