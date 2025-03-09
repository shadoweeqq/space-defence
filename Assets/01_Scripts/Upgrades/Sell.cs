using UnityEngine;

public class Sell : MonoBehaviour
{
    [SerializeField] private AudioQuery sellQuery;



    private void OnTriggerEnter2D(Collider2D other)
    {
        SellObject sellObject = other.GetComponent<SellObject>();
        if (sellObject != null)
        {
            SellObject(sellObject.sellValue, sellObject.gameObject);
        }
    }

    public void SellObject(int value, GameObject obj)
    {
        UpgradeManager.Instance.AddMoney(value);
        AudioManager.Instance.PlayAudioQuery(sellQuery, transform);
        Destroy(obj);
    }
}
