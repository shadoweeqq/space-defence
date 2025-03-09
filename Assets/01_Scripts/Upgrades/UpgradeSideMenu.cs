using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSideMenu : MonoBehaviour
{
    [Header("UI Components")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI descriptionText;
    public Image iconImage;
    public TextMeshProUGUI priceText;
    public Button buyButton;

    public UpgradeGrid upgradeGrid;
    private UpgradeManager upgradeManager;
    private Upgrade currentUpgrade;

    [SerializeField] private AudioQuery selectQuery;
    [SerializeField] private AudioQuery buyQuery;


    public void ShowUpgradeDetails(Upgrade upgrade, UpgradeManager manager)
    {
        AudioManager.Instance.PlayAudioQuery(selectQuery, transform);

        currentUpgrade = upgrade;
        upgradeManager = manager;

        UpdateUI();
    }

    private void OnEnable()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (currentUpgrade == null || upgradeManager == null) return;

        int currentLevel = upgradeManager.GetCurrentLevel(currentUpgrade);
        int nextLevelCost;

        nameText.text = currentUpgrade.upgradeName;
        if (currentUpgrade.maxLevel == -1)
        {
            nextLevelCost = currentUpgrade.costs[0];
            levelText.gameObject.SetActive(false);
            priceText.text = $"${nextLevelCost}";
        }
        else
        {
            nextLevelCost = currentLevel < currentUpgrade.costs.Length ? currentUpgrade.costs[currentLevel] : 0;
            levelText.text = $"Level {currentLevel}/{currentUpgrade.maxLevel}";
            priceText.text = currentLevel < currentUpgrade.maxLevel ? $"${nextLevelCost}" : "MAX";
        }
        
        descriptionText.text = currentUpgrade.description;
        iconImage.sprite = currentUpgrade.icon;
        

        //buyButton.interactable = currentLevel < currentUpgrade.maxLevel;
        if (((currentLevel < currentUpgrade.maxLevel) || currentUpgrade.maxLevel == -1) && (upgradeManager.money >= nextLevelCost))
        {
            buyButton.interactable = true;
        }
        else
        {
            buyButton.interactable = false;
        }
        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(BuyUpgrade);

        foreach (var layoutGroup in GetComponentsInChildren<LayoutGroup>())
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(layoutGroup.GetComponent<RectTransform>());
        }
    }

    private void BuyUpgrade()
    {
        if (upgradeManager.PurchaseUpgrade(currentUpgrade))
        {
            upgradeGrid.RefreshCoins();
            UpdateUI(); // Refresh after buying
            upgradeGrid.RefreshGrid();
            AudioManager.Instance.PlayAudioQuery(buyQuery, transform);
        }
        else
        {
            Debug.Log("Purchase failed!");
        }
    }
}
