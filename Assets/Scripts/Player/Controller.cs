﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {

    public Player player;
    //private AnimeManager am;
    public GameObject charaMesh;

    private float moveAxis = 0f;
    private float moveSpeed = 8.0f;
    Quaternion frontRotation = Quaternion.Euler(0f, 115f, 0f);    //欧拉角到四元数变换  def:115&-115
    Quaternion backRotation = Quaternion.Euler(0f, 245f, 0f);
    bool grounded = false;
    bool facingRight = false;

    private float runSpeed = 0f;
    public float jumpForce = 50000f;

    private Rigidbody rb; // Reference to rigidbody
    private LayerMask environmentLayerMask; // Layer for raycast to hit
    private float lastDistance; // The last distance between the player and the environment (directly down) 

    // Use this for initialization
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        charaMesh = GameObject.Find("PlayerMesh");
        environmentLayerMask = LayerMask.GetMask("Environment");
        player = transform.GetComponent<Player>();
        //am = GetComponent<AnimeManager>();
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
        if (!player.stay)
            moveAxis = Input.GetAxis("Horizontal");
        else
            moveAxis = 0;

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

    private void LateUpdate()
    {
        if (grounded)
        {
            this.transform.Translate(Vector3.right * Time.deltaTime * moveSpeed * moveAxis); //角色移动实现
        }
    }

    void FixedUpdate()
    {
        // Check if the user is grounded
        if (rb.velocity.z <= 0)
        {
            bool hit = Physics.Raycast(rb.position - new Vector3(0f, 0f, 0.5f), Vector3.down, 0.2f, environmentLayerMask);
            if (hit)
            {
                grounded = true;
            }
        }
        else
        {
            grounded = false;
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
        rb.AddForce(new Vector3(0, 0, force));
    }
	
}
