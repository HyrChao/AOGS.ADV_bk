//2018/03/29
//By Chao
//Used for control HUD

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

    private GameManager gm;
    
    private Slider hpSlider;
    private Slider mpSlider;
    private Slider spSlider;
    private Slider expSlider;
    
    private Text lvText;
    private Text gilText;
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

        gm = AO.GetGameManager();
        hpSlider = transform.Find("HPBar").GetComponent<Slider>();
        mpSlider = transform.Find("MPBar").GetComponent<Slider>();
        spSlider = transform.Find("SPBar").GetComponent<Slider>();
        expSlider = transform.Find("EXPBar").GetComponent<Slider>();

        lvText = transform.Find("LV").GetComponent<Text>();
        gilText = transform.Find("GIL").GetComponent<Text>();
        msgText = transform.Find("Msg").GetComponent<Text>();

    }

    private void Start ()
    {
		
	}
	

	private void Update ()
    {
        hpSlider.value = 100 * (gm.Player.HP / gm.Player.MaxHP);
        mpSlider.value = 100 * (gm.Player.MP / gm.Player.MaxMP);
        spSlider.value = 100 * (gm.Player.SP / gm.Player.MaxSP);
        expSlider.value = 100 * ((float)gm.Player.GetExp() / (gm.Player.GetRemainExp() + gm.Player.GetExp()));

        lvText.text = "Lv." + gm.Player.LV.ToString();
        msgText.text = msg.First.Value + "\n" + msg.First.Next.Value + "\n" + msg.First.Next.Next.Value + "\n" + msg.First.Next.Next.Next.Value + "\n" + msg.First.Next.Next.Next.Next.Value;
        gilText.text = "GIL."+gm.Player.Gold.ToString();

    }

}
