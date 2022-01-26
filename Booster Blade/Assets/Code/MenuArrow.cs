using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuArrow : MonoBehaviour
{
    private RectTransform rectTransform;
    [SerializeField]
    private RectTransform levelCanvas;
    public float xOffset = 0;
    //set recttransform instead?
    private void Awake()
    {
        
        rectTransform = GetComponent<RectTransform>();
        rectTransform.SetParent(levelCanvas, false);
        rectTransform.anchoredPosition = Vector3.zero;
        Debug.Log("AnchoredPosition =" + rectTransform.anchoredPosition);
    }
    public void PlaceArrow(RectTransform t)
    {
        
        Vector3 pos = t.anchoredPosition;
        pos.x -= (t.rect.width / 2f ) + xOffset;
        //pos.z = 0;
        //Debug.Log("setting arrow to position " + pos);
        rectTransform.anchoredPosition = pos;
        //rectTransform.anchoredPosition = new Vector3(0, 0, 0);
        Debug.Log("AnchoredPosition =" + rectTransform.anchoredPosition);
    }
}
