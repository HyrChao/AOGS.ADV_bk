//2017/2/23
//by Chao
//GameManager mono instance

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        if(AO.gm == null)
        {
            AO.gm = this;
        }
        AO.controller = GameObject.Find("Player").GetComponent<Controller>();
        AO.player = GameObject.Find("Player").GetComponent<Player>();
        AO.animeManager = GameObject.Find("Player").GetComponent<AnimeManager>();
        AO.slot = GameObject.Find("Player").GetComponent<SlotManager>();
        AO.hud = GameObject.Find("HUD").GetComponent<HUD>();
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


}
