using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject pauseScreen;
    public GameObject playerDeathScreen;
    public GameObject stationDeathScreen;

    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (!UpgradeManager.Instance.upgradeMenuActive)
            {
                pauseScreen.SetActive(!pauseScreen.activeSelf);
            }
        }
    }
}
