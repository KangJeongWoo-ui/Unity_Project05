using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [Header("# Bullet States")]

    // 총알 데미지
    public int damage;

    // 총알 이동 속도
    public float bulletSpeed;

    Rigidbody2D rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        rb.linearVelocity = Vector2.up * bulletSpeed;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("BulletBorder"))
        {
            Destroy(gameObject);
        }
        else if(collision.CompareTag("EnemyDragon"))
        {
            Destroy(gameObject);
        }
    }
}
