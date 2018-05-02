//2018/03/29
//By Chao
//Used for control HUD

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

    //debug
    public bool debugMobileGUI = false;

    private Transform playerStates;

    private Slider hpSlider;
    private Slider mpSlider;
    private Slider spSlider;
    private Slider expSlider;
    
    private Text lvText;
    private Text gilText;
    private Text ammoText;
    private Text msgText;

    private Transform platform;

    private TouchScreenControlHUD _touchControlHUD;
    public TouchScreenControlHUD touchControlHUD
    {
        get
        {
            if (_touchControlHUD != null)
                return _touchControlHUD;
            else
                return null;
        }
    }

    bool _isTouchScreenGUI = false;
    bool isTouchScreenGUI
    {
        get
        {
            return _isTouchScreenGUI;
        }
    }

    private LinkedList<string> msg; //A string list for in-game msg    
    public void Msg(string _msg)
    {
        msg.AddFirst(_msg);
    }

    private void Awake()
    {
        //Check current platform
        if (debugMobileGUI)
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
                _isTouchScreenGUI = false;
            }
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                platform = transform.Find("Mobile");
                platform.gameObject.SetActive(true);
                _isTouchScreenGUI = true;
                _touchControlHUD = platform.Find("TouchControlHUD").GetComponent<TouchScreenControlHUD>();
            }
        }

        msgText = platform.Find("Msg").GetComponent<Text>();

        playerStates = platform.Find("PlayerStates").transform;

        hpSlider = playerStates.Find("HPBar").GetComponent<Slider>();
        mpSlider = playerStates.Find("MPBar").GetComponent<Slider>();
        spSlider = playerStates.Find("SPBar").GetComponent<Slider>();
        expSlider = playerStates.Find("EXPBar").GetComponent<Slider>();
        lvText = playerStates.Find("LV").GetComponent<Text>();
        gilText = playerStates.Find("GIL").GetComponent<Text>();
        ammoText = playerStates.Find("Ammo").GetComponent<Text>();


        msg = new LinkedList<string>();
        Msg("1 - -| wurara");
        Msg("2 - -| wurara");
        Msg("3 - -| wurara");
        Msg("4 - -| wurara");
        Msg("5 - -| wurara");

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
