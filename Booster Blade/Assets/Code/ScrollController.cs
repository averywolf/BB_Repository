using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollController : MonoBehaviour
{
    public GameObject scrollObject;
    private List<GameObject> scrollingObjects = new List<GameObject>();
    private float scrollHorzLength;
    [SerializeField]
    private float scrollspeed = -14;
    //-14
    // Start is called before the first frame update

        //LIST SYSTEM IS BUSTED AND CONFUSING
    void Start()
    {
        scrollHorzLength= scrollObject.GetComponent<ScrollingObject>().sizeDeterminer.bounds.size.x;
        SetUpScrollObjects();
    }
    public void SetUpScrollObjects()
    {
 
        scrollingObjects.Add(scrollObject);
        
        GameObject scolObj = Instantiate(scrollObject, transform);

        scrollingObjects.Add(scolObj);
        //Vector3 groundOffSet = new Vector3(scrollHorzLength * 2f, 0, 0);
        Vector3 groundOffSet = new Vector3(scrollHorzLength, 0, 0);

        scolObj.transform.position = scolObj.transform.position + groundOffSet;
        scrollObject.GetComponent<ScrollingObject>().StartScrolling(scrollspeed);
        scolObj.GetComponent<ScrollingObject>().StartScrolling(scrollspeed);
        //GetEverythingScrolling();

    }
    private void Update()
    {
        for (int i = 0; i < scrollingObjects.Count; i++)
        {
            if (scrollingObjects[i].transform.position.x < -scrollHorzLength)
            {
                RepositionObject(scrollingObjects[i]);
            }

        }
    }
    public void GetEverythingScrolling()
    {
        scrollingObjects[0].GetComponent<ScrollingObject>().StartScrolling(scrollspeed);
        scrollingObjects[1].GetComponent<ScrollingObject>().StartScrolling(scrollspeed);
        //for (int i = 0; i <scrollingObjects.Count; i++)
        //{
        //    scrollingObjects[i].GetComponent<ScrollingObject>().StartScrolling(scrollspeed);
        //}
    }
    
    private void RepositionObject(GameObject objToMove)
    {
        //This is how far to the right we will move our background object, in this case, twice its length. This will position it directly to the right of the currently visible background object.
        Vector3 groundOffSet = new Vector3(scrollHorzLength, 0, 0);

        //Move this object from it's position offscreen, behind the player, to the new position off-camera in front of the player.
        objToMove.transform.position = transform.position + groundOffSet + new Vector3(0, 0, objToMove.GetComponent<ScrollingObject>().ogZValue);
    }
}
