using UnityEngine;

public class Meteor : MonoBehaviour
{
    [Header("# Meteor States")]

    // 메테오 이동 속도
    public float moveSpeed;

    // 메테오 데미지
    public int damage;

    Rigidbody2D rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        MeteorMovement();
    }

    // 메테오 움직임
    void MeteorMovement()
    {
        rb.linearVelocity = Vector2.down * moveSpeed;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Border"))
        {
            Destroy(gameObject);
        }
    }
}
