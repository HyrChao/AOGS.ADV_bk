//2017/2/23
//by Chao

using UnityEngine;
using UnityEngine.SceneManagement;

public enum Mode
{
    _2D,
    _3D
}


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

public class GameManager : MonoBehaviour
{
    //private static GameManager gm;
    //public static GameManager GM
    //{
    //    get
    //    {
    //        if (gm == null)
    //            gm = new GameManager();
    //        return gm;
    //    }
    //}
    private Player player;
    public Player Player
    {
        get
        {
            if (controller != null)
                return player;
            else
                return null;
        }
    }
    private Controller controller;
    public Controller Controller
    {
        get
        {
            if (controller != null)
                return controller;
            else
                return null;
        }
    }
    private AnimeManager am;
    public AnimeManager AM
    {
        get
        {
            if (am != null)
                return am;
            else
                return null;
        }
    }
    private HUD hud;
    public HUD HUD
    {
        get
        {
            if (hud != null)
                return hud;
            else
                return null;
        }
    }

    private void Awake()
    {
        controller = GameObject.Find("Player").GetComponent<Controller>();
        player = GameObject.Find("Player").GetComponent<Player>();
        am = GameObject.Find("Player").GetComponent<AnimeManager>();
        hud = GameObject.Find("HUD").GetComponent<HUD>();
    }

    void Start()
    {
        AO.GameOver = false;

    }
    private void Update()
    {
        if (player.HP <= 0)
        {
            GameOver();
        }
    }
    void LateUpdate()
    {

    }
    //Bool转换Float
    public static float BoolToFloat(bool boolean)
    {
        if (boolean)
            return 1f;
        else
            return 0f;
    }

    public void GameOver()
    {
        player.state = PlayerState.Dying;
        controller.enabled = false;
        SceneManager.LoadScene("Begin");
    }


}
