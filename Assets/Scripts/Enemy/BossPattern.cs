using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public static class BossPattern
{
    public static void CircleShot(GameObject bulletPrefab, int bulletCount, Vector3 pos)
    {
        float angleStep = 360f / bulletCount;
        float angle = 0f;

        for (int i = 0; i < bulletCount; i++)
        {
            Quaternion rot = Quaternion.Euler(0, 0, angle);
            Object.Instantiate(bulletPrefab, pos, rot);
            angle += angleStep;
        }
    }

    public static void AutoShot(GameObject bulletPrefab, Transform boss, Transform target)
    {
        Vector2 dir = (target.position - boss.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        Quaternion rot = Quaternion.Euler(0, 0, angle);

        Object.Instantiate(bulletPrefab, boss.position, rot);
    }

    public static IEnumerator BossMeteor(GameObject meteorLinePrefab, GameObject bossMeteorPrefab, Transform[] spawnPoint)
    {
        int rand = Random.Range(0, spawnPoint.Length);

        GameObject[] lines = new GameObject[spawnPoint.Length];

        for (int i = 0; i < spawnPoint.Length; i++)
        {
            if (i == rand) continue;

            lines[i] = Object.Instantiate(
                meteorLinePrefab,
                spawnPoint[i].position + new Vector3(0, -6, 0),
                spawnPoint[i].rotation
            );
        }

        yield return new WaitForSeconds(0.3f);

        for (int i = 0; i < spawnPoint.Length; i++)
        {
            if (i == rand) continue;

            if (lines[i] != null)
                Object.Destroy(lines[i]);

            Object.Instantiate(
                bossMeteorPrefab,
                spawnPoint[i].position,
                spawnPoint[i].rotation
            );
        }
        yield return new WaitForSeconds(0.5f);
    }
    public static void SpiralShot(GameObject bulletPrefab, int bulletCount, float angleOffset, Vector3 pos)
    {
        float angle = angleOffset;
        float angleStep = 360f / bulletCount;

        for (int i = 0; i < bulletCount; i++)
        {
            Quaternion rot = Quaternion.Euler(0, 0, angle);
            Object.Instantiate(bulletPrefab, pos, rot);
            angle += angleStep;
        }
    }
    public static void FanShot(GameObject bulletPrefab, int bulletCount, Vector3 pos, float startAngle, float endAngle)
    {
        float totalAngle = endAngle - startAngle;

        float angleStep = totalAngle / (bulletCount - 1);

        float currentAngle = startAngle;

        for (int i = 0; i < bulletCount; i++)
        {
            Quaternion rot = Quaternion.Euler(0, 0, currentAngle + 180f);
            Object.Instantiate(bulletPrefab, pos, rot);

            currentAngle += angleStep;
        }
    }
    public static void DragonShot(GameObject enemyPrefab, Transform[] spawnPoint)
    {
        int rand = Random.Range(0, spawnPoint.Length);

        for (int i = 0; i < spawnPoint.Length; i++)
        {
            if(rand == i)
            {
                Object.Instantiate(enemyPrefab,spawnPoint[i]);
            }
        }
    }
}
