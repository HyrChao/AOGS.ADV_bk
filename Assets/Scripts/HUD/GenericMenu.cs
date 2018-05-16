//2017/2/03
//by Chao

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GenericMenu : MonoBehaviour
{
    public static Menu currentMenu = Menu.None;

    public Menu tag = Menu.None; //便于Unity界面中更改
    public static MenuManager menuManager;  //窗口管理器(static)

    public Menu nextWindow = Menu.None;              
    public Menu previousWindow = Menu.None;

    protected int previousSelect;
    protected int currentSelect;
    public int defaultSelect = 0;

    public GameObject selectableGroup;
    public GameObject[] selectable;

    private int _select = 0;
    protected int select
    {
        get
        {
            return _select;
        }
        set
        {
            previousSelect = _select;
            int delta = value - _select;
            if (delta > 0)
                _select++;
            else
                _select--;

            int maxIndex = selectableGroup.transform.childCount -1;
            //数值在长度范围内重复
            if (_select > maxIndex)
                _select = 0;
            if (_select < 0)
                _select = maxIndex;
            //_select = (int)Mathf.Repeat(_select, selectableGroup.transform.childCount);    
            currentSelect = _select;
            Select(selectable[_select]);
        }
    }

    protected virtual void OnFocus()         //选中默认按钮
    {
        AO.eventSystem.SetSelectedGameObject(selectable[defaultSelect]);
    }

    protected void Select(GameObject selectObj)
    {
        AO.eventSystem.SetSelectedGameObject(selectObj);
        OnSelect();
    }

    protected void Confirm()
    {
        OnConfirm();
    }

    protected virtual void Display(bool value)
    {
        gameObject.SetActive(value);
    }

    public virtual void Open()         //打开窗口
    {
        Display(true);
        currentMenu = this.tag;
        OnFocus();
    }

    public virtual void Close()     //关闭窗口
    {
        Display(false);
    }

    protected virtual void OnSelect()
    {
        Debug.Log("Select Base");
    }

    protected virtual void OnConfirm()
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

    protected void Awake()
    {
        Close();

        selectable = new GameObject[selectableGroup.transform.childCount];

        for (int i = 0; i < selectableGroup.transform.childCount; i++)
        {
            GameObject selectObj = selectableGroup.transform.GetChild(i).gameObject;
            selectable[i] = selectObj;
        }

        if(AO.eventSystem == null)
            AO.eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();

    }

    protected virtual void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            OnConfirm();
        }
    }

}