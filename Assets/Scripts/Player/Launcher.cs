//2017/3/31
//by Chao
//Launcher, fire properties

using UnityEngine;
using System.Collections;

public class Launcher : MonoBehaviour {
    //define what kind of rocket to launch
    public Rocket rocket;
    public Transform rocketLaunchPos;

    private int damage=23;
    private float force = 1000f;
    private float coldTime = 0.5f;

    public Transform leftHandPos;//IK left hand pos
    public Transform rightHandPos;//IK right hand pos

    private float load = 0;//if or not loaded
    public float Load
    {
        get
        {
            return load;
        }
        set
        {
            load = value;
        }
    }
    
    public void Fire()
    {
        //load = Mathf.Lerp(0, 1, 0.5f);
        load = 1;
        AO.gm.Player.CanFire = false;
        AO.gm.Player.weapon.gameObject.SetActive(false);
        Invoke("FireDone", coldTime);
        this.transform.position = AO.gm.Slot.laucherLaunchSlot.position;
        this.transform.rotation = AO.gm.Slot.laucherLaunchSlot.rotation;
        Rocket newRocket = Instantiate(rocket, rocketLaunchPos.position, Quaternion.Euler(0,0,90)) as Rocket;
        newRocket.GetComponent<Rigidbody>().AddForce(force*AO.gm.Player.FaceDirection);
        newRocket.GetComponent<Rocket>().SetDamage(damage);
    }

    private void FireDone()
    {
        //load = Mathf.Lerp(1, 0, 0.5f);
        load = 0;
        AO.gm.Player.CanFire = true;
        AO.gm.Player.weapon.gameObject.SetActive(true);
        this.transform.position = AO.gm.Slot.laucherSlot.position;
        this.transform.rotation = AO.gm.Slot.laucherSlot.rotation;
    }

    private void Awake()
    {

    }
    private void Start()
    {
        
    }
    private void Update()
    {
        ///rocketFocus = lockPos.transform.position - launchPos.transform.position;//更新导弹发射方向
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag == ("Enemy"))
    //    {
    //        Debug.Log("Enemy Detected,LockOn!");
    //        //Vector3 enemypos =new Vector3(other.transform.position.x, other.transform.position.y,gameObject.transform.position.z);
    //        //lockPos.transform.position = enemypos;
    //    }
    //}
    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.tag == ("Enemy"))
    //    {
    //        Debug.Log("Target Missing");
    //        //rocketFocus = originalPos;
    //    }
    //}
    
}
