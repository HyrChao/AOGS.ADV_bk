//2018/03/23
//by Chao

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {

    private bool debug;

    private Player player;
    //private AnimeManager am;
    private GameObject charaMesh;
    private CapsuleCollider col;
    private GameManager gm;

    private float moveAxisX = 0f;
    private float moveAxisZ = 0f;
    private float jumpAxis = 0f;

    private float acceleration = 10f;
    private float speed = 10f;
    private float moveSpeed = 10f;
    private float runSpeed;
    private float speedMulti = 1.8f;
    private float jumpForce = 12f;
    private Vector3 moveDir = Vector3.zero;

    Quaternion frontRotation = Quaternion.Euler(0f, 115f, 0f);    //欧拉角到四元数变换  def:115&-115
    Quaternion backRotation = Quaternion.Euler(0f, 245f, 0f);

    private int environmentLayerMask; // Layer for raycast to hit
    private float gravityCenterHeight = 0f;
    private float groundRaycastDist = 3f;
    private float groundRaycastLapse = -0.5f;
    private bool grounded = false;
    private bool facingRight = false;

    private Rigidbody rb; // Reference to rigidbody

    // Use this for initialization
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        charaMesh = GameObject.Find("PlayerMesh");
        environmentLayerMask = LayerMask.GetMask("Ground");
        player = transform.GetComponent<Player>();
        col = GetComponent<CapsuleCollider>();
        //am = GetComponent<AnimeManager>();
        gravityCenterHeight = col.height / 2;
        debug = false;
    }

    // Use this for initialization
    void Start()
    {
        runSpeed = speedMulti * moveSpeed;
    }
    // Update is called once per frame for physics
    private void Update()
    {
        //获取输入
        moveAxisX = Input.GetAxis("Horizontal");
        moveAxisZ = Input.GetAxis("Vertical");
        jumpAxis = Input.GetAxisRaw("Jump");

        if (Input.GetButton("Run"))
        {
            speed = runSpeed;
        }
        else
        {
            speed = moveSpeed;
        }

        moveDir = new Vector3(moveAxisX, 0f, moveAxisZ);
        moveDir = transform.TransformDirection(moveDir);
        //moveDir.y = 0;
        moveDir *= speed;

        if(AO.Mode == Mode._2D)
        {
            moveDir.z = 0; //Lock z position
            if (moveAxisX != 0)
            {
                if (moveAxisX > 0f)                              //面向判定&改向
                {
                    charaMesh.transform.rotation = frontRotation;
                    facingRight = true;
                }

                if (moveAxisX < 0f)                              //面向判定&改向
                {
                    charaMesh.transform.rotation = backRotation;
                    facingRight = false;
                }
            }
        }

        else if (AO.Mode == Mode._3D)
        {

        }

        transform.Translate(moveDir * Time.deltaTime);  //移动实现

        //Refresh player states
        if (grounded)
        {
            player.state = PlayerState.Idle;
            if (moveDir != Vector3.zero)
                player.state = PlayerState.Walking;
            if (speed > moveSpeed)
                player.state = PlayerState.Running;
        }
        else
        {
            if (rb.velocity.y <= 0)
                player.state = PlayerState.Falling;
            else
                player.state = PlayerState.Jumping;
        }

        debug = grounded;
        Debug.Log(player.state.ToString());
        //Debug.Log(debug.ToString());

    }

    private void LateUpdate()
    {




        //if (grounded)
        //{
        //    transform.Translate(Vector3.right * Time.deltaTime * moveSpeed * moveAxis); //角色移动实现
        //}
    }

    void FixedUpdate()
    {
        // Check if the user is grounded
        bool hit = Physics.Raycast(transform.position - new Vector3(0, gravityCenterHeight - groundRaycastDist + groundRaycastLapse , 0), new Vector3(0f, -1f, 0f), groundRaycastDist, environmentLayerMask);
        if (hit)
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }

        if (jumpAxis > 0 && grounded && rb.velocity.y >= 0)
        {
            //Jump(jumpForce*10000);
            Jump(jumpForce*10000f * Time.deltaTime);
        }
    }

    void Jump(float force)
    {
        if (force < 0)
        {
            return;
        }

        rb.AddForce(new Vector3(0, force, 0));
    }
	
}
