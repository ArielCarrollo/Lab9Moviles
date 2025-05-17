using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int score = 15;

    public void UpdateScore()
    {
        DatabaseHandler.instance.UpdateScore(score);
    }
}