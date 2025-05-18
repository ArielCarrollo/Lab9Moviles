using UnityEngine;

public class ScoreUploadListener : MonoBehaviour
{
    [SerializeField] private UserSO userSO;

    public void UploadScoreIfNewRecord(float score)
    {
        int intScore = Mathf.RoundToInt(score);
        if (userSO.TrySetNewScore(intScore))
        {
            Debug.Log($"Nuevo récord: {intScore}, subiendo a Firebase...");
            DatabaseHandler.Instance.SaveUserToFirebase();
        }
        else
        {
            Debug.Log($"Score {intScore} no superó el récord. No se sube.");
        }
    }
}

