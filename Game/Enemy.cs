//2017/3/30
//by Chao

using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
    private Player player;//目标玩家
    private Item[] item;//可能掉落的物品
    private int dropGold=20;
    private int dropEXP=30;

    private Rigidbody rb;

    private bool idle;
    private int random;
    //敌人属性
    public int HP;
    public int MP;
    public int attack=20;
    public int attackAccuracy=10;
    public int defence;
    public int dodge;
    public float centerYPos = 0.8f;

    //出生点&移动相关
    private bool inMoveHitColdTime=false;//碰撞攻击冷却
    private float moveSpeed=3.5f;
    private Vector3 moveDir;
    public Spawn spawnPoint;
    //public void SetSpawn(Spawn spawn)
    //{
    //    spawnPoint = spawn;
    //} 
    //敌人随机跳跃
    virtual public void jump()
    {
        rb.AddForce(Vector3.up*10);
    }
    //碰撞减血
    virtual public void MoveDamage()
    {
        int atkValue = attack + (int)(attackAccuracy*0.5-Random.Range(0, attackAccuracy));
        player.HP -= atkValue;
        Debug.Log("Enemy Attack" + atkValue.ToString());
    }
    //掉落经验
    virtual public void DropEXP()
    {
        player.EXP += dropEXP;
    }
    //掉落金币
    virtual public void DropGold()
    {
        player.Gold += dropGold;
    }
    //掉落物品
    virtual public void DropItem()
    {

    }
    //攻击玩家
    virtual public void Attack()
    {
        
    }
    //碰撞停顿
    private IEnumerator MoveHitColdTime()
    {
        inMoveHitColdTime = true;
        yield return new WaitForSeconds(0.8f);
        inMoveHitColdTime = false;
    }


	void Start ()
    {
        rb = transform.GetComponent<Rigidbody>();
        player = GameObject.Find("Player").GetComponent<Player>();
        if (Random.value > 0.5f)
            moveDir = Vector3.right;
        else
            moveDir = Vector3.left;
                

    }

    public virtual void Update () {
        random = Random.Range(0, 50);
        if (random == 30)
            jump();
        //敌人死亡
        if (HP <= 0)
        {
            DropEXP();
            DropGold();
            DropItem();           
            if(player!=null)
                player.SendMessage("KillEnemy");//SendMessage调用Player中的KillEnemy函数
            spawnPoint.count--;
            Destroy(this.gameObject);
        }
        //敌人范围随机移动
        if (spawnPoint != null&&!inMoveHitColdTime)
        {
            if (transform.position.x > spawnPoint.rightLimit)
            {
                moveDir = Vector3.left;

            }

            if (transform.position.x < spawnPoint.leftLimit)
            {
                moveDir = Vector3.right;
            }

            transform.Translate(moveDir * Time.deltaTime * moveSpeed);
        }

    }
    private void LateUpdate()
    {

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (transform.position.x < collision.transform.position.x)
                moveDir = Vector3.left;
            else
                moveDir = Vector3.right;
        }


        if (collision.gameObject.tag == "Player")
        {
            if(player!=null)
                if (!player.isDamaged && !inMoveHitColdTime)
                {
                    StartCoroutine(MoveHitColdTime());
                    MoveDamage();
                }

        }
    }
    private void OnCollisionExit(Collision collision)
    {

    }
}
