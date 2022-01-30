using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EntityBehavior : MonoBehaviour
{
    [Header("usually should call whatever behavior wake up function you need")]
    public UnityEvent OnEntityWakeUp;

    private void OnEnable()
    {
        EntityManager.OnStartOfLevel += ActivateEntity;
    }
    private void OnDisable()
    {
        EntityManager.OnStartOfLevel -= ActivateEntity;
    }
    // Start is called before the first frame update

    public void ActivateEntity()
    {
        Debug.Log("I've been activated!");
        OnEntityWakeUp.Invoke();
    }
}
