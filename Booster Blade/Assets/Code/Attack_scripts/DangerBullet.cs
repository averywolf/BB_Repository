using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerBullet : MonoBehaviour
{
    //Combines bullet collisions and automatic disappearing into one component
    [SerializeField]
    public float disappearTime;
    [SerializeField]
    private bool isPersistant = false;
    //unsure if this stuff can activate before player takes damage on walls
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            gameObject.SetActive(false);
        }   
    }
    //called by PlayerCore if damage actually goes through
    public void RemoveBullet()
    {
        if (!isPersistant)
        {
            gameObject.SetActive(false);
        }
    }
    private void OnEnable()
    {
        StartCoroutine(VanishCountdown());
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    public IEnumerator VanishCountdown()
    {
        yield return new WaitForSeconds(disappearTime);
        gameObject.SetActive(false);
    }
}
