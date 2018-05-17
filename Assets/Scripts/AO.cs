//2018/03/30
//By Chao
//Static game manager

using UnityEngine;
using UnityEngine.EventSystems;
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

    private static GameManager _gm;
    public static GameManager gm
    {
        get
        {
            if (_gm == null)
                return null;
            else
                return _gm;                   
        }
        set
        {
            _gm = value;
        }
    }
    private static Player _player;
    public static Player player
    {
        get
        {
            if (_player != null)
                return _player;
            else
                return null;
        }
        set
        {
            _player = value;
        }
    }


    private static HUD _hud;
    public static HUD hud
    {
        get
        {
            if (_hud != null)
                return _hud;
            else
                return null;
        }
        set
        {
            _hud = value;
        }
    }
    private static MenuManager _menu;
    public static MenuManager menu
    {
        get
        {
            return _menu;
        }
        set
        {
            _menu = value;
        }
    }
    private static EventSystem _eventSystem;
    public static EventSystem eventSystem
    {
        get
        {
            return _eventSystem;
        }
        set
        {
            _eventSystem = value;
        }
    }
}

