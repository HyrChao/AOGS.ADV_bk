//2018-05-15
//By Chao
//Pause menu manage

using UnityEngine;

public class PauseMenu : GenericMenu {

    protected override void OnConfirm()
    {
        base.OnConfirm();
    }

    protected override void Update ()
    {
        base.Update();

        if (Input.GetButtonDown("Vertical"))
        {
            var hValue = Input.GetAxis("Vertical");

            if (hValue < 0)
                select++;
            else if (hValue > 0)
                select--;
        }
    }

}
