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

    private float moveAxis = 0f;
    private float moveSpeed = 8.0f;
    Quaternion frontRotation = Quaternion.Euler(0f, 115f, 0f);    //欧拉角到四元数变换  def:115&-115
    Quaternion backRotation = Quaternion.Euler(0f, 245f, 0f);
    float gravityCenterHeight = 0f;
    float groundRaycastDist = 3f;
    bool grounded = false;
    bool facingRight = false;


    private float runSpeed = 0f;
    public float jumpForce = 50f;

    private Rigidbody rb; // Reference to rigidbody
    private int environmentLayerMask; // Layer for raycast to hit
    private float lastDistance; // The last distance between the player and the environment (directly down) 

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
        runSpeed = 1.6f * moveSpeed;
    }
    // Update is called once per frame for physics
    private void Update()
    {
        //获取输入
        moveAxis = Input.GetAxis("Horizontal");

        if (moveAxis != 0)
        {
            player.state.walking = true;

            if (moveAxis > 0f)                              //面向判定&改向
            {
                charaMesh.transform.rotation = frontRotation;
                facingRight = true;
            }

            if (moveAxis < 0f)                              //面向判定&改向
            {
                charaMesh.transform.rotation = backRotation;
                facingRight = false;
            }
        }
        else
        {
            player.state.walking = false;
        }
        debug = grounded;
        //Debug.Log(grounded.ToString());
        Debug.Log(debug.ToString());
        //Debug.Log(Vector3.down.ToString());
    }

    private void LateUpdate()
    {
        if (grounded)
        {
            transform.Translate(Vector3.right * Time.deltaTime * moveSpeed * moveAxis); //角色移动实现
        }
    }

    void FixedUpdate()
    {
        // Check if the user is grounded
        bool hit = Physics.Raycast(transform.position - new Vector3(0, gravityCenterHeight - groundRaycastDist, 0), new Vector3(0f, -1f, 0f), groundRaycastDist, environmentLayerMask);
        if (hit)
        {
            grounded = true;
        }
        else
        {
            grounded = false;
            if (rb.velocity.y < 0)
            {
                player.state.falling = true;
            }
        }

        float jumpAxis = Input.GetAxisRaw("Jump");
        if (jumpAxis != 0 && grounded)
        {
            Jump(jumpAxis * jumpForce * Time.deltaTime);
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
