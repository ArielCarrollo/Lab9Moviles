// Guardar como Assets/Scripts/Game/Platform.cs
using UnityEngine;

public class Platform : MonoBehaviour
{
    private bool _hasBeenTouched = false;
    public bool HasBeenTouched => _hasBeenTouched;
    public enum MovementPattern { None, HorizontalPingPong }
    private MovementPattern currentPattern = MovementPattern.None;

    private float moveSpeed = 2f;
    private float moveDistance = 3f;
    private Vector3 startPosition; 
    private float pingPongOffset; 

    public void Initialize(MovementPattern pattern, Vector3 initialPosition, float speed, float distance)
    {
        transform.position = initialPosition;
        this.startPosition = initialPosition; 
        this.currentPattern = pattern;
        this.moveSpeed = speed;
        this.moveDistance = distance;

        if (currentPattern == MovementPattern.HorizontalPingPong)
        {
            pingPongOffset = Random.value * 100f;
        }
    }

    void Update()
    {
        if (currentPattern == MovementPattern.HorizontalPingPong)
        {
            float newX = startPosition.x + Mathf.PingPong((Time.time + pingPongOffset) * moveSpeed, moveDistance) - (moveDistance / 2f);
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
        }
    }
    public void MarkAsTouched()
    {
        _hasBeenTouched = true;
        GetComponent<SpriteRenderer>().color = Color.gray;
    }

    public void ResetPlatform()
    {
        _hasBeenTouched = false;
        GetComponent<SpriteRenderer>().color = Color.white;
    }

}