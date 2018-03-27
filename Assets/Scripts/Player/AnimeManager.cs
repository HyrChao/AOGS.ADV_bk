//2017/4/2
//By Chao
//角色动画管理器，将角色的动画状态实时更新，将角色的状态值更新到ANIMATOR
//IK控制器

using UnityEngine;
using System.Collections;
public class AnimeManager : MonoBehaviour {

    //获取动画状态
    private Animator animator;
    private Player player;
    //private IKLookAt IKLook;
    public AnimatorStateInfo currentBaseState;

    //static public int standingState = Animator.StringToHash("Base Layer.Standing");//将参数转为hash
    //static public int walkingState = Animator.StringToHash("Base Layer.Walking");
    //static public int runningState = Animator.StringToHash("Base Layer.Running");
    //static public int jump0State = Animator.StringToHash("Base Layer.Jumping0");
    //static public int jump1State = Animator.StringToHash("Base Layer.Jumping1");
    //static public int jump2State = Animator.StringToHash("Base Layer.Jumping2");
    
    void Start ()
    {
        animator = transform.FindChild("PlayerMesh").GetComponent<Animator>();
        player = GetComponent<Player>();
        //IKLook = transform.FindChild("PlayerMesh").GetComponent<IKLookAt>();
    }
    private void FixedUpdate()
    {
        animator.SetInteger("State", player.state.GetHashCode());

        //if (!animator.IsInTransition(0))
        //将游戏管理器中角色状态值赋予动画组件
        //currentBaseState = animator.GetCurrentAnimatorStateInfo(0);//更新动画状态


        //animator.SetBool("isUpgrade", player.state.upgraded);
        //animator.SetBool("isDamaged", player.state.damaged);
        //animator.SetBool("isDying", player.state.dying);
        //animator.SetBool("isAttacking", player.state.attacking);
        //animator.SetBool("isFiring", player.state.firing);
        //animator.SetBool("isStanding", player.state.standing);
        //animator.SetBool("isRunning", player.state.running);
        //animator.SetBool("isWalking", player.state.walking);
        //animator.SetBool("isJumping", player.state.jumping);
    }
    void Update ()
    {
        if (player.state.GetHashCode()==4)
            animator.SetTrigger("Jump");
        if (AO.GameOver)
            animator.SetTrigger("Die");
    }
    private void LateUpdate()
    {
        //currentBaseState = animator.GetCurrentAnimatorStateInfo(0);//更新动画状态
    }

    //IK更新
    void OnAnimatorIK(int layerIndex)
    {
        //Debug.Log("OnAnimatorIK");
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
        //        animator.SetIKPosition(AvatarIKGoal.LeftHand, gun.leftHandPos.position);//左手IKposition
        //        animator.SetIKRotation(AvatarIKGoal.LeftHand, gun.leftHandPos.rotation);
        //        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, gun.laod);
        //        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, gun.laod);
        //    }
        //    if (gun.rightHandPos != null)
        //    {
        //        animator.SetIKPosition(AvatarIKGoal.RightHand, gun.rightHandPos.position);//左手IKposition
        //        animator.SetIKRotation(AvatarIKGoal.RightHand, gun.rightHandPos.rotation);
        //        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, gun.laod);
        //        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, gun.laod);
        //    }
        //}
    }
}
