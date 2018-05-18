//2017/2/11
//by Chao
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameoverMenu : GenericMenu
{
    //public ToggleGroup gameOverToggleGroup;

    //public int gameOverButtonID
    //{
    //    get
    //    {
    //        var total = gameOverToggleGroup.transform.childCount;
    //        for(var i = 0; i < total; i++)
    //        {
    //            var toggle = gameOverToggleGroup.transform.GetChild(i).GetComponent<Toggle>();
    //            if(toggle.isOn)
    //                return i;
    //        }
    //        return 0;
    //    }
    //    set
    //    {
    //        value = (int)Mathf.Repeat(value, gameOverToggleGroup.transform.childCount);    //数值在长度范围内重复
    //        var currentSelectuiion = gameOverToggleGroup.ActiveToggles().FirstOrDefault();     //Linq 当前选项为第一或者默认选项
    //        if (currentSelectuiion != null)
    //        {
    //            currentSelectuiion.isOn = false;           //若有选项激活则关闭选项

    //        }
    //        currentSelectuiion = gameOverToggleGroup.gameObject.transform.GetChild(value).GetComponent<Toggle>();
    //        currentSelectuiion.isOn = true;
    //        Debug.Log("SN" + value);
    //    }
    //}


    override protected void OnConfirm()
    {
        //NextWindow();
        //PlayerPrefs.GetInt("gameOverButtonID", gameOverButtonID);
        if (select == 0)
        {
            SceneManager.LoadScene("Scene1");
        }

        if (select == 1)
        {
            SceneManager.LoadScene("Scene1");
        }

        if (select == 2)
        {
            Application.Quit();
        }
    }
    
    override protected void OnFocus()
    {
        selectable[defaultSelect].GetComponent<Toggle>().isOn = true;
    }

    override protected void OnSelect()
    {
        selectable[previousSelect].GetComponent<Toggle>().isOn = false;
        selectable[currentSelect].GetComponent<Toggle>().isOn = true;
        //Debug.Log("GameOver Select "+ "currentSelect "+currentSelect.ToString());
    }

    public override void Open()         //打开窗口
    {
        
        base.Open();
    }

    protected override void Update()
    {
        //if (Input.GetButtonDown("Horizontal"))
        //{
        //    float hValue = Input.GetAxis("Horizontal");

        //    if (hValue > 0)
        //        gameOverButtonID++;

        //    else if (hValue < 0)
        //        gameOverButtonID--;

        //    //OnFocus();
        //}

        base.Update();

        if (Input.GetButtonDown("Horizontal"))
        {
            float axis = Input.GetAxisRaw("Horizontal");
            if (axis == 0)
                return;
            if (axis > 0)
                select++;
            else
                select--;
        }

        //if (Input.GetButtonDown("Attack"))
        //{
        //    select--;
        //}
        //if (Input.GetButtonDown("Fire"))
        //{
        //    select++;
        //}
    }
}