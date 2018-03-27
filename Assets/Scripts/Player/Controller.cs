//2018/03/23
//by Chao

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {

    //Debug
    private bool debug;

    //Referances
    private Player player;
    private GameObject charaMesh;
    private CapsuleCollider col;
    private GameManager gm;
    private Rigidbody rb;

    //Store axis value
    private float moveAxisX = 0f;
    private float moveAxisZ = 0f;

    //Parameters for movement
    private float acceleration = 10f;
    private float speed = 10f;
    private float moveSpeed = 6f;
    private float runSpeed;
    private float speedMulti = 1.8f;
    Quaternion frontRotation = Quaternion.Euler(0f, 115f, 0f);    //欧拉角到四元数变换  def:115&-115
    Quaternion backRotation = Quaternion.Euler(0f, 245f, 0f);

    //Parameters for jumping
    private int jumpPower = 0;
    private bool jumpJudging = false;
    private int jumpHeldFrame = 0; //Frames used to check how long jump key player hold down
    private float jumpForce = 8f;
    private bool canJump = false;
    private Vector3 moveDir = Vector3.zero;

    //Parameters for ground check
    private int environmentLayerMask; // Layer for raycast to hit
    private float gravityCenterHeight = 0f;
    private float groundRaycastDist = 3f;
    private float groundRaycastLapse = 0f;
    private bool grounded = false;
    private bool facingRight = false;

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
        //Get player input
        moveAxisX = Input.GetAxis("Horizontal");
        moveAxisZ = Input.GetAxis("Vertical");

        if (Input.GetButton("Jump"))
        {
            jumpJudging = false;
            ++jumpHeldFrame;
        }
            
        else
            jumpHeldFrame = 0;

        if (Input.GetButton("Run"))
        {
            speed = runSpeed;
        }
        else
        {
            speed = moveSpeed;
        }

        moveDir.Set(moveAxisX, 0f, moveAxisZ);
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

        //Movement
        transform.Translate(moveDir * Time.deltaTime);  

        //Jump
        if (jumpAxis > 0 && grounded && canJump)
        {
            //Jump(jumpForce*10000);
            if (jumpAxis < 0.1)
                jumpAxis = 0.1f;
            Jump(jumpForce * 10000f * Time.deltaTime);
        }



        debug = grounded;
        Debug.Log(grounded);
        //Debug.Log(debug.ToString());

    }

    private void LateUpdate()
    {




        //if (grounded)
        //{
        //    transform.Translate(Vector3.right * Time.deltaTime * moveSpeed * moveAxis); 
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

        //Refresh player states
        if (grounded)
        {
            player.state = PlayerState.Idle;
            if (moveDir != Vector3.zero)
            {
                player.state = PlayerState.Walking;
                if (speed > moveSpeed)
                    player.state = PlayerState.Running;
            }
            if (rb.velocity.y == 0)
                canJump = true;
        }
        else
        {
            if (rb.velocity.y <= 0)
                player.state = PlayerState.Falling;
            else
            {
                player.state = PlayerState.Jumping;
                canJump = false;
            }

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
