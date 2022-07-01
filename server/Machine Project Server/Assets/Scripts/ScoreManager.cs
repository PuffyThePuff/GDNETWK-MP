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

    public void UpdateScore(bool isPlayerOne)
    {
        if(TimerManager.Singleton.activeGame) SendScore(isPlayerOne);
    }

    private void SendScore(bool isPlayerOne)
    {
        Message message = Message.Create(MessageSendMode.reliable, ServerToClientID.goalScored);
        message.AddBool(isPlayerOne);

        NetworkManager.Singleton.Server.SendToAll(message);
    }
}