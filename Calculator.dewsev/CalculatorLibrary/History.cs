namespace CalculatorLibrary;

using System;
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
        if (LatestOperations.Count < HistorySize)
        {
            LatestOperations.Add(operation);
        }
        else
        {
            LatestOperations[HistorySize - 1] = operation;        
        }
        
        LatestOperations.Sort((a, b) => DateTime.Compare(b.CreatedAt, a.CreatedAt));
    }
}