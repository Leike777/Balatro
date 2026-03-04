using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIinGamePlay : MonoBehaviour
{
    public void OpenPanel(GameObject panel)
    {
        panel.SetActive(true);
    }

    public void AttachSkip(GameObject skip)
    {
        skip.SetActive(true);
    }
    public void ClosePanel(GameObject panel)
    {
        panel.SetActive(false);
    }
}
