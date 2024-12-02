using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;

public class Manager : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_InputField RoomName;
    [SerializeField] TMP_InputField nickName;

    [SerializeField] RectTransform contentRT;

    public static List<string> roomNamesList = new List<string>();
    public static List<string> roomNamesList_WithServer = new List<string>();
    int summCountOfElements;
    string SavedRooms;
    string SavedRooms_WithServer;
    string CurrentDialog;
    string LastCurrentRoom;

    public GameObject RoomButtonPrefab;
    public GameObject RoomButtonPrefabWithServer;
    public GameObject LoadingScreen;
    public GameObject MainScreen;
    public GameObject DeleteScreen;
    public GameObject InfoScreen;
    public GameObject Content;

    public static bool StartJoin = false;
    public static bool StartJoinToServer = false;

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        nickName.text = PlayerPrefs.GetString("Nick");
        DownloadRooms();

        QualitySettings.vSyncCount = 0;
        Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true, 120);
        Application.targetFrameRate = 120;
    }

    private void DownloadRooms()
    {
        SavedRooms = PlayerPrefs.GetString("SavedRooms");
        SavedRooms_WithServer = PlayerPrefs.GetString("SavedRooms_WithServer");
        CurrentDialog = PlayerPrefs.GetString("CurrentDialog");
        LastCurrentRoom = PlayerPrefs.GetString("CurrentRoom");
        int count = 1;
        if (SavedRooms != "")
        {
            roomNamesList = SavedRooms.Split(' ').ToList();
            roomNamesList.RemoveAt(roomNamesList.Count - 1);
            foreach (string i in roomNamesList)
            {
                RoomName.text = i;
                GameObject NewRoomButton = GameObject.Instantiate(RoomButtonPrefab, contentRT);
                NewRoomButton.GetComponentInChildren<Text>().text = RoomName.text;
                contentRT.sizeDelta += new Vector2(0, -220);
                count += 1;
                NewRoomButton.transform.position += new Vector3(0, 110 * count, 0);
                contentRT.position += new Vector3(0, -220, 0);
                RoomName.text = "";
            }
        }

        if (CurrentDialog != "")
        {
            PlayerPrefs.SetString(LastCurrentRoom, CurrentDialog);
        }

        if (SavedRooms_WithServer != "")
        {
            count = roomNamesList.Count + 1;
            roomNamesList_WithServer = SavedRooms_WithServer.Split(' ').ToList();
            roomNamesList_WithServer.RemoveAt(roomNamesList_WithServer.Count - 1);
            foreach (string i in roomNamesList_WithServer)
            {
                RoomName.text = i;
                GameObject NewRoomButton = GameObject.Instantiate(RoomButtonPrefabWithServer, contentRT);
                NewRoomButton.GetComponentInChildren<Text>().text = RoomName.text;
                contentRT.sizeDelta += new Vector2(0, -220);
                count += 1;
                NewRoomButton.transform.position += new Vector3(0, 110 * count, 0);
                contentRT.position += new Vector3(0, -220, 0);
                RoomName.text = "";
            }
        }
    }

    public override void OnConnectedToMaster()
    {
        LoadingScreen.SetActive(false);
        MainScreen.SetActive(true);
    }

    public void CreateChat(GameObject NewRoomButton)
    {
        if (RoomName.text != "")
        {
            roomNamesList.Add(RoomName.text);
            NewRoomButton = GameObject.Instantiate(RoomButtonPrefab, contentRT);
            NewRoomButton.GetComponentInChildren<Text>().text = RoomName.text;
            contentRT.sizeDelta += new Vector2(0, -220);
            int count = summCountOfElements + 1;
            NewRoomButton.transform.position += new Vector3(0, 110 * count, 0);
            contentRT.position += new Vector3(0, -220, 0);

            SavedRooms = "";
            foreach (string i in roomNamesList)
            {
                SavedRooms += i + " ";
            }
            PlayerPrefs.SetString("SavedRooms", SavedRooms);
        }
    }

    public void CreateChatWithServer(GameObject NewRoomButton)
    {
        if (RoomName.text != "")
        {
            roomNamesList_WithServer.Add(RoomName.text);
            NewRoomButton = GameObject.Instantiate(RoomButtonPrefabWithServer, contentRT);
            NewRoomButton.GetComponentInChildren<Text>().text = RoomName.text;
            contentRT.sizeDelta += new Vector2(0, -220);
            int count = summCountOfElements + 1;
            NewRoomButton.transform.position += new Vector3(0, 110 * count, 0);
            contentRT.position += new Vector3(0, -220, 0);

            SavedRooms_WithServer = "";
            foreach (string i in roomNamesList_WithServer)
            {
                SavedRooms_WithServer += i + " ";
            }
            PlayerPrefs.SetString("SavedRooms_WithServer", SavedRooms_WithServer);
        }
    }

    public void CreateOrJoinRoom()
    {
        RoomName.text = RoomButton.CurrentRoomName;
        PlayerPrefs.SetString("CurrentRoom", RoomName.text);
        PlayerPrefs.SetString("Nick", nickName.text);

        if (nickName.text == "")
        {
            PhotonNetwork.NickName = "Пользователь";
        }
        else
        {
            PhotonNetwork.NickName = nickName.text;
        }

        if (PhotonNetwork.IsConnected && RoomName.text != "")
        {
            PlayerPrefs.SetString("IsJtoServ", "no");
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 2;
            PhotonNetwork.JoinOrCreateRoom(RoomName.text, roomOptions, TypedLobby.Default);
        }
    }

    public void JoinToServer()
    {
        RoomName.text = RoomButtonWithServer.CurrentRoomName;
        PlayerPrefs.SetString("CurrentRoom", RoomName.text);
        PlayerPrefs.SetString("Nick", nickName.text);

        if (nickName.text == "")
        {
            PhotonNetwork.NickName = "Пользователь";
        }
        else
        {
            PhotonNetwork.NickName = nickName.text;
        }

        if (PhotonNetwork.IsConnected && RoomName.text != "")
        {
            PlayerPrefs.SetString("IsJtoServ", "yes");
            PhotonNetwork.JoinRoom(RoomName.text);
        }
    }

    private void Update()
    {
        if (StartJoin == true)
        {
            StartJoin = false;
            CreateOrJoinRoom();
        }

        if (StartJoinToServer == true)
        {
            StartJoinToServer = false;
            JoinToServer();
        }

        summCountOfElements = roomNamesList.Count + roomNamesList_WithServer.Count + 1;
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Chat");
    }

    public void IwantClearAll()
    {
        DeleteScreen.SetActive(true);
    }

    public void Otmena()
    {
        DeleteScreen.SetActive(false);
    }

    public void Info_Open()
    {
        InfoScreen.SetActive(true);
    }

    public void Info_Close()
    {
        InfoScreen.SetActive(false);
    }

    public void ClearAll()
    {
        DeleteScreen.SetActive(false);
        PlayerPrefs.SetString("CurrentDialog", "");
        PlayerPrefs.SetString("Nick", "");
        nickName.text = string.Empty;

        foreach (string i in roomNamesList)
        {
            if (i != "")
            {
                PlayerPrefs.SetString(i, "");
            }
        }
        roomNamesList.Clear();
        foreach (string i in roomNamesList_WithServer)
        {
            if (i != "")
            {
                PlayerPrefs.SetString(i, "");
            }
        }
        roomNamesList_WithServer.Clear();

        PlayerPrefs.SetString("SavedRooms", "");
        SavedRooms = "";
        PlayerPrefs.SetString("SavedRooms_WithServer", "");
        SavedRooms_WithServer = "";

        int CountOfChats = contentRT.transform.childCount;
        while (CountOfChats > 0)
        {
            Transform child = Content.transform.GetChild(CountOfChats - 1);
            Destroy(child.gameObject);
            CountOfChats--;
        }

        PlayerPrefs.DeleteAll();

        contentRT.position = new Vector3(40, 0, 0);
        contentRT.sizeDelta = new Vector2(0, 0);
    }
}
