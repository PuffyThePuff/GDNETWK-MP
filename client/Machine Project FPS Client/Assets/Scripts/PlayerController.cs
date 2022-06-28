using RiptideNetworking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//uses server-side logic to handle player movement
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform camTransform;

    private bool[] inputs;

    private void Start()
    {
        inputs = new bool[6];
    }

    //for testing
    private void Update()
    {
        if (Input.GetKey(KeyCode.W))
            inputs[0] = true;

        if (Input.GetKey(KeyCode.S))
            inputs[1] = true;

        if (Input.GetKey(KeyCode.A))
            inputs[2] = true;

        if (Input.GetKey(KeyCode.D))
            inputs[3] = true;

        if (Input.GetKey(KeyCode.Space))
            inputs[4] = true;

        if (Input.GetKey(KeyCode.LeftShift))
            inputs[5] = true;
    }

    private void FixedUpdate()
    {
        SendInput();

        for (int i = 0; i < inputs.Length; i++)
        {
            inputs[i] = false;
        }
    }

    //sends user input to server
    //uses unreliable sending
    private void SendInput()
    {
        Message message = Message.Create(MessageSendMode.unreliable, ClientToServerID.input);
        message.AddBools(inputs, false);
        message.AddVector3(camTransform.forward);
        
        NetworkManager.Singleton.Client.Send(message);
    }
}
