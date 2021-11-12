using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonGem : KeyGem
{
    public List<BasicButton> basicButtons;

    public void Update()
    {
        if (!conditionsHaveBeenMet)
        {
            for (int i = 0; i < basicButtons.Count; i++)
            {
                if (!basicButtons[i].isPressed)
                {
                    return;
                }
            }
            UnlockDoor();
        }
    }

}
