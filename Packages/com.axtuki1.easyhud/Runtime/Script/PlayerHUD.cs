
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace AX.EasyHUD
{
    public class PlayerHUD : UdonSharpBehaviour
    {
        [SerializeField]
        private Transform targetObj;
        [SerializeField]
        private float speed;
        [SerializeField, Tooltip("高速モードに切り替わる距離の閾値")]
        private float fastModeDistanceThreshold = 3f;
        [SerializeField, Tooltip("高速モード時の速度倍率 speedベース")]
        private float fastModeSpeedMultiplier = 10f;
        
        private float currentSpeed;

        private void Start()
        {
            currentSpeed = speed;
        }

        private void Update()
        {
            // targetObjに追従する
            var transform1 = transform;
            // 距離が一定以上離れている場合は高速モードにする
            if (Vector3.Distance(transform1.position, targetObj.position) > fastModeDistanceThreshold)
            {
                currentSpeed = speed * fastModeSpeedMultiplier;
            }
            else
            {
                currentSpeed = speed;
            }
            transform1.position = Vector3.Lerp(transform1.position, targetObj.position, currentSpeed * Time.deltaTime);
            transform1.rotation = Quaternion.Lerp(transform1.rotation, targetObj.rotation, currentSpeed * Time.deltaTime);
        }
        
    }

}