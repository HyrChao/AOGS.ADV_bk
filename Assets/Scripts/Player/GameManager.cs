//2017/2/23
//by Chao
//GameManager mono instance

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
    private SlotManager slot;
    public SlotManager Slot
    {
        get
        {
            if (slot != null)
                return slot;
            else
                return null;
        }
    }
    private void Awake()
    {
        if(AO.gm == null)
        {
            AO.gm = this;
        }
        controller = GameObject.Find("Player").GetComponent<Controller>();
        player = GameObject.Find("Player").GetComponent<Player>();
        am = GameObject.Find("Player").GetComponent<AnimeManager>();
        slot = GameObject.Find("Player").GetComponent<SlotManager>();
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

    public void GameOver()
    {
        player.state = PlayerState.Dying;
        controller.enabled = false;
        SceneManager.LoadScene("Begin");
    }


}
