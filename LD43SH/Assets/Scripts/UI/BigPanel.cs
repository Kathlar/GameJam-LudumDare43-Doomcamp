using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigPanel : MiniPanel
{
    public static List<BigPanel> bigPanels;
    public MiniPanel miniPanel;

    void Start()
    {
        if (bigPanels == null) bigPanels = new List<BigPanel>();
        if (!bigPanels.Contains(this)) bigPanels.Add(this);
    }

    public void HidePanel()
    {
        miniPanel.gameObject.SetActive(true);
        gameObject.SetActive(false);
        foreach (MiniPanel miniPan in miniPanels)
        {
            miniPan.gameObject.SetActive(true);
        }
    }
}
