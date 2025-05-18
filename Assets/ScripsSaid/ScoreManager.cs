using UnityEngine;
using TMPro;

public class ScoreManager : SingletonNonPersistent<ScoreManager>
{
    private int _currentScore = 0;
    [SerializeField] private TMP_Text scoreText;

    public int CurrentScore => _currentScore;

    public void Initialize()
    {
        _currentScore = 0;
        UpdateScoreUI();
    }

    public void AddScore(int amount)
    {
        _currentScore += amount;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = $"Score: {_currentScore}";
    }

    public void SetScoreUIReference(TMP_Text textComponent)
    {
        scoreText = textComponent;
        UpdateScoreUI();
    }
}