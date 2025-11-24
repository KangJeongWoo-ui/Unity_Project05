using UnityEngine;

public class BossBullet : MonoBehaviour
{
    [Header("# Boss Bullet States")]
    
    // ÃÑ¾Ë µ¥¹ÌÁö
    public int damage;

    // ÃÑ¾Ë ¼Óµµ
    public float moxeSpeed;

    Rigidbody2D rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        rb.linearVelocity = transform.up * moxeSpeed;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BulletBorder"))
        {
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
