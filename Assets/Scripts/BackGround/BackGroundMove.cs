using UnityEngine;

public class BackGroundMove : MonoBehaviour
{
    [Header("# BackGround MoveSpeed")]

    // 배경 이동 속도
    public float moveSpeed;

    Rigidbody2D rb;
    void Update()
    {
        BackgroundMovement();
    }
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void BackgroundMovement()
    {
        rb.linearVelocity = Vector2.down * moveSpeed;

        // 이동 중 y좌표가 -11이 넘어가면 위로 +12 이동
        if(transform.position.y < -11)
        {
            transform.position = new Vector3(0, 12, 0);
        }
    }
}
