using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonGem : KeyGem
{
    public List<BasicButton> basicButtons;
    public GameObject keyBolt;
    //should only listen to buttons, buttons don't need to listen back
    public void Start()
    {
        for (int i = 0; i < basicButtons.Count; i++)
        {
            basicButtons[i].keyGem = this;
        }
    }
    public void ButtonSignal(Transform buttonTransform)
    {
     
        if (!conditionsHaveBeenMet)
        {
            StartCoroutine(ShootKeyBolt(buttonTransform, 6));

           // CheckAllButtons();
        }
    }
    public void Update()
    {
    }
    //maybe check buttones before but open the door after keybolt?
    public void CheckAllButtons()
    {
        Debug.Log("Checking all buttons for this door now!");
        for (int i = 0; i < basicButtons.Count; i++)
        {
            if (!basicButtons[i].isPressed)
            {
                //it should be possible for it to count how many are leftx
                Debug.Log("More buttons need to be pressed!");
                return;
            }
        }
        
        UnlockDoor();//might need to move position of conditions have been met for keybolt to work
    }
    public IEnumerator ShootKeyBolt(Transform startingPoint, float timeSpeed)
    {
        Transform doortarget = transform;
        GameObject bolt = Instantiate(keyBolt, startingPoint.position, transform.rotation);
        Transform boltTrans = bolt.transform;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime * timeSpeed)
        {
            boltTrans.position = Vector3.Lerp(startingPoint.position, doortarget.position, t);

            yield return null;
        }
        bolt.GetComponent<ParticleSystem>().Stop();
        bolt.GetComponentInChildren<ParticleSystem>().gameObject.SetActive(true); //doesn't quite work yet, use triggerexplosion method
        //needs to have a particle impact
        Destroy(bolt, bolt.GetComponentInChildren<ParticleSystem>().main.startLifetimeMultiplier);
        CheckAllButtons();
    }
}
