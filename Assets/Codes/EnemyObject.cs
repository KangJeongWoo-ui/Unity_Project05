using UnityEngine;

public class EnemyObject : MonoBehaviour
{
    Rigidbody2D rb;

    public float speed;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        rb.linearVelocity = Vector2.down * speed;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Border"))
        {
            Destroy(gameObject);
        }
    }
}
