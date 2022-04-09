using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakX : MonoBehaviour
{

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void SummonWeakX()
    {
        animator.Play("weakX_appear");
    }
    public void HideWeakX()
    {
        animator.Play("weakX_empty");
    }
}
