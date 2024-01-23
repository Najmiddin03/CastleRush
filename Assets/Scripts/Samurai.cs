using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Samurai : MonoBehaviour
{
    [SerializeField]
    private int faction = 0;
    [SerializeField]
    public float attackSpeed = 1f;
    [SerializeField]
    private float moveSpeed = 5f; // Adjust the speed as needed
    public bool canMove = true; // Flag to determine if the object can move
    private Animator animator; // Reference to the Animator component
    private bool dead = false;
    public bool canAttack = true;
    private Lives damageReceiver;
    private Lives castle;
    [SerializeField]
    private int damage = 0;
    private SpawnManager spawnManager;
    private SpawnManager enemy;
    

    // Start is called before the first frame update
    void Start()
    {
        if (faction == 2)
        {
            RotateObjectBy180Degrees();
            spawnManager = GameObject.Find("RightSpawnManager").GetComponent<SpawnManager>();
            enemy = GameObject.Find("LeftSpawnManager").GetComponent<SpawnManager>();
            castle = GameObject.Find("Castle").GetComponent<Lives>();
            if (castle == null)
            {
                Debug.LogWarning("No castle");
            }
        }
        else
        {
            spawnManager = GameObject.Find("LeftSpawnManager").GetComponent<SpawnManager>();
            enemy = GameObject.Find("RightSpawnManager").GetComponent<SpawnManager>();
            castle = GameObject.Find("Castle2").GetComponent<Lives>();
            if (castle == null)
            {
                Debug.LogWarning("No castle");
            }
        }

        // Get the Animator component attached to the GameObject
        animator = GetComponent<Animator>();
    }

    void RotateObjectBy180Degrees()
    {
        // Get the current rotation
        Vector3 currentRotation = transform.rotation.eulerAngles;

        // Set the new rotation by adding 180 degrees to the Y-axis
        Vector3 newRotation = new Vector3(currentRotation.x, currentRotation.y + 180f, currentRotation.z);

        // Apply the new rotation to the object
        transform.rotation = Quaternion.Euler(newRotation);
    }

    // Update is called once per frame
    void Update()
    {
        // You can call the Move function from other parts of your code or events
        if (canMove)
        {
            Move();
        }
        else if(canAttack)
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        canAttack = false;
        while (!canMove)
        {
            animator.SetTrigger("Attack");
            yield return new WaitForSeconds(attackSpeed);
            if (spawnManager.getCastle())
            {
                castle.GetDamage(damage);
            }
            else
            {
                damageReceiver = enemy.GetSoldier().GetComponent<Lives>();
                if (damageReceiver != null)
                {
                    // Call the GetDamage function on the collided object
                    damageReceiver.GetDamage(damage); // You can pass the desired damage value
                }
            }
        }
        canAttack = true;
    }

    // Called when another collider enters the trigger collider
    void OnCollisionStay2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;
        if (other.tag == "Castle")
        {
            // Set canMove to false when a collider enters the trigger
            canMove = false;
            spawnManager.setCastle(true);
            // Set the "Enemy" parameter to true
            animator.SetBool("Enemy", true);
        }
        else if(other.tag == "Left" || other.tag == "Right")
        {
            //soldier

            // Check if the colliding object is on the left side
            bool isOnLeftSide;
            if (faction == 1)
            {
                isOnLeftSide = other.transform.position.x < transform.position.x;
            }
            else
            {
                isOnLeftSide = other.transform.position.x > transform.position.x;
            }

            if (!isOnLeftSide)
            {
                // Set canMove to false when a collider enters the trigger
                canMove = false;
                // Set the "Enemy" parameter to true
                animator.SetBool("Enemy", true);
            }
        }
    }

    // Called when another collider exits the trigger collider
    void OnCollisionExit2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;
        if(other.tag != "Spear")
        {
            StartCoroutine(TriggerExit(0.5f));
        }
    }

    IEnumerator TriggerExit(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (!dead)
        {
            damageReceiver = null;
            // Set the "Enemy" parameter to false
            animator.SetBool("Enemy", false);
            // Set canMove to true when a collider exits the trigger
            canMove = true;
        }
    }

    // Function to move the game object towards the right in the X-axis
    void Move()
    {
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
    }

    public void Death()
    {
        spawnManager.setCastle(false);
        dead = true;
        animator.SetTrigger("Dead");
        StartCoroutine(destroy(0.5f));
    }

    IEnumerator destroy(float delay)
    {
        yield return new WaitForSeconds(delay);
        spawnManager.RemoveSoldier();
        Destroy(gameObject);
    }

    public int getFaction()
    {
        return faction;
    }
}