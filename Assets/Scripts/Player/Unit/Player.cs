using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("# Player States")]
    // 플레이어 체력
    public int hp;
    
    // 플레이어 이동 속도
    public float moveSpeed;

    // 총알 발사 간격
    public float fireBulletRate;

    // 폭탄 발사 간격
    public float fireBoomRate;

    // powerLevel이 올라갈 시 총알에 추가되는 데미지
    public int plusBulletDamage;

    // 총알 레벨
    public int powerLevel = 1;

    // 폭탄 횟수
    public int boomCount = 0;

    [Header("# Player Object")]
    public GameObject playerBullet;
    public GameObject boomEffect;
    public GameObject rightDron;
    public GameObject leftDron;

    [Header("# Item Message")]
    public GameObject levelUpBulletMessage;
    public GameObject spawnDronMessage;
    public GameObject emptyBoomMessage;
    public GameObject healMessage;

    public BoomIconUI boomIconUI;

    // 플레이어 생존 여부 확인
    bool isDead = false;
    // 플레이어 피격 여부 확인
    bool isHit = false;

    private float nextFireBulletRate = 0f;
    private float nextFireBoomRate = 0f;

    Rigidbody2D rb;
    Animator anim;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        if (isDead) return;

        FireBullet();

        FireBoom();
    }
    void FixedUpdate()
    {
        if (isDead) return;

        PlayerMovement();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyDragon"))
        {
            Dragon dragon = collision.GetComponent<Dragon>();
            TakeDamage(dragon.damage);
        }
        if (collision.CompareTag("BossBullet"))
        {
            BossBullet bossbullet = collision.GetComponent<BossBullet>();
            TakeDamage(bossbullet.damage);
        }
        else if(collision.CompareTag("Meteor"))
        {
            Meteor meteor = collision.GetComponent<Meteor>();
            TakeDamage(meteor.damage);
        }
        else if(collision.CompareTag("HealItem"))
        {
            StartCoroutine(HealMessage());
            hp += 10;
            if(hp >= 100)
            {
                hp = 100;
            }
        }
        else if (collision.CompareTag("PowerUpItem"))
        {
            powerLevel += 1;

            if (powerLevel == 5 || powerLevel == 10)
            {
                StartCoroutine(SpawnDronMessage());
                SpawnDrone();
            }
            else
            {
                StartCoroutine(LevelUpBulletMessage());
                plusBulletDamage += 1;
            }
        }
        else if (collision.CompareTag("BoomItem"))
        {
            boomCount += 1;

            if(boomCount >= 3)
            {
                boomCount = 3;
            }
            UpdateBoomCount();
        }
    }

    // 플레이어 움직임
    void PlayerMovement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector2 curVec = transform.position;
        Vector2 moveVec = new Vector2 (h, v).normalized;

        rb.MovePosition(curVec +  moveVec * moveSpeed * Time.deltaTime);
    }

    // 총알 발사
    void FireBullet()
    {
        if(Input.GetButton("Fire1") && Time.time >= nextFireBulletRate)
        {
            Shot();

            nextFireBulletRate = Time.time + fireBulletRate;
        }
    }

    // powerLevel 에 따른 공격 구현
    void Shot()
    {
        GameObject bullet = Instantiate(playerBullet, transform.position, transform.rotation);
        PlayerBullet playerbullet = bullet.GetComponent<PlayerBullet>();

        playerbullet.damage += plusBulletDamage;
    }

    // powerLevel이 5, 10일때 각각 추가 드론 생성
    void SpawnDrone()
    {
        switch(powerLevel)
        {
            case 5:
                Instantiate(rightDron, new Vector3(0, 0, 0), transform.rotation);
                break;
            case 10:
                Instantiate(leftDron, new Vector3(0, 0, 0), transform.rotation);
                break;
        }
    }

    // 폭탄 발사
    void FireBoom()
    {
        if(Input.GetButtonDown("Fire2"))
        {
            if(boomCount == 0)
            {
                // 폭탄이 없다면 폭탄 없음 메세지 출력
                StartCoroutine(EmptyBoomMessage());
                return;
            }
            else if(Time.time >= nextFireBoomRate)
            {
                boomCount--;

                // 남은 폭탄 갯수 아이콘 갱신
                UpdateBoomCount();

                nextFireBoomRate = Time.time + fireBoomRate;
                StartCoroutine(BoomCoroutine());
            }
        }
    }

    // 폭탄 UI 갱신
    void UpdateBoomCount()
    {
        boomIconUI.SetBoomCount(boomCount);
    }

    // 데미지를 받음 
    public void TakeDamage(int damage)
    {
        if (isHit) return;
        if (isDead) return;

        StartCoroutine(HitCoroutine(damage));
    }

    // 죽음
    void Die()
    {
        isDead = true;
        anim.SetTrigger("doDie");
        StartCoroutine(DieCoroutine());
    }
    IEnumerator HitCoroutine(int damage)
    {
        isHit = true;
        hp -= damage;

        anim.SetTrigger("doHit");

        if(hp <= 0)
        {
            Die();
            yield break;
        }
        yield return new WaitForSeconds(0.5f);
        isHit = false;
    }
    IEnumerator BoomCoroutine()
    {
        GameObject boomeffect = Instantiate(boomEffect, new Vector3(0, 0, 0), transform.rotation);
        yield return new WaitForSeconds(1f);
        Destroy(boomeffect);
    }
    IEnumerator DieCoroutine()
    {
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("GameOverScene");
        Destroy(gameObject);
    }

    // 총알 레벨 업 메세지 출력
    IEnumerator LevelUpBulletMessage()
    {
        levelUpBulletMessage.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        levelUpBulletMessage.SetActive(false);
    }

    // 드론 생성 메세지 출력
    IEnumerator SpawnDronMessage()
    {
        spawnDronMessage.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        spawnDronMessage.SetActive(false);
    }

    // 폭탄 부족함 경고 메세지 출력
    IEnumerator EmptyBoomMessage()
    {
        emptyBoomMessage.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        emptyBoomMessage.SetActive(false);
    }

    // 체력 회복됨 메세지 출력
    IEnumerator HealMessage()
    {
        healMessage.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        healMessage.SetActive(false);
    }
}
