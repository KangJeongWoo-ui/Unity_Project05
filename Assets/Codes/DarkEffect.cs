using System.Collections;
using UnityEngine;

public class DarkEffect : MonoBehaviour
{
    SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        StartCoroutine(darkCoroutine());
    }

    IEnumerator darkCoroutine()
    {
        while(true)
        {
            float cur = (Mathf.Sin(Time.time * 2f) + 1f) * 0.5f;
            Color c = sr.color;
            c.a = cur;
            sr.color = c;

            yield return null;
        }
    }
    
}
