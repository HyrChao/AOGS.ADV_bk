//2017/2/03
//by Chao

using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class GenericMenu : MonoBehaviour
{

    public static MenuManager menuManager;  //窗口管理器(static)

    public Menu nextWindow;              //便于Unity界面中更改
    public Menu previousWindow;

    public GameObject defaultSelected;



    public virtual void OnFocus()         //选中默认按钮
    {
        AO.eventSystem.SetSelectedGameObject(defaultSelected);
    }

    protected virtual void Display(bool value)
    {
        gameObject.SetActive(value);
    }

    public virtual void Open()         //打开窗口
    {
        Display(true);
        OnFocus();
    }

    public virtual void Close()     //关闭窗口
    {
        Display(false);
    }

    public void NextWindow()
    {
        menuManager.Open((int)nextWindow - 1);
    }

    public void PreviousWindow()
    {
        menuManager.Open((int)previousWindow - 1);

    }

    protected virtual void Awake()
    {
        Close();
        if(AO.eventSystem == null)
            AO.eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
    }

    void Update()
    {

    }

}