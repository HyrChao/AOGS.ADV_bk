//2017/3/30
//by Chao

using UnityEngine;
using System.Collections;

public enum PlayerState
{
    None,       //0
    Idle,       //1
    Walking,    //2
    Running,    //3
    Jumping,    //4
    Falling,    //5
    Flying,     //6
    Climbing,   //7
    Attacking,  //8
    Firing,     //9
    Damaging,   //10
    Dying,      //11
}


public class Player : MonoBehaviour {

    //持有武器
    public Weapon weapon;
    public Gun gun;
    //public FaceUpdate emo;//表情管理器
    public PlayerState state = PlayerState.None;
    //角色数值
    public int maxHP = 100;
    public int maxMP = 30;
    public int maxSP = 30;

    public int HP;
    public int MP;
    public int SP;

    //经验技能
    private int lv = 1;
    static private int maxLV=100;
    private int exp=0;
    private int[] expLv = new int[maxLV];//每级所需经验

    //每级经验初始化
    private void SetupExpForEveryLevel()
    {
        expLv[1] = 30;

        for (int i = 1; i < maxLV; ++i)
        {
            expLv[i] = expLv[i - 1] + 50 + 25 * (int)(i * 0.5);
        }
    }
    public int GetTotalEXP()
    {
        return exp;
    }

    public int GetRemainEXP()
    {
        int remainExp = expLv[lv + 1] - exp;
        return remainExp;
    }

    public void AddEXP(int _exp)
    {
        exp += _exp;
    }


    //统计
    private int enemyKilled = 0;

    //金钱&点数
    private int gold=0;
    public int Gold()
    {
        return gold;
    }
    public void AddGold(int _gold)
    {
        gold += _gold;
    }
    //持有物品

    public float jumpSpeed = 3;       //delta time优化，影响起跳-落地时间
    public float jumpVelocity = 6f;  //跳跃初速度，影响自由落体最大高度
    public float jumpMultiple = 1.15f;//跳跃-高跳系数 
    public float moveSpeed = 6;     //6
    public float rumMultiple = 1.6f; //移动-奔跑系数1.6
    //private AnimatorStateInfo currentBaseState;

    public float attackSpeed = 0.05f;
    public float fireSpeed = 0.1f;
    //角色运动状态

    public bool faceRight = true;
    //Awake
    public void ResetPlayer()
    {
        HP = maxHP;
        MP = maxMP;
        SP = maxSP;
    }

    private void Awake()
    {
        SetupExpForEveryLevel();
    }

    //Start
    private void Start()
    {
        ResetPlayer();
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {

    }

    //回复HP
    private void RecoverHP(int reHP)
    {
        HP += reHP;
        if (HP > maxHP) ;
        HP = maxHP;
    }
    //回复MP
    private void RecoverMP(int reMP)
    {
        MP += reMP;
        if (HP > maxHP);
        MP = maxMP;
    }
    public void Upgrade()
    {
        lv++;
    }
    private void Damaged()
    {
        Debug.Log("Damaged!"+HP.ToString());
    }
    //攻击
    //private IEnumerator Attack()
    //{
    //    Debug.Log("Attack");
    //    state.attacking = true;
    //    //emo.atkEmo();
    //    yield return new WaitForSeconds(attackSpeed);
    //    Debug.Log("Attack End");
    //    state.attacking = false;
    //    //emo.norEmo();
    //    //yield return StartCoroutine(InnerUnityCoroutine());协程嵌套
    //}
    //远程导弹攻击
    //private IEnumerator Fire()
    //{
    //    Debug.Log("Fire");
    //    gun.Fire();
    //    state.firing = true;
    //    //emo.firEmo();
    //    yield return new WaitForSeconds(fireSpeed);
    //    Debug.Log("Fire End");
    //    state.firing = false;
    //    //emo.norEmo();
    //}
    //防御
    private void Defence()
    {

    }
    private void KillEnemy()
    {
        enemyKilled++;
        Debug.Log("Kill Total" + enemyKilled.ToString());
    }
}
