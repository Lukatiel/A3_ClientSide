using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameSystemManager : MonoBehaviour
{

    GameObject submitButton, userNameInput, passwordInput, createToggle, loginToggle;
    GameObject joinAsPlayerButton, joinAsObserverButton, ticTacToeSquareButton;
    GameObject textNameInfo, textPasswordInfo;
    GameObject chatroomBackgroundImage, sendChatButton;
    GameObject networkedClient;

    //Chat Variables
    public TMP_InputField chatInputField;
    public TextMeshProUGUI chatLogText;
    [SerializeField]
    private List<string> messages = new List<string>();
    private int maxMessages = 18;
    private float messageDelay = 4f;
    //--

    //TicTacToe Variables
    public GameObject boardUI;
    public Text[] gridSpaceButtonList;
    public bool canPlayGame;
    public string playerSide;
    public GameObject gameOverPanel;
    public Text gameOverText;
    private int moveCount;
    public Player playerX, playerO;
    public PlayerColour activePlayerColour, inactivePlayerColour;
    public GameObject[] gridSpaceButtonArray;
    public Text[] gridSpaceText;
    GameObject gridPanel_x_1, gridPanel_x_2, gridPanel_y_1, gridPanel_y_2;
    GameObject playerX_board, playerO_board;
    GameObject backGroundPanel, boardPanel;
    //--

    //Replay System Variables
    //private List<ReplayManager> ActionReplayManager = new List<ReplayManager>();
    //private bool isInReplayMode;
    //--
    void Start()
    {
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

        foreach(GameObject go in allObjects)
        {
            /// Login Game Objects
            if (go.name == "UsernameInputField")
                userNameInput = go;
            else if (go.name == "PasswordInputField")
                passwordInput = go;
            else if (go.name == "SubmitButton")
                submitButton = go;
            else if (go.name == "CreateToggle")
                createToggle = go;
            else if (go.name == "LoginToggle")
                loginToggle = go;
            else if (go.name == "UsernameText")
                textNameInfo = go;
            else if (go.name == "PasswordText")
                textPasswordInfo = go;

            /// Main Menu Game Objects
            else if (go.name == "JoinAsPlayerButton")
                joinAsPlayerButton = go;
            else if (go.name == "JoinAsObserverButton")
                joinAsObserverButton = go;
            else if (go.name == "TicTacToeSquareButton")
                ticTacToeSquareButton = go;
            else if (go.name == "ChatroomBackgroundImage")
                chatroomBackgroundImage = go;
            else if (go.name == "SendChatButton")
                sendChatButton = go;

            ///Board Objects
            else if (go.name == "BackgroundBoardPanel")
                backGroundPanel = go;
            else if (go.name == "BoardPanel")
                boardPanel = go;
            else if (go.name == "GridPanel_x_1")
                gridPanel_x_1 = go;
            else if (go.name == "GridPanel_x_2")
                gridPanel_x_2 = go;
            else if (go.name == "GridPanel_y_1")
                gridPanel_y_1 = go;
            else if (go.name == "GridPanel_y_2")
                gridPanel_y_2 = go;
            else if (go.name == "PlayerX")
                playerX_board = go;
            else if (go.name == "PlayerO")
                playerO_board = go;

            else if (go.name == "BoardUI")
                boardUI = go;


            ///Networked Client
            else if (go.name == "NetworkedClient")
                networkedClient = go;
            
        }

        foreach (GameObject go in gridSpaceButtonArray)
        {

            if (go.name == "GridSpaceButton_0")
                gridSpaceButtonArray[0] = go;
            else if (go.name == "GridSpaceButton_1")
                gridSpaceButtonArray[1] = go;
            else if (go.name == "GridSpaceButton_2")
                gridSpaceButtonArray[2] = go;
            else if (go.name == "GridSpaceButton_3")
                gridSpaceButtonArray[3] = go;
            else if (go.name == "GridSpaceButton_4")
                gridSpaceButtonArray[4] = go;
            else if (go.name == "GridSpaceButton_5")
                gridSpaceButtonArray[5] = go;
            else if (go.name == "GridSpaceButton_6")
                gridSpaceButtonArray[6] = go;
            else if (go.name == "GridSpaceButton_7")
                gridSpaceButtonArray[7] = go;
            else if (go.name == "GridSpaceButton_8")
                gridSpaceButtonArray[8] = go;
        }

        submitButton.GetComponent<Button>().onClick.AddListener(SubmitButtonPressed);
        loginToggle.GetComponent<Toggle>().onValueChanged.AddListener(LoginToggleChanged);
        createToggle.GetComponent<Toggle>().onValueChanged.AddListener(CreateToggleChanged);

        joinAsPlayerButton.GetComponent<Button>().onClick.AddListener(JoinAsPlayerButtonPressed);
        joinAsObserverButton.GetComponent<Button>().onClick.AddListener(JoinAsObserverButtonPressed);
        ticTacToeSquareButton.GetComponent<Button>().onClick.AddListener(TicTacToeSquareButtonPressed);
        sendChatButton.GetComponent<Button>().onClick.AddListener(SendChatButtonPressed);

        //for (int i = 0; i < gridSpaceButtonArray.Length; i++)
        //{
        //    //int _i = ++i;
        //    gridSpaceButtonArray[i].GetComponent<Button>().onClick.AddListener(delegate { SetSpace(i); });
        //}


        ChangeState(GameStates.LoginMenu);
        Debug.Log("Change State to login");
    }

    void Update()
    {

        chatLogText.maxVisibleLines = maxMessages;
        if (messages.Count > maxMessages)
        {
            messages.RemoveAt(0);
        }
        if (messageDelay < Time.time)
        {
            BuildChatContents();
            messageDelay = Time.time + 0.25f;
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SubmitChat();
        }

        //if(Input.GetKeyDown(KeyCode.R))
        //{
        //    if(isInReplayMode)
        //    {
        //        SetTransform(0);
        //        foreach(GameObject go in gridSpaceButtonArray)
        //        {
        //            go.GetComponent<Button>().interactable = true;
        //        }
        //    }
        //    else
        //    {
        //        SetTransform(ActionReplayManager.Count - 1);

        //        foreach (GameObject go in gridSpaceButtonArray)
        //        {
        //            go.GetComponent<Button>().interactable = false;
        //        }
        //    }
        //}
        /*
         if the replay button is pressed - set replayMode to true
            if - in replay mode
            SetTransform(0)
            
            else 
            SetTransform(ActionReplayManager.Count - 1)
        
         */

    }

    //private void FixedUpdate()
    //{
    //    if (isInReplayMode == false)
    //    {
    //        ActionReplayManager.Add(new ReplayManager { position = transform.position });
    //    }
        
    //}

    /// Button Pressed Functions
    public void SubmitButtonPressed()
    {
        string p = passwordInput.GetComponent<InputField>().text;
        string u = userNameInput.GetComponent<InputField>().text;

        string msg;
        
        if(createToggle.GetComponent<Toggle>().isOn)
            msg = ClientToServerSignifiers.CreateAccount + "," + u + "," + p;
        else
            msg = ClientToServerSignifiers.Login + "," + u + "," + p;

        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(msg);

        Debug.Log(msg);
    }
    public void JoinAsPlayerButtonPressed()
    {
        Debug.Log("Client is a player");

        string msg;

        msg = ClientToServerSignifiers.ClientIsPlayer + "";

        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(msg);
        Debug.Log(msg);

        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(ClientToServerSignifiers.JoinGameRoomQueue + "");
        ChangeState(GameStates.WaitingInQueue);
    }
    public void JoinAsObserverButtonPressed()
    {
        Debug.Log("Client is observer");

        string msg;

        msg = ClientToServerSignifiers.ClientIsObserver + "";

        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(msg);
        Debug.Log(msg);

        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(ClientToServerSignifiers.JoinGameRoomQueue + "");
        ChangeState(GameStates.WaitingInQueue);
    }
    public void TicTacToeSquareButtonPressed()
    {
        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(ClientToServerSignifiers.TicTacToeSomethingPlay + "");

    }
    public void SendChatButtonPressed()
    {
        SubmitChat();
    }

    public void LoginToggleChanged(bool newValue)
    {
        createToggle.GetComponent<Toggle>().SetIsOnWithoutNotify(!newValue);
    }
    public void CreateToggleChanged(bool newValue)
    {
        loginToggle.GetComponent<Toggle>().SetIsOnWithoutNotify(!newValue);
    }

    ///Chat Functions
    void AddNewMessage(string msg)
    {
        messages.Add(msg);
    }

    public void SendChat(string msg)
    {
        string newMessage = userNameInput.GetComponent<InputField>().text + ": " + msg;
        Debug.Log(msg);
        AddNewMessage(newMessage);
    }

    public void SubmitChat()
    {
        string blankChat = chatInputField.text;
        if (blankChat == "")
        {
            chatInputField.ActivateInputField();
            chatLogText.text = "";
            return;
        }

        
        SendChat(chatInputField.text);
       
        chatInputField.ActivateInputField();
        chatInputField.text = "";
    }

    void BuildChatContents()
    {
        string NewContents = "";
        foreach(string s in messages)
        {
            NewContents += s + "\n";
        }
        chatLogText.text = NewContents;
    }
    //--

    ///TicTacToe Functions

    public void SetSpace(int spot)
    {
        Debug.Log("Player Side: " + playerSide);
        Debug.Log("Before:" + gridSpaceButtonArray[spot].GetComponentInChildren<Text>().text);
        gridSpaceButtonArray[spot].GetComponentInChildren<Text>().text = playerSide;
        Debug.Log("After:" + gridSpaceButtonArray[spot].GetComponentInChildren<Text>().text);
        gridSpaceButtonArray[spot].GetComponent<Button>().interactable = false;
        EndTurn();
    }


    //public string GetPlayerSide()
    //{
    //    if(networkedClient.GetComponent<NetworkedClient>().CheckPlayerSide() == true)
    //    {
    //        return playerSide = "X";
    //    }
    //    else if(networkedClient.GetComponent<NetworkedClient>().CheckPlayerSide() == false)
    //    {
    //        return playerSide = "O";
    //    }
    //    else
    //    {
    //        return null;
    //    }
        
    //}

    public void EndTurn()
    {
        //Debug.Log("end turn function");
        moveCount++;
        //Just doing this through brute force as its a small game
        //Check if the First Row is a win
        if(gridSpaceButtonArray[0].GetComponentInChildren<Text>().text == playerSide && gridSpaceButtonArray[1].GetComponentInChildren<Text>().text == playerSide 
                                                     && gridSpaceButtonArray[2].GetComponentInChildren<Text>().text == playerSide)
        {
            GameOver(playerSide);
        }
        //Second Row Win Check
        if (gridSpaceButtonArray[3].GetComponentInChildren<Text>().text == playerSide && gridSpaceButtonArray[4].GetComponentInChildren<Text>().text == playerSide
                                                     && gridSpaceButtonArray[5].GetComponentInChildren<Text>().text == playerSide)
        {
            GameOver(playerSide);
        }
        //Third Row Win Check
        if (gridSpaceButtonArray[6].GetComponentInChildren<Text>().text == playerSide && gridSpaceButtonArray[7].GetComponentInChildren<Text>().text == playerSide
                                                     && gridSpaceButtonArray[8].GetComponentInChildren<Text>().text == playerSide)
        {
            GameOver(playerSide);
        }
        //First Column
        if (gridSpaceButtonArray[0].GetComponentInChildren<Text>().text == playerSide && gridSpaceButtonArray[3].GetComponentInChildren<Text>().text == playerSide
                                                     && gridSpaceButtonArray[6].GetComponentInChildren<Text>().text == playerSide)
        {
            GameOver(playerSide);
        }
        //Second Column
        if (gridSpaceButtonArray[1].GetComponentInChildren<Text>().text == playerSide && gridSpaceButtonArray[4].GetComponentInChildren<Text>().text == playerSide
                                                     && gridSpaceButtonArray[7].GetComponentInChildren<Text>().text == playerSide)
        {
            GameOver(playerSide);
        }
        //Third Column
        if (gridSpaceButtonArray[2].GetComponentInChildren<Text>().text == playerSide && gridSpaceButtonArray[5].GetComponentInChildren<Text>().text == playerSide
                                                     && gridSpaceButtonArray[8].GetComponentInChildren<Text>().text == playerSide)
        {
            GameOver(playerSide);
        }
        //Left to right Diag
        if (gridSpaceButtonArray[2].GetComponentInChildren<Text>().text == playerSide && gridSpaceButtonArray[4].GetComponentInChildren<Text>().text == playerSide
                                                     && gridSpaceButtonArray[6].GetComponentInChildren<Text>().text == playerSide)
        {
            GameOver(playerSide);
        }
        //Right to left diag
        if (gridSpaceButtonArray[0].GetComponentInChildren<Text>().text == playerSide && gridSpaceButtonArray[4].GetComponentInChildren<Text>().text == playerSide
                                                     && gridSpaceButtonArray[8].GetComponentInChildren<Text>().text == playerSide)
        {
            //networkedClient.GetComponent<NetworkedClient>().SendMessageToHost();
            GameOver(playerSide);
        }

        if(moveCount>= 9)
        {
            GameOver("draw");
        }

        ChangeSides();
    }

    void GameOver(string WinningPlayer)
    {
        for(int i = 0; i < gridSpaceButtonList.Length; i++)
        {
            SetBoardInteractable(false);
        }
        
        gameOverPanel.SetActive(true);
        //Change with signifiers
        if(WinningPlayer == "draw")
        {
            SetGameOverText("Its A Draw!");
            SetPlayerColorsInactive();
        }
        else
        {
            SetGameOverText(WinningPlayer + " Wins");
        }
        Debug.Log("Game Over");
    }

    void ChangeSides()
    {
        //TO CHANGE TO THE SERVER ID OR SOME SHIT IDK
        if(playerSide == "X")
        {
            SetPlayerColour(playerX, playerO);
        }
        else if(playerSide == "O")
        {
            SetPlayerColour(playerO, playerX);
        }
    }

    void SetGameOverText(string msg)
    {

        gameOverPanel.SetActive(true);
        gameOverText.text = msg;
    }

    public void RestartGame()
    {
       // playerSide = "X";
        moveCount = 0;
        gameOverPanel.SetActive(false);
        SetPlayerColour(playerX, playerO);
        for (int i = 0; i < gridSpaceButtonList.Length; i++)
        {
            gridSpaceButtonList[i].text = "";
        }
    }

    void SetBoardInteractable(bool toggle)
    {
        for (int i = 0; i < gridSpaceButtonList.Length; i++)
        {
            gridSpaceButtonList[i].GetComponentInParent<Button>().interactable = toggle;
        }
    }

    void SetPlayerColour(Player newPlayer, Player oldPlayer)
    {
        newPlayer.panel.color = activePlayerColour.panelColour;
        newPlayer.text.color = activePlayerColour.textColour;
        oldPlayer.panel.color = inactivePlayerColour.panelColour;
        oldPlayer.text.color = inactivePlayerColour.textColour;
    }
    void StartGame()
    {
        SetBoardInteractable(true);
    }
    void SetPlayerColorsInactive() 
    { 
        playerX.panel.color = inactivePlayerColour.panelColour; 
        playerX.text.color = inactivePlayerColour.textColour; 
        playerO.panel.color = inactivePlayerColour.panelColour; 
        playerO.text.color = inactivePlayerColour.textColour; 
    }
    
    bool CanPlayGame(bool playGame)
    {
        if(playGame)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    ///Replay System Functions
    //private void SetTransform(int index)
    //{
    //    ReplayManager replayManager = ActionReplayManager[index];

    //    transform.position = replayManager.position;
    //}
    /// Pretty much the state machine 
    public void ChangeState(int newState)
    {
        //Login UI
        submitButton.SetActive(false);
        userNameInput.SetActive(false);
        passwordInput.SetActive(false);
        createToggle.SetActive(false);
        loginToggle.SetActive(false);
        textNameInfo.SetActive(false);
        textPasswordInfo.SetActive(false);

        //Main Menu UI
        joinAsPlayerButton.SetActive(false);
        ticTacToeSquareButton.SetActive(false);
        joinAsObserverButton.SetActive(false);

        //Chat UI
        chatroomBackgroundImage.SetActive(false);
        chatLogText.gameObject.SetActive(false);
        chatInputField.gameObject.SetActive(false);
        sendChatButton.SetActive(false);

        ////Board UI
        boardUI.SetActive(false);
        //for (int i = 0; i < gridSpaceButtonArray.Length; i++)
        //{
        //    gridSpaceButtonArray[i].SetActive(false);
        //}
        gameOverPanel.SetActive(false);
        //playerX_board.SetActive(false);
        //playerO_board.SetActive(false);
        //gridPanel_x_1.SetActive(false); gridPanel_x_2.SetActive(false);
        //gridPanel_y_1.SetActive(false); gridPanel_y_2.SetActive(false);
        //backGroundPanel.SetActive(false);
        //boardPanel.SetActive(false);

        if (newState == GameStates.LoginMenu)
        {
            submitButton.SetActive(true);
            userNameInput.SetActive(true);
            passwordInput.SetActive(true);
            createToggle.SetActive(true);
            loginToggle.SetActive(true);
            textNameInfo.SetActive(true);
            textPasswordInfo.SetActive(true);

            Debug.Log(newState);
        }
        else if(newState == GameStates.MainMenu)
        {
            chatroomBackgroundImage.SetActive(true);
            chatLogText.gameObject.SetActive(true);
            chatInputField.gameObject.SetActive(true);
            sendChatButton.SetActive(true);


            joinAsPlayerButton.SetActive(true);
            joinAsObserverButton.SetActive(true);
            //Maybe have observer here
            Debug.Log(newState);
        }
        else if (newState == GameStates.WaitingInQueue)
        {

            //Create the player to player communication here
            //Also create the toggle to switch from player to observer
            //When in player mode the client will be able to control their shape 
            //as well as chat
            //When in observer the client can only chat
            
            Debug.Log(newState);

        }
        else if (newState == GameStates.TicTacToeGame)
        {
            //ticTacToeSquareButton.SetActive(true);
            //set tictactoe game board ui to active

            boardUI.SetActive(true);
            //backGroundPanel.SetActive(true);
            //boardPanel.SetActive(true);
            //playerX_board.SetActive(true);
            //playerO_board.SetActive(true);
            //gridPanel_x_1.SetActive(true); gridPanel_x_2.SetActive(true);
            //gridPanel_y_1.SetActive(true); gridPanel_y_2.SetActive(true);
            //for (int i = 0; i < gridSpaceButtonArray.Length; i++)
            //{
            //    gridSpaceButtonArray[i].SetActive(true);
            //}
            //gridSpaceText.gameObject.SetActive(true);


            moveCount = 0;
            //SetTicTacToeButtons();
            
            playerSide = networkedClient.GetComponent<NetworkedClient>().GetPlayerSide();
            Debug.Log(playerSide);
            SetPlayerColour(playerX, playerO);

            StartGame();
            Debug.Log(newState);
        }

    }

}

[System.Serializable]
public class Player
{
    public Image panel;
    public Text text;
}

[System.Serializable]
public class PlayerColour
{
    public Color panelColour;
    public Color textColour;
}




static public class GameStates
{
    public const int LoginMenu = 1;
    public const int MainMenu = 2;
    public const int WaitingInQueue = 3;
    public const int TicTacToeGame = 4;
    public const int TicTacToeSomethingSomething = 5;
    public const int ClientIsObserver = 6;
}
