using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CollectIcon : MonoBehaviour
{
    public Image collectImage;
    public List<Sprite> colImg;
    private void Awake()
    {
        collectImage.enabled = false;
    }
    public void PickIcon(int iconID)
    {
        collectImage.sprite = colImg[iconID];
    }
    public void IconOn()
    {
        collectImage.enabled = true;
    }
}
