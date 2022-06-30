using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager singleton;

    public static ScoreManager Singleton
    {
        get
        {
            if (singleton == null) singleton = FindObjectOfType<ScoreManager>();
            return singleton;
        }
    }

    public int playerOneScore = 0;
    public int playerTwoScore = 0;
}