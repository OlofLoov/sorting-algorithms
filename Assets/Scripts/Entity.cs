using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

public class Entity : MonoBehaviour
{ 
    public int value = 0;
    private SpriteRenderer? sprite;
    private Color originalColor;
    private bool isSelected = false;

    private float animationSpeed = 0.250f;
    private float animationDuration = 0.25f;
    private float layoutScaleFactor = 0.75f;

    private Color highlightColor = new Color (45.0f/255.0f, 52.0f/255.0f, 54.0f/255.0f, 1);
    private Color selectColor = new Color (45.0f/255.0f, 102.0f/255.0f, 104.0f/255.0f, 1); 

    Vector3 minScale, targetScale;

    IEnumerator Start() {           
        value = UnityEngine.Random.Range(1, 100);
        animationDuration = UnityEngine.Random.Range(0.08f, 0.22f);

        //entity wrapped in holder object for easy scaling along one thus transform.parent.localScale instead of localScale
        transform.parent.localScale = new Vector3(transform.localScale.x * layoutScaleFactor, transform.parent.localScale.y, transform.localScale.z * layoutScaleFactor); 
        sprite = GetComponent<SpriteRenderer>();
        originalColor = sprite.color;
        minScale = transform.parent.localScale;
        targetScale = new Vector3(transform.localScale.x * layoutScaleFactor, value, transform.localScale.z * layoutScaleFactor); 
        yield return AnimateHeight(minScale, targetScale, animationDuration);        
    }

    IEnumerator AnimateHeight(Vector3 a, Vector3 b, float time) {
        float i = 0.0f;
        float rate = (1.0f / time) * animationSpeed;
        while (i < 1.0f) {
            i += Time.deltaTime * rate;
            transform.parent.localScale = Vector3.Lerp(a, b, i);
            yield return null;
        } 
    }

    public void Highlight() {
        if (isSelected) 
            return;
        

        sprite.color = highlightColor;
    }

    public void ResetHighlight() {
        if (isSelected) 
            return;
        sprite.color = originalColor;
    }

    public void Select() {
        isSelected = true;
        sprite.color = selectColor;
    }

    public void Deselect() {
        isSelected = false;
        sprite.color = originalColor;
    }

    public Vector3 GetPosition() {
        return transform.parent.position;
    }

    public async Task MoveLerpAsync (Vector3 endPosition, float moveSpeed) {
        float time = moveSpeed;
        Vector3 startingPos  = transform.parent.transform.position;
        Vector3 finalPos = endPosition;
        float elapsedTime = 0;

        while (elapsedTime < time) {
            transform.parent.transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            await Task.Yield();
        }

        transform.parent.transform.position = finalPos;
    }
}