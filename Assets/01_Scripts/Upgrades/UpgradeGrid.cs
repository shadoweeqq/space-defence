using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeGrid : MonoBehaviour
{
    public GameObject upgradeGridItemPrefab;   // Prefab for each upgrade in the grid
    public Transform gridContainer;           // Parent container for grid items
    public UpgradeSideMenu sideMenu;          // Reference to the side menu
    public UpgradeManager upgradeManager; // Reference to the Upgrade Manager
    public TextMeshProUGUI coinsText;


    private void Start()
    {
        PopulateGrid();
    }

    private void OnEnable()
    {
        RefreshCoins();
    }

    private void PopulateGrid()
    {
        foreach (Upgrade upgrade in upgradeManager.availableUpgrades)
        {
            GameObject gridItem = Instantiate(upgradeGridItemPrefab, gridContainer);
            UpgradeGridItem gridItemScript = gridItem.GetComponent<UpgradeGridItem>();
            gridItemScript.Setup(upgrade, sideMenu, upgradeManager);
        }
    }

    public void RefreshGrid()
    {
        foreach (Transform child in gridContainer.transform)
        {
            UpgradeGridItem gridItem = child.GetComponent<UpgradeGridItem>();
            gridItem.UpdateLevelBar();
            
        }
    }

    public void RefreshCoins()
    {
        coinsText.text = $"${upgradeManager.money}";
    }
}
