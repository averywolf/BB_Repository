using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class BasicRepeatController : MonoBehaviour
{
    public UnityEvent RepeatedAction;

    public float delay1 = 1;
    public float delay2 = 1;
    public IEnumerator ActionProcess()
    {
        while (true)
        {

            yield return new WaitForSeconds(delay1);
        }
    }
}
