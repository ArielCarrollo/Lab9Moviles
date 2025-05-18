using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "User", menuName = "ScriptableObjects/Example/User")]
public class UserSO : ScriptableObject
{
    [SerializeField] private User userData;

    public User UserData => userData;

    public void SetUserData(string nickName, int id)
    {
        // Si el ID es 0 (valor por defecto), generar uno nuevo
        if (id == 0)
        {
            id = Random.Range(1000000, 9999999);
        }
        userData = new User(nickName, id, userData?.highScore ?? 0); // Mantener el highScore existente
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




