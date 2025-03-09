using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class TurretManager : MonoBehaviour
{
    public static TurretManager Instance;

    [System.Serializable]
    public class TurretData
    {
        public string name;
        public GameObject turretPrefab;
        public TextMeshProUGUI amountText;
        public Image selectionOutline;
        public int amount;
    }

    public TurretData[] turrets;
    private int selectedTurretIndex = -1;
    private Transform playerTransform;

    [SerializeField] private AudioQuery deployQuery;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        UpdateUI();
    }

    private void Update()
    {
        HandleTurretSelection();
    }

    private void HandleTurretSelection()
    {
        for (int i = 0; i < turrets.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                selectedTurretIndex = i;
                Debug.Log($"Selected: {turrets[i].name}");
                TipText.Instance.SetTimedText($"Deployed: {turrets[i].name}", 2f);
                UpdateUI();
                DeploySelectedTurret();
            }
        }
    }

    private void DeploySelectedTurret()
    {
        if (selectedTurretIndex == -1) return;
        TurretData selectedTurret = turrets[selectedTurretIndex];

        if (selectedTurret.amount > 0)
        {
            AudioManager.Instance.PlayAudioQuery(deployQuery, playerTransform);
            Instantiate(selectedTurret.turretPrefab, playerTransform.position, Quaternion.identity);
            selectedTurret.amount--;
            UpdateUI();
        }
        else
        {
            Debug.Log("No more turrets left!");
            TipText.Instance.SetTimedText($"You don't have any turrets to deploy", 2f);
        }
    }

    public void AddTurret(string turretName, int amount)
    {
        foreach (var turret in turrets)
        {
            if (turret.name == turretName)
            {
                turret.amount += amount;
                UpdateUI();
                return;
            }
        }
        Debug.LogWarning("Turret not found!");
    }

    private void UpdateUI()
    {
        for (int i = 0; i < turrets.Length; i++)
        {
            turrets[i].amountText.text = turrets[i].amount.ToString();
            turrets[i].selectionOutline.enabled = (i == selectedTurretIndex);
        }
    }
}