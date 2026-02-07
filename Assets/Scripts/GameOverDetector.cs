// ゲームオーバーを検出するスクリプト

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOverDetector : MonoBehaviour
{
    private bool isGameOver = false;

    void Update()
    {
        if (isGameOver) return;

        // ゲームオーバー判定
        if (transform.position.y < GameConfig.DeathYThreshold || transform.position.x < GameConfig.DeathXThreshold)
        {
            StartCoroutine(GameOverSequence());
        }
    }

    IEnumerator GameOverSequence()
    {
        isGameOver = true;
        Debug.Log("Game Over!");

        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.StartGameOverEffect(GameConfig.GameOverDelay);
        }

        Time.timeScale = GameConfig.SlowMotionScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        yield return new WaitForSecondsRealtime(GameConfig.GameOverDelay);

        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = 0.02f;
        SceneManager.LoadScene("TitleScene");
    }
}
