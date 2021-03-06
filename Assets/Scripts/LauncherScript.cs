﻿using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace Com.MyCompany.MyGame
{
    // public class LauncherScript : MonoBehaviour
    public class LauncherScript : MonoBehaviourPunCallbacks {
#region Private Serializable Fields
        [SerializeField]
        private byte maxPlayersPerRoom = 4;
        [Tooltip("The Ui Panel to let the user enter name, connect and play")]
        [SerializeField]
        private GameObject controlPanel;
        [Tooltip("The UI Label to inform the user that the connection is in progress")]
        [SerializeField]
        private GameObject progressLabel;
        bool isConnecting;
#endregion


#region Private Fields
        // This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
        string gameVersion = "1";


#endregion


#region MonoBehaviour CallBacks
        // MonoBehaviour method called on GameObject by Unity during early initialization phase.
        void Awake() {
            Debug.Log("Awake() called in LauncherScript");
            // #Critical
            // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
            PhotonNetwork.AutomaticallySyncScene = true;
        }
        // MonoBehaviour method called on GameObject by Unity during initialization phase.
        void Start() {
            Debug.Log("Start() called in LauncherScript");
            // Connect();
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
        }
#endregion
#region Public Methods
        // Start the connection process.
        // - If already connected, we attempt joining a random room
        // - if not yet connected, Connect this application instance to Photon Cloud Network
        public void Connect() {
            Debug.Log("Connect() called in LauncherScript");
            progressLabel.SetActive(true);
            controlPanel.SetActive(false);
            // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
            if (PhotonNetwork.IsConnected) {
                Debug.Log("PhotonNetwork.IsConnected true");
                // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
                PhotonNetwork.JoinRandomRoom();
            }
            else {
                Debug.Log("PhotonNetwork.IsConnected false");
                isConnecting = PhotonNetwork.ConnectUsingSettings();
                // #Critical, we must first and foremost connect to Photon Online Server.
                // PhotonNetwork.ConnectUsingSettings(); // moved to isConnecting boolean
                PhotonNetwork.GameVersion = gameVersion;
            }
        }
#endregion
#region MonoBehaviourPunCallbacks Callbacks

        public override void OnConnectedToMaster() {
            Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
            if (isConnecting) {
                // #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnJoinRandomFailed()
                PhotonNetwork.JoinRandomRoom();
                isConnecting = false;
            }
        }


        public override void OnDisconnected(DisconnectCause cause) {
            Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
            isConnecting = false;
        }

        public override void OnJoinRandomFailed(short returnCode, string message) {
            Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

            // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
            // PhotonNetwork.CreateRoom(null, new RoomOptions());
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
        }

        public override void OnJoinedRoom() {
            Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
            // #Critical: We only load if we are the first player, else we rely on `PhotonNetwork.AutomaticallySyncScene` to sync our instance scene.
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1) {
                Debug.Log("We load the 'Room for 1' ");
                // #Critical - Load the Room Level.
                PhotonNetwork.LoadLevel("Room for 1");
            }
        }
#endregion

    }
}