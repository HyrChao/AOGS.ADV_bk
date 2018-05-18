//2017/2/03
//by Chao

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GenericMenu : MonoBehaviour
{
    public static MenuManager menuManager;  //窗口管理器(static)
    public static Menu currentMenu = Menu.None;

    public Menu label = Menu.None; //便于Unity界面中更改
    public Menu nextWindow = Menu.None;              
    public Menu previousWindow = Menu.None;

    protected int previousSelect;
    protected int currentSelect;
    public int defaultSelect = 0;

    public GameObject selectableGroup;
    public Selectable[] selectable;
    private bool allSelectableDisabled = false;
    private int _select = 0;
    protected int select
    {
        get
        {
            return _select;
        }
        set
        {
            if (allSelectableDisabled)
                return;
            previousSelect = _select;
            //From input to index
            int delta = value - _select;
            //Pass if current index of selectable is disabled

            int maxIndex = selectableGroup.transform.childCount -1;
            if (delta > 0)
            {
                do
                {
                    _select++;
                    if (_select > maxIndex)
                        _select = 0;
                }
                //Check if selectable is active
                while (!selectable[_select].gameObject.activeInHierarchy);
            }
            else
            {
                do
                {
                    _select--;
                    if (_select < 0)
                        _select = maxIndex;
                }
                while (!selectable[_select].gameObject.activeInHierarchy);
            }

            //数值在长度范围内重复
            //if (_select > maxIndex)
            //    _select = 0;
            //if (_select < 0)
            //    _select = maxIndex;
            //_select = (int)Mathf.Repeat(_select, maxIndex);
            //_select = (int)Mathf.Repeat(_select, selectableGroup.transform.childCount);    
            currentSelect = _select;
            //Debug.Log(currentSelect.ToString());
            Select(selectable[_select]);
        }
    }

    protected virtual void OnFocus()         //选中默认按钮
    {
        Select(selectable[defaultSelect]);
    }

    protected void Select(Selectable selectObj)
    {
        //AO.eventSystem.SetSelectedGameObject(selectObj);
        selectObj.Select();
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
        currentMenu = this.label;
        OnFocus();
    }

    public virtual void Close()     //关闭窗口
    {
        Display(false);
        //AO.eventSystem.SetSelectedGameObject(selectable[currentSelect]); //Reset selection
        currentMenu = Menu.None;
        _select = 0;
    }

    protected virtual void OnSelect()
    {
        //Debug.Log("Select Base");
    }

    protected virtual void OnConfirm()
    {
        //Debug.Log("Confirm Base");
    }

    protected virtual void Awake()
    {

        int totalSelectableCount = selectableGroup.transform.childCount;
        selectable = new Selectable[totalSelectableCount];

        int disabledSelectable = 0;
        for (int i = 0; i < selectableGroup.transform.childCount; i++)
        {
            Selectable selectObj = selectableGroup.transform.GetChild(i).GetComponent<Selectable>();
            selectable[i] = selectObj;
            if(!selectObj.gameObject.activeInHierarchy)
                disabledSelectable++;
        }
        if (disabledSelectable == totalSelectableCount)
            allSelectableDisabled = true;
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