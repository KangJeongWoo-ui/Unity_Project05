using System;
using System.Collections;
using UnityEngine;

public class IceBoom : MonoBehaviour
{
    public GameObject bossBullet;
    Rigidbody2D rb;
    void Start()
    {
        rb.linearVelocity = Vector2.down * 2;
        StartCoroutine(BoomCoroutine());
    }
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    IEnumerator BoomCoroutine()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);

        float duration = 1f;       // 전체 지속 시간
        float elapsed = 0f;        // 경과 시간
        int bulletsPerWave = 20;   // 한 번에 발사할 탄 수
        float fireRate = 0.35f;    // 웨이브 간격
        float bulletSpeed = 3.2f;  // 총알 속도
        float spinSpeed = 150f;    // 초당 회전 각도
        float angleOffset = 0f;    // 시작 각도

        while (elapsed < duration)
        {
            float angleStep = 360f / bulletsPerWave;

            for (int i = 0; i < bulletsPerWave; i++)
            {
                float angle = angleOffset + i * angleStep;

                GameObject bullet = Instantiate(bossBullet, transform.position, Quaternion.identity);
                Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad),
                                          Mathf.Sin(angle * Mathf.Deg2Rad)).normalized;

                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                rb.linearVelocity = dir * bulletSpeed;
            }

            yield return new WaitForSeconds(fireRate);
            elapsed += fireRate;

            angleOffset += spinSpeed * fireRate;
            if (angleOffset >= 360f) angleOffset -= 360f;
            else if (angleOffset <= -360f) angleOffset += 360f;
        }
    }
}
