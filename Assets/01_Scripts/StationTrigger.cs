using UnityEngine;

public class StationTrigger : MonoBehaviour
{
    private Station station;

    [SerializeField] private AudioQuerySet stationDamageQuerySet;

    private void Awake()
    {
        station = GetComponentInParent<Station>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyBullet"))
        {
            AudioManager.Instance.PlayRandomAudioQuery(stationDamageQuerySet, transform);
            station.TakeDamage(collision.GetComponent<Bullet>().damage);
            Destroy(collision.gameObject);
        }

        if ((collision.GetComponent<Asteroid>() != null) && Time.deltaTime < 5f)
        {
            Destroy(collision.gameObject);
        }
    }


}
