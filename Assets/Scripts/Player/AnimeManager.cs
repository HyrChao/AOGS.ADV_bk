//2017/4/2
//By Chao
//Anime controller
//IK controller

using UnityEngine;
using System.Collections;

public class AnimeManager : MonoBehaviour {

    //获取动画状态
    private Animator animator;
    public Animator Animator
    {
        get
        {
            if (animator != null)
                return animator;
            else
                return null;
        }
    }
    private GameManager gm;
    //private IKLookAt IKLook;
    public AnimatorStateInfo currentBaseState;

    private void Awake()
    {
        animator = transform.Find("PlayerMesh").GetComponent<Animator>();
        gm = AO.GetGameManager();
    }

    void Start ()
    {
        //IKLook = transform.FindChild("PlayerMesh").GetComponent<IKLookAt>();
    }
    private void FixedUpdate()
    {
        animator.SetInteger("State", gm.Player.state.GetHashCode());
        //if (!animator.IsInTransition(0))
        //将游戏管理器中角色状态值赋予动画组件
        //currentBaseState = animator.GetCurrentAnimatorStateInfo(0);//更新动画状态
    }
    void Update ()
    {
        if (AO.GameOver)
            animator.SetTrigger("Die");
    }
    private void LateUpdate()
    {
        currentBaseState = animator.GetCurrentAnimatorStateInfo(0);//更新动画状态
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


        //if (gun.leftHandPos != null)
        //{
        //    animator.SetIKPosition(AvatarIKGoal.LeftHand, gun.leftHandPos.position);//左手IKposition
        //    animator.SetIKRotation(AvatarIKGoal.LeftHand, gun.leftHandPos.rotation);
        //    animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, gun.laod);
        //    animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, gun.laod);
        //}
        //if (gun.rightHandPos != null)
        //{
        //    animator.SetIKPosition(AvatarIKGoal.RightHand, gun.rightHandPos.position);//左手IKposition
        //    animator.SetIKRotation(AvatarIKGoal.RightHand, gun.rightHandPos.rotation);
        //    animator.SetIKPositionWeight(AvatarIKGoal.RightHand, gun.laod);
        //    animator.SetIKRotationWeight(AvatarIKGoal.RightHand, gun.laod);
        //}
        //}
    }
}
