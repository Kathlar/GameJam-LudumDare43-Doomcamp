using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniPanel : MonoBehaviour
{
    private Camera mainCamera;
    public Transform objectPanel;
    public Image greenImage, yellowImage;
    public Slider slider;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if(objectPanel != null)
            transform.position = mainCamera.WorldToScreenPoint(objectPanel.position + objectPanel.up);

        yellowImage.rectTransform.position = new Vector3(greenImage.rectTransform.position.x + greenImage.rectTransform.rect.width * greenImage.fillAmount
            , yellowImage.rectTransform.position.y, yellowImage.rectTransform.position.z);

        float maxFill = slider.value;
        greenImage.fillAmount = Mathf.Clamp(greenImage.fillAmount, 0, maxFill);
        yellowImage.fillAmount = Mathf.Clamp(yellowImage.fillAmount, 0, maxFill - greenImage.fillAmount);
    }
}
