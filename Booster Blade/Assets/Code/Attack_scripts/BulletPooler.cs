using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPooler : MonoBehaviour
{
    // Modified object pooler script built to store different types of bullets more cleanly and efficiently.

    private Dictionary<GameObject, List<GameObject>> bulDict;

    public List<BulletType> bulletCategories;

    public static BulletPooler instance;
    [System.Serializable]
    public class BulletType
    {
        public int bulAmount;
        public GameObject bulletPrefab;
        public List<GameObject> bulletList;
    }

    // Having the pool be set up in Awake() should avoid null references if other scripts grab bullets in their Start() methods
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        PoolHolder.Pooler = this;
        SetUpPools();
    }

    private void SetUpPools()
    {
        bulDict = new Dictionary<GameObject, List<GameObject>>();

        foreach (BulletType bulType in bulletCategories)
        {
            bulDict.Add(bulType.bulletPrefab, bulType.bulletList); //probably can simplify the "list" part

            for (int i = 0; i < bulType.bulAmount; i++)
            {
                GameObject bul = Instantiate(bulType.bulletPrefab, gameObject.transform);
                bulDict[bulType.bulletPrefab].Add(bul);

                bul.SetActive(false);
            }

        }
    }

    // Called by Shot scripts when they need to prep a bullet to fire.
    // Since this is not a singleton, a reference to this script might need to be "threaded" to Shot, first.

    public GameObject GrabBullet(GameObject prefab)
    {
        if (bulDict.TryGetValue(prefab, out List<GameObject> theList))
        {
            for (int i = 0; i < theList.Count; i++)
            {
                if (!theList[i].activeInHierarchy)
                {
                    // some one suggested that you could move the bullet to the back of the list?  
                    return theList[i];
                }
            }
        }
        return null;
    }
    //should hopefully work
    public void ClearAllBullets()
    {
        foreach (KeyValuePair<GameObject, List<GameObject>> coolList in bulDict)
        {
            //show contents of individual list
            foreach (GameObject coolBul in coolList.Value)
            {
                coolBul.SetActive(false);
            }
        }
    }
}
