using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "User", menuName = "ScriptableObjects/Example/User")]
public class UserSO : ScriptableObject
{
    [SerializeField] private User userData;

    public User UserData => userData;

    public void SetUserData(string nickName, int id)
    {
        userData = new User(nickName, id, new int[0]);
    }

    public void AddScore(int score)
    {
        List<int> updatedScores = new List<int>(userData.scores);

        if (updatedScores.Count < 5)
        {
            updatedScores.Add(score);
        }
        else
        {
            updatedScores.RemoveAt(0); // FIFO
            updatedScores.Add(score);
        }

        userData.scores = updatedScores.ToArray();
    }
}


