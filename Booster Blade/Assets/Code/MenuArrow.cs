using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuArrow : MonoBehaviour
{
    public float xOffset = 0;
    public void PlaceArrow(RectTransform t)
    {
        Vector2 pos = t.position;
        pos.x -= (t.rect.width / 2f ) + xOffset;
        transform.position = pos;
    }
}
