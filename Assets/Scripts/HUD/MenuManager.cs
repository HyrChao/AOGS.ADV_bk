//2017/2/09
//by Chao
//管理游戏中的全局变量/方法，更新游戏的进行状态
using UnityEngine;

public class MenuManager : MonoBehaviour {

    public GenericMenu[] menu;          //窗口数组
    public GenericMenu GetWindow(Menu value)
    {
        for (int i=0; i<menu.Length; i++)
            if(menu[i].tag == value)
                return menu[i];
        return null;
    }

    public void Open(Menu value) //打开value编号的窗口，关闭其它窗口
    {
        var total = menu.Length;
        for (var i = 0; i < total; i++)
        {
            GenericMenu window = menu[i];
            if (window.tag == value)
                window.Open();
            else if (window.gameObject.activeSelf)
                window.Close();
        }
    }

    public Menu currentMenu = Menu.None;

    public Menu defaultMenu = Menu.None;

    private void Awake()
    {
        if (AO.menu == null)
        {
            AO.menu = this;
        }
    }

    void Start()
    {
        GenericMenu.menuManager = this;      //使每个继承GericWindows的窗口都有Manager   
        Open(defaultMenu);
    }

}