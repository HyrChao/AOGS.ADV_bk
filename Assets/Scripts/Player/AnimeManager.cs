//2017/4/2
//By Chao
//角色动画管理器，将角色的动画状态实时更新，将角色的状态值更新到ANIMATOR
//IK控制器

using UnityEngine;
using System.Collections;
public class AnimeManager : MonoBehaviour {

    //获取动画状态
    private Animator anim;
    private Player player;
    private Gun gun;
    private IKLookAt IKLook;
    public AnimatorStateInfo currentBaseState;
    static public int standingState = Animator.StringToHash("Base Layer.Standing");//将参数转为hash
    static public int walkingState = Animator.StringToHash("Base Layer.Walking");
    static public int runningState = Animator.StringToHash("Base Layer.Running");
    static public int jump0State = Animator.StringToHash("Base Layer.Jumping0");
    static public int jump1State = Animator.StringToHash("Base Layer.Jumping1");
    static public int jump2State = Animator.StringToHash("Base Layer.Jumping2");
    



    void Start ()
    {
        anim = transform.FindChild("Misaki").GetComponent<Animator>();
        player = GetComponent<Player>();
        gun= transform.FindChild("Gun").GetComponent<Gun>();
        IKLook = transform.FindChild("Misaki").GetComponent<IKLookAt>();
    }
    private void FixedUpdate()
    {
        //if (!anim.IsInTransition(0))
        //将游戏管理器中角色状态值赋予动画组件
        //currentBaseState = anim.GetCurrentAnimatorStateInfo(0);//更新动画状态
        currentBaseState = anim.GetCurrentAnimatorStateInfo(0);//更新动画状态
        anim.SetBool("isUpgrade", player.upgraded);
        anim.SetBool("isDamaged", player.isDamaged);
        anim.SetBool("isDying", player.isDying);
        anim.SetBool("isAttacking", player.isAttacking);
        anim.SetBool("isFiring", player.isFiring);
        anim.SetBool("isStanding", player.isStanding);
        anim.SetBool("isRunning", player.isRunning);
        anim.SetBool("isWalking", player.isWalking);
        anim.SetBool("isJumping", player.isJumping);
    }
    void Update ()
    {
        if (player.isFiring)
            gun.laod = 1;
        else
            gun.laod = 0;

        if (anim == null)
            return;
        if (player.isFiring || player.isAttacking)
        {
            IKLook.ikActive = true;
        }
        else
            IKLook.ikActive = false;


    }
    //IK更新
    void OnAnimatorIK(int layerIndex)
    {
        Debug.Log("OnAnimatorIK");
        //if (layerIndex == 0)
        //{
        //    if (gun != null)
        //    {
        //        Vector3 scale = gun.gunScale * gun.laod;
        //        gun.transform.localPosition = gun.gunPos;
        //        gun.transform.localScale = scale;
        //    }
        //}
        //if (layerIndex == 2)
        //{

        //    if (gun.leftHandPos != null)
        //    {
        //        anim.SetIKPosition(AvatarIKGoal.LeftHand, gun.leftHandPos.position);//左手IKposition
        //        anim.SetIKRotation(AvatarIKGoal.LeftHand, gun.leftHandPos.rotation);
        //        anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, gun.laod);
        //        anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, gun.laod);
        //    }
        //    if (gun.rightHandPos != null)
        //    {
        //        anim.SetIKPosition(AvatarIKGoal.RightHand, gun.rightHandPos.position);//左手IKposition
        //        anim.SetIKRotation(AvatarIKGoal.RightHand, gun.rightHandPos.rotation);
        //        anim.SetIKPositionWeight(AvatarIKGoal.RightHand, gun.laod);
        //        anim.SetIKRotationWeight(AvatarIKGoal.RightHand, gun.laod);
        //    }
        //}
    }
}
