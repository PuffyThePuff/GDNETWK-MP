using RiptideNetworking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Dictionary<ushort, Player> list = new Dictionary<ushort, Player>();

    public ushort ID{get; private set;}
    public bool IsLocal{get; private set;}
    public ushort Team{get; private set;}

    [SerializeField] private PlayerAnimationManager animationManager;
    [SerializeField] private Transform camTransform;

    private string username;

    private void OnDestroy()
    {
        list.Remove(ID);  
    }

    private void Move(Vector3 newPosition, Vector3 forward)
    {
        transform.position = newPosition;

        if (!IsLocal)
        {
            camTransform.forward = forward;
            animationManager.AnimateBasedOnSpeed();
        }
    }

    //spawns players and adds them to dictionary
    public static void Spawn(ushort id, string username, Vector3 position)
    {
        Player player;

        //checks player id if it matches the client's instance
        if (id == NetworkManager.Singleton.Client.Id)
        {
            player = Instantiate(GameLogic.Singleton.LocalPlayerPrefab, position, Quaternion.identity).GetComponent<Player>();
            player.IsLocal = true;
        }
        else
        {
            player = Instantiate(GameLogic.Singleton.PlayerPrefab, position, Quaternion.identity).GetComponent<Player>();
            player.IsLocal = false;
        }

        player.name = $"Player {id} ({(string.IsNullOrEmpty(username) ? "Guest" : username)})";
        player.ID = id;
        player.username = username;

        //Team One or Team Two
        player.Team = (ushort)((list.Count % 2) + 1);

        list.Add(id, player);
    }

    //lets riptide know messages with this ID should be handled with this method (must be static, can be private)
    [MessageHandler((ushort)ServerToClientID.playerSpawned)]
    private static void SpawnPlayer(Message message)
    {
        Spawn(message.GetUShort(), message.GetString(), message.GetVector3());
        UIManager.Singleton.DisplayPlayerNames();
    }

    [MessageHandler((ushort)ServerToClientID.playerMovement)]
    private static void PlayerMovement(Message message)
    {
        if (list.TryGetValue(message.GetUShort(), out Player player))
            player.Move(message.GetVector3(), message.GetVector3());
    }

    public string GetUsername()
    {
        return username;
    }
}
