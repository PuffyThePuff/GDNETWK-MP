using RiptideNetworking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallMovement : MonoBehaviour
{
    private void FixedUpdate()
    {
        SendMovement();
    }

    private void SendMovement()
    {
        Message message = Message.Create(MessageSendMode.unreliable, ServerToClientID.ballMovement);
        message.AddVector3(transform.position);
        
        NetworkManager.Singleton.Server.SendToAll(message);
    }
}
