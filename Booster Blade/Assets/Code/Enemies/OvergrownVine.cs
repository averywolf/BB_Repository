using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvergrownVine : MonoBehaviour
{
    [SerializeField]
    PlantWeakpoint weakPoint;

    [SerializeField]
    Transform pointA;

    [SerializeField]
    Transform pointZ;

    [SerializeField]
    LineRenderer vineLineA;
    [SerializeField]
    LineRenderer vineLineZ;

    [SerializeField]
    private GameObject vineIdleFX;

    public bool isVineCut=false;

    private void Awake()
    {
        isVineCut = false;
        vineLineA.enabled = true;
        vineLineA.useWorldSpace = true;
        vineLineZ.enabled = true;
        vineLineZ.useWorldSpace = true;
    }

    // Update is called once per frame
    void Update()
    {
        DrawVines();
    }
    public void DrawVines()
    {
        vineLineA.SetPosition(0, pointA.position);
        vineLineA.SetPosition(1, weakPoint.transform.position);
        vineLineZ.SetPosition(0, pointZ.position);
        vineLineZ.SetPosition(1, weakPoint.transform.position);
    }
    private float GetPerpAngle(Vector2 p1, Vector2 p2)
    {
        return (Mathf.Atan2(p2.y - p1.y, p2.x - p1.x) * 180 / Mathf.PI) + 90f;
    }
    public void CutVine()
    {
        gameObject.SetActive(false);
        isVineCut = true;
    }

}
