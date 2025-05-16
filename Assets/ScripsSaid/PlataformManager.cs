using UnityEngine;
using System.Collections.Generic;

public class PlatformManager : MonoBehaviour
{
    [Header("Object Pooling")]
    public DinamicObjectPooling platformPooler;

    [Header("Platform Settings")]
    public Transform playerTransform;
    public float horizontalSpawnRange = 3.5f;
    public float minYSpacing = 2.0f;
    public float maxYSpacing = 4.0f;
    public int initialPlatforms = 10;
    public float spawnAheadDistance = 15f;
    public float despawnBehindDistance = 20f;
    public float playerYOffsetFromFirstPlatform = 0.75f;

    [Header("Moving Platform Settings")]
    public bool allowMovingPlatforms = true;
    [Range(0f, 1f)]
    public float movingPlatformChance = 0.25f;
    public float minMoveSpeed = 1f;
    public float maxMoveSpeed = 3f;
    public float minMoveDistance = 2f;
    public float maxMoveDistance = 4f;

    private List<PoolObject> activePlatforms = new List<PoolObject>();
    private float lastSpawnedYPosition;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;

        PlayerController playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            playerTransform = playerController.transform;
        }
        else
        {
            Debug.LogError("PlayerController no encontrado en la escena. PlatformManager no puede funcionar.");
            enabled = false;
            return;
        }

        Vector3 firstPlatformPosition = new Vector3(0f, 0f, 0f);

        SpawnSpecificPlatform(firstPlatformPosition, false);
        if (activePlatforms.Count > 0)
        {
            activePlatforms[0].gameObject.name = "InitialPlatform";
        }
        else
        {
            Debug.LogError("No se pudo generar la plataforma inicial desde el pool.");
            enabled = false;
            return;
        }

        Vector3 playerStartPosition = new Vector3(
            firstPlatformPosition.x,
            firstPlatformPosition.y + playerYOffsetFromFirstPlatform,
            firstPlatformPosition.z
        );

        playerController.InitializePositionAndHeight(playerStartPosition);

        lastSpawnedYPosition = firstPlatformPosition.y;

        for (int i = 0; i < initialPlatforms; i++)
        {
            GeneratePlatform(true);
        }
    }

    void Update()
    {
        if (playerTransform == null || (GameManager.Instance != null && GameManager.Instance.isGameOver)) return;

        if (playerTransform.position.y + spawnAheadDistance > lastSpawnedYPosition)
        {
            GeneratePlatform(true);
        }

        DespawnOldPlatforms();
    }

    void GeneratePlatform(bool spawnAboveLast)
    {
        float randomX = Random.Range(-horizontalSpawnRange, horizontalSpawnRange);
        float yPos;

        if (spawnAboveLast)
        {
            float randomYSpacing = Random.Range(minYSpacing, maxYSpacing);
            yPos = lastSpawnedYPosition + randomYSpacing;
        }
        else
        {
            yPos = lastSpawnedYPosition;
        }

        Vector3 spawnPosition = new Vector3(randomX, yPos, 0);
        bool shouldMove = allowMovingPlatforms && Random.value < movingPlatformChance;
        SpawnSpecificPlatform(spawnPosition, shouldMove);
        lastSpawnedYPosition = yPos;
    }

    void SpawnSpecificPlatform(Vector3 position, bool shouldMove)
    {
        PoolObject platformInstance = platformPooler.GetObject();
        if (platformInstance != null)
        {
            platformInstance.transform.position = position;

            Platform platformScript = platformInstance.GetComponent<Platform>();
            if (platformScript != null)
            {
                if (shouldMove)
                {
                    float speed = Random.Range(minMoveSpeed, maxMoveSpeed);
                    float dist = Random.Range(minMoveDistance, maxMoveDistance);
                    platformScript.Initialize(Platform.MovementPattern.HorizontalPingPong, position, speed, dist);
                }
                else
                {
                    platformScript.Initialize(Platform.MovementPattern.None, position, 0, 0);
                }
            }
            activePlatforms.Add(platformInstance);
        }
    }

    void DespawnOldPlatforms()
    {
        if (mainCamera == null) return;
        float despawnYLimit = mainCamera.transform.position.y - mainCamera.orthographicSize - despawnBehindDistance;

        for (int i = activePlatforms.Count - 1; i >= 0; i--)
        {
            PoolObject platform = activePlatforms[i];
            if (platform.transform.position.y < despawnYLimit)
            {
                platformPooler.ReturnObject(platform);
                activePlatforms.RemoveAt(i);
            }
        }
    }
}