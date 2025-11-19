using UnityEngine;
using System.Collections;
public class IceLaser : MonoBehaviour
{
    Vector2 moveVec;
    public float moveTime;
    public float speed;

    public void Init(Vector2 mv)
    {
        moveVec = mv.normalized;
        StartCoroutine(MoveCoroutine());
    }
    IEnumerator MoveCoroutine()
    {
        float elapsed = 0f;
        while (elapsed < moveTime)
        {
            transform.Translate(moveVec * speed * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
