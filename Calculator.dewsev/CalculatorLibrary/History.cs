namespace CalculatorLibrary;

using System;
using System.Collections.Generic;

internal class History
{
    public int TotalOperationsPerformed { get; private set; }
    public List<Operation> LatestOperations { get; } = [];

    internal void Update(Operation operation)
    {
        TotalOperationsPerformed++;
        LatestOperations.Add(operation);
        LatestOperations.Sort((a, b) => DateTime.Compare(b.CreatedAt, a.CreatedAt));
    }

    public void SaveToJson()
    {
        JsonHelpers.SaveToJsonFile(this, "history.json");
    }
}