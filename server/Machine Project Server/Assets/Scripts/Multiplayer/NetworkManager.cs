using RiptideNetworking;
using RiptideNetworking.Utils;
using UnityEngine;

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

    public Server Server
    {
        get;
        private set;
    }

    [SerializeField] private ushort port;
    [SerializeField] private ushort maxClientCount;

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        Application.targetFrameRate = 60;

        //initializes Riptide logger to print to Unity's console
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogError, Debug.LogWarning, false);

        //assign value to server and starts it
        Server = new Server();
        Server.Start(port, maxClientCount);

        Server.ClientDisconnected += PlayerLeft;
    }

    private void FixedUpdate()
    {
        Server.Tick();
    }

    private void OnApplicationQuit()
    {
        Server.Stop();
    }

    private void PlayerLeft(object sender, ClientDisconnectedEventArgs e)
    {
        if (Player.list.TryGetValue(e.Id, out Player player))
            Destroy(player.gameObject);
    }
}
