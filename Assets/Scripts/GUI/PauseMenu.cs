//2018-05-15
//By Chao
//Pause menu manage

using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : GenericMenu {

    public Button debugButton;

    protected override void OnConfirm()
    {
        base.OnConfirm();
        //Resume
        if(select == 0)
        {
            AO.gm.Resume();
        }
        //Dubug
        if (select == 1)
        {

        }
        //Opeion
        if (select == 2)
        {

        }
        //Quit
        if (select == 3)
        {

        }
    }

    protected override void Update ()
    {
        base.Update();

        if (Input.GetButtonDown("Vertical"))
        {
            float vValue = Input.GetAxisRaw("Vertical");

            if (vValue < 0)
                select++;
            else if (vValue > 0)
                select--;
        }
    }

    protected override void Awake()
    {
        base.Awake();
    }

}
