//2017/3/30
//by Chao
//Enemy base script

using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
    private GameManager gm;
    private Item[] item;//Drop items
    private int dropGold=20;
    private int dropEXP=30;

    private Rigidbody rb;
    private bool grounded;

    private bool leftEncounting = false;
    public bool isEncounting = false;
    private float defEncountingBackSpeed = 8;
    private float encountingBackSpeed = 0;
    private float encountingAcceleration = 20;
    private int random;

    //Enemy properties
    public int HP;
    public int MP;
    public int attack=20;
    public int attackAccuracy=10;
    public int defence;
    public int dodge;
    public float centerYPos;

    private Vector3 currentPosition;
    private Vector3 privousPosition;

    //Spawn
    public Spawn spawnArea;
    
    //Movenment
    private bool inMoveHitColdTime=false;//Hit attack cold time
    private float moveSpeed=3.5f;
    private Vector3 moveDir;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        centerYPos = rb.worldCenterOfMass.y;
        gm = AO.GetGameManager();
    }

    void Start ()
    {
        //Initialize initial direction

        if (Random.value > 0.5f)
            moveDir = Vector3.right;
        else
            moveDir = Vector3.left;               
    }

    public virtual void Update ()
    {
        //Ramdom jump for enemy
        random = Random.Range(0, 200);
        if (random == 20 && grounded)
        {
            Debug.Log("Enemy Jump!");
            Jump();
        }

        //Enemy killed
        if (HP <= 0)
        {
            DropEXP();
            DropGold();
            DropItem();
            if (gm.Player != null)
                gm.Player.EnemyKilled++;
            spawnArea.CurrentEnemyCount--;
            Destroy(this.gameObject);
        }

        //Enemy move in range
        if (spawnArea != null&&!inMoveHitColdTime)
        {
            if (transform.position.x > spawnArea.RightLimit)
            {
                moveDir = Vector3.left;

            }

            if (transform.position.x < spawnArea.LeftLimit)
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
        //Enemy hit backstep
        if (isEncounting)
        {
            //Judge direction
            if (transform.position.x - gm.Player.transform.position.x > 0)
            {
                leftEncounting = false;
            }
            if (transform.position.x - gm.Player.transform.position.x < 0)
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
            transform.position = currentPosition;//update position
        }

    }

    private void FixedUpdate()
    {
        float groundRaycastDist = 3f;
        bool hit = Physics.Raycast(transform.position - new Vector3(0, centerYPos - groundRaycastDist, 0), Vector3.down, groundRaycastDist, LayerMask.GetMask("Ground"));
        if (hit)
        {
            grounded = true;
        }
        else
        {
            grounded = false;
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
            if(gm.Player!=null)
                if (!gm.Player.state.Equals(PlayerState.Damaging)&& !inMoveHitColdTime)
                {
                    StartCoroutine(MoveHitColdTime());
                    MoveDamage();
                }

        }
    }
    private void OnCollisionExit(Collision collision)
    {

    }
    private void Jump()
    {
        rb.AddForce(Vector3.up * 50000);
    }

    //Damage when player hits
    virtual public void MoveDamage()
    {
        int atkValue = attack + (int)(attackAccuracy * 0.5 - Random.Range(0, attackAccuracy));
        gm.Player.DropHP(atkValue);
        Debug.Log("Enemy Attack" + atkValue.ToString());
    }
    //Drop exp
    virtual public void DropEXP()
    {
        gm.Player.AddEXP(dropEXP);
    }
    //Drop gold
    virtual public void DropGold()
    {
        
    }
    //Drop items
    virtual public void DropItem()
    {

    }
    //Attack player
    virtual public void Attack()
    {

    }
    //Cold time while hit
    private IEnumerator MoveHitColdTime()
    {
        inMoveHitColdTime = true;
        yield return new WaitForSeconds(0.8f);
        inMoveHitColdTime = false;
    }


}
