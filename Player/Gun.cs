//2017/3/31
//by Chao
//决定发射炮弹的速度位置及攻击力

using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {
    public GameObject rocket;
    public GameObject launchPos;
    public GameObject lockPos;
    public Vector3 rocketFocus;
    private float force =600;
    private int damage=23;

    public Transform leftHandPos;//IK左手位置
    public Transform rightHandPos;//IK右手位置
    public float laod = 0;//是否抬起
    public Vector3 gunPos;
    public Vector3 gunScale;
    public float faceRight = 1f;//举枪方向

    public void Fire()
    {
        GameObject newRocket = Instantiate(rocket, launchPos.transform.position, Quaternion.Euler(0,0,90)) as GameObject;
        newRocket.GetComponent<Rigidbody>().AddForce(rocketFocus* force);
        newRocket.GetComponent<Rocket>().SetDamage(damage);
    }

    private void Start()
    {
        gunPos = transform.localPosition;
        gunScale = transform.FindChild("GunMesh").transform.localScale;
    }
    private void Update()
    {
        rocketFocus = lockPos.transform.position - launchPos.transform.position;//更新导弹发射方向
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
