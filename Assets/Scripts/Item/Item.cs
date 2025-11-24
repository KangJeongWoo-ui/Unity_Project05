using System.Xml.Serialization;
using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("# Item States")]

    // 아이템 이동 속도
    public float moveSpeed;

    Rigidbody2D rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        ItemMovement();
    }
    
    // 아이템 움직임
    void ItemMovement()
    {
        rb.linearVelocity = Vector2.down * moveSpeed;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
