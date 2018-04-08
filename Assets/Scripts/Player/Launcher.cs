//2017/3/31
//by Chao
//Launcher, fire properties

using UnityEngine;
using System.Collections;

public class Launcher : MonoBehaviour {
    //define what kind of rocket to launch
    public Rocket rocket;
    public Transform rocketLaunchPos;

    private int ammo;
    public int currentAmmmo
    {
        get
        {
            return ammo;
        }
    }
    private int maxAmmo = 100;

    private int damage=23;
    private float force = 1000f;
    private float coldTime = 0.5f;

    public Transform leftHandPos;//IK left hand pos
    public Transform rightHandPos;//IK right hand pos

    private float load = 0f;//if or not loaded
    public float Load
    {
        get
        {
            return load;
        }
    }
    
    public void Fire()
    {
        //load = Mathf.Lerp(0, 1, 0.5f);
        load = 1f;
        AO.player.CanFire = false;
        AO.player.weapon.gameObject.SetActive(false);
        Invoke("FireDone", coldTime);

        this.transform.parent = AO.slot.laucherLaunchSlot;
        this.transform.position = AO.slot.laucherLaunchSlot.position;
        this.transform.rotation = AO.slot.laucherLaunchSlot.rotation;

        Rocket newRocket = Instantiate(rocket, rocketLaunchPos.position, Quaternion.Euler(0,0,90)) as Rocket;
        newRocket.GetComponent<Rigidbody>().AddForce(force*AO.player.FaceDirection);
        newRocket.GetComponent<Rocket>().SetDamage(damage);

        ammo--;
    }

    private void FireDone()
    {
        //load = Mathf.Lerp(1, 0, 0.5f);
        load = 0f;
        AO.player.CanFire = true;
        AO.player.weapon.gameObject.SetActive(true);

        this.transform.parent = AO.slot.laucherSlot;
        this.transform.position = AO.slot.laucherSlot.position;
        this.transform.rotation = AO.slot.laucherSlot.rotation;
    }

    private void Awake()
    {
        ammo = maxAmmo;
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
