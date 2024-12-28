using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class ManagerChat : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_InputField InputMassage;
    [SerializeField] TMP_InputField InputPassword;
    [SerializeField] TMP_Text Massage;
    [SerializeField] TMP_Text RoomName;

    [SerializeField] RectTransform textRT;
    [SerializeField] RectTransform contentRT;

    public GameObject OnlineOff;
    public GameObject OnlineOn;
    public GameObject ChisloLudey;
    public Text ChisloLudey_Text;
    public GameObject CheckingScreen;
    public GameObject WaitingScreen;
    public GameObject PasswordScreen;

    private PhotonView PhotonView;

    public string Diolog;
    string CurrentMessage;
    int CountOfPeople;
    bool Ending = false;
    string Password;

    private void Start()
    {
        PhotonView = GetComponent<PhotonView>();
        RoomName.text = PlayerPrefs.GetString("CurrentRoom");

        if (PlayerPrefs.GetString("IsJtoServ") == "no")
        {
            PasswordScreen.SetActive(false);
            Check();
        }

        else
        {
            CheckingScreen.SetActive(false);
            WaitingScreen.SetActive(false);
            PasswordScreen.SetActive(true);
        }
            
        QualitySettings.vSyncCount = 0;
        Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true, 120);
        Application.targetFrameRate = 120;
    }
    
    async void Check()
    {
        await System.Threading.Tasks.Task.Delay(150);
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            string checking = PlayerPrefs.GetString(RoomName.text);
            checking = checking.Replace(" ", "");
            checking = checking.Replace("\n", "");
            string ch_Diolog = Massage.text;
            ch_Diolog = ch_Diolog.Replace(" ", "");
            ch_Diolog = ch_Diolog.Replace("\n", "");
            if (checking != ch_Diolog)
            {
                PhotonNetwork.LeaveRoom();
            }
            else
            {
                CheckingScreen.SetActive(false);
            }
        }

        else if (PlayerPrefs.GetString(RoomName.text) != "")
        {          
            InputMassage.text = PlayerPrefs.GetString(RoomName.text);
            SendMassage();
            InputMassage.text = "";                      
        }

        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            CheckingScreen.SetActive(false);
        }
    }

    [PunRPC]
    private async void Send_Password(string password)
    {
        Password = password;
    }

    public void EnterPassword()
    {
        if (InputPassword.text != "")
        {
            if (InputPassword.text == Password)
            {
                PasswordScreen.SetActive(false);
            }
            else
            {
                PhotonNetwork.LeaveRoom();
            }
        }
    }

    private void Update()
    {
        var size = contentRT.sizeDelta;
        size.y = textRT.sizeDelta.y;
        contentRT.sizeDelta = size;

        if (Ending == false)
        {
            CountOfPeople = PhotonNetwork.CurrentRoom.PlayerCount;
        }
        if (CountOfPeople == 2)
        {
            OnlineOff.SetActive(false);
            OnlineOn.SetActive(true);
            WaitingScreen.SetActive(false);
        }
        else if (CountOfPeople == 1)
        {
            OnlineOff.SetActive(true);
            OnlineOn.SetActive(false);
            WaitingScreen.SetActive(true);
        }
        if (PlayerPrefs.GetString("IsJtoServ") == "yes")
        {
            OnlineOff.SetActive(false);
            OnlineOn.SetActive(false);
            ChisloLudey.SetActive(true);
            int CountOfPeople2 = CountOfPeople - 1;
            ChisloLudey_Text.text = CountOfPeople2.ToString();
        }
    }

    public void Copy()
    {
        GUIUtility.systemCopyBuffer = Massage.text;
    }

    public void ExitRoom()
    {
        PlayerPrefs.SetString("CurrentDialog", Diolog);
        Ending = true;
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel("Menu");
    }

    [PunRPC]
    private async void Update_View(string diolog)
    {
        if (diolog != "")
        {
            Massage.text = diolog;
        }
    }

    public void SendMassage()
    {
        PhotonView.RpcSecure("Send_Data", RpcTarget.AllBufferedViaServer, true, PhotonNetwork.NickName, InputMassage.text);
    }

    [PunRPC]
    private async void Send_Data(string nick, string message)
    {
        if (PlayerPrefs.GetString(RoomName.text) != "" && Diolog == "" && InputMassage.text == "")
        {
            CurrentMessage = message;
        }
        else
        {
            CurrentMessage = nick + ": " + message;
        }

        if (Diolog == "")
        {
            Diolog = Diolog + CurrentMessage;
        }
        else
        {
            Diolog = Diolog + "\n" + CurrentMessage;
        }
        Massage.text = Diolog;

        await System.Threading.Tasks.Task.Delay(100);
        if (contentRT.sizeDelta.y > 1800)
        {
            if (CurrentMessage.Length < 45)
            {
                contentRT.position = new Vector2(contentRT.position.x, contentRT.position.y + 46);
            }
            else if (CurrentMessage.Length < 45 * 2)
            {
                contentRT.position = new Vector2(contentRT.position.x, contentRT.position.y + (46) * 2);
            }
            else if (CurrentMessage.Length < 45 * 3)
            {
                contentRT.position = new Vector2(contentRT.position.x, contentRT.position.y + (46) * 3);
            }
            else
            {
                contentRT.position = new Vector2(contentRT.position.x, contentRT.position.y + (46) * 4);
            }
        }
        
    }

}
