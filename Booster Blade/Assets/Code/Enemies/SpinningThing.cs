using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningThing : MonoBehaviour
{
    [SerializeField]
    private float spinSpeed;

    float zRotation = 0;
    private Transform objTransform;
    private bool isSpinning=false;
    private void Awake()
    {
        objTransform = transform;

    }

    private void Update()
    {
        if (isSpinning)
        {
            zRotation += Time.deltaTime * spinSpeed;
            zRotation = zRotation % 360;

            objTransform.rotation = Quaternion.Euler(0, 0, zRotation);
        }
 
;
    }
    public void StartSpinning(bool shouldSpin)
    {
        isSpinning = shouldSpin;
    }
}
