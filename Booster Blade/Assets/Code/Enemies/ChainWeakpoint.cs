using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainWeakpoint : MonoBehaviour
{
    private Chain chain;
    private void Awake()
    {
        chain = GetComponentInParent<Chain>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<SwordSlash>())
        {
            chain.CutChain();
        }
    }
    public void WeakpointCutEffect()
    {
        gameObject.SetActive(false);
    }
}
