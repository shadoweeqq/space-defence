using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [Header("Upgrades")]
    public Upgrade[] availableUpgrades;
    public Dictionary<Upgrade, int> upgradeLevels = new Dictionary<Upgrade, int>();

    [Header("Currency")]
    public int money = 500;

    [Header("References")]
    public GameObject shopUI;
    public TextMeshProUGUI moneyText;
    public bool upgradeMenuActive = false;
    public static UpgradeManager Instance;
    private SpaceshipController spaceshipController;
    private DraggingObjects draggingObjects;
    private Station station;
    [SerializeField] private CinemachineCamera cinemachineCamera;

    [Header("Stats")]
    public float sellMultiplier = 1;
    public float asteroidDamageMultiplier = 1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        spaceshipController = player.GetComponent<SpaceshipController>();
        draggingObjects = player.GetComponent<DraggingObjects>();

        foreach (Upgrade upgrade in availableUpgrades)
        {
            upgradeLevels[upgrade] = 0;
            AddMoney(0);
        }
    }

    public bool PurchaseUpgrade(Upgrade upgrade)
    {
        if (!upgradeLevels.ContainsKey(upgrade)) return false;

        int currentLevel = upgradeLevels[upgrade];
        if (upgrade.maxLevel != -1 && currentLevel >= upgrade.maxLevel)
        {
            Debug.Log("Upgrade is already maxed out!");
            return false;
        }

        int cost = (currentLevel < upgrade.costs.Length) ? upgrade.costs[currentLevel] : upgrade.costs[0];
        if (money < cost)
        {
            Debug.Log("Not enough coins!");
            return false;
        }

        SubtractMoney(cost);
        upgradeLevels[upgrade]++;
        ApplyUpgradeEffect(upgrade, upgradeLevels[upgrade]);
        return true;
    }

    private void ApplyUpgradeEffect(Upgrade upgrade, int level)
    {
        float effectValue = (level - 1 < upgrade.effectValues.Length) ? upgrade.effectValues[level - 1] : upgrade.effectValues[0];

        switch (upgrade.type)
        {
            case UpgradeType.ShipSpeed:
                spaceshipController.thrustPower += effectValue;
                break;
            case UpgradeType.FireRate:
                spaceshipController.fireRate -= effectValue;
                break;
            case UpgradeType.DragPower:
                draggingObjects.dragForce += effectValue;
                break;
            case UpgradeType.Zoom:
                cinemachineCamera.Lens.OrthographicSize += effectValue;
                break;
            case UpgradeType.SellMultiplier:
                sellMultiplier += effectValue;
                break;
            case UpgradeType.Turret:
                TurretManager.Instance.AddTurret("Auto Sentry", (int)effectValue);
                break;
            case UpgradeType.Repair:
                station.Heal((int)station.maxHealth/2);
                break;
            case UpgradeType.AsteroidDamageMultiplier:
                asteroidDamageMultiplier += effectValue;
                break;
        }
    }

    public int GetCurrentLevel(Upgrade upgrade)
    {
        return upgradeLevels.ContainsKey(upgrade) ? upgradeLevels[upgrade] : 0;
    }

    public int GetUpgradeCost(Upgrade upgrade)
    {
        int currentLevel = GetCurrentLevel(upgrade);
        return (currentLevel < upgrade.costs.Length) ? upgrade.costs[currentLevel] : upgrade.costs[0];
    }

    public void OpenShop()
    {
        spaceshipController.enabled = false;
        shopUI.SetActive(true);
        shopUI.transform.localScale = Vector3.zero;
        shopUI.transform.DOScale(1, 0.3f)
            .OnComplete(() => Time.timeScale = 0f)
            .SetEase(Ease.OutBack);
        upgradeMenuActive = true;
    }

    public void CloseShop()
    {
        Time.timeScale = 1f;
        spaceshipController.enabled = true;
        upgradeMenuActive = false;
        shopUI.transform.DOScale(Vector3.zero, 0.3f)
            .OnComplete(() => shopUI.SetActive(false))
            .SetEase(Ease.InBack);
    }

    public void AddMoney(int amount)
    {
        money += (int)((float)amount*sellMultiplier);
        moneyText.text = $"${money}";
    }

    public void SubtractMoney(int amount)
    {
        money -= amount;
        moneyText.text = $"${money}";
    }
}