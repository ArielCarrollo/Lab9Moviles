using UnityEngine;
[System.Serializable]
public class User
{
    public string nickName;
    public int id;
    public int highScore;

    public User(string nickName, int id, int highScore)
    {
        this.nickName = nickName;
        this.id = id;
        this.highScore = highScore;
    }
}
