using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public TMP_InputField nombreInputField;
    public UserSO userSO;
    public GameEvent onUserReadyToUpload;

    private void Start()
    {
        // Cargar nombre si ya existía
        nombreInputField.text = userSO.UserData.nickName;
    }

    public void GuardarNombre()
    {
        string nombre = nombreInputField.text.Trim();
        if (string.IsNullOrEmpty(nombre)) return;

        int id = Random.Range(1000000, 9999999);
        userSO.SetUserData(nombre, id);

        onUserReadyToUpload?.Raise();
    }

    public void Jugar()
    {
        GuardarNombre(); // Asegura que se guarde antes
        SceneManager.LoadScene("GameplayScene");
    }

    public void VerPuntajes()
    {
        // Puedes cargar una escena o abrir un panel
        Debug.Log("Puntajes aún no implementado");
    }

    public void Salir()
    {
        Application.Quit();
    }
}
