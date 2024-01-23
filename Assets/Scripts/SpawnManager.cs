using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private int money = 150;
    public float spawnCooldown = 2f; // Time limit between spawns
    public float xPos;
    private bool canSpawn = true;
    public TextMeshProUGUI moneyText;
    [SerializeField]
    public string tag;
    public bool castle = false;

    private Queue<GameObject> soldiers = new Queue<GameObject> ();

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AddMoney());
        moneyText.text = money.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnObject(GameObject objectToSpawn, int cost)
    {
        if (canSpawn && money - cost >= 0 && soldiers.Count < 5)
        {
            StartCoroutine(SpawnCooldown());
            // Subtract amount of money according to cost of unit and display it
            money -= cost;
            moneyText.text = money.ToString();
            // Specify the position where you want to spawn the object
            Vector3 spawnPosition = new Vector3(xPos, -3.3f, 0f);
            // Instantiate a new GameObject based on the prefab at the specified position
            GameObject spawnedSoldier = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);

            soldiers.Enqueue(spawnedSoldier);
        }
    }

    public void setCastle(bool castle)
    {
        this.castle = castle;
    }

    public bool getCastle() { return this.castle; }

    // Get the first inserted soldier without removing it
    public GameObject GetSoldier()
    {
        bool findLeftmost;
        if (tag == "Left")
        {
            findLeftmost = false;
        }
        else
        {
            findLeftmost= true;
        }
        // Find all GameObjects with the specified tag
        GameObject[] soldiers = GameObject.FindGameObjectsWithTag(tag);

        // Check if there are any soldiers
        if (soldiers.Length > 0)
        {
            // Use LINQ to order the soldiers based on x-axis position
            var orderedSoldiers = findLeftmost
                ? soldiers.OrderBy(obj => obj.transform.position.x)
                : soldiers.OrderByDescending(obj => obj.transform.position.x);

            // Return the extreme soldier based on the provided flag
            return orderedSoldiers.First();
        }

        // Return null if no soldiers are found
        return null;
    }

    // Remove and return the first inserted soldier
    public void RemoveSoldier()
    {
        if (soldiers.Count > 0)
        {
            soldiers.Dequeue();
        }
        else
        {
            Debug.LogWarning("Queue is empty. Unable to remove soldier.");
        }
    }

    IEnumerator SpawnCooldown()
    {
        canSpawn = false;
        yield return new WaitForSeconds(spawnCooldown);
        canSpawn = true;
    }

    IEnumerator AddMoney()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            money += 20;
            moneyText.text = money.ToString();
        }
    }
}
