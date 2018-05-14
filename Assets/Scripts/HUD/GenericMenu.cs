//2017/2/03
//by Chao

using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GenericMenu : MonoBehaviour
{

    public static MenuManager menuManager;  //窗口管理器(static)

    public Menu nextWindow;              //便于Unity界面中更改
    public Menu previousWindow;

    public GameObject defaultSelected;
    public GameObject selectableGroup;
    private GameObject[] selectableArray;

    private int buttonID = 0;
    protected int ButtonID
    {
        get
        {
            return buttonID;
            //var total = buttonGroup.transform.childCount;
            //for (var i = 0; i < total; i++)
            //{
            //    Button btn = buttonGroup.transform.GetChild(i).GetComponent<Button>();
            //    if (btn.isActiveAndEnabled)
            //        return i;
            //}
            //return 0;
        }
        set
        {
            value = (int)Mathf.Repeat(value, selectableGroup.transform.childCount);    //数值在长度范围内重复
            OnFocus();
        }
    }

    public virtual void OnFocus()         //选中默认按钮
    {
        AO.eventSystem.SetSelectedGameObject(selectableArray[buttonID]);
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

    virtual public void OnSelect()
    {

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

        for(int i = 0; i < selectableGroup.transform.childCount; i++)
        {
            GameObject selectObj = selectableGroup.transform.GetChild(i).gameObject;
            selectableArray[i] = selectObj;
        }

        if(AO.eventSystem == null)
            AO.eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();

    }

    void Update()
    {

    }

}