using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Winner : MonoBehaviour
{
    public static int winner = 0;
    
    public static void SetWinner(int w)
    {
        winner = w;
    }
}
