using System.Collections;
using UnityEngine;

public class SlowDebuff : MonoBehaviour
{
    public float slowAmount;
    public float duration;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Player player = FindFirstObjectByType<Player>();
            player.ApplySlow(slowAmount, duration);
        }
    }
}
