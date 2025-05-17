using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase.Database;
using System;

public class DatabaseHandler : MonoBehaviour
{
    private string User_ID;
    private DatabaseReference reference;

    private string name_player;

    public Text txt;
    public InputField input_field;

    public static DatabaseHandler instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        User_ID = SystemInfo.deviceUniqueIdentifier;
    }

    void Start()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        SetName();
    }

    private void SetName()
    {
        StartCoroutine(GetFirstName(UpdateName));
    }

    private void UpdateName(string name)
    {
        txt.text = name;
        Debug.Log($"Nombre seteado {name}");
    }

    public void EnterName()
    {
        CreateUser(input_field.text);
        SetName();
    }

    private void CreateUser(string name)
    {
        User new_user = new(name, "Piedrito", 123456);
        string json = JsonUtility.ToJson(new_user);

        name_player = name;

        reference.Child("users").Child(User_ID).SetRawJsonValueAsync(json);
    }

    private IEnumerator GetFirstName(Action<string> onCallBack, bool fromDatabase = false)
    {
        var user_name_data = reference.Child("users").Child(User_ID).Child("first_name").GetValueAsync();

        yield return new WaitUntil(predicate: () => user_name_data.IsCompleted);

        if (user_name_data != null)
        {
            DataSnapshot snapshot = user_name_data.Result;
            onCallBack?.Invoke(snapshot.Value.ToString());
        }
        else
        {
            Debug.LogWarning("No se conecto a la base de datos");
        }
    }

    private IEnumerator GetLastName(Action<string> onCallBack)
    {
        var user_name_data = reference.Child("users").Child(User_ID).Child("last_name").GetValueAsync();

        yield return new WaitUntil(predicate: () => user_name_data.IsCompleted);

        if (user_name_data != null)
        {
            DataSnapshot snapshot = user_name_data.Result;
            onCallBack?.Invoke(snapshot.Value.ToString());
        }
    }

    private IEnumerator GetUserID(Action<int> onCallBack)
    {
        var user_name_data = reference.Child("users").Child(User_ID).Child(nameof(User.score)).GetValueAsync();

        yield return new WaitUntil(predicate: () => user_name_data.IsCompleted);

        if (user_name_data != null)
        {
            DataSnapshot snapshot = user_name_data.Result;
            onCallBack?.Invoke(int.Parse(snapshot.Value.ToString()));
        }
        StartCoroutine(GetFirstName(UpdateName, true));
    }


    public void GetUserInfo()
    {
        StartCoroutine(GetFirstName(PrintData));
        StartCoroutine(GetLastName(PrintData));
        StartCoroutine(GetUserID(PrintData));
    }

    private void PrintData(string data)
    {
        Debug.Log(data);
    }

    private void PrintData(int data)
    {
        Debug.Log(data);
    }

    public void SaveUser()
    {
        CreateUser("Player");
    }

    public void LoadUser()
    {
        Invoke(nameof(GetUserInfo), 1f);
    }

    public void GoToPlay()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void UpdateScore(int score)
    {
        User new_user = new(name_player, "Piedrito", 123456);
        string json = JsonUtility.ToJson(new_user);

        reference.Child("users").Child(User_ID).SetRawJsonValueAsync(json);
    }
}

public class User
{
    public string first_name;
    public string last_name;

    public int score;

    public User(string first_name, string last_name, int score, bool initialize = true)
    {
        this.first_name = first_name;
        this.last_name = last_name;
        this.score = score;

    }


}