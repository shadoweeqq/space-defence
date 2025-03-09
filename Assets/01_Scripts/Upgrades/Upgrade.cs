using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Upgrade")]
public class Upgrade : ScriptableObject
{
    [Header("Upgrade Details")]
    public string upgradeName;          // Name of the upgrade
    [TextArea] public string description;  // Description of what it does
    public Sprite icon;                 // Icon for the upgrade
    public int maxLevel = 1;            // Maximum level of the upgrade
    public int[] costs;                 // Cost for each level (size = maxLevel)

    [Header("Effect")]
    public UpgradeType type;            // Type of the upgrade
    public float[] effectValues;        // Effect values for each level (size = maxLevel)

    public bool IsMaxLevel(int currentLevel)
    {
        return currentLevel >= maxLevel;
    }
}

public enum UpgradeType
{
    ShipSpeed,
    Fuel,
    DragPower,
    FireRate,
    Damage,
    Zoom,
    Aim,
    Turret,
    Repair,
    Shield,
    Durability,
    Radar,
    SellMultiplier,
    AsteroidDamageMultiplier,
}
