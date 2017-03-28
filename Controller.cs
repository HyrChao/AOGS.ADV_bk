// 2017/03/27 by Chao
// Unity5 (only)

using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {

    private const float jumpSpeed=GameManager.jumpSpeed;
    private float jumpVelocity = GameManager.jumpVelocity;
    private float moveSpeed= GameManager.moveSpeed;
    private float runSpeed= GameManager.moveSpeed * GameManager.rumMultiple;
    private float jumpHight = 0;
    private bool nextJump = false;
    private float jumpV_x = 0;
    //private  bool isDecelerating = false;
    //private  bool isAccelerating = false;
    private float accelerateSmooth = 2.5f;

    //面向方向参数
    static Vector3 currentPosition;
    static Vector3 privousPosition;
    Quaternion frontRotation = Quaternion.Euler(0f, 115f, 0f);    //欧拉角到四元数变换  def:115&-115
    Quaternion backRotation =  Quaternion.Euler(0f, -115f, 0f);
 
    public bool hasAxisInput = true;
    private static float Haxis=0f;

    public GameObject charaMesh;

    //获取动画状态
    private Animator anim;
    private AnimatorStateInfo currentBaseState;
    static int standingState = Animator.StringToHash("Base Layer.Standing");//将参数转为hash
    static int walkingState = Animator.StringToHash("Base Layer.Walking");
    static int runningState = Animator.StringToHash("Base Layer.Running");
    static int jump0State = Animator.StringToHash("Base Layer.Jumping0");
    static int jump1State = Animator.StringToHash("Base Layer.Jumping1");
    static int jump2State = Animator.StringToHash("Base Layer.Jumping02");

    //Update每帧调用
    void Start()
    {
        anim = transform.FindChild("Misaki").GetComponent<Animator>();
        currentPosition = transform.position;
    }
    //Update
    void Update()
    {
        //前帧位置储存
        privousPosition = currentPosition;
        currentPosition = transform.position;
        //移动
        Haxis = Input.GetAxis("Horizontal");

        if (Haxis > 0f)                              //面向判定&改向
        {
            charaMesh.transform.rotation = frontRotation;
         }

        if (Haxis <0f)                              //面向判定&改向
        {
            charaMesh.transform.rotation = backRotation;
        }
        //走—停判定
        if (!GameManager.isJumping)
            if (Haxis > 0.1f || Haxis < -0.1f)
            {
                GameManager.isStanding = false;
                GameManager.isWalking = true;
            }
            else
            {
                GameManager.isStanding = true;
                GameManager.isWalking = false;
            }

        //跑判定

        if (Input.GetButton("Run") && GameManager.isWalking)
            {
                GameManager.isWalking = false;
                GameManager.isRunning = true;
            }
        else
            {
                GameManager.isRunning = false;

            }
        
        //跳跃判定
        if (Input.GetButtonDown("Jump") &&nextJump)            //不能在落地前跳跃
            if (currentBaseState.fullPathHash == walkingState|| 
                currentBaseState.fullPathHash == runningState||
                currentBaseState.fullPathHash == standingState)//不能在动画完成前跳跃
            {
                nextJump = false;
                GameManager.isJumping = true;
                if (GameManager.isStanding)
                {
                    jumpV_x = 0;
                    GameManager.isStanding = false;
                }
                if (GameManager.isWalking)
                {
                    jumpV_x = Haxis * moveSpeed;
                    GameManager.isWalking = false;
                }
                if (GameManager.isRunning)                //加速跳跃
                {
                    jumpV_x = Haxis * moveSpeed;
                    jumpVelocity = GameManager.jumpVelocity * GameManager.jumpMultiple;
                    GameManager.isRunning = false;
                }
            }
        
           
        
    }

    //LateUpdate
    private void LateUpdate()
    {
        transform.Translate(Vector3.right * Time.deltaTime * moveSpeed * Haxis); //角色移动实现
        if (GameManager.isJumping)                   //跳跃实现
        {

            jumpHight += jumpVelocity * Time.deltaTime * jumpSpeed;
            jumpVelocity = jumpVelocity - 9.8f * Time.deltaTime * jumpSpeed;
            currentPosition.y = jumpHight;
            currentPosition.x = privousPosition.x + Time.deltaTime * jumpV_x; //空中水平移动实现
            transform.position = currentPosition;

        }
        if (GameManager.isRunning)                //奔跑平滑加速实现
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
        else if (moveSpeed > GameManager.moveSpeed)            //减速实现
            moveSpeed -= accelerateSmooth * Time.deltaTime;
        else
            moveSpeed = GameManager.moveSpeed;

    }

    //FixedUpdate
    private void FixedUpdate()
    {
        currentBaseState = anim.GetCurrentAnimatorStateInfo(0);//更新动画状态
        
        //回归速度值
        if (GameManager.isStanding)
            moveSpeed = 0;
        if(GameManager.isRunning)
            moveSpeed = GameManager.moveSpeed * GameManager.rumMultiple;
        if (GameManager.isWalking)
            moveSpeed = GameManager.moveSpeed;
        if (GameManager.isJumping)
            moveSpeed = 0;

        //if (!anim.IsInTransition(0))
        //{


        //}

        //将游戏管理器中角色状态值赋予动画组件
        anim.SetBool("isStanding", GameManager.isStanding);
        anim.SetBool("isRunning", GameManager.isRunning);
        anim.SetBool("isWalking", GameManager.isWalking);
        anim.SetBool("isJumping", GameManager.isJumping);

    }

    //碰撞器
    void OnCollisionEnter(Collision collider)
    {
        //落地检测
        if (collider.gameObject.tag == "Ground")
        {
            nextJump = true;
            GameManager.isGround = true;
            GameManager.isJumping = false;

            //落地还原速度
            moveSpeed = GameManager.moveSpeed;           
            jumpVelocity = GameManager.jumpVelocity;
            jumpHight = 0;
            jumpV_x = 0;

            Debug.Log("ground!");
        }
    }
    void OnCollisionExit(Collision collider)
    {
        //离地检测
        if (collider.gameObject.tag == "Ground")
            GameManager.isGround = false;
        Debug.Log("offground!");
    }
}

