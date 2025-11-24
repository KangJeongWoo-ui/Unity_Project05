using System;
using System.Collections;
using System.Collections.Specialized;
using Unity.VisualScripting;
using UnityEngine;

public enum bossType { Boss0, Boss1, Boss2 }

public class BossDragon : MonoBehaviour
{
    public GameObject[] bossBullet;
    public Transform target;
    public GameObject meteorLine;
    public GameObject[] bossMeteor;
    public Transform[] meteorSpawnPoints;
    public GameObject enemyDragon;

    [Header("# BossDragon States")]
    
    public bossType bossType;

    // 보스 체력
    public int hp;

    // 보스 이동 속도
    public float moveSpeed;

    // 보스 생존 여부 확인
    bool isDead = false;

    // 보스가 죽을 시 이벤트를 알림
    public event System.Action OnBossDie;

    private float currentAngle = 0f;

    Rigidbody2D rb;
    Animator anim;
    void Start()
    {
        StartCoroutine(StartPattern());
    }
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        BossDragonMovement();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("PlayerBullet"))
        {
            if (isDead) return;

            PlayerBullet playerBullet = collision.GetComponent<PlayerBullet>();
            TakeDamage(playerBullet.damage);
        }
    }

    // 움직임
    void BossDragonMovement()
    {
        rb.linearVelocity = Vector2.down * moveSpeed;

        // 생성 후 내려오다가 3초 후 멈춤
        StartCoroutine(StopMovementCoroutine());
    }
    IEnumerator StopMovementCoroutine()
    {
        yield return new WaitForSeconds(3.0f);
        rb.linearVelocity = Vector2.zero;
        moveSpeed = 0f;
    }

    // 데미지를 받음
    void TakeDamage(int damage)
    {
        hp -= damage;
        anim.SetTrigger("doHit");
        if (hp <= 0)
        {
            isDead = true;
            hp = 0;
            Die();

            // 보스가 죽으면 이벤트를 알림
            OnBossDie?.Invoke();
        }
    }

    // 죽음
    void Die()
    {
        anim.SetTrigger("doDie");

        // 죽은 후 오브젝트 삭제
        StartCoroutine(DieCoroutine());
    }
    IEnumerator DieCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
    
    // 보스 생성 후 3초후 패턴 시작
    IEnumerator StartPattern()
    {
        yield return new WaitForSeconds(5f);
        StartCoroutine(Pattern());
    }
    
    // 보스 패턴
    IEnumerator Pattern()
    {
        while(!isDead)
        {
            int randPattern = UnityEngine.Random.Range(0, 3);

            // 보스0 패턴
            if(bossType == bossType.Boss0)
            {
                switch(randPattern)
                {
                    case 0:
                        for (int i = 0; i < 5; i++)
                        {
                            int randx = UnityEngine.Random.Range(-2, 2);
                            int randy = UnityEngine.Random.Range(-2, 2);

                            BossPattern.CircleShot(bossBullet[0], 20, new Vector3(randx,randy,0));
                            yield return new WaitForSeconds(1.0f);
                        }
                        break;
                    case 1:
                        for (int i = 0; i < 50; i++)
                        {
                            if (target == null)
                                target = GameObject.FindWithTag("Player").transform;
                
                            BossPattern.AutoShot(bossBullet[0], transform, target);
                            yield return new WaitForSeconds(0.2f);
                        }
                        break;
                    case 2:
                        yield return BossPattern.BossMeteor(meteorLine, bossMeteor[0], meteorSpawnPoints);
                        break;
                }
                yield return new WaitForSeconds(2f);
            }

            // 보스1 패턴
            if (bossType == bossType.Boss1)
            {
                switch (randPattern)
                {
                    case 0:
                        for(int i=0; i<10; i++)
                        {
                            int randx = UnityEngine.Random.Range(-1, 1);

                            BossPattern.FanShot(bossBullet[1], 8, transform.position + new Vector3(randx,0,0), -30f, 30f);
                            yield return new WaitForSeconds(0.5f);
                        }
                        break;
                    case 1:
                        yield return BossPattern.BossMeteor(meteorLine, bossMeteor[1], meteorSpawnPoints);
                        break;
                    case 2:
                        for (int i = 0; i < 50; i++)
                        {
                            currentAngle += 10f;
                            BossPattern.SpiralShot(bossBullet[1], 10, currentAngle, transform.position);
                            yield return new WaitForSeconds(0.2f);
                        }
                        break;
                }
                yield return new WaitForSeconds(2f);
            }

            // 보스2 패턴
            if (bossType == bossType.Boss2)
            {
                switch (randPattern)
                {
                    case 0:
                        for (int i = 0; i < 10; i++)
                        {
                            int randx = UnityEngine.Random.Range(-1, 1);

                            BossPattern.FanShot(bossBullet[2], 8, transform.position + new Vector3(randx, 0, 0), -30f, 30f);
                            yield return new WaitForSeconds(0.5f);
                        }
                        break;
                    case 1:
                        for (int i = 0; i < 5; i++)
                        {
                            int randx = UnityEngine.Random.Range(-2, 2);
                            int randy = UnityEngine.Random.Range(-2, 2);

                            BossPattern.CircleShot(bossBullet[2], 20, new Vector3(randx, randy, 0));
                            yield return new WaitForSeconds(1.0f);
                        }
                        break;
                    case 2:
                        for(int i=0; i<10; i++)
                        {
                            BossPattern.DragonShot(enemyDragon, meteorSpawnPoints);
                            yield return new WaitForSeconds(0.2f);
                        }
                        break;
                }
                yield return new WaitForSeconds(2f);
            }
        }

    }
}
