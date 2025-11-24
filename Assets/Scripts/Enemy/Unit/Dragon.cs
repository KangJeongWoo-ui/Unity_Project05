using System.Collections;
using UnityEngine;

public class Dragon : MonoBehaviour
{
    [Header("# EnemyDragon States")]

    // 적 체력
    public int hp;

    // 적 이동 속도
    public float moveSpeed;

    // 적 데미지
    public int damage;

    [Header("# Reward Object")]
    public GameObject powerUp;
    public GameObject boom;
    public GameObject heal;

    Rigidbody2D rb;
    Animator anim;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        EnemyDragonMovement();
    }

    // 움직임
    void EnemyDragonMovement()
    {
        rb.linearVelocity = Vector2.down * moveSpeed;
    }

    // 데미지를 받음
    void TakeDamage(int damage)
    {
        hp -= damage;
        anim.SetTrigger("doHit");

        if(hp <= 0)
        {
            hp = 0;
            Die();
        }
    }

    // 죽음
    void Die()
    {
        anim.SetTrigger("doDie");
        
        // 랜덤 보상 드랍 후 오브젝트 삭제
        StartCoroutine(RandomRewardCoroutine());
    }

    // 적이 죽으면 랜덤으로 power 보상, boom 보상, heal 보상 드랍
    void RandomReward()
    {
        int random = Random.Range(0, 9);

        switch(random)
        {
            case 0:
            case 1:
            case 2:
            case 3:
            case 4:
                break;
            case 5:
                Instantiate(heal, transform.position, Quaternion.identity);
                break;
            case 6:
            case 7:
                Instantiate(powerUp, transform.position, Quaternion.identity);
                break;
            case 8:
            case 9:
                Instantiate(boom, transform.position, Quaternion.identity);
                break;
        }
    }
    IEnumerator RandomRewardCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        RandomReward();
        Destroy(gameObject);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Border"))
        {
            Destroy(gameObject);
        }
        else if (collision.CompareTag("PlayerBullet"))
        {
            PlayerBullet playerBullet = collision.GetComponent<PlayerBullet>();
            TakeDamage(playerBullet.damage);
        }
        else if (collision.CompareTag("Boom"))
        {
            BoomEffect boomEffect = collision.GetComponent<BoomEffect>();
            TakeDamage(boomEffect.damage);
        }
    }
}
