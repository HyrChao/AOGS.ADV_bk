//2017/2/21
//by Chao
//摄像机自动跟随
using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour
{
    public Transform pos;   //摄像机要跟随的人物
    private float smoothTime = 0.1f;  //摄像机平滑移动的时间
    public float distance = -12f;
    private Vector3 cameraVelocity = Vector3.zero;
    //private Vector3 lerpVelocity = Vector3.zero;
    private bool currentFaceDir;
    private void Start()
    {

    }
    void Update()
    {
        //if(gm.player.faceRight==currentFaceDir)
        transform.position = Vector3.SmoothDamp(transform.position, pos.position + new Vector3(0, 0, distance), ref cameraVelocity, smoothTime);
        //else
        //{
        //    transform.position = Vector3.SmoothDamp(transform.position, pos.position + new Vector3(0, 0, distance), ref lerpVelocity, 10000f);
        //    Debug.Log("Camera Lerp");
        //}
        //currentFaceDir = gm.player.faceRight;

        //transform.LookAt(pos.position);

    }

}