//2018/03/30
//By Chao
//Static game manager
using UnityEngine;

static class AO
{
    static private Mode mode = Mode._2D;
    public static Mode Mode
    {
        get
        {
            return mode;
        }
        set
        {
            mode = value;
        }
    }

    private static bool gameOver = false;
    public static bool GameOver
    {
        get
        {
            return gameOver;
        }
        set
        {
            gameOver = value;
        }
    }

    private static float gravity = 10f;
    public static float Gravity
    {
        get
        {
            return gravity;
        }
        set
        {
            gravity = value;
            //Change gravity once upgrated value
            Physics.gravity = new Vector3(0f, -gravity, 0f);
        }
    }

    public static GameManager GetGameManager()
    {
        GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (gm != null)
        {
            return gm;
        }
        else
            return null;
    }

}