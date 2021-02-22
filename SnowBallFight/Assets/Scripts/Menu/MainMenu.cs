using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameplayCore.Menu;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public PageType fistPage = PageType.none;




    // Start is called before the first frame update
    void Start()
    {
        PageController.Instance.TurnPageOn(fistPage);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!PageController.Instance.StepBackPage())
            {
                Debug.Log("You cant back!");
            }
        
        }
    }
    
    public void ChangePage(Page _page)
    {
        PageController.Instance.TurnPageOff(_page.previousPageType, _page.pageType);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void StepBackPage()
    {
        PageController.Instance.StepBackPage();
    }

}
