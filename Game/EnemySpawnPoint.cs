//2017/4/1
//by Chao

using UnityEngine;
using System.Collections;

public class EnemySpawnPoint : MonoBehaviour {

    public bool canSpawn=false;

	void Start () {
	
	}
    private void OnTriggerStay(Collider other)
    {
        if (other.tag != "Ground")
        {
            if (other != null)
            {
                canSpawn = false;
            }
            else
                canSpawn = true;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        canSpawn = true;
    }
}
