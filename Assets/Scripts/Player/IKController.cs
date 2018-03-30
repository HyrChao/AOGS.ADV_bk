
//2017/4/3
//By Chao

using UnityEngine;
using System.Collections;

public class IKController : MonoBehaviour
{
    private Animator avator;
    public Launcher launcher;
    private Transform gunMesh;
    public bool ikActive = false;
    void Start()
    {
        avator = GetComponent<Animator>();
        gunMesh = launcher.transform.Find("GunMesh");
    }
    void OnAnimatorIK(int layerIndex)
    {
        if (avator)
        {
            if (layerIndex == 0)
            {
                if (launcher != null)
                {
                    Vector3 scale = launcher.transform.localScale* launcher.Load;
                    gunMesh.localScale = scale;
                }
            }
            if (layerIndex == 2)
            {

                if (launcher.leftHandPos != null)
                {
                    avator.SetIKPosition(AvatarIKGoal.LeftHand, launcher.leftHandPos.position);//左手IKposition
                    avator.SetIKRotation(AvatarIKGoal.LeftHand, launcher.leftHandPos.rotation);
                    avator.SetIKPositionWeight(AvatarIKGoal.LeftHand, launcher.Load);
                    avator.SetIKRotationWeight(AvatarIKGoal.LeftHand, launcher.Load);
                }
                if (launcher.rightHandPos != null)
                {
                    avator.SetIKPosition(AvatarIKGoal.RightHand, launcher.rightHandPos.position);//左手IKposition
                    avator.SetIKRotation(AvatarIKGoal.RightHand, launcher.rightHandPos.rotation);
                    avator.SetIKPositionWeight(AvatarIKGoal.RightHand, launcher.Load);
                    avator.SetIKRotationWeight(AvatarIKGoal.RightHand, launcher.Load);
                }
            }
        }
    }
}
