using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostPanel : MonoBehaviour
{

    /// <summary>
    /// Currently doesn't work on multiples of these values
    /// </summary>
    /// <returns></returns>
    [HideInInspector]
    public Transform boostPanelTransform;
    private void Awake()
    {
        boostPanelTransform = transform;
    }
}
