//2017/3/30
//by Chao

using UnityEngine;
using UnityEngine.Events;
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


    //public FaceUpdate emo;//表情管理器
    public PlayerState state = PlayerState.None;

    //Abilities
    public int maxHP = 100;
    public int maxMP = 30;
    public int maxSP = 30;
    public int HP;
    public int MP;
    public int SP;
    public void ResetPlayer()
    {
        HP = maxHP;
        MP = maxMP;
        SP = maxSP;
    }

    //Level&Exp
    private int lv = 1;
    static private int maxLV=100;
    private int exp=0;
    private int[] expLv = new int[maxLV];//每级所需经验
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

    //Gold/Items
    private int gold = 0;
    public int Gold()
    {
        return gold;
    }
    public void AddGold(int _gold)
    {
        gold += _gold;
    }

    //Point/Statics
    private int enemyKilled = 0;

    //Weapons
    public Weapon weapon;
    public Gun gun;

    //Awake
    private void Awake()
    {
        SetupExpForEveryLevel();
        ResetPlayer();        
    }

    //Start
    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire"))
            Fire();
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

    private void Defence()
    {

    }

    private void Attack()
    {

    }

    private void Fire()
    {
        gun.Fire();
    }

    private void KillEnemy()
    {
        enemyKilled++;
        Debug.Log("Kill Total" + enemyKilled.ToString());
    }
}
