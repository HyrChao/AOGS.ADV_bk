//2017/4/16
//By Chao

using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {

    protected Player player;
    public GameObject item;
	void Start () {
        player = GameObject.Find("Player").GetComponent<Player>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnCollisionEnter(Collision collider)
    {
        if (collider.transform.tag == "Player")
        {
            player = collider.transform.GetComponent<Player>();
            Picked();
        }
    }
    virtual protected void Picked()
    {

    }
}
