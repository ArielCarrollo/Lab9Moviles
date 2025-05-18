using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public TMP_InputField nombreInputField;
    public UserSO userSO;
    public GameEvent onUserReadyToUpload;
    [Header("Puntajes")]
    [SerializeField] private GameObject scoresPanel;
    [SerializeField] private TMP_Text[] scoreTexts;

    private void Start()
    {
        // Cargar nombre si ya existía
        nombreInputField.text = userSO.UserData.nickName;
    }

    public void GuardarNombre()
    {
        string nombre = nombreInputField.text.Trim();
        if (string.IsNullOrEmpty(nombre)) return;

        // Comparar con el nickname actual del UserSO
        if (nombre != userSO.UserData.nickName)
        {
            int nuevoID = UnityEngine.Random.Range(1000000, 9999999);
            userSO.SetUserData(nombre, nuevoID); // Generar nuevo ID solo si el nombre cambia
        }
        else
        {
            // Mantener el ID existente si el nickname es el mismo
            userSO.SetUserData(nombre, userSO.UserData.id);
        }

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
        StartCoroutine(DatabaseHandler.Instance.GetTopHighScores(scoreTexts.Length, MostrarScores));
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
