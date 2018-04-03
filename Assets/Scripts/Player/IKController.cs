
//2017/4/3
//By Chao

using UnityEngine;
using System.Collections;

public class IKController : MonoBehaviour
{
    private Animator avator;
    private Launcher launcher;
    public bool ikActive = false;
    void Start()
    {
        avator = AO.gm.AM.Animator;
        launcher = AO.gm.Player.launcher;
    }
    void OnAnimatorIK(int layerIndex)
    {
        AO.gm.HUD.Msg("IKIKIKIKIK");

        if (avator)
        {
            if (layerIndex == 0)
            {

            }
            if (layerIndex == 2)
            {
                //if (launcher != null)
                //{
                //    launcher.transform.position = AO.gm.Slot.laucherLaunchSlot.position;
                //}
                AO.gm.HUD.Msg("IKIKIKIKIK");

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
