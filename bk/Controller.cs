using UnityEngine;
using System.Collections;


public class Controller : MonoBehaviour {

    //moveindex
    static float defMoveSpeed = 8;
    static float moveSpeed = defMoveSpeed;

    //state define
    enum MoveState : int
    {
        Stand,
        Move,
        Run,
        Jump,

    }
    static MoveState state = MoveState.Stand;
    //jump index
    static float g = 80f;

    static bool jumping = false;
    static float v0 = 20;
    static float v = v0;
    static float deltaH = 0;
    static float j_t1 = 0;
    static float j_t2 = 0;

    public void jump()
    {

    }


    void Start () {
	
	}

	void Update () {



        float Haxis = Input.GetAxis("Horizontal");
        //移动物体
        if (Haxis != 0)
        {
            
            transform.Translate(Vector3.right * Time.deltaTime * moveSpeed * Haxis);
            state = MoveState.Move;

        }
        else
            state = MoveState.Stand;
        //跳跃
        if (Input.GetButtonDown("Jump")&&!jumping)
        {
            state = MoveState.Jump;
            jumping = true;

        }


        //跑
        if (state == MoveState.Move&&!jumping)
            if (Input.GetButton("Run"))
                state = MoveState.Run;
            else
                 {
             
                    state = MoveState.Move;
                    moveSpeed = defMoveSpeed;
                 }
               
            
            else;      
           
        

     }

    void LateUpdate()
    {
        Vector3 position = transform.position;

        if (state == MoveState.Run)
        {
            moveSpeed = defMoveSpeed * 1.6f;

        }
        
        
        if (jumping)
        {
            j_t1 = j_t2;
            j_t2 += Time.deltaTime;
            if (v + v0 <= 0)
            {
                state = MoveState.Stand;
                jumping = false;
                j_t1 = j_t2 = 0;
                v = v0;
     

            }
            else

            {
                deltaH = v0 * (j_t2 - j_t1) + 0.5f * g * (j_t1 * j_t1 - j_t2 * j_t2);

                v = v - g * (j_t2-j_t1);
                position.y += deltaH;
                transform.position = position;

            }




          }

    }


}

