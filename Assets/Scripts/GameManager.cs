//2017/2/23
//by Chao
//GameManager mono instance

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    //Debug Settings
    private uint debugSetting = 0;
    public uint DebugSetting
    {
        get
        {
            return debugSetting;
        }
        set
        {
            debugSetting = value;
        }
    }

    //System Settings
    private uint systemSetting = 0;
    public uint SystemSetting
    {
        get
        {
            return systemSetting;
        }
        set
        {
            systemSetting = value;
        }
    }

    private void Awake()
    {
        //Setup fps
        Application.targetFrameRate = 600;

        if(AO.gm == null)
        {
            AO.gm = this;
        }
        AO.controller = GameObject.Find("Player").GetComponent<Controller>();
        AO.player = GameObject.Find("Player").GetComponent<Player>();
        AO.animeManager = GameObject.Find("Player").GetComponent<AnimeManager>();
        AO.slot = GameObject.Find("Player").GetComponent<SlotManager>();
        AO.hud = GameObject.Find("HUD").GetComponent<HUD>();
        AO.eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();

        AO.player.launcher = Instantiate(AO.player.launcher, AO.slot.laucherSlot.position, AO.slot.laucherSlot.rotation);
        AO.player.launcher.gameObject.transform.parent = AO.slot.laucherSlot;//Initialize as child of launcher slot
        AO.player.weapon = Instantiate(AO.player.weapon, AO.slot.weaponSlot.position, AO.slot.weaponSlot.rotation);
        AO.player.weapon.gameObject.transform.parent = AO.slot.weaponSlot;//Initialize as child of weapon slot


    }

    void Start()
    {
        AO.GameOver = false;
    }
    private void Update()
    {
        if (AO.player.HP <= 0)
        {
            GameOver();
        }

        if (Input.GetButtonDown("Pause"))
        {
            if (Time.timeScale == 0f)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    void LateUpdate()
    {

    }

    public void GameOver()
    {
        AO.player.state = PlayerState.Dying;
        AO.controller.enabled = false;
        SceneManager.LoadScene("Begin");
    }

    private void Pause()
    {
        Time.timeScale = 0f;
        AO.hud.Hide();
    }

    private void Resume()
    {
        Time.timeScale = 1.0f;
        AO.hud.Show();
    }
}
