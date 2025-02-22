using System;
using UdonSharp;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

namespace AX.EasyHUD
{
    public class PlayerTracking : UdonSharpBehaviour
    {
        [SerializeField]
        private VRCPlayerApi.TrackingDataType trackingDataType;

        private Transform _camera;
        private Transform _audioListener;
        private Vector3 userOffsetVector3;
        [SerializeField]
        private Quaternion userOffsetQuaternion = Quaternion.identity;

        [SerializeField]
        private Vector3 offsetVector3;

        [SerializeField]
        private Quaternion offsetQuaternion = Quaternion.identity;

        [SerializeField]
        private Transform hud;

        [SerializeField, Header("-----------------\nuGUIからの入力を受け付けるスライダー")]
        private Slider xSlider;

        [SerializeField]
        private Slider ySlider;

        [SerializeField]
        private Slider zSlider;

        [SerializeField, Header("Editor上で追従する対象")]
        private Transform debugObj;

        private void Start()
        {
            _camera = transform.Find("Camera");
            _audioListener = transform.Find("ScaleRef");
            if (Utilities.IsValid(_camera))
            {
                Camera cam = _camera.GetComponent<Camera>();
                if (Utilities.IsValid(cam)) cam.enabled = true;
            }

            if (Utilities.IsValid(_audioListener)) _audioListener.gameObject.SetActive(true);
        }

        private void LateUpdate()
        {

            var data = Networking.LocalPlayer.GetTrackingData(trackingDataType);
            
            Vector3 vec3 = data.position;
            Quaternion quat = data.rotation;
            
#if UNITY_EDITOR
            // エディタ上でのデバッグ用
            if (Utilities.IsValid(debugObj))
            {
                vec3 = debugObj.position;
                quat = debugObj.rotation;
            }
#endif

            var transform1 = transform;
            transform1.position = vec3 + offsetVector3 + userOffsetVector3;
            transform1.rotation = userOffsetQuaternion * offsetQuaternion * quat;

            if (Utilities.IsValid(_camera))
            {
                hud.localScale = (1.0f / _audioListener.localScale.x) * Vector3.one;
            }
        }

        public void SetOffset(Vector3 pos, Quaternion rot)
        {
            userOffsetVector3 = pos;
            userOffsetQuaternion = rot;
        }

        public void ReadSlider()
        {
            userOffsetVector3 = new Vector3(xSlider.value, ySlider.value, zSlider.value);
        }
    }
}