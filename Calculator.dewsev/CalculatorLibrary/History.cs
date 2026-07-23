using Newtonsoft.Json;

namespace CalculatorLibrary;

using System.Collections.Generic;

internal class History
{
    internal const int DefaultSize = 6;
    
    [JsonProperty]
    internal int Size { get; private set; } = DefaultSize;
    [JsonProperty]
    internal int TotalOperationsPerformed { get; private set; }
    [JsonProperty]
    internal List<Operation> LatestOperations { get; } = [];
    
    internal void Update(Operation operation)
    {
        if (Size == 0)
        {
            return;
        }
        
        TotalOperationsPerformed++;
        
        LatestOperations.Insert(0, operation);
        
        if (LatestOperations.Count > Size)
        {
            LatestOperations.RemoveAt(Size);
        }
    }
    
    internal void SetSize(int newSize)
    {
        Size = newSize;
    }

    internal void ClearOperationHistory()
    {
        LatestOperations.Clear();
    }
    
    internal void ResetTotalOperationsPerformed()
    {
        TotalOperationsPerformed = 0;

    }
    
    internal void ClearAllData()
    {
        ResetTotalOperationsPerformed();
        ClearOperationHistory();
    }
}