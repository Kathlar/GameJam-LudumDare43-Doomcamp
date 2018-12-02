using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkplaceUpgradeUI : MonoBehaviour
{
    public Text error;
    public Text cost;
    public Text level;
    public Button btn;

    Workplace workplace;

    public void Init(Workplace workplace)
    {
        this.workplace = workplace;
    }

    private void Awake()
    {
        btn.onClick.AddListener(OnUpgradeBtnClicked);
    }

    void Start ()
    {
        InvokeRepeating("UpdateState", 0, 1);
	}
	
	void UpdateState ()
    {
        string errorText;
        bool canUpgrade = workplace.CanUpgrade(out errorText);

        error.text = errorText;
        error.gameObject.SetActive(!canUpgrade);
        btn.interactable = canUpgrade;

        string cost = "";

        level.text = "Level: " + (workplace.level + 1).ToString();
	}

    void OnUpgradeBtnClicked()
    {
        string tmp;
        if (!workplace.CanUpgrade(out tmp))
            return;

        workplace.Upgrade();
    }
}
