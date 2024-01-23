using UnityEngine;

public class Ballista : MonoBehaviour
{
    public GameObject spearPrefab; // Reference to the Spear prefab
    public Transform launchPoint; // Reference to the LaunchPoint transform
    public float spawnInterval = 2f; // Interval between spawns in seconds
    [SerializeField]
    private SpawnManager spawnManager = null;
    private bool canShoot = false;
    public int faction = 0;
    private Quaternion initialRotation;
    private float timer = 0f;


    void Start()
    {
        // Store the initial rotation of the ballista
        initialRotation = transform.rotation;
    }

    void Update()
    {
        PointBallista();

        // Increment the timer
        timer += Time.deltaTime;

        // Check if it's time to spawn a spear
        if (timer >= spawnInterval)
        {
            // Reset the timer
            timer = 0f;

            // Spawn a spear at the launch point's position
            if (canShoot)
            {
                SpawnSpear();
            }
        }
    }

    void SpawnSpear()
    {
        // Check if the spear prefab and launch point are assigned
        if (spearPrefab != null && launchPoint != null)
        {
            // Instantiate the spear at the launch point's position and rotation
            GameObject spearInstance = Instantiate(spearPrefab, launchPoint.position, launchPoint.rotation);

            // Optionally, you can add further customization to the spawned spear here
        }
        else
        {
            Debug.LogWarning("Spear prefab or launch point is not assigned!");
        }
    }

    void PointBallista()
    {
        GameObject soldier = spawnManager.GetSoldier();

        if (soldier != null)
        {
            // Get the position of the soldier
            Vector3 lookAtPosition = soldier.transform.position;

            // Ensure the ballista only rotates in the Z-axis
            lookAtPosition.z = transform.position.z;

            // Make the ballista look at the soldier
            transform.right = lookAtPosition - transform.position;

            if (faction == 1 && lookAtPosition.x < -10)
            {
                canShoot = true;
            } 
            else if (faction == 2 && lookAtPosition.x > 10)
            {
                canShoot = true;
            }
            else
            {
                canShoot = false;
            }
        }
        else
        {
            canShoot = false;
            // Return the ballista to its initial rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, initialRotation, 2f * Time.deltaTime);
        }
    }
}
