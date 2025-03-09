using UnityEngine;
using UnityEngine.UI;

public class UpgradeShop : MonoBehaviour
{
    //FishingManager fishingManager => FishingManager.Instance;
    UpgradeManager upgradeManager => UpgradeManager.Instance;
    bool entered;

    private void Update()
    {
        if (!entered) return;
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (upgradeManager.shopUI.activeSelf == true)
            {
                CloseShop();
            }
            else
            {
                OpenShop();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TipText.Instance.ChangeText($"Upgrade Shop\nPress <b>E</b> to enter");
            entered = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TipText.Instance.ResetText();
            entered = false;
        }
    }

    public void OpenShop()
    {
        upgradeManager.OpenShop();
    }

    public void CloseShop()
    {
        upgradeManager.CloseShop();
    }
}
