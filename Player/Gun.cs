//2017/3/31
//by Chao

using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {
    public GameObject rocket;
    public GameObject launchPos;
    public GameObject lockPos;
    private Vector3 originalPos;
    private Vector3 rocketFocus;
    private float force =100;
    private int damage=23;
    // Use this for initialization
    public void Fire()
    {
        rocketFocus = lockPos.transform.position - launchPos.transform.position;
        GameObject newRocket = Instantiate(rocket, launchPos.transform.position, Quaternion.Euler(0,0,90)) as GameObject;
        newRocket.GetComponent<Rigidbody>().AddForce(rocketFocus* force);
        newRocket.GetComponent<Rocket>().SetDamage(damage);
    }

    private void Start()
    {
        originalPos = lockPos.transform.position - launchPos.transform.position;
    }
    private void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == ("Enemy"))
        {
            Debug.Log("Enemy Detected,LockOn!");
            //Vector3 enemypos =new Vector3(other.transform.position.x, other.transform.position.y,gameObject.transform.position.z);
            //lockPos.transform.position = enemypos;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == ("Enemy"))
        {
            Debug.Log("Target Missing");
            //rocketFocus = originalPos;
        }
    }
    
}
