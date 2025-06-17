using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using ExitGames.Client.Photon;
using System.Collections.Generic;
using System.Linq;

public class PhotonInitHandler : MonoBehaviourPunCallbacks
{
    public string DestinationScene;

    public string DefaultSceneID;

#if UNITY_EDITOR
    public UnityEditor.SceneAsset SceneDefaultAsset;
#endif

    [HideInInspector] public bool InRandom;
    [HideInInspector] public bool InLobby;

    public static Action<bool> OnMatchmakingReady;
    public static Action OnStartSolo;
    public static Action OnRoomListChange;
    public static Action<Player> OnPlayerEnter;
    public static Action<Player> OnPlayerLeft;

    private string _createdRoomName = null;

    private List<RoomInfo> _cachedRoomList = new List<RoomInfo>();


    private void Awake()
    {
        var playerEntity = EntityModule.GetEntity<PlayerEntity>();

        if (PhotonNetwork.IsConnectedAndReady)
        {
            Hashtable playerProperties = new Hashtable();
            playerProperties["PlayFabID"] = playerEntity.PlayFabUniqueID;

            PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
        }

    }

    public void LoadSceneWithPhoton()
    {
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            PhotonNetwork.LoadLevel(2);
        }
    }

    public void TryToJoin(RoomInfo room)
    {
        if (room == null)
        {
            Debug.LogWarning("Попытка подключения к пустой комнате!");
            return;
        }

        if (!room.IsOpen || !room.IsVisible)
        {
            Debug.LogWarning($"Комната {room.Name} закрыта или невидима.");
            return;
        }

        PhotonNetwork.JoinRoom(room.Name);
        Debug.Log($"Подключаемся к комнате: {room.Name}");
    }

    public void CreateLobbyWithCustomProperties()
    { 
        RoomOptions roomOptions = new RoomOptions
        {
            MaxPlayers = 2,
        };

        _createdRoomName = Guid.NewGuid().ToString();

        PhotonNetwork.CreateRoom(_createdRoomName, roomOptions);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);

        foreach (var room in roomList)
        {
            if (room.RemovedFromList)
                _cachedRoomList.RemoveAll(r => r.Name == room.Name);
            else
                _cachedRoomList.Add(room);
        }

        OnRoomListChange?.Invoke();
    }
    public void FindRoom()
    {
        InRandom = true;
        InLobby = false;

        PhotonNetwork.NickName = $"Player number {UnityEngine.Random.Range(0, 100)}";

        if (PhotonNetwork.InRoom)
        {
            Debug.LogWarning("Already in a room, skipping room search.");
            return;
        }

        Debug.Log("Trying to find a room...");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        var roomOptions = new RoomOptions { MaxPlayers = 2 };

        _createdRoomName = Guid.NewGuid().ToString();

        Debug.Log($"No available rooms found. Creating a new one... {_createdRoomName}");

        PhotonNetwork.CreateRoom(_createdRoomName, roomOptions);

        OnMatchmakingReady?.Invoke(false);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"Failed to create room: {message}");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"Joined room: {PhotonNetwork.CurrentRoom.Name}");

        if (_createdRoomName != null && _createdRoomName == PhotonNetwork.CurrentRoom.Name)
        {
            Debug.Log("This is our own created room, waiting for players...");
        }
        else
        {
            Debug.Log("Joined an existing room, removing our own if it was created.");
            _createdRoomName = null; // Очистка, если мы нашли чужую комнату
        }

        if (PhotonNetwork.CurrentRoom.MaxPlayers == 1)
        {
            if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("MapID", out object sceneName))
            {
                Debug.Log($"Загружаю сцену: {sceneName}");
                PhotonNetwork.LoadLevel(sceneName.ToString());
            }
            else
            {
                Debug.LogWarning("SceneName не найден в Room Properties!");
            }
        }

        CheckRoomStatus();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);

        OnMatchmakingReady?.Invoke(false);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"Player {newPlayer.NickName} joined.");

        OnPlayerEnter?.Invoke(newPlayer);

        CheckRoomStatus();
    }

    private void CheckRoomStatus()
    {
        Debug.Log($"Current players in room: {PhotonNetwork.CurrentRoom.PlayerCount}");

        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            Debug.Log("Room is full, ready to start!");
            OnMatchmakingReady?.Invoke(true);
            // Здесь можно вызывать коллбэк для UI, например, кнопку старта
        }
    }

    private void OnValidate()
    {
#if UNITY_EDITOR
        DefaultSceneID = SceneDefaultAsset.name;
#endif
    }
}
