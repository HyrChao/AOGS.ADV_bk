using UnityEngine;
using System.Collections;


public class Controller : MonoBehaviour {

    //moveindex
    static float defMoveSpeed = 8;
    static float moveSpeed = defMoveSpeed;

    //state define
    enum State : int
    {
        Stand,
        Move,
        Run,
        Jump,

    }
    static State state = State.Stand;
    //jump index
    static float g = 80f;

    static bool jumping = false;
    static float v0 = 20;
    static float v = v0;
    static float deltaH = 0;
    static float j_t1 = 0;
    static float j_t2 = 0;




    void Start () {
	
	}

	void Update () {



        float Haxis = Input.GetAxis("Horizontal");
        //移动物体
        if (Haxis != 0)
        {
            
            transform.Translate(Vector3.right * Time.deltaTime * moveSpeed * Haxis);
            state = State.Move;

        }
        else
            state = State.Stand;
        //跳跃
        if (Input.GetButtonDown("Jump")&&!jumping)
        {
            state = State.Jump;
            jumping = true;

        }


        //跑
        if (state == State.Move&&!jumping)
            if (Input.GetButton("Run"))
                state = State.Run;
            else
                 {
             
                    state = State.Move;
                    moveSpeed = defMoveSpeed;
                 }
               
            
            else;      
           
        

     }

    void LateUpdate()
    {
        Vector3 position = transform.position;

        if (state == State.Run)
        {
            moveSpeed = defMoveSpeed * 1.6f;

        }
        
        
        if (jumping)
        {
            j_t1 = j_t2;
            j_t2 += Time.deltaTime;
            if (v + v0 <= 0)
            {
                state = State.Stand;
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

