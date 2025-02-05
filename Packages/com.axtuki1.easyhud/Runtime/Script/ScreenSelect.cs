
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace AX.EasyHUD
{
    public class ScreenSelect : UdonSharpBehaviour
    {
        [SerializeField]
        private GameObject vrModeParentObject, desktopModeParentObject, targetObject;

        void Start()
        {
            var localPlayer = Networking.LocalPlayer;
            var parentObject = localPlayer.IsUserInVR() ? vrModeParentObject : desktopModeParentObject;
            targetObject.transform.SetParent(parentObject.transform, false);
        }
    }
}