using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuArrow : MonoBehaviour
{
    private RectTransform rectTransform;
    public float xOffset = 0;
    //set recttransform instead?
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public void PlaceArrow(RectTransform t)
    {
        //Vector3 pos = t.position;
        //pos.x -= (t.rect.width / 2f ) + xOffset;
        //pos.z = 0;
        //Debug.Log("setting arrow to position " + pos);
        //rectTransform.position = pos;
        rectTransform.position = new Vector3(0, 0, 0);
    }
}
