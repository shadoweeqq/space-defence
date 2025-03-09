using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeGridItem : MonoBehaviour
{
    public Image iconImage;                  // Icon of the upgrade
    public TextMeshProUGUI priceText;        // Price displayed on the grid item
    public Slider levelBar;                  // Bar to show current level progress

    private Upgrade upgrade;
    private UpgradeSideMenu sideMenu;
    private UpgradeManager upgradeManager;

    public int currentLevel;

    public void Setup(Upgrade upgrade, UpgradeSideMenu menu, UpgradeManager manager)
    {
        this.upgrade = upgrade;
        sideMenu = menu;
        upgradeManager = manager;

        iconImage.sprite = upgrade.icon;
        priceText.text = $"${upgrade.costs[0]}";
        UpdateLevelBar();

        GetComponent<Button>().onClick.AddListener(() => sideMenu.ShowUpgradeDetails(upgrade, upgradeManager));

        if (upgrade.maxLevel == -1)
        {
            levelBar.gameObject.SetActive(false);
        }
    }

    public void UpdateLevelBar()
    {
        currentLevel = upgradeManager.GetCurrentLevel(upgrade);
        if (upgrade.maxLevel == -1)
        {
            levelBar.gameObject.SetActive(false);
        }
        else
        {
            levelBar.maxValue = upgrade.maxLevel;
            levelBar.value = currentLevel;
            priceText.text = currentLevel < upgrade.maxLevel ? $"${upgrade.costs[currentLevel]}" : "MAX";
        }

    }
}
