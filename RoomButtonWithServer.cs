using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class RoomButtonWithServer : MonoBehaviour
{
    public static string CurrentRoomName;
    string SavedRooms;

    List<string> roomNamesList_WithServer = new List<string>();

    public void Join(GameObject CurrentRoomButton)
    {
        CurrentRoomName = CurrentRoomButton.GetComponentInChildren<Text>().text;
        Manager.StartJoinToServer = true;
    }

    public void DeleteRoom(GameObject CurrentRoomButton)
    {
        CurrentRoomName = CurrentRoomButton.GetComponentInChildren<Text>().text;
        Manager.roomNamesList_WithServer.Remove(CurrentRoomName);
        SavedRooms = PlayerPrefs.GetString("SavedRooms_WithServer");
        roomNamesList_WithServer = SavedRooms.Split(' ').ToList();
        roomNamesList_WithServer.Remove(CurrentRoomName);
        SavedRooms = "";
        foreach (string i in roomNamesList_WithServer)
        {
            if (i != "")
            {
                SavedRooms += i + " ";
            }
        }
        PlayerPrefs.SetString("SavedRooms_WithServer", SavedRooms);
        Transform child = CurrentRoomButton.transform.GetChild(2);
        child.gameObject.SetActive(true);
    }
}
