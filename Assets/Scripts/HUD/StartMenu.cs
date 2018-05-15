using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenu : GenericMenu {

    public Button continueButton;         //按钮容器

    public override void Open()
    {
        //var canContinue = true;
        //continueButton.gameObject.SetActive(canContinue);
        //if (continueButton.gameObject.activeSelf)
        //    defaultSelect = continueButton.gameObject;

        base.Open();
    }

    public void Continue()
    {
        NextWindow();
    }

    public void NewGame()
    {
        SceneManager.LoadScene("Scene1");      //到场景1
    }

    public void Option()
    {
        menuManager.Open(1);
    }

    void Start () {
        Open();
	}
	
}
