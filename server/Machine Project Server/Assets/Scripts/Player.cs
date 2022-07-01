using RiptideNetworking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Dictionary<ushort, Player> list = new Dictionary<ushort, Player>();
    public ushort ID{get; private set;}
    public string Username{get; private set;}
    public ushort Team{get; private set;}

    public PlayerMovement Movement => movement;
    [SerializeField] private PlayerMovement movement;

    private void OnDestroy()
    {
        list.Remove(ID);
    }

    //spawns player prefab and assigns player name, id, and username
    //also calls SendSpawned() and adds player to dictionary
    public static void Spawn(ushort id, string username)
    {
        //sends info of existing players to new clients
        foreach (Player otherPlayer in list.Values)
        {
            otherPlayer.SendSpawned(id);
        }

        Player player = Instantiate(GameLogic.Singleton.PlayerPrefab, new Vector3(0f, 1f, 0f), Quaternion.identity).GetComponent<Player>();
        player.name = $"Player {id} ({(string.IsNullOrEmpty(username) ? "Guest" : username)})";
        player.ID = id;
        player.Username = string.IsNullOrEmpty(username) ? $"Guest {id}" : username;

        // Team One or Team Two
        player.Team = (ushort)((list.Count % 2) + 1);

        player.SendSpawned();
        list.Add(id, player);

        if (list.Count == 2) TimerManager.Singleton.StartGame();
    }

    //creates reliable message with ID, username, and postion of newly connected player and sends to all connected players
    private void SendSpawned()
    {
        NetworkManager.Singleton.Server.SendToAll(AddSpawnData(Message.Create(MessageSendMode.reliable, ServerToClientID.playerSpawned)));
    }

    //sends info of existing players to newly connected clients
    private void SendSpawned(ushort toClientID)
    {
        NetworkManager.Singleton.Server.Send(AddSpawnData(Message.Create(MessageSendMode.reliable, ServerToClientID.playerSpawned)), toClientID);
    }

    //helper function for creating message data
    private Message AddSpawnData(Message message)
    {
        message.AddUShort(ID);
        message.AddString(Username);
        message.AddVector3(transform.position);

        return message;
    }

    //lets riptide know messages with this ID should be handled with this method (must be static, can be private)
    [MessageHandler((ushort)ClientToServerID.name)]
    private static void Name(ushort fromClientID, Message message)
    {
        Spawn(fromClientID, message.GetString());
    }

    [MessageHandler((ushort)ClientToServerID.input)]
    private static void Input(ushort fromClientID, Message message)
    {
        if (list.TryGetValue(fromClientID, out Player player))
            player.Movement.SetInput(message.GetBools(6), message.GetVector3());
    }
}
