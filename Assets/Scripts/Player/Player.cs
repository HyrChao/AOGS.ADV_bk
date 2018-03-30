//2017/3/30
//by Chao

using UnityEngine;
using UnityEngine.Events;
using System.Collections;


public class Player : MonoBehaviour {

    private GameManager gm;

    public PlayerState state = PlayerState.None;

    //Abilities
    private int maxHP = 100;
    public int MaxHP
    {
        get
        {
            return maxHP;
        }
    }
    private int maxMP = 30;
    public int MaxMP
    {
        get
        {
            return maxMP;
        }
    }
    private int maxSP = 30;
    public int MaxSP
    {
        get
        {
            return maxSP;
        }
    }
    private int hp;
    public float HP
    {
        get
        {
            return (float)hp;
        }
    }
    private int mp;
    public float MP
    {
        get
        {
            return (float)mp;
        }
    }
    private float sp;
    public float SP
    {
        get
        {
            return sp;
        }
    }
    public void ResetPlayer()
    {
        hp = maxHP;
        mp = maxMP;
        sp = (float)maxSP;
    }

    //Level&Exp
    private Level lv;
    public int LV
    {
        get
        {
            return lv.CurrentLevel;
        }
    }
    public int GetExp()
    {
        return lv.CurrentExp;
    }
    public int GetRemainExp()
    {
        return lv.RamainExp;
    }

    public void AddEXP(int _exp)
    {
        lv.AddExp(_exp);
    }

    //Gold/Items
    private int gold = 0;
    public int Gold
    {
        get
        {
            return gold;
        }
    }
    public void AddGold(int _gold)
    {
        gold += _gold;
    }

    //Point/Statics
    private int enemyKilled = 0;
    public int EnemyKilled
    {
        get
        {
            return enemyKilled;
        }
        set
        {
            enemyKilled = value;
        }
    }

    //Weapons
    public Weapon weapon;
    public Gun gun;

    //Other parameters
    private bool inRunColdTime;
    public bool InRunColdTime
    {
        get
        {
            return inRunColdTime;
        }
    }

    //Awake
    private void Awake()
    {
        lv = new Level(100);
        gm = AO.GetGameManager();
        //gun = 
        ResetPlayer();
        inRunColdTime = false;
    }

    //Start
    private void Start()
    {

    }

    //Update
    private void Update()
    {
        if (Input.GetButtonDown("Fire"))
            Fire();

        if (state.Equals(PlayerState.Running))
            sp -= 0.1f;
        else
        {
            if (sp < maxSP)
                sp += 0.1f;
        }

        if (sp > 0)
            gm.Controller.CanRun = true;
        else
        {
            gm.Controller.CanRun = false;
            inRunColdTime = true;
        }

        if (inRunColdTime && Input.GetButtonUp("Run"))
        {
            inRunColdTime = false;
        }

    }

    //Fixed Update
    private void FixedUpdate()
    {

    }

    public void DropHP(int dpHp)
    {
        hp -= dpHp;
        if (hp < 0)
            hp = 0;
    }

    //Recover hp
    public void RecoverHP(int rehp)
    {
        hp += rehp;
        if (hp > maxHP) ;
        hp = maxHP;
    }
    //Recover mp
    public void RecoverMP(int remp)
    {
        mp += remp;
        if (hp > maxHP);
        mp = maxMP;
    }
    public void Upgrade()
    {
        gm.HUD.Msg("Upgrade!!");
    }
    private void Damaged()
    {
        Debug.Log("Damaged!"+hp.ToString());
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

    private class Level
    {
        public Level(int _maxLevel = 100)
        {
            maxLevel = _maxLevel;
            expForLv = new int[maxLevel];
            SetupExpForEveryLevel();
            currentExp = 0;
            currentLevel = 1;
        }

        private GameManager gm;
        private int exp;
        public int TotalExp
        {
            get
            {
                return exp;
            }
        }
        private int currentExp;
        public int CurrentExp
        {
            get
            {
                return currentExp;
            }
        }
        private int remainExp;
        public int RamainExp
        {
            get
            {
                return remainExp;
            }
        }
        private int[] expForLv;//Exp for every level
        private int currentLevel;
        public int CurrentLevel
        {
            get
            {
                return currentLevel;
            }
        }
        private int maxLevel;
        public int MaxLevel
        {
            get
            {
                return maxLevel;
            }
        }
        private void SetupExpForEveryLevel()
        {
            expForLv[1] = 30;

            for (int i = 1; i < maxLevel; ++i)
            {
                expForLv[i] = expForLv[i - 1] + 50 + 25 * (int)(i * 0.5);
            }
        }

        public void AddExp(int _exp)
        {
            //Check if get the max level
            if (currentLevel == maxLevel)
                return;
            if (gm == null)
                gm = AO.GetGameManager();
            exp += _exp;
            //Check if upgrade
            if (exp - expForLv[currentLevel] >= 0)
            {
                currentLevel++;
                gm.Player.Upgrade();
            }
            currentExp = exp - expForLv[currentLevel - 1];
            remainExp = expForLv[currentLevel] - currentExp;
            //HUD show information
            gm.HUD.Msg("获得经验 " + _exp.ToString());
        }
    }
}
