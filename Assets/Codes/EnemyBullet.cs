using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public int damage;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Border"))
        {
            Destroy(gameObject);
        }
    }
}
