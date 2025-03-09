using UnityEngine;
using UnityEngine.UI;

public class UpgradePanel : MonoBehaviour
{
    /*
    public Text upgradeNameText;
    public Text descriptionText;
    public Text costText;
    public Text levelText;
    public Image iconImage;
    public Button purchaseButton;

    private int upgradeIndex;
    private UpgradeShop shop;
    private BoatUpgradeManager upgradeManager;

    public void Setup(BoatUpgrade upgrade, int index, UpgradeShop upgradeShop, BoatUpgradeManager manager)
    {
        upgradeNameText.text = upgrade.upgradeName;
        descriptionText.text = upgrade.description;
        iconImage.sprite = upgrade.icon;
        shop = upgradeShop;
        upgradeManager = manager;
        upgradeIndex = index;

        UpdateUI(upgrade, index);
        purchaseButton.onClick.AddListener(() => shop.PurchaseUpgrade(index));
    }

    public void UpdateUI(BoatUpgrade upgrade, int index)
    {
        int currentLevel = upgradeManager.GetCurrentLevel(upgrade, index);
        levelText.text = $"Level: {currentLevel}/{upgrade.maxLevel}";
        costText.text = currentLevel < upgrade.costs.Length
            ? $"Cost: ${upgradeManager.GetUpgradeCost(upgrade, index)}"
            : "Max Level";
        purchaseButton.interactable = !upgrade.IsMaxLevel(currentLevel);
    }
    */
}
