using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class MiniPanel : MonoBehaviour
{
    private Camera mainCamera;
    protected Transform objectPanel;
    public Image greenImage, yellowImage;
    public Slider slider;
    public Text currentValueText;

    public BigPanel bigPanel;

    protected virtual void Start()
    {
        mainCamera = Camera.main;
        if(bigPanel != null)
            bigPanel.gameObject.SetActive(false);
    }

    protected virtual void Update()
    {
        if(objectPanel != null)
            transform.position = mainCamera.WorldToScreenPoint(objectPanel.position + objectPanel.up);

        yellowImage.rectTransform.position = new Vector3(greenImage.rectTransform.position.x + greenImage.rectTransform.rect.width * greenImage.fillAmount
            , yellowImage.rectTransform.position.y, yellowImage.rectTransform.position.z);

        float maxFill = slider.value;
        greenImage.fillAmount = Mathf.Clamp(greenImage.fillAmount, 0, maxFill);
        yellowImage.fillAmount = Mathf.Clamp(yellowImage.fillAmount, 0, maxFill - greenImage.fillAmount);
    }

    public void ShowBigPanel()
    {
        bigPanel.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
