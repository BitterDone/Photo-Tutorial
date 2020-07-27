using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace Com.MyCompany.MyGame
{
    // public class LauncherScript : MonoBehaviour
    public class LauncherScript : MonoBehaviourPunCallbacks
    {
#region Private Serializable Fields
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
            Connect();
        }
#endregion
#region Public Methods
        // Start the connection process.
        // - If already connected, we attempt joining a random room
        // - if not yet connected, Connect this application instance to Photon Cloud Network
        public void Connect() {
            Debug.Log("Connect() called in LauncherScript");
            // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
            if (PhotonNetwork.IsConnected) {
                Debug.Log("Connect() called in LauncherScript");
                // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
                PhotonNetwork.JoinRandomRoom();
            }
            else {
                Debug.Log("Connect() called in LauncherScript");
                // #Critical, we must first and foremost connect to Photon Online Server.
                PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
            }
        }
#endregion
#region MonoBehaviourPunCallbacks Callbacks

public override void OnConnectedToMaster() {
    Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
}


public override void OnDisconnected(DisconnectCause cause) {
    Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
}

#endregion

    }
}