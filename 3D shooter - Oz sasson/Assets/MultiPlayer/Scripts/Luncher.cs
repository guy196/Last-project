using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Linq;

public class Luncher : MonoBehaviourPunCallbacks
{
    public static Luncher Instance;
    [SerializeField] TMP_InputField roomnameInputField;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text roomNameText;
    [SerializeField] Transform roomlistContent;
    [SerializeField] GameObject roomlistItemPrefab;
    [SerializeField] GameObject PlayerlistItemPrefab;
    [SerializeField] Transform PlayerlistContent;
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        Debug.Log("connecting to Master");

        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("connected to Master");

        PhotonNetwork.JoinLobby(); // we have to join lobby before we create or join a room
    }

    public override void OnJoinedLobby()
    {
        MenuManager.Instance.OpenMenu("title");
        Debug.Log("Joined lobby");
        PhotonNetwork.NickName = "Player" + Random.Range(0, 1000).ToString("0000");
    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomnameInputField.text))
        {
            return;
        }
        PhotonNetwork.CreateRoom(roomnameInputField.text);
        MenuManager.Instance.OpenMenu("loading");
    }

    public override void OnJoinedRoom()
    {
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;
        MenuManager.Instance.OpenMenu("room");
        Player[] players = PhotonNetwork.PlayerList;

        for (int i = 0; i < players.Count(); i++)
        {
            Instantiate(PlayerlistItemPrefab, PlayerlistContent).GetComponent<PlayerListItem>().SetUp(players[i]);
        }   
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "room creation Failed" + message;
        MenuManager.Instance.OpenMenu("error");
    }

    public void leaveroom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenu("loading");
    }

    public override void OnLeftRoom()
    {
        MenuManager.Instance.OpenMenu("title");
    }

    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.Instance.OpenMenu("loading");

    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform trans in roomlistContent)
        {
            Destroy(trans.gameObject);
        }
        for (int i = 0; i < roomList.Count; i++)
        {
            Instantiate(roomlistItemPrefab, roomlistContent).GetComponent<roomlistItem>().Setup(roomList[i]);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(PlayerlistItemPrefab, PlayerlistContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }
}
