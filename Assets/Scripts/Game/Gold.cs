using UnityEngine;
using System.Collections;

public class Gold : Item{
    private int goldAmount;    
    public void SetGoldAmount(int value)
    {
        goldAmount = value;
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    override protected void Picked()
    {
        player.AddGold(dropGold);
    }

}
