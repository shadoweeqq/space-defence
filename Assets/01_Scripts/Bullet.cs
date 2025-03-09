using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 3f;
    public int damage = 1;

    [SerializeField] private AudioQuery shootQuery;
    [SerializeField] private AudioQuerySet hitQuerySet;

    void Start()
    {
        AudioManager.Instance.PlayAudioQuery(shootQuery, transform);
        Destroy(gameObject, lifetime); // Destroy bullet after a few seconds
    }

    void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
        if (damageable != null)
        {
            AudioManager.Instance.PlayRandomAudioQuery(hitQuerySet, transform);
            damageable.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
