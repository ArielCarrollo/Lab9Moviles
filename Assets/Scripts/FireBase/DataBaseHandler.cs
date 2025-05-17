using UnityEngine;
using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;

public class DatabaseHandler : SingletonPersistent<DatabaseHandler>
{
    private string userID;
    private DatabaseReference reference;
    [SerializeField] private UserSO userSO;

    public override void Awake()
    {
        base.Awake();
        userID = SystemInfo.deviceUniqueIdentifier;
    }

    private void Start()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        Invoke(nameof(GetUserInfo), 1f);
    }

    public void SaveUserToFirebase()
    {
        User currentUser = userSO.UserData;
        string json = JsonUtility.ToJson(currentUser);
        reference.Child("users").Child(userID).SetRawJsonValueAsync(json);
        Debug.Log("Datos subidos a Firebase");
    }

    public IEnumerator GetNickname(Action<string> onCallBack)
    {
        var data = reference.Child("users").Child(userID).Child("nickName").GetValueAsync();
        yield return new WaitUntil(() => data.IsCompleted);

        if (data != null && data.Result.Exists)
            onCallBack?.Invoke(data.Result.Value.ToString());
    }

    public IEnumerator GetID(Action<int> onCallBack)
    {
        var data = reference.Child("users").Child(userID).Child("id").GetValueAsync();
        yield return new WaitUntil(() => data.IsCompleted);

        if (data != null && data.Result.Exists)
            onCallBack?.Invoke(int.Parse(data.Result.Value.ToString()));
    }

    public IEnumerator GetScores(Action<int[]> onCallBack)
    {
        var data = reference.Child("users").Child(userID).Child("scores").GetValueAsync();
        yield return new WaitUntil(() => data.IsCompleted);

        if (data != null && data.Result.Exists)
        {
            List<int> scores = new();
            foreach (var child in data.Result.Children)
                scores.Add(int.Parse(child.Value.ToString()));
            onCallBack?.Invoke(scores.ToArray());
        }
    }

    public void GetUserInfo()
    {
        StartCoroutine(GetNickname(PrintData));
        StartCoroutine(GetID(PrintData));
        StartCoroutine(GetScores(PrintScores));
    }

    private void PrintData(string value) => Debug.Log($"Nickname: {value}");
    private void PrintData(int value) => Debug.Log($"ID: {value}");
    private void PrintScores(int[] scores)
    {
        Debug.Log("Scores:");
        foreach (int score in scores)
            Debug.Log(score);
    }
}

