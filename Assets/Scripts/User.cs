using UnityEngine;

[System.Serializable]
public class User
{
    public string nickName;
    public int id;
    public int[] scores;

    public User(string nickName, int id, int[] scores)
    {
        this.nickName = nickName;
        this.id = id;
        this.scores = scores;
    }
}