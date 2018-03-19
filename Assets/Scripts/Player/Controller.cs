using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {

    public Player player;
    private AnimeManager am;
    private float moveSpeed;
    private float runSpeed;


    public float jumpForce = 50000f;

    private Rigidbody rb; // Reference to rigidbody
    private LayerMask environmentLayerMask; // Layer for raycast to hit
    private float lastDistance; // The last distance between the player and the environment (directly down)
                                // Use this for initialization
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        environmentLayerMask = LayerMask.GetMask("Environment");
    }
    // Update is called once per frame for physics
    void FixedUpdate()
    {
        // Check if the user is attempting to jump
        float jumpAxis = Input.GetAxisRaw("Jump");
        if (jumpAxis != 0 && rb.velocity.y <= 0)
        {
            // Raycast from the feet of the player directly down (or the origin, doesn't matter)
            bool hit = Physics.Raycast(rb.position - new Vector3(0f, 0f,0.5f), Vector3.down, 0.2f, environmentLayerMask);
            // If the raycast hit something
            if (hit)
            {
                Jump(jumpAxis * jumpForce * Time.deltaTime);
            }
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

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
