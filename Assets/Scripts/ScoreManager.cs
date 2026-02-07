using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    private float currentScore = 0f;
    public float pointsPerSecond = 10f;

    void Update()
    {
        currentScore += Time.deltaTime * pointsPerSecond;

        if (scoreText != null)
        {
            scoreText.text = "Score: " + Mathf.FloorToInt(currentScore).ToString();
        }
    }
}