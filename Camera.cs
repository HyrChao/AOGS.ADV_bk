using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour {

    public Transform character;   //摄像机要跟随的人物
    public float smoothTime = 0.01f;  //摄像机平滑移动的时间
    private Vector3 cameraVelocity = Vector3.zero;


  

    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, character.position + new Vector3(0, 0, -5), ref cameraVelocity, smoothTime);
    }

}
