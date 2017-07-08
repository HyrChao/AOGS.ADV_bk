//2017/2/03
//by Chao
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class GenericWindow : MonoBehaviour
{

    public static WindowManager windowManager;  //窗口管理器(static)

    public Windows nextWindow;              //便于Unity界面中更改
    public Windows previousWindow;

    public GameObject firstSelected;
    protected EventSystem eventSystem
    {
        get { return GameObject.Find("EventSystem").GetComponent<EventSystem>(); }
    }
    public virtual void OnFocus()         //选中默认按钮
    {
        eventSystem.SetSelectedGameObject(firstSelected);
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
        windowManager.Open((int)nextWindow - 1);
    }

    public void PreviousWindow()
    {
        windowManager.Open((int)previousWindow - 1);

    }

    protected virtual void Awake()
    {
        Close();
    }


    void Update()
    {

    }
}