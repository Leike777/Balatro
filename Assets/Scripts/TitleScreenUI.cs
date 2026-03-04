using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenUI : MonoBehaviour
{
    private GameObject SelectingChildPanel;
    public GameObject whiteBackground;
    
    public void OpenPanel(GameObject panel)
    {
        panel.SetActive(true);
        whiteBackground.SetActive(true);
        
        //处理子面板的状态
        if(SelectingChildPanel != null) CloseChildPanel();
        Transform childTransform = panel.transform.Find("ChildPanel0");
        if (childTransform != null)
        {
            SelectingChildPanel = childTransform.gameObject;
        }
        if(SelectingChildPanel != null)OpenChildPanel(SelectingChildPanel);
    }

    public void ClosePanel(GameObject panel)
    {
        panel.SetActive(false);
        whiteBackground.SetActive(false);
    }
    
    public void OpenChildPanel(GameObject newChildPanel)
    {
        newChildPanel.SetActive(true);
        SelectingChildPanel = newChildPanel;
    }

    public void CloseChildPanel()
    {
        SelectingChildPanel.SetActive(false);
    }
    
    public void UpdateTriangle(GameObject Triangle)
    {
        short newIndex = short.Parse(SelectingChildPanel.name.Replace("ChildPanel", ""));
        Triangle.GetComponent<SelectingTriangle>().SetPosition(newIndex);
    }

    public void ExitGame()
    {
        // 在编辑器中停止播放，在发布版本中退出应用
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
    }

    public void EnterNewScene()
    {
        SceneManager.LoadScene("GamePlay");
    }
}
