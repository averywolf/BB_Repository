using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonGem : KeyGem
{
    public List<BasicButton> basicButtons;
  
    //should only listen to buttons, buttons don't need to listen back
    public void Start()
    {
        for (int i = 0; i < basicButtons.Count; i++)
        {
            basicButtons[i].keyGem = this;
        }
    }

    public override void CheckConditions()
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
        conditionsHaveBeenMet = true;
       // UnlockDoor();//might need to move position of conditions have been met for keybolt to work
    }
    
}
