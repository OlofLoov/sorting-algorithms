using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;
using System.Linq;

class MergeSort: ISortStrategy {
    public event EventHandler<ISortStrategyEventArgs> Sorted;

    public void Sort(GameObject[] entities, SortVisualizationConfig config) {
        StartMergeSortAsync(entities, config);
    }

    private async void StartMergeSortAsync(GameObject[] mergeEntities, SortVisualizationConfig config) {
        var sorted = await MergeSortAsync(mergeEntities, config);
        Sorted?.Invoke(this, new ISortStrategyEventArgs());      
    }

    private async Task<GameObject[]> MergeSortAsync(GameObject[] mergeEntities, SortVisualizationConfig config) {
        GameObject[] leftEntities, rightEntities;
        GameObject[] merged = new GameObject[mergeEntities.Length];  

        if (mergeEntities.Length <= 1) {
            return mergeEntities;              
        }
            
        int middleOfArray = mergeEntities.Length / 2;  
        leftEntities = new GameObject[middleOfArray];

        for (var i = 0; i < mergeEntities.Length; i++) {
            var entity = mergeEntities[i].transform.GetChild(0).GetComponent<Entity>();
            entity.Highlight();
            //await Task.Delay(config.PauseDurationMs);
            entity.ResetHighlight();
        }       

        //if array has an even number of elements, the left and right array will have the same number of 
        //elements            
        if (mergeEntities.Length % 2 == 0) {
            rightEntities = new GameObject[middleOfArray];  
        }
        else {
            rightEntities = new GameObject[middleOfArray + 1];  
        }                
            
        //populate
        for (int i = 0; i < middleOfArray; i++) {
            leftEntities[i] = mergeEntities[i];  
        }
        int j = 0;
        for (int i = middleOfArray; i < mergeEntities.Length; i++) {
            rightEntities[j] = mergeEntities[i];
            j++;
        }  

        //Recursively sort from left
        leftEntities = await MergeSortAsync(leftEntities, config);
        //Recursively sort the right array
        rightEntities = await MergeSortAsync(rightEntities, config);
        merged = await Merge(leftEntities, rightEntities, config);  

        return merged;
    }

    private async Task<GameObject[]> Merge(GameObject[] left, GameObject[] right, SortVisualizationConfig config) {
        int resultLength = right.Length + left.Length;
        GameObject[] result = new GameObject[resultLength];
        int leftIndex = 0;
        int rightIndex = 0;
        int mergedIndex = 0;

        while (leftIndex < left.Length || rightIndex < right.Length) { // as long as some entities left to handle
            if (leftIndex < left.Length && rightIndex < right.Length)   {  
                // if the left is smaller then -> insert left
                if (left[leftIndex].transform.GetChild(0).GetComponent<Entity>().value <= right[rightIndex].transform.GetChild(0).GetComponent<Entity>().value) {
                    result[mergedIndex] = left[leftIndex];
                    leftIndex++;
                    mergedIndex++;
                } else { // right is smaller when comparing -> insert right
                    result[mergedIndex] = right[rightIndex];
                    rightIndex++;
                    mergedIndex++;
                }
            } else if (rightIndex < right.Length) { //if no more left just insert all right into the res array
                result[mergedIndex] = right[rightIndex];
                rightIndex++;
                mergedIndex++;
            } else if (leftIndex < left.Length) { // if no more right just insert all left into the res array
                result[mergedIndex] = left[leftIndex];
                leftIndex++;
                mergedIndex++;
            }  
        }

        await MoveEntities(left, right, result, config);      

        return result;       
    }

    private async Task MoveEntities(GameObject[] left, GameObject[] right, GameObject[] result, SortVisualizationConfig config) {
        GameObject[] combined = left.Concat(right).ToArray();
                for (var i = 0; i < result.Length; i++) {
            // determine how much to move each element relative the entities array
            var indexInJoinedArray = Array.IndexOf(combined, result[i]);
            var pos = result[i].transform.position + new Vector3(i - indexInJoinedArray, 0, 0);
            var entity = result[i].transform.transform.GetChild(0).GetComponent<Entity>();
            await entity.MoveLerpAsync(pos, config.MoveDuration);
        }

    }
}