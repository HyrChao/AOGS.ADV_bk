// 2017/03/27 by Chao
// Unity5 (only)

using UnityEngine;
using System.Collections;

public class Controller_bk : MonoBehaviour{

    public Player player;
    private AnimeManager am;
    private float moveSpeed;
    private float runSpeed;
    private bool nextJump = false;
    //遇敌后退
    private  bool leftEncounting = false;
    private  bool isEncounting = false;
    private float defEncountingBackSpeed = 8;
    private float encountingBackSpeed = 0;
    private float encountingAcceleration = 20;
    private float accelerateSmooth = 2.5f;
    //后坐力参数
    private float defFiringBackSpeed = 10;
    private float firingBackSpeed = 0;
    private float firingAcceleration = 35;

    private float jumpV_x = 0;
    private float jumpHaxis = 0;
    private float jumpVelocity = 0;
    private float fallVelocity = 0;
    private float dropVelocity = 0;
    private float deltaJumpHight = 0;
    private float deltaFallHight = 0;
    private float deltaDropHight = 0;
    private float yPosBeforeJump;
    private float yPosBeforeFall;
    private float yPosBeforeDrop;
    private bool jumpBeforeTop=false;
    private bool inDropColdTime=false;
    private IEnumerator DropCold()
    {
        inDropColdTime = true;
        yield return new WaitForSeconds(0.5f);
        inDropColdTime = false;
    }


    private Rigidbody rb;
    private CapsuleCollider col;
    private float orgColHight;
    private Vector3 orgVectColCenter;
    void resetCollider()
    {
        // Collider的Height、Center值初期化
        col.height = orgColHight;
        col.center = orgVectColCenter;
    }
    //位置方向参数
    private Vector3 currentPosition;
    private Vector3 privousPosition;

    public bool hasAxisInput = true;
    private static float Haxis=0f;

    public GameObject charaMesh;
    public Gun gun;
    public Transform lookAtPos;
    private Transform launchPos;
    Quaternion frontRotation = Quaternion.Euler(0f, 115f, 0f);    //欧拉角到四元数变换  def:115&-115
    Quaternion backRotation = Quaternion.Euler(0f, -115f, 0f);
    Vector3 currentLookAtPos;
    private bool pFaceRight=true;


    //Start-----------------------------------------------------------------------------------------------------------
    void Start()
    {
        player = transform.GetComponent<Player>();
        am = GetComponent<AnimeManager>();
        launchPos= gun.launchPos.transform;

        jumpVelocity = player.jumpVelocity;
        moveSpeed = player.moveSpeed;
        runSpeed = player.moveSpeed * player.rumMultiple;

        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        col = GetComponent<CapsuleCollider>();
        orgColHight = col.height;
        orgVectColCenter = col.center;
        currentPosition = transform.position;
        encountingBackSpeed = defEncountingBackSpeed;
        firingBackSpeed = defFiringBackSpeed;
        //状态参数初始化
        player.isDroping = true;

    }

    //FixedUpdate-----------------------------------------------------------------------------------------------------------
    private void FixedUpdate()
    {
        //游戏状态判断
        if (currentPosition.y <= -30f)
            GameManager.gameOver = true;

        //回归速度值
        if (player.isStanding)
            moveSpeed = 0;
        if (player.isRunning)
            moveSpeed = player.moveSpeed * player.rumMultiple;
        if (player.isWalking)
            moveSpeed = player.moveSpeed;
        if (player.isJumping)
            moveSpeed = 0;
    }

    //Update-----------------------------------------------------------------------------------------------------------
    private void Update()
    {
        player.isWalking = false;
        player.isRunning = false;
        //前帧位置储存
        privousPosition = currentPosition;
        currentPosition = transform.position;
        //移动
        if (!player.stay)
            Haxis = Input.GetAxis("Horizontal");
        else
            Haxis = 0;
        //if (!Input.GetButton("Horizontal"))
        //{
        //    player.isStanding = true;
        //    player.isWalking = false;
        //}

        if (Haxis > 0f)                              //面向判定&改向
        {
            charaMesh.transform.rotation = frontRotation;
            player.faceRight = true;
        }

        if (Haxis <0f)                              //面向判定&改向
        {
            charaMesh.transform.rotation = backRotation;
            player.faceRight = false;
        }
        if (pFaceRight != player.faceRight)//每次转向执行一次,注视点X轴，导弹发射时的引导位置X轴反向
        {
            Vector3 localPos= new Vector3(-lookAtPos.localPosition.x, lookAtPos.localPosition.y,lookAtPos.localPosition.z);
            lookAtPos.localPosition = localPos;//注视点
            Vector3 launchPosLocal = new Vector3(-launchPos.localPosition.x, launchPos.localPosition.y, launchPos.localPosition.z);
            launchPos.localPosition = launchPosLocal;//导弹引导位置
            Vector3 gunLocalScale = new Vector3(-gun.transform.localScale.x, gun.transform.localScale.y, gun.transform.localScale.z);
            gun.transform.localScale = gunLocalScale;//枪
            gun.gunScale.x *= -1f;//Gun沿X轴镜像
            Vector3 gunPosVessel = gun.transform.localPosition;
            gunPosVessel.x *= -1f;
            gun.transform.localPosition = gunPosVessel;

            Vector3 handPosVessel = gun.rightHandPos.localPosition;//交换左右手在武器上的IK Position与Rotation
            gun.rightHandPos.localPosition = gun.leftHandPos.localPosition;
            gun.leftHandPos.localPosition = handPosVessel;
            Quaternion handRotVessel = gun.rightHandPos.localRotation;
            gun.rightHandPos.localRotation = gun.leftHandPos.localRotation;
            gun.leftHandPos.localRotation = handRotVessel;
            gun.faceRight *= -1f;//Gun反向判定更新

        }
        pFaceRight = player.faceRight;
        //走—停判定
        if (player.isGround)
        {
            if (Haxis != 0)
            {
                player.isStanding = false;
                player.isWalking = true;
            }
            else
            {
                player.isStanding = true;
                player.isWalking = false;
            }

        }
        //跑判定

        if (Input.GetButton("Run") && player.isWalking)
            {
                player.isWalking = false;
                player.isRunning = true;
            }
        else
            {
                player.isRunning = false;

            }

        if(player.isJumping||player.isFalling||player.isDroping)
            rb.useGravity = false;

        //下跳判定
        if (Input.GetButtonDown("Jump") && Input.GetAxis("Vertical") < 0)
            if (!inDropColdTime && player.isGround)
            {
                player.isGround = false;
                player.isDroping = true;
                yPosBeforeDrop = currentPosition.y;
                StartCoroutine(DropCold());
            }
        //跳跃判定
        if (Input.GetButtonDown("Jump") &&nextJump&&!player.isDroping&&!inDropColdTime)            //不能在落地前跳跃
            if (am.currentBaseState.fullPathHash == AnimeManager.walkingState ||
                am.currentBaseState.fullPathHash == AnimeManager.runningState ||
                am.currentBaseState.fullPathHash == AnimeManager.standingState)//不能在动画完成前跳跃
            {
                nextJump = false;//落地前无法再次起跳
                yPosBeforeJump = currentPosition.y;//储存起跳前的Y值
                player.isJumping = true;//进入跳跃状态
                jumpBeforeTop = true;
                if (player.isStanding)
                {
                    jumpV_x = 0;//处于站立状态时水平初速度为0
                }
                if (player.isWalking)
                {
                    jumpV_x = Haxis * moveSpeed;
                }
                if (player.isRunning)                //加速跳跃
                {
                    jumpV_x = Haxis * moveSpeed;
                    jumpVelocity = player.jumpVelocity * player.jumpMultiple;//加速跳跃时竖向分速度也提高
                }
            }     
    }

    //LateUpdate-----------------------------------------------------------------------------------------------------------
    private void LateUpdate()
    {
        transform.Translate(Vector3.right * Time.deltaTime * moveSpeed * Haxis); //角色移动实现
        if (player.isJumping)       //跳跃实现,自由落体运动
        {

            deltaJumpHight += jumpVelocity * Time.deltaTime * player.jumpSpeed;
            jumpVelocity = jumpVelocity - GameManager.gravity * Time.deltaTime * player.jumpSpeed;//自由落体公式
            currentPosition.y = yPosBeforeJump+deltaJumpHight;
            currentPosition.x = privousPosition.x + Time.deltaTime * jumpV_x; //空中水平移动实现
            transform.position = currentPosition;//更新位置
            if (jumpVelocity <= 0f)//判断跳跃是否到最高点
                jumpBeforeTop = false;
        }
        if(player.isFalling)       //坠落实现，平抛运动
        {
            deltaFallHight += fallVelocity * Time.deltaTime* player.jumpSpeed;
            fallVelocity -= GameManager.gravity * Time.deltaTime * player.jumpSpeed;
            currentPosition.y = yPosBeforeFall + deltaFallHight;
            currentPosition.x = privousPosition.x + Time.deltaTime * Haxis * moveSpeed; //空中水平移动实现
            transform.position = currentPosition;//更新位置
        }

        if (player.isDroping)       //掉落实现，竖直掉落
        {
            deltaDropHight += dropVelocity * Time.deltaTime * player.jumpSpeed*0.5f;
            dropVelocity -= GameManager.gravity * Time.deltaTime * player.jumpSpeed;
            currentPosition.y = yPosBeforeDrop + deltaDropHight;
            transform.position = currentPosition;//更新位置
        }

        if (isEncounting)
        {
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
        //后坐力
        if (player.isFiring&&!player.backStay)
        {
            firingBackSpeed = firingBackSpeed - firingAcceleration * Time.deltaTime;
            if (player.faceRight)
            {
                currentPosition.x = privousPosition.x - Time.deltaTime * firingBackSpeed;
            }
            else
            {
                currentPosition.x = privousPosition.x + Time.deltaTime * firingBackSpeed;
            }
            if (firingBackSpeed < 0.05)
            {
                isEncounting = false;
                player.backStay = true;
                firingBackSpeed = defFiringBackSpeed;
            }
            transform.position = currentPosition;//更新位置
        }
        if (player.isRunning)                //奔跑平滑加速实现
        {
            if (moveSpeed < runSpeed)
            {
                moveSpeed += accelerateSmooth * Time.deltaTime;
            }
            else
            {
                moveSpeed = runSpeed;
            }
        }
        else if (moveSpeed > player.moveSpeed)            //减速实现
            moveSpeed -= accelerateSmooth * Time.deltaTime;
        else
            moveSpeed = player.moveSpeed;

        Haxis = 0;
        

    }



    //碰撞器-----------------------------------------------------------------------------------------------------------
    void OnCollisionEnter(Collision collider)
    {
        //落地检测
        if (collider.gameObject.tag == "Ground"&&!jumpBeforeTop&&!player.isGround)
        {
            float posChecker = transform.position.y - collider.transform.position.y;
            if (posChecker > -0.3 && posChecker < 0.3) //若为底部碰撞
            {
                nextJump = true;
                player.isGround = true;
                player.isFalling = false;
                player.isJumping = false;
                player.isDroping = false;

                //落地还原速度
                moveSpeed = player.moveSpeed;
                jumpVelocity = player.jumpVelocity;
                fallVelocity = 0;
                dropVelocity = 0;
                deltaJumpHight = 0;
                deltaFallHight = 0;
                deltaDropHight = 0;
                jumpV_x = 0;

                rb.useGravity = true;
                resetCollider();
                Debug.Log("ground!" + posChecker.ToString());
            }
        }
        //遇敌退后
        if (collider.gameObject.tag == "Enemy")
        {
            isEncounting = true;
            if (transform.position.x - collider.gameObject.transform.position.x > 0)
            {
                leftEncounting = false;
            }
            if (transform.position.x - collider.gameObject.transform.position.x < 0)
            {
                leftEncounting = true;
            }
            else
                return;

        }
    }
    void OnCollisionExit(Collision collider)
    {
        float posChecker = transform.position.y - collider.transform.position.y;
        //离地检测
        if (collider.gameObject.tag == "Ground"&&posChecker>0f&&player.isGround)
        {
            player.isGround = false;
            player.isStanding = false;
            player.isRunning = false;
            player.isWalking = false;
            //判断是否坠落
            if (!player.isJumping&&!player.isDroping)
            {
                player.isFalling = true;
                currentPosition= transform.position;
                yPosBeforeFall = currentPosition.y;
                Debug.Log("Falling!");
            }
            Debug.Log("offground!"+posChecker.ToString());
        }
    }
}

