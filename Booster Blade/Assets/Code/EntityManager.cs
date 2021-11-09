using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    public delegate void LevelStarted();
    public static event LevelStarted OnStartOfLevel;

    
    public static EntityManager instance;
 
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

    }
    //make sure to have a function when dying and going through doors
    public void ActivateEntities()
    {
        if(OnStartOfLevel != null)
        {

            OnStartOfLevel.Invoke();
        }
    }
}
