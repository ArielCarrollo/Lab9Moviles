
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform playerTransform;
    public float verticalOffset = 4f;   
    public float smoothSpeed = 5f;    
    public float minYPosition;         

    private Vector3 initialOffsetFromPlayerXY; 

    void Start()
    {

        minYPosition = transform.position.y; 

    }

    void LateUpdate() 
    {
        if (playerTransform != null && !GameManager.Instance.isGameOver)
        {

            float targetY = playerTransform.position.y + verticalOffset;

            if (targetY > transform.position.y && targetY > minYPosition)
            {
                Vector3 targetPosition = new Vector3(transform.position.x, targetY, transform.position.z);


                transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
            }
        }
    }
}