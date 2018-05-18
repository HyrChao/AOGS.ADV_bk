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

    //State
    private bool canPause;

    private void Awake()
    {
        //Setup fps
        Application.targetFrameRate = 600;

        if(AO.gm == null)
        {
            AO.gm = this;
        }

        AO.hud = GameObject.Find("HUD").GetComponent<HUD>();
        AO.eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
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
        AO.player.controller.enabled = false;
        SceneManager.LoadScene("Begin");
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        AO.hud.Hide();
        AO.menu.Open(Menu.PauseMenu);
    }

    public void Resume()
    {
        Time.timeScale = 1.0f;
        AO.hud.Show();
        AO.menu.Close();
    }
}
