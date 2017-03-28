using UnityEngine;
using System.Collections;

public class GameManager
{  
    //角色数值
    public static int maxHP = 100;
    public static int maxMP = 30;

    public static int hp;
    public static int mp;

    public const float jumpSpeed = 3;       //delta time优化，影响起跳-落地时间
    public static float jumpVelocity = 8;  //跳跃初速度，影响自由落体最大高度
    public static float jumpMultiple = 1.15f;//跳跃-高跳系数 
    public static float moveSpeed = 6;     //6
    public static float rumMultiple = 1.6f; //移动-奔跑系数1.6

    //角色运动状态
    public static bool isStanding = false;
    public static bool isWalking = false;
    public static bool isClimbing = false;
    public static bool isJumping = false;
    public static bool isRunning = false;


    //角色攻击状态
    public static bool isAttacking = false;
    public static bool isDefancing = false;

    //角色位置状态
    public static bool isGround = false;
}
