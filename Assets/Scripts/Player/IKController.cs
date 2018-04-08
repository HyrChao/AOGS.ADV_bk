
//2017/4/3
//By Chao

using UnityEngine;
using System.Collections;

public class IKController : MonoBehaviour
{
    private Animator avator;
    private Launcher launcher;
    void Start()
    {
        avator = AO.animeManager.Animator;
        launcher = AO.player.launcher;
    }
    void OnAnimatorIK(int layerIndex)
    {
        AO.hud.Msg(launcher.currentAmmmo.ToString());
        Debug.Log(launcher.ToString());
        if (avator)
        {
            if (layerIndex == 0)
            {

            }
            if (layerIndex == 2)
            {
                //if (launcher != null)
                //{
                //    launcher.transform.position = AO.Slot.laucherLaunchSlot.position;
                //}
                //AO.HUD.Msg("IKIKIKIKIK");
                //Debug.Log(launcher.transform.position.ToString());

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
