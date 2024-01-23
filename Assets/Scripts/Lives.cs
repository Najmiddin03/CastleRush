using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lives : MonoBehaviour
{
    [SerializeField]
    private int lives = 100;
    [SerializeField]
    public Samurai samurai;
    [SerializeField]
    private GameObject explosion;
    public Healthbar healthbar;
    int maxHealth;
    [SerializeField]
    private GameObject blood;
    public Castle castle;


    void Start()
    {
        maxHealth = lives;
        healthbar = GetComponentInChildren<Healthbar>();
        healthbar.SetHealth(lives, maxHealth);
    }

    public void GetDamage(int damage)
    {
        lives -= damage;
        healthbar.SetHealth(lives, maxHealth);

        if (gameObject.tag != "Castle")
        {
            Vector3 newPosition = new Vector3(transform.position.x + 0.5f, transform.position.y + 1.5f, transform.position.z + 1f);
            Instantiate(blood, newPosition, Quaternion.identity);
        }
        if (lives <= 0)
        {
            DisableCollider();
        }
    }

    // Function to disable the BoxCollider2D
    private void DisableCollider()
    {
        // Get the BoxCollider2D component and disable it
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider != null)
        {
            if (gameObject.tag != "Castle")
            {
                samurai.Death();
            }
            else
            {
                Instantiate(explosion, gameObject.transform.position, gameObject.transform.rotation);
                SetWinner(castle.getFaction());
                StartCoroutine(LoadScene());
            }
            boxCollider.enabled = false;
        }
    }

    static void SetWinner(int w)
    {
        Winner.winner = w;
    }

    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
