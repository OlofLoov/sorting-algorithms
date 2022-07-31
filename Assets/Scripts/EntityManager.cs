using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;
using System.Linq;


public class EntityManager : MonoBehaviour
{
    [SerializeField]
    GameObject EntityPrefab = null;

    [SerializeField]
    int numberOfObjects = 125;

    [SerializeField]
    float MoveDuration = 0.025f;

    [SerializeField]
    int PauseDurationMs = 100;

    GameObject[] entities;

    bool isSorting = false;

    void Start() {           
        GenerateDataSet();
    }

    public void GenerateDataSet() {     
        isSorting = false;      
        entities = new GameObject[numberOfObjects];
        foreach (Transform child in transform) {
            GameObject.Destroy(child.gameObject);
        }

        for (int x = 0; x < numberOfObjects; x++) {
               var go = Instantiate(EntityPrefab, new Vector3(x,transform.position.x,transform.position.z), Quaternion.identity);
               go.transform.parent = transform;
               entities[x] = go;
        }   
    }

    public void StartBubbleSort() {
        StartSort(new BubbleSort());
    }

    public void StartInsertionSort() {
        StartSort(new InsertionSort());
    }

    public void StartSelectionSort() {
        StartSort(new SelectionSort());
    }

    public void StartMergeSort() {
        StartSort(new MergeSort());
    }  

    private void StartSort(ISortStrategy strategy) {
        if (isSorting)
            return;

        var config = new SortVisualizationConfig(MoveDuration, PauseDurationMs);
        strategy.Sort(entities, config);
        isSorting = true;
        strategy.Sorted += (s, e) => {
            isSorting = false;
        };        
    }
}
