using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;
using Firebase.Auth;

public class MainMenuManager : MonoBehaviour
{
    public UserSO userSO;
    public GameEvent onUserReadyToUpload;

    [Header("Puntajes")]
    [SerializeField] private GameObject scoresPanel;
    [SerializeField] private TMP_Text[] scoreTexts;

    private void Start()
    {
        // Asignar el email como nickName, si el usuario está autenticado
        var currentUser = FirebaseAuth.DefaultInstance.CurrentUser;
        if (currentUser != null)
        {
            string email = currentUser.Email;
            if (!string.IsNullOrEmpty(email))
            {
                userSO.SetUserData(email, userSO.UserData.id); // Mantén el id si lo tienes o pon uno
            }
        }
    }

    public void GuardarNombre()
    {
        var currentUser = FirebaseAuth.DefaultInstance.CurrentUser;
        if (currentUser == null)
        {
            Debug.LogWarning("No user authenticated");
            return;
        }

        string email = currentUser.Email;
        if (string.IsNullOrEmpty(email)) return;

        // Si quieres mantener un id, asegúrate de que esté asignado:
        int id = userSO.UserData.id;
        if (id == 0)
            id = UnityEngine.Random.Range(1000000, 9999999);

        userSO.SetUserData(email, id);

        onUserReadyToUpload?.Raise();
    }

    public void Jugar()
    {
        GuardarNombre(); // Asegura que se guarde antes
        SceneManager.LoadScene("GameplayScene");
    }

    public void VerPuntajes()
    {
        scoresPanel.SetActive(true);
        FirestoreHandler.Instance.GetTopHighScores(scoreTexts.Length, MostrarScores);
    }

    private void MostrarScores(List<(string name, int score)> topScores)
    {
        for (int i = 0; i < scoreTexts.Length; i++)
        {
            if (i < topScores.Count)
            {
                var entry = topScores[i];
                scoreTexts[i].text = $"{i + 1}. {entry.name} - {entry.score}";
            }
            else
            {
                scoreTexts[i].text = $"{i + 1}. ---";
            }
        }
    }

    public void CerrarPanelPuntajes()
    {
        scoresPanel.SetActive(false);
    }

    public void Salir()
    {
        Application.Quit();
    }
}

