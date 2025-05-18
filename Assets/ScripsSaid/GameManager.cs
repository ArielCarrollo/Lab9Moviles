// Guardar como Assets/Scripts/Game/GameManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : SingletonNonPersistent<GameManager>
{
    public bool isGameOver = false;
    private float finalScore = 0f;

    [SerializeField] private GameEventFloat onPlayerDiedEvent;

    [SerializeField] private GameObject restartPanel;
    void Start()
    {
        Time.timeScale = 1f; 
        isGameOver = false;
    }

    public void HandlePlayerDeath()
    {
        if (isGameOver) return;
        isGameOver = true;

        finalScore = ScoreManager.Instance.CurrentScore;
        onPlayerDiedEvent.Raise(finalScore);

        Debug.Log($"GAME OVER! Puntaje Final: {finalScore}");
        restartPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        ScoreManager.Instance.Initialize();
        SceneManager.LoadScene("Menu");
    }

}