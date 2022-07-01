using RiptideNetworking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    private static TimerManager singleton;

    public static TimerManager Singleton
    {
        get
        {
            if (singleton == null) singleton = FindObjectOfType<TimerManager>();
            return singleton;
        }
    }

    [SerializeField] private float gameTimer = 90.0f;
    private bool activeGame = false;

    private float shuttingDownTimer = 5.0f;
    private bool isShuttingDown = false;

    // Update is called once per frame
    void Update()
    {
        if (activeGame)
        {
            gameTimer -= Time.deltaTime;
            SendTime();

            if(gameTimer <= 0.0f)
            {
                gameTimer = 0.0f;
                StopGame();
                SendEndCue();
            }
        }

        if (isShuttingDown)
        {
            shuttingDownTimer -= Time.deltaTime;
            if (shuttingDownTimer <= 0.0f)
            {
                if (Application.isEditor) UnityEditor.EditorApplication.ExitPlaymode();
                Application.Quit();
            }
        }
    }

    public void StartGame()
    {
        activeGame = true;
    }

    public void StopGame()
    {
        activeGame = false;
        isShuttingDown = true;
    }

    private void SendTime()
    {
        Message message = Message.Create(MessageSendMode.unreliable, ServerToClientID.timerTicked);
        message.AddFloat(gameTimer);

        NetworkManager.Singleton.Server.SendToAll(message);
    }

    private void SendEndCue()
    {
        Message message = Message.Create(MessageSendMode.reliable, ServerToClientID.gameEnded);

        NetworkManager.Singleton.Server.SendToAll(message);
    }
}