using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField userName;
    public TMP_InputField roomName;
    public TMP_InputField maxPlayer;

    public GameObject playerNamePanel;
    public GameObject connectingPanel;
    public GameObject lobbyPanel;
    public GameObject createRoomPanel;
    public GameObject roomListPanel;
    public GameObject roomListPrefab;
    public GameObject roomListParent;


    public Button roomList;

    private Dictionary<string, RoomInfo> roomListData = new Dictionary<string, RoomInfo>();
    private Dictionary<string, GameObject> roomListGameObject;

    #region UnityMehtods
    private void Awake()
    {
        
    }
    void Start()
    {
        ActivatePanel(playerNamePanel.name);
       
        roomListGameObject = new Dictionary<string, GameObject>();
        roomList.onClick.AddListener(RoomListBtn);
    }

    void Update()
    {
        Debug.Log("Network state: " + PhotonNetwork.NetworkClientState);
    }

    #endregion

    #region UIMethods
    public void OnLoginClick()
    {
        string name = userName.text;

        if (!string.IsNullOrEmpty(name))
        {
            PhotonNetwork.LocalPlayer.NickName = name;
            PhotonNetwork.ConnectUsingSettings();
            ActivatePanel(connectingPanel.name);
        }
        else
        {
            Debug.Log("Empty Name");
        }
    }

    public void OnClickRoomCreate()
    {
        string roomname = roomName.text;
        if (string.IsNullOrEmpty(roomname))
        {
            roomname = roomname + Random.Range(0, 1000);
        }

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = (byte)int.Parse(maxPlayer.text);

        PhotonNetwork.CreateRoom(roomname, roomOptions);
    }

    public void OnCancelClick()
    {
        ActivatePanel(lobbyPanel.name);
    }

   /* public void RoomListBtnClicked()
    {
        if(!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
        ActivatePanel(roomListPanel.name);
    }*/

    public void RoomListBtn()
    {
            PhotonNetwork.JoinLobby();
       /* if (!PhotonNetwork.InLobby)
        {
            Debug.Log("Joined lobby");
        }
        Debug.Log("In lobby lobby");*/
        
    }

    #endregion


    #region PHOTON_CALLBACKS
    public override void OnConnected()
    {
        Debug.Log("Connected to Internet!");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " is connected to photon");
        ActivatePanel(lobbyPanel.name);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log(PhotonNetwork.CurrentRoom.Name + " is created!");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " room joined...");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {

        //  clearRoom();
        Debug.Log("1" + roomList.Count);
       foreach(var s in roomList)
        {
            Debug.Log(s);
        }
        foreach (RoomInfo rooms in roomList)
        {
            Debug.Log("2");
            Debug.Log("Room name : " + rooms.Name);
            if (!rooms.IsOpen || !rooms.IsVisible || rooms.RemovedFromList)
            {
                if (roomListData.ContainsKey(rooms.Name))
                {
                    roomListData.Remove(rooms.Name);
                }
            }
            else
            {
                if (roomListData.ContainsKey(rooms.Name))
                {
                    roomListData[rooms.Name] = rooms;
                }
                else
                {
                    roomListData.Add(rooms.Name, rooms);
                }
            }
        }

        foreach (RoomInfo roomItem in roomListData.Values)
        {
            Debug.Log("Room exist: " + roomItem.Name);
            GameObject item = Instantiate(roomListPrefab, roomListParent.transform);
            item.transform.localScale = Vector3.one;
            item.transform.GetChild(0).GetComponent<TMP_Text>().text = roomItem.Name;
            item.transform.GetChild(1).GetComponent<TMP_Text>().text = roomItem.PlayerCount + "/" + roomItem.MaxPlayers;
            item.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(delegate { RoomJoinFromList(roomItem.Name); });
           // roomlistGameobject.Add(roomItem.Name, item);

        }
    }
    public override void OnJoinedLobby()
    {
        Debug.Log("Lohh");
        ActivatePanel(roomListPanel.name);
    }

    #endregion

    #region Public_Mehtods

    public void RoomJoinFromList(string roomName)
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }
        PhotonNetwork.JoinRoom(roomName);
    }

    public void ClearRoomList()
    {
        Debug.Log("Cleared");
        if (roomListGameObject.Count>0)
        {
            foreach (var v in roomListGameObject.Values)
            {
                Destroy(v);
            }
            roomListGameObject.Clear();
        }
    }

    public void ActivatePanel(string panelName)
    {
        lobbyPanel.SetActive(panelName.Equals(lobbyPanel.name));
        playerNamePanel.SetActive(panelName.Equals(playerNamePanel.name));
        createRoomPanel.SetActive(panelName.Equals(createRoomPanel.name));
        connectingPanel.SetActive(panelName.Equals(connectingPanel.name));
        roomListPanel.SetActive(panelName.Equals(roomListPanel.name));
    }

    #endregion
}
