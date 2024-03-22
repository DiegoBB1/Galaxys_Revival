using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    private static Tooltip instance;
    public TextMeshProUGUI tooltipText;
    public RectTransform tooltipBackground;

    public Camera canvasCamera;
    public static bool hidden = true;
    private void Awake(){
        // toolTipText
        // showTooltip("Testing String");
        Debug.Log("Awake. Instance Initialized");
        instance = this;
    }

    public void Update(){
        Vector2 mousePoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), Input.mousePosition, canvasCamera, out mousePoint);
        transform.localPosition = mousePoint;
        if(hidden){
            gameObject.SetActive(false);
        }
    }

    private void showTooltip(string tipString){
        hidden = false;
        gameObject.SetActive(true);
        tooltipText.text = tipString;
        float padding = 3f;
        Vector2 backgroundSize = new Vector2(tooltipText.preferredWidth + padding * 2, tooltipText.preferredHeight + padding * 2);
        tooltipBackground.sizeDelta = backgroundSize; 

        hideTooltip();
    }

    private void hideTooltip(){
        StartCoroutine(fadeRoutine());

        IEnumerator fadeRoutine(){
            yield return new WaitForSeconds(3);    
            gameObject.SetActive(false);
            hidden = true;
        }
    }

    public static void showTooltip_Static(string tipString){
        instance.showTooltip(tipString);
    }

    // public static void hideTooltip_Static(){
    //     instance.hideTooltip();
    // }

}
