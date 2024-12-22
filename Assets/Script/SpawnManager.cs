using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Coin Prefabs")]
    [SerializeField] private GameObject[] coinPrefabs; // Array to hold multiple coin prefabs

    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 5f;   // Time interval between spawns
    [SerializeField] private float spawnOffset = 2.0f;     // Offset to add/subtract from the player's position
    [SerializeField] private float minX = -2f;             // Minimum X position
    [SerializeField] private float maxX = 2f;              // Maximum X position
    [SerializeField] private float spawnDistance = 15.0f;  // Distance ahead of the player to spawn coins
    float minimiumConstraint;
    float maximumConstraint;
    [SerializeField] private int SpawnRatio = 5;


    private Transform playerTransform;

    private void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        minimiumConstraint=playerTransform.position.x + minX;
        maximumConstraint = playerTransform.position.x + maxX;
        InvokeRepeating("SpawnCoin", 3, spawnInterval); // Spawn coins at regular intervals
    }

    private void SpawnCoin()
    {
        // Calculate the spawn position based on the player's current position on the X-axis
        float playerX = playerTransform.position.x;
        Debug.Log("playerX" + playerX);

        float posX = Random.Range(minimiumConstraint, maximumConstraint);
        // Dynamic spawn offset
        //float spawnX = Mathf.Clamp(playerX + Random.Range(-spawnOffset, spawnOffset),playerTransform.position.x +minX, playerTransform.position.x+maxX); // Clamp X to the range
        Debug.Log("Spawn X" + posX);

        // Choose a random prefab from the array
        GameObject selectedPrefab = coinPrefabs[Random.Range(0, coinPrefabs.Length)];

        // Determine the spawn position
        Vector3 spawnPosition = new Vector3(posX, 1f, playerTransform.position.z + spawnDistance);

        for(int i = 0; i <= SpawnRatio; i++)
        {
            float incrementPosition = spawnPosition.z;
           

           Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);
            spawnPosition.z = incrementPosition + 2;

        }

        // Instantiate the selected prefab
    }

    
}
