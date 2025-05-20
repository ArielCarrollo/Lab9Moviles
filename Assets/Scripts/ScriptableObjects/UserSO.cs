using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "User", menuName = "ScriptableObjects/Example/User")]
public class UserSO : ScriptableObject
{
    [SerializeField] private User userData = new();

    public User UserData => userData;

    public void SetUserData(string nickName, int id)
    {
        userData.nickName = nickName;
        userData.id = id;
    }

    public bool TrySetNewScore(int score)
    {
        if (score > userData.highScore)
        {
            userData.highScore = score;
            return true;
        }
        return false;
    }
}






