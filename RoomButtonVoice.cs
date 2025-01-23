using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Photon.Voice.Unity;

public class RoomButtonVoice : MonoBehaviour
{
    public static string CurrentRoomName;
    string SavedRooms;

    List<string> roomNamesList_Voice = new List<string>();

    public void Join(GameObject CurrentRoomButton)
    {
        CurrentRoomName = CurrentRoomButton.GetComponentInChildren<Text>().text;
        Manager.StartJoin_Voice = true;
    }

    public void DeleteRoom(GameObject CurrentRoomButton)
    {
        CurrentRoomName = CurrentRoomButton.GetComponentInChildren<Text>().text;
        Manager.roomNamesList_Voice.Remove(CurrentRoomName);
        SavedRooms = PlayerPrefs.GetString("SavedRooms_Voice");
        roomNamesList_Voice = SavedRooms.Split('/').ToList();
        roomNamesList_Voice.Remove(CurrentRoomName);
        SavedRooms = "";
        foreach (string i in roomNamesList_Voice)
        {
            if (i != "")
            {
                SavedRooms += i + "/";
            }
        }
        PlayerPrefs.SetString("SavedRooms_Voice", SavedRooms);
        Transform child = CurrentRoomButton.transform.GetChild(2);
        child.gameObject.SetActive(true);
    }
}
