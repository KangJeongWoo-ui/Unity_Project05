using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SpawnManager : MonoBehaviour
{
    BossDragon boss;

    [Header("# Object to create")]
    public GameObject[] dragon;
    public GameObject[] bossDragon;
    public GameObject meteorLine;
    public GameObject meteor;

    [Header("# Location to create")]
    public Transform[] spawnPoint;

    [Header("# Spawn Rate")]

    // 다음 생성 시간
    private float nextSpawn = 0f;

    // 최대 다음 생성 시간
    public float maxSpawnRate;

    // 최소 다음 생성 시간
    public float minSpawnRate;

    [Header("# Boss Dragon Spawn")]

    // 보스 생성 시간
    public float bossSpawnTime;

    // 보스 스테이지 레벨
    public int bossStageLevel;

    private float bossTime;

    // 보스 스테이지 확인 여부
    bool bossStage = false;

    // 보스 생성 메세지
    public GameObject bossSpawnMessage;

    void Update()
    {
        // 보스 스테이지가 아니라면 적 유닛과 메테오 생성
        if (bossStage) return;
        bossTime += Time.deltaTime;
        SpawnTimer();

        // 보스 생성 시간이 되면 보스 생성
        if(bossTime >= bossSpawnTime)
        {
            bossStage = true;
            BossSpawn();
        }
    }

    // 랜덤한 생성 시간 설정
    void SpawnTimer()
    {
        nextSpawn -= Time.deltaTime;
        if(nextSpawn <= 0f)
        {
            EnemyDragonSpawn();
            MeteorSpawn();
            nextSpawn += Random.Range(minSpawnRate, maxSpawnRate);
        }
    }

    // 적 유닛 생성
    void EnemyDragonSpawn()
    {
        // 랜덤한 적 유닛 생성
        int randomEnemyDragonObject = Random.Range(0, dragon.Length);

        // 랜덤한 스폰 위치 지정
        int randomSpawnPosition = Random.Range(0, spawnPoint.Length);

        // 랜덤한 적 유닛을 랜덤한 스폰 위치에 생성
        Instantiate(dragon[randomEnemyDragonObject], spawnPoint[randomSpawnPosition].position, spawnPoint[randomSpawnPosition].rotation);
    }

    // 보스 생성
    void BossSpawn()
    {
        StartCoroutine(BossMessage());

        GameObject bossdragon = Instantiate(bossDragon[bossStageLevel], spawnPoint[2].position, spawnPoint[2].rotation);

        BossDragon boss = bossdragon.GetComponent<BossDragon>();

        boss.meteorSpawnPoints = spawnPoint;
        
        // 보스가 죽으면 이벤트를 받음
        boss.OnBossDie += BossDie;
    }
    void BossDie()
    {
        // 보스가 죽을 시

        // 보스 스테이지 레벨을 올림
        bossStageLevel +=1;

        // 보스 생성 시간 초기화
        bossTime = 0f;

        // 보스 스테이지 끔
        bossStage = false;

        // 보스 스테이지가 3이상이면 (3단계 보스까지 죽으면)
        // 게임 클리어 씬
        if(bossStageLevel >= 3)
        {
            SceneManager.LoadScene("GameClearScene");
        }
    }


    // 메테오 생성
    void MeteorSpawn()
    {
        int randomSpawnPosition = Random.Range(0, 5);

        StartCoroutine(MeteorSpawnCoroutine(randomSpawnPosition));
    }

    IEnumerator MeteorSpawnCoroutine(int index)
    {
        // 메테오 경고 메세지 출력
        GameObject meteorline = Instantiate(meteorLine, spawnPoint[index].position + new Vector3(0,-6,0), spawnPoint[index].rotation);
        yield return new WaitForSeconds(1f);

        Destroy(meteorline);
        yield return new WaitForSeconds(0.5f);

        // 메테오 경고 메세지 삭제 후 메테오 생성
        Instantiate(meteor, spawnPoint[index].position, spawnPoint[index].rotation);
    }
    IEnumerator BossMessage()
    {
        bossSpawnMessage.SetActive(true);
        yield return new WaitForSeconds(2f);
        bossSpawnMessage.SetActive(false);
    }
}
