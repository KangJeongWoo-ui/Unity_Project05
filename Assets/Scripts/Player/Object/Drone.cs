using UnityEngine;

public enum Dronetype { Right, Left }; // 왼쪽 드론, 오른쪽 드론

public class Drone : MonoBehaviour
{
    [Header("# Dron States")]
    public Dronetype type;
    public float smoothTime;
    public float maxSpeed;
    public float fireBulletRate;

    public GameObject dronBullet;

    // 따라갈 대상의 위치
    private Transform target;

    private Vector3 offset;
    private float nextFireBulletRate = 0;

    void Start()
    {
        if(target == null)
            target = GameObject.FindWithTag("Player").transform;

        // 오른쪽 드론이면 플레이어 오른쪽에 위치
        if(type == Dronetype.Right)
        {
            offset = new Vector3(0.5f, -0.5f, 0f);
        }

        // 왼쪽 드론이면 플레이어 왼쪽에 위치
        else if(type == Dronetype.Left)
        {
            offset = new Vector3(-0.5f, -0.5f, 0);
        }
    }
    void Update()
    {
        FindTarget();
        FireBullet();
    }

    // 플레이어를 부드럽게 따라감
    void FindTarget()
    {
        Vector3 curPos = transform.position;
        Vector3 targetPos = target.transform.position + offset;
        Vector3 velocity = Vector3.zero;

        transform.position = Vector3.SmoothDamp(curPos, targetPos, ref velocity, smoothTime, maxSpeed);
    }
    void FireBullet()
    {
        if(Time.time >= nextFireBulletRate)
        {
            Shot();
            nextFireBulletRate = Time.time + fireBulletRate;
        }
    }
    void Shot()
    {
        Instantiate(dronBullet, transform.position, Quaternion.identity);
    }
}
