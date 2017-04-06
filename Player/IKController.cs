
//2017/4/3
//By Chao

using UnityEngine;
using System.Collections;

public class IKController : MonoBehaviour
{
    private Animator avator;
    public Gun gun;
    private Transform gunMesh;
    public bool ikActive = false;
    void Start()
    {
        avator = GetComponent<Animator>();
        gunMesh = gun.transform.FindChild("GunMesh");
    }
    void OnAnimatorIK(int layerIndex)
    {
        if (avator)
        {
            if (layerIndex == 0)
            {
                if (gun != null)
                {
                    Vector3 scale = gun.gunScale*gun.laod;
                    gunMesh.localScale = scale;
                }
            }
            if (layerIndex == 2)
            {

                if (gun.leftHandPos != null)
                {
                    avator.SetIKPosition(AvatarIKGoal.LeftHand, gun.leftHandPos.position);//左手IKposition
                    avator.SetIKRotation(AvatarIKGoal.LeftHand, gun.leftHandPos.rotation);
                    avator.SetIKPositionWeight(AvatarIKGoal.LeftHand, gun.laod);
                    avator.SetIKRotationWeight(AvatarIKGoal.LeftHand, gun.laod);
                }
                if (gun.rightHandPos != null)
                {
                    avator.SetIKPosition(AvatarIKGoal.RightHand, gun.rightHandPos.position);//左手IKposition
                    avator.SetIKRotation(AvatarIKGoal.RightHand, gun.rightHandPos.rotation);
                    avator.SetIKPositionWeight(AvatarIKGoal.RightHand, gun.laod);
                    avator.SetIKRotationWeight(AvatarIKGoal.RightHand, gun.laod);
                }
            }
        }
    }
}
