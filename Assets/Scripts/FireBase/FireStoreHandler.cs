using UnityEngine;
using Firebase.Firestore;
using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

public class FirestoreHandler : SingletonPersistent<FirestoreHandler>
{
    private FirebaseFirestore db;
    private string userID;
    [SerializeField] private UserSO userSO;

    public override void Awake()
    {
        base.Awake();
        db = FirebaseFirestore.DefaultInstance;
    }

    private void Start()
    {
        FirebaseUser currentUser = FirebaseAuth.DefaultInstance.CurrentUser;
        if (currentUser != null)
        {
            userID = currentUser.UserId;
            LoadUserData(); // Cargar al inicio
        }
        else
        {
            Debug.LogWarning("No user authenticated.");
        }
    }

    public async void SaveUserToFirestore()
    {
        if (string.IsNullOrEmpty(userID))
        {
            Debug.LogError("User ID is null.");
            return;
        }

        User user = userSO.UserData;

        Debug.Log($"Saving user: {user.nickName}, ID: {user.id}, Score: {user.highScore}");

        DocumentReference docRef = db.Collection("users").Document(userID);
        await docRef.SetAsync(user);

        Debug.Log("User data saved to Firestore.");
    }


    public async void LoadUserData()
    {
        DocumentReference docRef = db.Collection("users").Document(userID);
        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

        if (snapshot.Exists)
        {
            User user = snapshot.ConvertTo<User>();
            userSO.SetUserData(user.nickName, user.id);
            userSO.TrySetNewScore(user.highScore);
            Debug.Log($"User data loaded: {user.nickName} - {user.id} - {user.highScore}");
        }
        else
        {
            Debug.Log("No user data found, creating new...");
            SaveUserToFirestore();
        }
    }

    public async void GetTopHighScores(int count, Action<List<(string name, int score)>> onCallback)
    {
        Query query = db.Collection("users").OrderByDescending("highScore").Limit(count);
        QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

        List<(string, int)> topScores = new();

        foreach (DocumentSnapshot doc in querySnapshot.Documents)
        {
            User user = doc.ConvertTo<User>();
            topScores.Add((user.nickName, user.highScore));
        }

        onCallback?.Invoke(topScores);
    }
}

