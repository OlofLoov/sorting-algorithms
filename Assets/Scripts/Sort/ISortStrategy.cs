using UnityEngine;
using System;

interface ISortStrategy {
    void Sort(GameObject[] entities, SortVisualizationConfig config);

    public event EventHandler<ISortStrategyEventArgs> Sorted;
}

public class ISortStrategyEventArgs {}