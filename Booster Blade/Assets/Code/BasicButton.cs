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

    //public GameObject KeyBolt;
    [HideInInspector]
    public ButtonGem keyGem;
    public ParticleSystem keyParticle;
    private void Awake()
    {
        keyParticle.Play();

        butAnim = GetComponent<Animator>();
    }
    public void PressButton()
    {

        if (!isPressed)
        {
        
            //if(KeyBolt != null)
            //{
            //    StartCoroutine(TestShootBolt(testDoor, 3f));
            //}
            
            SpawnParticles(ButtonPressFX, transform.position);
           // StartCoroutine(testDoor.get)
            //should play clicking sound
            //might play particle effect when pressed?
            ChangeButtonStatus(true);
            if (keyGem != null)
            {
                keyGem.ButtonSignal(transform);
            }
        }
    }

 
    public void ChangeButtonStatus(bool setToPressed)
    {
        if (setToPressed)
        {
            keyParticle.Stop();
            butAnim.Play("butpressed");
            isPressed = true;
            OnButtonPressed.Invoke();
            AudioManager.instance.Play("ButtonPress");
        }
        else
        {
            keyParticle.Play();
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
