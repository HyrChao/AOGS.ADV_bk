//2017/2/23
//by Chao

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

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
