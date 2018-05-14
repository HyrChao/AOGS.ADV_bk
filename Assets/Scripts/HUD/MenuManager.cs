//2017/2/09
//by Chao
//管理游戏中的全局变量/方法，更新游戏的进行状态
using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {
    public GenericMenu[] menu;          //窗口数组

    public GenericMenu GetWindow(int value)
    {
        return menu[value];
    }

    public void ToggleWindowVisiblity(int value) //打开value编号的窗口，关闭其它窗口
    {
        var total = menu.Length;
        for (var i = 0; i < total; i++)
        {
            var window = menu[i];
            if (i == value)
                window.Open();
            else if (window.gameObject.activeSelf)
                window.Close();
        }
    }

    public int currentWindowID;
    public int defaultWindowID;
    public GenericMenu Open(int value)        //判断value范围是否正确并且打开窗口
    {
        if (value < 0 || value >= menu.Length)
            return null;
        currentWindowID = value;
        ToggleWindowVisiblity(currentWindowID);
        return GetWindow(currentWindowID);

    }

    void Start()
    {
        GenericMenu.menuManager = this;      //使每个继承GericWindows的窗口都有Manager   
        Open(defaultWindowID);
    }
}