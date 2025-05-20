using Firebase.Firestore;
using UnityEngine;
[FirestoreData]
[System.Serializable]
public class User
{
    [FirestoreProperty] public string nickName { get; set; }
    [FirestoreProperty] public int id { get; set; }
    [FirestoreProperty] public int highScore { get; set; }

    public User() { } // Necesario para Firestore
    public User(string nickName, int id, int highScore)
    {
        this.nickName = nickName;
        this.id = id;
        this.highScore = highScore;
    }
}

