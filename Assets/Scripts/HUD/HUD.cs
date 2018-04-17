//2018/03/29
//By Chao
//Used for control HUD

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

    private Slider hpSlider;
    private Slider mpSlider;
    private Slider spSlider;
    private Slider expSlider;
    
    private Text lvText;
    private Text gilText;
    private Text ammoText;
    private Text msgText;
    
    private LinkedList<string> msg; //A string list for in-game msg    
    public void Msg(string _msg)
    {
        msg.AddFirst(_msg);
    }

    private void Awake()
    {
        msg = new LinkedList<string>();
        Msg("1 - -| wurara");
        Msg("2 - -| wurara");
        Msg("3 - -| wurara");
        Msg("4 - -| wurara");
        Msg("5 - -| wurara");

        //Check current platform

        Transform platform = null;

        if (AO.debug)
        {
            platform = transform.Find("Mobile");
            platform.gameObject.SetActive(true);
        }
        else
        {
            if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
            {
                platform = transform.Find("PC");
                platform.gameObject.SetActive(true);
            }
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                platform = transform.Find("Mobile");
                platform.gameObject.SetActive(true);
            }
        }

        hpSlider = platform.Find("HPBar").GetComponent<Slider>();
        mpSlider = platform.Find("MPBar").GetComponent<Slider>();
        spSlider = platform.Find("SPBar").GetComponent<Slider>();
        expSlider = platform.Find("EXPBar").GetComponent<Slider>();

        lvText = platform.Find("LV").GetComponent<Text>();
        gilText = platform.Find("GIL").GetComponent<Text>();
        ammoText = platform.Find("Ammo").GetComponent<Text>();
        msgText = platform.Find("Msg").GetComponent<Text>();

    }

    private void Start ()
    {
		
	}
	

	private void Update ()
    {
        hpSlider.value = 100 * (AO.player.HP / AO.player.MaxHP);
        mpSlider.value = 100 * (AO.player.MP / AO.player.MaxMP);
        spSlider.value = 100 * (AO.player.SP / AO.player.MaxSP);
        expSlider.value = 100 * ((float)AO.player.GetExp() / (AO.player.GetRemainExp() + AO.player.GetExp()));

        lvText.text = "Lv." + AO.player.LV.ToString();
        msgText.text = msg.First.Next.Next.Next.Next.Value + "\n" + msg.First.Next.Next.Next.Value + "\n" + msg.First.Next.Next.Value + "\n" + msg.First.Next.Value + "\n" + msg.First.Value;
        gilText.text = "GIL."+AO.player.Gold.ToString();
        ammoText.text = "Ammo::" + AO.player.launcher.currentAmmmo.ToString();

    }

}
