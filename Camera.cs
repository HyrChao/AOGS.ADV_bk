
using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour
{
    public Transform pos;   //摄像机要跟随的人物
    private float smoothTime = 0.15f;  //摄像机平滑移动的时间
    public float distance = -8f;
    private Vector3 cameraVelocity = Vector3.zero;

    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, pos.position + new Vector3(0, 0, distance), ref cameraVelocity, smoothTime);
        //transform.LookAt(pos.position);
    }

}