using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkedClient : MonoBehaviour
{

    int connectionID;
    int maxConnections = 1000;
    int reliableChannelID;
    int unreliableChannelID;
    int hostID;
    int socketPort = 5491;
    byte error;
    bool isConnected = false;
    int ourClientID;

    public string playerSide;

    GameObject gameSystemManager;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

        foreach (GameObject go in allObjects)
        {
            if(go.GetComponent<GameSystemManager>() != null)
            {
                gameSystemManager = go;
            }
        }

        Connect();
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.S))
        //    SendMessageToHost("Hello from client");

        ////Challenge 1- Prefixed messages:
        //if(Input.GetKeyDown(KeyCode.P))
            

        UpdateNetworkConnection();
    }

    private void UpdateNetworkConnection()
    {
        if (isConnected)
        {
            int recHostID;
            int recConnectionID;
            int recChannelID;
            byte[] recBuffer = new byte[1024];
            int bufferSize = 1024;
            int dataSize;
            //Called to poll underlying systems for events
            NetworkEventType recNetworkEvent = NetworkTransport.Receive(out recHostID, out recConnectionID, out recChannelID, recBuffer, bufferSize, out dataSize, out error);

            switch (recNetworkEvent)
            {
                //For Connecting
                case NetworkEventType.ConnectEvent:
                    Debug.Log("connected.  " + recConnectionID);
                    ourClientID = recConnectionID;
                    break;
                //For Data Event
                case NetworkEventType.DataEvent:
                    string msg = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
                    ProcessRecievedMsg(msg, recConnectionID);
                    //Debug.Log("got msg = " + msg);
                    break;
                //For Disconnecting
                case NetworkEventType.DisconnectEvent:
                    isConnected = false;
                    Debug.Log("disconnected.  " + recConnectionID);
                    break;
            }
        }
    }
    
    private void Connect()
    {

        if (!isConnected)
        {
            Debug.Log("Attempting to create connection");

            NetworkTransport.Init();

            ConnectionConfig config = new ConnectionConfig();
            reliableChannelID = config.AddChannel(QosType.Reliable);
            unreliableChannelID = config.AddChannel(QosType.Unreliable);
            HostTopology topology = new HostTopology(config, maxConnections);
            hostID = NetworkTransport.AddHost(topology, 0);
            Debug.Log("Socket open.  Host ID = " + hostID);
            //Put my Local IP so that it connects with the server.
            connectionID = NetworkTransport.Connect(hostID, "192.168.0.10", socketPort, 0, out error); // server is local on network

            if (error == 0)
            {
                isConnected = true;

                Debug.Log("Connected, id = " + connectionID);

            }
        }
    }
    
    public void Disconnect()
    {
        NetworkTransport.Disconnect(hostID, connectionID, out error);
    }
    
    //Used to send messages to the server
    public void SendMessageToHost(string msg)
    {
        byte[] buffer = Encoding.Unicode.GetBytes(msg);
        NetworkTransport.Send(hostID, connectionID, reliableChannelID, buffer, msg.Length * sizeof(char), out error);
    }

    private void ProcessRecievedMsg(string msg, int id)
    {
        Debug.Log("msg recieved = " + msg + ".  connection id = " + id);

        string[] csv = msg.Split(',');

        int signifier = int.Parse(csv[0]);

        if(signifier == ServerToClientSignifiers.AccountCreationComplete)
        {
            gameSystemManager.GetComponent<GameSystemManager>().ChangeState(GameStates.MainMenu);
        }
        else if(signifier == ServerToClientSignifiers.LoginComplete)
        {
            gameSystemManager.GetComponent<GameSystemManager>().ChangeState(GameStates.MainMenu);
        }
        else if (signifier == ServerToClientSignifiers.GameStart)
        {
            Debug.Log("Game Start");

            gameSystemManager.GetComponent<GameSystemManager>().ChangeState(GameStates.TicTacToeGame);
            
        }
        //else if (signifier == ServerToClientSignifiers.OpponentPlay)
        //{
        //    Debug.Log("Opponent Play");
        //    gameSystemManager.GetComponent<GameSystemManager>().canPlayGame = true;
        //}
        else if(signifier == ServerToClientSignifiers.ClientIsObserver)
        {
            Debug.Log("Player is now observer");
            gameSystemManager.GetComponent<GameSystemManager>().canPlayGame = false;
        }
        else if(signifier == ServerToClientSignifiers.PlayerX)
        {
            Debug.Log("Client is player X");
            playerSide = "X";
        }
        else if (signifier == ServerToClientSignifiers.PlayerO)
        {
            Debug.Log("Client is player O");
            playerSide = "O";
        }
        else if (signifier == ServerToClientSignifiers.OpponentPlay)
        {
            Debug.Log("Opponent Play called");
            Debug.Log(csv[1] + "" + "," + csv[2]);
            gameSystemManager.GetComponent<GameSystemManager>().UpdateSpace(int.Parse(csv[1]), csv[2]);
        }
        else if (signifier == ServerToClientSignifiers.GameOver)
        {
            gameSystemManager.GetComponent<GameSystemManager>().GameOver(csv[1]);
        }
    }

    public string GetPlayerSide()
    {
        return playerSide;

    }

    public bool IsConnected()
    {
        return isConnected;
    }


}



public static class ClientToServerSignifiers
{
    public const int CreateAccount = 1;
    public const int Login = 2;
    public const int JoinGameRoomQueue = 3;
    public const int TicTacToeSomethingPlay = 4;
    public const int ClientIsObserver = 5;
    public const int ClientIsPlayer = 6;
    public const int ClientWon = 7;
    public const int ClientLost = 8;
    public const int PlayerX = 9;
    public const int PlayerO = 10;
    public const int OpponentPlay = 11;
    public const int GameOver = 12;
}

public static class ServerToClientSignifiers
{
    public const int LoginComplete = 1;
    public const int LoginFailed = 2;
    public const int AccountCreationComplete = 3;
    public const int AccountCreationFailed = 4;
    public const int OpponentPlay = 5;
    public const int GameStart = 6;
    public const int ClientIsObserver = 7;
    public const int ClientIsPlayer = 8;
    public const int ClientWon = 9;
    public const int ClientLost = 10;
    public const int PlayerX = 11;
    public const int PlayerO = 12;
    public const int GameOver = 13;
}
