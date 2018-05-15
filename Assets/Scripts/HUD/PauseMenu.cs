using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : GenericMenu {



	void Start ()
    {
		
	}
	
	void Update ()
    {
        if (Input.GetButtonDown("Vertical"))
        {
            var hValue = Input.GetAxis("Vertical");

            if (hValue > 0)
                select++;
            else if (hValue < 0)
                select--;

            OnFocus();
        }

        if (Input.GetButtonDown("Submit"))
        {
            OnSelect();
        }
    }
}
