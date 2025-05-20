using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using Firebase;
using System.Threading.Tasks;
using UnityEngine.Events;
using TMPro;

public class Authentication : MonoBehaviour
{
    [Header("UI Fields")]
    public TMP_InputField emailField;
    public TMP_InputField passwordField;
    public GameObject loginPanel;
    public GameObject mainMenuPanel;

    private FirebaseAuth auth;

    public UnityEvent OnLoginSuccess;

    private void Awake()
    {
        auth = FirebaseAuth.DefaultInstance;
    }

    private void Start()
    {
        loginPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }

    public void OnLoginButton()
    {
        string email = emailField.text;
        string password = passwordField.text;
        StartCoroutine(SignInWithEmail(email, password));
        loginPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void OnRegisterButton()
    {
        string email = emailField.text;
        string password = passwordField.text;
        StartCoroutine(RegisterUser(email, password));
    }

    private IEnumerator RegisterUser(string email, string password)
    {
        var task = auth.CreateUserWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(() => task.IsCompleted);

        if (task.Exception != null)
        {
            Debug.LogWarning("Registration failed: " + task.Exception);
        }
        else
        {
            Debug.Log("User registered: " + task.Result.User.Email);
            OnLoginSuccess.Invoke(); // Activa panel principal
        }
    }

    private IEnumerator SignInWithEmail(string email, string password)
    {
        var task = auth.SignInWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(() => task.IsCompleted);

        if (task.Exception != null)
        {
            Debug.LogWarning("Login failed: " + task.Exception);
        }
        else
        {
            Debug.Log("User logged in: " + task.Result.User.Email);
            OnLoginSuccess.Invoke(); // Activa panel principal
        }
    }

    public void Logout()
    {
        auth.SignOut();
        loginPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }
}

