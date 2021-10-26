using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlash : MonoBehaviour
{
    // Attach this script to the part of the enemy/player you want to flash when taking damage
    public Color flashColor;

    Material mat;

    private IEnumerator flashCoroutine;

    private void Awake()
    {
        mat = GetComponent<SpriteRenderer>().material;
    }

    private void Start()
    {
        mat.SetColor("_FlashColor", flashColor);
    }
    public void Flash(bool flashWhite, float flashDuration)
    {
        if (flashWhite)
        {
            mat.SetColor("_FlashColor", Color.white);
        }
        else
        {
            mat.SetColor("_FlashColor", flashColor);
        }

        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);

        flashCoroutine = DoFlash(flashDuration);
        StartCoroutine(flashCoroutine);
    }

    private IEnumerator DoFlash(float flashDuration)
    {
        float lerpTime = 0;

        while (lerpTime < flashDuration)
        {
            lerpTime += Time.deltaTime;
            float perc = lerpTime / flashDuration;

            SetFlashAmount(1f - perc);
            yield return null;
        }
        SetFlashAmount(0);
    }

    //color flash

    private void SetFlashAmount(float flashAmount)
    {
        mat.SetFloat("_FlashAmount", flashAmount);
    }

}
