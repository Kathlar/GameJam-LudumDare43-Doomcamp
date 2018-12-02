using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimescaleButton : MonoBehaviour
{
    public Button btn;
    public float desiredTimescale = 3;

    private void Start()
    {
        btn.onClick.AddListener(BtnClicked);
    }

    void BtnClicked()
    {
        if (Time.timeScale != 0.0f) Time.timeScale = desiredTimescale;
    }
}
