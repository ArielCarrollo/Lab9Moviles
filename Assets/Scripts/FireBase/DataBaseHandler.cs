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
        userID = userSO.UserData.id.ToString(); // Usar el ID actual del UserSO
        Invoke(nameof(GetUserInfo), 1f);
    }

    public void SaveUserToFirebase()
    {
        User currentUser = userSO.UserData;
        userID = currentUser.id.ToString(); // Actualizar el userID antes de guardar
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
    public IEnumerator GetTopHighScores(int count, Action<List<(string name, int score)>> onCallback)
    {
        var task = reference.Child("users")
                            .OrderByChild("highScore")
                            .LimitToLast(count)
                            .GetValueAsync();

        yield return new WaitUntil(() => task.IsCompleted);

        List<(string name, int score)> topScores = new();

        if (task.Result != null && task.Result.Exists)
        {
            foreach (var user in task.Result.Children)
            {
                string name = user.Child("nickName").Value.ToString();
                int score = int.Parse(user.Child("highScore").Value.ToString());
                topScores.Add((name, score));
            }

            // Ordenar descendente ya que Firebase lo devuelve ascendente
            topScores.Sort((a, b) => b.score.CompareTo(a.score));
        }

        onCallback?.Invoke(topScores);
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

