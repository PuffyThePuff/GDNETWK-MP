using RiptideNetworking;
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

    public void UpdateScore(bool isPlayerOne)
    {
        if (isPlayerOne) playerOneScore += 1;
        else playerTwoScore += 1;

        SendScore();
    }

    // Message Sending function
    private void SendScore()
    {
        
    }
}