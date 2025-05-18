using UnityEngine;

public class ScoreUploadListener : MonoBehaviour
{
    [SerializeField] private UserSO userSO;

    public void UploadScoreIfNewRecord(float score)
    {
        int intScore = Mathf.RoundToInt(score);
        if (userSO.TrySetNewScore(intScore))
        {
            Debug.Log($"Nuevo r�cord: {intScore}, subiendo a Firebase...");
            DatabaseHandler.Instance.SaveUserToFirebase();
        }
        else
        {
            Debug.Log($"Score {intScore} no super� el r�cord. No se sube.");
        }
    }
}

