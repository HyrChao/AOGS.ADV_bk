//2017/3/30
//by Chao
//敌人基类脚本

using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
    private Player player;//目标玩家
    private Item[] item;//可能掉落的物品
    private int dropGold=20;
    private int dropEXP=30;

    private Rigidbody rb;

    private bool leftEncounting = false;
    public bool isEncounting = false;
    private float defEncountingBackSpeed = 8;
    private float encountingBackSpeed = 0;
    private float encountingAcceleration = 20;
    private float accelerateSmooth = 2.5f;

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

    private Vector3 currentPosition;
    private Vector3 privousPosition;

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

	void Start ()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player").GetComponent<Player>();

        if (Random.value > 0.5f)
            moveDir = Vector3.right;
        else
            moveDir = Vector3.left;               
    }

    public virtual void Update () {
        random = Random.Range(0, 200);
        if (random == 20)
        {
            //Debug.Log("Enemy Jump!");
            jump();
        }

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
        privousPosition = currentPosition;
        currentPosition = transform.position;
        //敌人被攻击后退
        if (isEncounting)
        {
            //判断退后方向
            if (transform.position.x - player.transform.position.x > 0)
            {
                leftEncounting = false;
            }
            if (transform.position.x - player.transform.position.x < 0)
            {
                leftEncounting = true;
            }

            encountingBackSpeed = encountingBackSpeed - encountingAcceleration * Time.deltaTime;
            if (leftEncounting)
            {
                currentPosition.x = privousPosition.x - Time.deltaTime * encountingBackSpeed;
            }
            else
            {
                currentPosition.x = privousPosition.x + Time.deltaTime * encountingBackSpeed;
            }
            if (encountingBackSpeed < 0.05)
            {
                isEncounting = false;
                encountingBackSpeed = defEncountingBackSpeed;
            }
            transform.position = currentPosition;//更新位置
        }

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
                if (!player.state.damaged&& !inMoveHitColdTime)
                {
                    StartCoroutine(MoveHitColdTime());
                    MoveDamage();
                }

        }
    }
    private void OnCollisionExit(Collision collision)
    {

    }
    virtual public void jump()
    {
        rb.AddForce(Vector3.up * 1000);
    }
    //碰撞减血
    virtual public void MoveDamage()
    {
        int atkValue = attack + (int)(attackAccuracy * 0.5 - Random.Range(0, attackAccuracy));
        player.HP -= atkValue;
        Debug.Log("Enemy Attack" + atkValue.ToString());
    }
    //掉落经验
    virtual public void DropEXP()
    {
        player.AddEXP(dropEXP);
    }
    //掉落金币
    virtual public void DropGold()
    {
        
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


}
