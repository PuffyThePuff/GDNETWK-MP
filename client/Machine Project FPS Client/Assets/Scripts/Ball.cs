using RiptideNetworking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public static Ball ball;

    private void Start()
    {
        ball = this;
    }

    private void Move(Vector3 newPosition)
    {
        ball.transform.position = newPosition;
    }

    [MessageHandler((ushort)ServerToClientID.ballMovement)]
    private static void BallMovement(Message message)
    {
        ball.Move(message.GetVector3());
    }
}
