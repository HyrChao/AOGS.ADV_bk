//2017/3/31
//by Chao
using UnityEngine;
using System.Collections;

public class Rocket : MonoBehaviour {
    
    Enemy enemy;
    private int damage;
    public void SetDamage(int value)
    {
        damage = value;
    }
    private IEnumerator LifeTimer()
    {
        Debug.Log("RocketLaunch!!");
        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);
    }
	void Start () {
        StartCoroutine(LifeTimer());
        //Vector3.Cross(new Vector3(Gun.force * Player.faceRight, 0,0) , Gun.rocketFocus
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            enemy = collision.transform.GetComponent<Enemy>();
            Debug.Log("RocketHit!");
            enemy.HP -= (damage-enemy.defence);
            Destroy(this.gameObject);
            
        }
    }
}
