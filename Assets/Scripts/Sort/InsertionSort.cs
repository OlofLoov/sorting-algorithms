
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;
using Cysharp.Threading.Tasks;

class InsertionSort: ISortStrategy {
    public event EventHandler<ISortStrategyEventArgs> Sorted;
    public void Sort(GameObject[] entities, SortVisualizationConfig config) {
        InsertionSortAsync(entities, config);
    }

    private async void InsertionSortAsync(GameObject[] entities, SortVisualizationConfig config) {
        for (int i = 1; i < entities.Length; i++) {
            int j = i;

            while(j > 0 && entities[j-1].transform.GetChild(0).GetComponent<Entity>().value > entities[j].transform.GetChild(0).GetComponent<Entity>().value) {
                var leftEntity = entities[j-1].transform.GetChild(0).GetComponent<Entity>();
                var rightEntity = entities[j].transform.GetChild(0).GetComponent<Entity>();
                var leftEntityPosition = leftEntity.GetPosition();

                leftEntity.Highlight();
                rightEntity.Highlight();
                
                await UniTask.Delay(config.PauseDurationMs);
                await leftEntity.MoveLerpAsync(rightEntity.GetPosition(), config.MoveDuration);
                await rightEntity.MoveLerpAsync(leftEntityPosition, config.MoveDuration);
                GameObject temp = entities[j-1];
                entities[j-1] = entities[j];
                entities[j] = temp;
                j--;
                leftEntity.ResetHighlight();
                rightEntity.ResetHighlight();
            }
        }     
   
        Sorted?.Invoke(this, new ISortStrategyEventArgs());
    }
}