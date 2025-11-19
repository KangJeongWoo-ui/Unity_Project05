using System.Collections;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.PlayerSettings;
using static UnityEngine.Rendering.DebugUI.Table;

public class Boss : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    Spawn spawn;
    public enum BossType { Boss0, Boss1, Boss2 }
    public BossType bossType;

    [Header("Boss")]
    public float hp; // 보스 체력
    public float speed; // 보스 이동 속도
    public GameObject[] bossBullet; // 보스 총알
    public GameObject iceBoom;
    public GameObject iceLaser;
    public GameObject dark;

    float maxRight = 3.0f;
    float maxLeft = -3.0f;
    float maxBottom = -5.0f;
    bool isDead = false;

    void Start()
    {
        rb.linearVelocity = Vector2.down * speed;
        StartCoroutine(MoveStopCoroutine()); // 보스가 생성후 내려오다가 3초후 정지
    }
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        spawn = FindFirstObjectByType<Spawn>();
    }
    void Update()
    {
        Dead();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBullet")) // 캐릭터 총알에 맞으면 캐릭터 총알은 삭제되고 적의 색을 빨간색으로 바꿔 피격 당한것을 표현
        {
            PlayerBullet playerbullet = collision.GetComponent<PlayerBullet>();
            hp -= playerbullet.damage;
            Damage();
            Destroy(collision.gameObject);
        }
    }
    void Damage() // 데미지를 받을때 빨간색으로 변한 적을 0.5초 뒤에 흰색으로 바꿔 피격당한 효과를 구현하는 코루틴
    {
        StartCoroutine(DamageCoroutine());
    }
    IEnumerator DamageCoroutine()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sr.color = Color.white;
    }
    void Dead() // 보스 죽음
    {
        if(hp <= 0)
        {
            isDead = true;
            Destroy(gameObject);
            Spawn spawn;
            spawn = FindFirstObjectByType<Spawn>();
            spawn.NextStage();

            if(bossType == BossType.Boss2)
            {
                SceneManager.LoadScene("GameClearScene");
            }
        }
        
    }
    IEnumerator MoveStopCoroutine() //  보스 생성후 3초후 정지
    {
        yield return new WaitForSeconds(3f);
        rb.linearVelocity = Vector2.zero;
        StartCoroutine(bossPattern());
    }
    IEnumerator bossPattern() // 보스 패턴
    {
        switch(bossType)
        {
            case BossType.Boss0:
                while (!isDead)
                {
                    int randomBossPattern = Random.Range(0, 3);
                    switch (randomBossPattern)
                    {
                        case 0:
                            yield return StartCoroutine(ShotgunShot());
                            break;
                        case 1:
                            yield return StartCoroutine(SpiralShot());
                            break;
                        case 2:
                            yield return StartCoroutine(SpiralStream());
                            break;
                    }
                    yield return new WaitForSeconds(3f);
                }
                break;
            case BossType.Boss1:
                while (!isDead)
                {
                    int randomBossPattern = Random.Range(0, 3);
                    switch (randomBossPattern)
                    {
                        case 0:
                            yield return StartCoroutine(spawn.IceLance());
                            break;
                        case 1:
                            yield return StartCoroutine(IceBoom());
                            break;
                        case 2:
                            yield return StartCoroutine(IceLaser());
                            break;
                    }
                    yield return new WaitForSeconds(3f);
                }
                break;
            case BossType.Boss2:
                while (!isDead)
                {
                    GameObject darkObject = Instantiate(dark, new Vector3(0,0,0), transform.rotation);
                    int randomBossPattern = Random.Range(0, 3);
                    switch (randomBossPattern)
                    {
                        case 0:
                            yield return StartCoroutine(ShotgunShot());
                            break;
                        case 1:
                            yield return StartCoroutine(spawn.IceLance());
                            break;
                        case 2:
                            yield return StartCoroutine(SpiralStream());
                            break;
                    }
                    yield return new WaitForSeconds(3f);
                }
                break;
        }
    }
    IEnumerator ShotgunShot() // 산탄총 발사
    {
        GameObject player = GameObject.FindWithTag("Player");

        float fireRate = 0.5f; // 발사 간격
        float bulletSpeed = 3f; // 총알 속도
        float duration = 10f;   // 지속 시간
        float elapsed = 0f;     // 경과 시간
        int pelletCount = 5;    // 한 번에 발사할 총알 수
        float spreadAngle = 30f; // 퍼지는 각도

        while (elapsed < duration)
        {
            // 플레이어를 향한 기본 방향
            Vector2 baseDir = (player.transform.position - transform.position).normalized;
            float baseAngle = Mathf.Atan2(baseDir.y, baseDir.x) * Mathf.Rad2Deg;

            // 여러 발 생성
            for (int i = 0; i < pelletCount; i++)
            {
                // 각 총알마다 퍼지는 각도 계산
                float angle = baseAngle + Random.Range(-spreadAngle / 2f, spreadAngle / 2f);
                Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

                GameObject bullet = Instantiate(bossBullet[0], transform.position, Quaternion.identity);
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                rb.linearVelocity = dir * bulletSpeed;
            }

            yield return new WaitForSeconds(fireRate);
            elapsed += fireRate;
        }
    }
    IEnumerator SpiralShot() // 원형 총알
    {
        float duration = 6f;       // 지속 시간
        float elapsed = 0f;        // 경과 시간
        int bulletsPerWave = 24;   // 한 번에 발사할 탄 수
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

                GameObject bullet = Instantiate(bossBullet[0], transform.position, Quaternion.identity);
                Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad),
                                          Mathf.Sin(angle * Mathf.Deg2Rad)).normalized;

                Rigidbody2D rb2d = bullet.GetComponent<Rigidbody2D>();
                rb2d.linearVelocity = dir * bulletSpeed;
            }

            yield return new WaitForSeconds(fireRate);
            elapsed += fireRate;

            angleOffset += spinSpeed * fireRate;
            if (angleOffset >= 360f) angleOffset -= 360f;
            else if (angleOffset <= -360f) angleOffset += 360f;
        }
    }
    IEnumerator SpiralStream() // 회전 총알
    {
        float duration = 5f;
        float elapsed = 0f;
        float bulletSpeed = 3f;
        float stepInterval = 0.03f;   // 총알 한 번 찍는 간격
        float angle = 0f;             // 현재 진행 각도
        float angleSpeed = 360f;      // 초당 각도 증가량
        int arms = 2;                 // 나선 팔 개수

        while (elapsed < duration)
        {
            float armGap = 360f / arms;

            for (int a = 0; a < arms; a++)
            {
                float ang = angle + a * armGap;

                GameObject bullet = Instantiate(bossBullet[0], transform.position, Quaternion.identity);
                Vector2 dir = new Vector2(Mathf.Cos(ang * Mathf.Deg2Rad),
                                          Mathf.Sin(ang * Mathf.Deg2Rad)).normalized;

                Rigidbody2D rb2d = bullet.GetComponent<Rigidbody2D>();
                rb2d.linearVelocity = dir * bulletSpeed;
            }

            // 다음 탄까지 아주 짧게 대기
            yield return new WaitForSeconds(stepInterval);
            elapsed += stepInterval;

            angle += angleSpeed * stepInterval;
            if (angle >= 360f) angle -= 360f;
        }
    }
    IEnumerator IceBoom() // 얼음 폭탄
    {
        float fireRate = 5f; // 발사 간격
        float duration = 10f;   // 지속 시간
        float elapsed = 0f;     // 경과 시간
        while (elapsed < duration)
        {
            GameObject builet = Instantiate(iceBoom, transform.position, transform.rotation);

            yield return new WaitForSeconds(fireRate);
            elapsed += fireRate;
        }
    }
    IEnumerator IceLaser() // 얼음 레이저
    {
        float fireRate = 2f; // 발사 간격
        float duration = 10f;   // 지속 시간
        float elapsed = 0f;     // 경과 시간
        while (elapsed < duration)
        {
            int randomIceLaser = Random.Range(0, 3);

            Vector3 pos = Vector3.zero;
            Quaternion rot = Quaternion.identity;
            Vector2 dir = Vector2.zero;

            switch (randomIceLaser)
            {
                case 0: // 오른쪽 → 왼쪽
                    pos = new Vector3(maxRight, 0f, 0f); // y = 0
                    dir = Vector2.left;
                    rot = Quaternion.identity;
                    break;

                case 1: // 왼쪽 → 오른쪽
                    pos = new Vector3(maxLeft, 0f, 0f); // y = 0
                    dir = Vector2.right;
                    rot = Quaternion.identity;
                    break;

                case 2: // 아래쪽 → 위
                    pos = new Vector3(0f, maxBottom, 0f); // x = 0
                    dir = Vector2.right;
                    rot = Quaternion.Euler(0, 0, 90); // 90도 회전
                    break;
            }
            GameObject laser = Instantiate(iceLaser, pos, rot);
            laser.GetComponent<IceLaser>().Init(dir);
            yield return new WaitForSeconds(fireRate);
            elapsed += fireRate;
        }

    }
}
