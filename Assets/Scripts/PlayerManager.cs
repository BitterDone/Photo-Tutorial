using UnityEngine;
using UnityEngine.EventSystems;

using Photon.Pun;

using System.Collections;

namespace Com.MyCompany.MyGame {
    public class PlayerManager : MonoBehaviourPunCallbacks {
#region Private Fields

        [Tooltip("The Beams GameObject to control")]
        [SerializeField]
        private GameObject beams;
        //True, when the user is firing
        bool IsFiring;
#endregion

#region MonoBehaviour CallBacks

        void Awake() {
            if (beams == null) {
                Debug.LogError("<Color=Red><a>Missing</a></Color> Beams Reference.", this);
            }
            else {
                beams.SetActive(false);
            }
        }

        void Update() {

            ProcessInputs();

            // trigger Beams active state
            if (beams != null && IsFiring != beams.activeInHierarchy) {
                beams.SetActive(IsFiring);
            }
        }

#endregion

#region Custom

        void ProcessInputs() {
            if (Input.GetButtonDown("Fire1")) {
                if (!IsFiring) {
                    IsFiring = true;
                }
            }
            if (Input.GetButtonUp("Fire1")) {
                if (IsFiring) {
                    IsFiring = false;
                }
            }
        }

#endregion
    }
}