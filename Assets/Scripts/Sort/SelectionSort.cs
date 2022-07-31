
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;
using Cysharp.Threading.Tasks;

class SelectionSort: ISortStrategy {
    public event EventHandler<ISortStrategyEventArgs> Sorted;

    public void Sort(GameObject[] entities, SortVisualizationConfig config) {
        SelectionSortAsync(entities, config);
    }

    private async void SelectionSortAsync(GameObject[] entities, SortVisualizationConfig config) {
        for (int j = 0; j < entities.Length - 1; j++) {
            int iMin = j;

            var jEntity = entities[j].transform.GetChild(0).GetComponent<Entity>();
            jEntity.Select();

            for (int i = j + 1; i < entities.Length; i++) {
                var leftEntity = entities[iMin].transform.GetChild(0).GetComponent<Entity>();
                var rightEntity = entities[i].transform.GetChild(0).GetComponent<Entity>();

                leftEntity.Highlight();
                rightEntity.Highlight();
                
                await UniTask.Delay(config.PauseDurationMs);

                if (rightEntity.value < leftEntity.value) 
                    iMin = i;                
                leftEntity.ResetHighlight();
                rightEntity.ResetHighlight();
            }

            if (iMin != j) {        
                var minEntity = entities[iMin].transform.GetChild(0).GetComponent<Entity>();
                var currentEntity = entities[j].transform.GetChild(0).GetComponent<Entity>();
                minEntity.Highlight();
                currentEntity.Highlight();

                var minEntityPosition = minEntity.GetPosition();
                var currentEntityPosition = currentEntity.GetPosition();

                await minEntity.MoveLerpAsync(currentEntityPosition, config.MoveDuration);
                await currentEntity.MoveLerpAsync(minEntityPosition, config.MoveDuration);

                GameObject temp = entities[iMin];
                entities[iMin] = entities[j];
                entities[j] = temp;

                minEntity.ResetHighlight();
                currentEntity.ResetHighlight();
            }
            jEntity.Deselect();
        }

        Sorted?.Invoke(this, new ISortStrategyEventArgs());      
    }
}