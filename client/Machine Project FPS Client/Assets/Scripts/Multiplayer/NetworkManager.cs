using RiptideNetworking;
using RiptideNetworking.Utils;
using UnityEngine;
using System;

public enum ServerToClientID : ushort
{
    playerSpawned = 1,
    playerMovement,
    ballMovement,
    goalScored
}

public enum ClientToServerID : ushort
{
    name = 1,
    input,
}

public class NetworkManager : MonoBehaviour
{
    //Miguel's really cool singleton code he made in 2020 and probably still works
    //put Singleton = this in Awake()
    private static NetworkManager _singleton;

    //getter and setter for singleton
    public static NetworkManager Singleton
    {
        get => _singleton;

        private set
        {
            if (_singleton == null)
            {
                _singleton = value;
            }
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(NetworkManager)} instance already exists, destroying duplicate");
                Destroy(value);
            }
        }
    }

    public Client Client
    {
        get;
        private set;
    }

    [SerializeField] private string ip;
    [SerializeField] private ushort port;

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        //initializes Riptide logger to print to Unity's console
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogError, Debug.LogWarning, false);

        Client = new Client();

        Client.Connected += DidConnect;
        Client.ConnectionFailed += FailedToConnect;
        Client.ClientDisconnected += PlayerLeft;
        Client.Disconnected += DidDisconnect;
    }

    private void FixedUpdate()
    {
        Client.Tick();
    }

    private void OnApplicationQuit()
    {
        Client.Disconnect();
    }

    //attempts connection
    public void Connect()
    {
        Client.Connect($"{ip}:{port}");
    }

    private void DidConnect(object sender, EventArgs e)
    {
        UIManager.Singleton.SendName();
        foreach (Player player in Player.list.Values)
            Destroy(player.gameObject);
    }

    private void FailedToConnect(object sender, EventArgs e)
    {
        UIManager.Singleton.BackToMain();
    }

    private void PlayerLeft(object sender, ClientDisconnectedEventArgs e)
    {
        if (Player.list.TryGetValue(e.Id, out Player player))
            Destroy(player.gameObject);

        UIManager.Singleton.ResetUI();
    }
    
    private void DidDisconnect(object sender, EventArgs e)
    {
        UIManager.Singleton.BackToMain();
    }
}
