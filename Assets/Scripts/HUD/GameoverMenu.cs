//2017/2/11
//by Chao
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameoverMenu : GenericMenu
{
    public ToggleGroup gameOverToggleGroup;
    public int gameOverButtonID
    {
        get
        {
            var total = gameOverToggleGroup.transform.childCount;
            for(var i = 0; i < total; i++)
            {
                var toggle = gameOverToggleGroup.transform.GetChild(i).GetComponent<Toggle>();
                if(toggle.isOn)
                    return i;
            }
            return 0;
        }
        set
        {
            value = (int)Mathf.Repeat(value, gameOverToggleGroup.transform.childCount);    //数值在长度范围内重复
            var currentSelectuiion = gameOverToggleGroup.ActiveToggles().FirstOrDefault();     //Linq 当前选项为第一或者默认选项
            if (currentSelectuiion != null)
            {
                currentSelectuiion.isOn = false;           //若有选项激活则关闭选项

            }
            currentSelectuiion = gameOverToggleGroup.gameObject.transform.GetChild(value).GetComponent<Toggle>();
            currentSelectuiion.isOn = true;
            Debug.Log("SN" + value);
        }
    }

    override public void OnSelect()
    {
        //NextWindow();
        PlayerPrefs.GetInt("gameOverButtonID", gameOverButtonID);
        if (gameOverButtonID == 0)
        {
            SceneManager.LoadScene("Scene1");
            Debug.Log("continue");
        }

        if (gameOverButtonID == 1)
        {
            SceneManager.LoadScene("Scene1");
            Debug.Log("rebirth");
        }

        if (gameOverButtonID == 2)
        {
            Application.Quit();
            Debug.Log("quit");
        }
    }

    public override void Open()         //打开窗口
    {
        gameOverButtonID = PlayerPrefs.GetInt("gameOverButtonID", 0); //初始化预设

        base.Open();
    }

    void Update()
    {
        if (Input.GetButtonDown("Vertical"))
        {
            var newGameOverButtonID = gameOverButtonID;
            var hValue = Input.GetAxis("Vertical");

            if (hValue > 0)
                newGameOverButtonID++;
            else if (hValue < 0)
                newGameOverButtonID--;
            if (newGameOverButtonID != gameOverButtonID)
                gameOverButtonID = newGameOverButtonID;

            OnSelect();
        }

        if (Input.GetButtonDown("Submit"))
        {
            OnSelect();
        }
    }
}