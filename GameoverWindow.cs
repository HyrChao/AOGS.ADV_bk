using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;

public class GameoverWindow : GenericWindow
{
    public ToggleGroup gameOverToggleGroup;
    public float delay = 0;
    public float inputDelay = .3f;

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

    public void OnSelect()
    {
        NextWindow();
        PlayerPrefs.GetInt("gameOverButtonID", gameOverButtonID);
    }

    public override void Open()         //打开窗口
    {
        gameOverButtonID = PlayerPrefs.GetInt("gameOverButtonID", 0); //初始化预设

        base.Open();
    }


    void Update()
    {
        delay += Time.deltaTime;
        if (delay > inputDelay)
        {
            var newGameOverButtonID = gameOverButtonID;
            var hValue = Input.GetAxis("Horizontal");

            if (hValue > 0)
            {
                newGameOverButtonID++;
            }
            else if (hValue < 0)
                newGameOverButtonID--;
            if (newGameOverButtonID != gameOverButtonID)
                gameOverButtonID = newGameOverButtonID;
            delay = 0;
           
        }
        if (Input.GetButtonDown("Submit"))
        {
            OnSelect();
        }
    }

}