using UnityEngine;

public class SpearMovement : MonoBehaviour
{
    [SerializeField]
    private float spearSpeed = 20f; // Speed of the spear
    [SerializeField]
    private int damage = 20; // spear damage

    void Start()
    {
        // Get the Rigidbody2D component of the spear
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        // Check if Rigidbody2D is present
        if (rb != null)
        {
            // Apply a force to the spear in the forward direction
            rb.AddForce(transform.right * spearSpeed, ForceMode2D.Impulse);
        }
        else
        {
            Debug.LogWarning("Rigidbody2D component not found on the spear!");
        }

        Destroy(gameObject, 2f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Right" || other.tag == "Left")
        {
            Lives damageReceiver = other.GetComponent<Lives>();
            damageReceiver.GetDamage(damage);
            Destroy(gameObject);
        }
        else if (other.tag == "Castle")
        {
            Destroy(gameObject);
        }
    }
}
