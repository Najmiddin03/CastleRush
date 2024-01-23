using UnityEngine;

public class LaunchPointSpawner : MonoBehaviour
{
    public GameObject spearPrefab; // Reference to the Spear prefab
    public Transform launchPoint; // Reference to the LaunchPoint transform
    public float spawnInterval = 2f; // Interval between spawns in seconds
    [SerializeField]
    private string tag;

    private float timer = 0f;

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
            SpawnSpear();
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
        GameObject[] soldiers = GameObject.FindGameObjectsWithTag(tag);

        if (soldiers.Length > 0)
        {
            GameObject leftmostSoldier = soldiers[0];

            foreach (GameObject soldier in soldiers)
            {
                if (soldier.transform.position.x < leftmostSoldier.transform.position.x)
                {
                    leftmostSoldier = soldier;
                }
            }

            // Get the position of the soldier
            Vector3 lookAtPosition = leftmostSoldier.transform.position;

            // Ensure the ballista only rotates in the Z-axis
            lookAtPosition.z = transform.position.z;

            // Make the ballista look at the soldier
            transform.right = lookAtPosition - transform.position;

            Debug.Log("Ballista is now pointing towards the leftmost soldier.");
        }
        else
        {
            Debug.LogWarning("No soldiers found in the scene.");
        }
    }
}
