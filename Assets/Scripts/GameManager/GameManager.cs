using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    // 게임 시작 씬
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    // 게임 재시작 씬
    public void RetryGame()
    {
        SceneManager.LoadScene("MainScene");
    }
}
