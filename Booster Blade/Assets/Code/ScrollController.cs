using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollController : MonoBehaviour
{
    public GameObject scrollObject;
    private List<GameObject> scrollingObjects = new List<GameObject>();
    public float scrollHorzLength; //set by automatically by the object being used. change back to private.
    [SerializeField]
    private float scrollspeed = -14;
    //used to be -26 and -14
    //-14
    // Start is called before the first frame update

        //LIST SYSTEM IS BUSTED AND CONFUSING
    void Start()
    {
        //scrollspeed = PlayerController.testBaseSpeed;
        scrollHorzLength= scrollObject.GetComponent<ScrollingObject>().sizeDeterminer.bounds.size.x;
        SetUpScrollObjects();
    }
    public void SetUpScrollObjects()
    {
 
        scrollingObjects.Add(scrollObject);
        
        //Duplicates the scrolling object and places it next to it.
        GameObject scolObj = Instantiate(scrollObject, transform);
        scrollingObjects.Add(scolObj);
        Vector3 groundOffSet = new Vector3(scrollHorzLength, 0, 0);
        scolObj.transform.position = scolObj.transform.position + groundOffSet;

        scrollObject.GetComponent<ScrollingObject>().StartScrolling(scrollspeed);
        scolObj.GetComponent<ScrollingObject>().StartScrolling(scrollspeed);
    }
    

    private void FixedUpdate()
    {
        //test method checks if both are on target
        for (int i = 0; i < scrollingObjects.Count; i++)
        {
            if (scrollingObjects[i].transform.position.x < -scrollHorzLength)
            {
                RepositionObject(scrollingObjects[0]);
                scrollingObjects[1].transform.position= new Vector3(0, 0, scrollingObjects[1].GetComponent<ScrollingObject>().ogZValue);
            }
        }
        //maybe check both objects at once?
    }

    private void RepositionObject(GameObject objToMove)
    {
        //This is how far to the right we will move our background object, in this case, twice its length. This will position it directly to the right of the currently visible background object.
        Vector3 groundOffSet = new Vector3(scrollHorzLength, 0, 0);

        //Move this object from its position offscreen, behind the player, to the new position off-camera in front of the player.
        objToMove.transform.position = transform.position + groundOffSet + new Vector3(0, 0, objToMove.GetComponent<ScrollingObject>().ogZValue);
    }
}
