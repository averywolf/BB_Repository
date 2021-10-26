using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class BasicButton : MonoBehaviour
{
    /// <summary>
    /// A very basic button that triggers something.
    /// </summary>

    private Animator butAnim;
    public bool isPressed = false;

    public UnityEvent OnButtonPressed;
    public GameObject ButtonPressFX;
    private void Awake()
    {
        butAnim = GetComponent<Animator>();
    }
    public void PressButton()
    {

        if (!isPressed)
        {
            Debug.Log("PressingButton");
            SpawnParticles(ButtonPressFX, transform.position);
            //should play clicking sound
            //might play particle effect when pressed?
            ChangeButtonStatus(true);
           
        }
    }
    
    public void ChangeButtonStatus(bool setToPressed)
    {
        if (setToPressed)
        {
            Debug.Log("Trying to press button!");
            butAnim.Play("butpressed");
            isPressed = true;
            OnButtonPressed.Invoke();
        }
        else
        {

            butAnim.Play("butnotpressed");
            isPressed = false;

        }
    }
    public void SpawnParticles(GameObject particleEffectPrefab, Vector2 spawnPoint)
    {
        GameObject particleEffect = Instantiate(particleEffectPrefab, spawnPoint, particleEffectPrefab.transform.rotation);
        Destroy(particleEffect, particleEffect.GetComponent<ParticleSystem>().main.startLifetimeMultiplier);
    }
}
