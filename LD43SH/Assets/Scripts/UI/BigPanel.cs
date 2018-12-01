using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigPanel : MiniPanel
{
    public MiniPanel miniPanel;

    public void HidePanel()
    {
        miniPanel.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
