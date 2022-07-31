
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;

class BubbleSort: ISortStrategy {
    public event EventHandler<ISortStrategyEventArgs> Sorted;

    public void Sort(GameObject[] entities, SortVisualizationConfig config) {
        BubbleSortAsync(entities, config);
    }

    private async void BubbleSortAsync(GameObject[] entities, SortVisualizationConfig config) {
        for (int j = 0; j <= entities.Length - 2; j++) {
            for (int i = 0; i <= entities.Length - 2; i++) {
                // extract entity from holder object               
                var entity = entities[i].transform.GetChild(0).GetComponent<Entity>();
                var nextEntity = entities[i + 1].transform.GetChild(0).GetComponent<Entity>();
                if (entity.value > nextEntity.value) {
                    entity.Highlight();
                    nextEntity.Highlight();
                    var nextEntityPosition = nextEntity.GetPosition();
                    var entityPosition = entity.GetPosition();

                    await entity.MoveLerpAsync(nextEntityPosition, config.MoveDuration);
                    await nextEntity.MoveLerpAsync(entityPosition, config.MoveDuration);

                    await Task.Delay(config.PauseDurationMs);

                    GameObject temp = entities[i + 1];
                    entities[i + 1] = entities[i];
                    entities[i] = temp;
                } 
                entity.ResetHighlight();
                nextEntity.ResetHighlight();
            }
        } 

        Sorted?.Invoke(this, new ISortStrategyEventArgs());
    }
}