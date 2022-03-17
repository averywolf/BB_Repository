using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTell : MonoBehaviour
{
    LineRenderer lineRenderer;
    Vector2 startPos;
    Vector2 endPos;
    public float transparency=1;
    public bool isTellActive;
    private Color startColor;
    private void Awake()
    {
      
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true; // important I think?
        startColor = lineRenderer.material.color;
    }
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer.material.SetColor("_Color", new Color(startColor.r,  startColor.g, startColor.b, transparency));
    }
    //RaycastHit2D ray = Physics2D.Raycast(transform.position, transform.up, Mathf.Infinity, layerMaskk);
    // Update is called once per frame

    public void ShowLaserTell(bool show)
    {
        lineRenderer.enabled = show;
    }
    //often called in base object Updateloop
    public void ShootLaserTell(Vector2 startPoint, Vector2 endPoint)
    {

        lineRenderer.SetPosition(0, startPoint);
        lineRenderer.SetPosition(1, endPoint);
    }
    public void Flicker()
    {

    }
}
