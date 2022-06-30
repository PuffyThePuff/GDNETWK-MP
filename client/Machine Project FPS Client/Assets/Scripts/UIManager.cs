using RiptideNetworking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //Miguel's really cool singleton code he made in 2020 and probably still works
    //put Singleton = this in Awake()
    private static UIManager _singleton;

    //getter and setter for singleton
    public static UIManager Singleton
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
                Debug.Log($"{nameof(UIManager)} instance already exists, destroying duplicate");
                Destroy(value);
            }
        }
    }

    [Header("Connect")]
    [SerializeField] private GameObject connectUI;
    [SerializeField] private InputField usernameField;

    [Header("HUD")]
    [SerializeField] private GameObject headsUpDisplay;
    [SerializeField] private GameObject goalText;
    [SerializeField] private Text playerOneScoreText;
    [SerializeField] private Text playerTwoScoreText;
    [SerializeField] private Text playerOneNameText;
    [SerializeField] private Text playerTwoNameText;

    private int playerOneScore = 0;
    private int playerTwoScore = 0;

    private float textTimer = 2.0f;
    private bool goalTextIsActive = false;

    private void Awake()
    {
        Singleton = this;
    }

    public void ConnectClicked()
    {
        //hides the UI after clicking the connect button
        usernameField.interactable = false;
        connectUI.SetActive(false);
        headsUpDisplay.SetActive(true);

        NetworkManager.Singleton.Connect();
    }

    public void BackToMain()
    {
        //shows the UI after connection error
        headsUpDisplay.SetActive(false);
        usernameField.interactable = true;
        connectUI.SetActive(true);
    }

    //creates new message containing chosen username and uses reliable data transfer to send to server
    public void SendName()
    {
        Message message = Message.Create(MessageSendMode.reliable, ClientToServerID.name);
        message.AddString(usernameField.text);
        NetworkManager.Singleton.Client.Send(message);
    }

    public void DisplayPlayerNames()
    {
        foreach(Player player in Player.list.Values)
        {
            if (player.Team == 1) playerOneNameText.text = player.GetUsername();
            else playerTwoNameText.text = player.GetUsername();
        }
    }

    private void ActivateGoalText()
    {
        goalText.SetActive(true);
        goalTextIsActive = true;
    }

    private void Update()
    {
        if (goalTextIsActive)
        {
            textTimer -= Time.deltaTime;
            if (textTimer <= 0.0f)
            {
                textTimer = 2.0f;
                goalTextIsActive = false;
                goalText.SetActive(false);
            }
        }
    }

    [MessageHandler((ushort)ServerToClientID.goalScored)]
    private static void ChangeScore(Message message)
    {
        if (message.GetBool())
        {
            Singleton.playerOneScore++;
            Singleton.playerOneScoreText.text = Singleton.playerOneScore.ToString();
        }
        else
        {
            Singleton.playerTwoScore++;
            Singleton.playerTwoScoreText.text = Singleton.playerTwoScore.ToString();
        }

        Singleton.ActivateGoalText();
    }
}