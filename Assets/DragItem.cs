using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class DragItem : MonoBehaviour, IPointerDownHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private RectTransform rect;
    private CanvasGroup canvasGroup;
    public Sprite sprite;
    private Transform itemIcon;
    private Vector2 position;
    public string itemType;
    public void Awake(){
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        itemIcon = transform.Find("ItemIcon");
        sprite = itemIcon.GetComponent<Image>().sprite;
        itemType = itemIcon.tag;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rect.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        rect.anchoredPosition = position;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        position = rect.anchoredPosition;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
