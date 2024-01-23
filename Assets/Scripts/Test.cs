using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public int side = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (Winner.winner == side)
        {
            // If not equal, disable the game object
            gameObject.SetActive(false);
        }
    }
}
