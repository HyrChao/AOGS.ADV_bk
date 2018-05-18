//2017/2/09
//by Chao
//管理游戏中的全局变量/方法，更新游戏的进行状态
using UnityEngine;

public class MenuManager : MonoBehaviour {

    public GenericMenu[] menu;          //窗口数组
    public GenericMenu GetWindow(Menu value)
    {
        for (int i=0; i<menu.Length; i++)
            if(menu[i].label == value)
                return menu[i];
        return null;
    }
    private GenericMenu currentMenu;
    public Menu currentMenuLabel = Menu.None;

    public void Open(Menu value) //打开value编号的窗口，关闭其它窗口
    {
        int total = menu.Length;
        for (int i = 0; i < total; i++)
        {
            currentMenu = menu[i];
            currentMenuLabel = value;
            if (currentMenu.label == value)
                currentMenu.Open();
            else if (currentMenu.gameObject.activeSelf)
                currentMenu.Close();
        }
    }

    public void Close()
    {
        currentMenu.Close();
    }

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
        //Open(defaultMenu);
    }

    public void NextWindow()
    {
        Open(currentMenu.nextWindow);
    }

    public void PreviousWindow()
    {
        Open(currentMenu.previousWindow);

    }

}