// Guardar como Assets/Scripts/Game/GameManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool isGameOver = false;
    private float finalScore = 0f;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Time.timeScale = 1f; 
        isGameOver = false;


        PlayerController.OnPlayerDied += HandlePlayerDeath;
    }

    void OnDestroy()
    {
        PlayerController.OnPlayerDied -= HandlePlayerDeath; 
    }

    void HandlePlayerDeath(float score)
    {
        if (isGameOver) return;

        isGameOver = true;
        finalScore = score;
        Time.timeScale = 0f; 

        Debug.Log($"GAME OVER! Altura Final: {finalScore:F2}"); 

    }


    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
   
}