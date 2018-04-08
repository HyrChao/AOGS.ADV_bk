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

        hpSlider = transform.Find("HPBar").GetComponent<Slider>();
        mpSlider = transform.Find("MPBar").GetComponent<Slider>();
        spSlider = transform.Find("SPBar").GetComponent<Slider>();
        expSlider = transform.Find("EXPBar").GetComponent<Slider>();

        lvText = transform.Find("LV").GetComponent<Text>();
        gilText = transform.Find("GIL").GetComponent<Text>();
        ammoText = transform.Find("Ammo").GetComponent<Text>();
        msgText = transform.Find("Msg").GetComponent<Text>();

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
