
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace AX.EasyHUD
{
    public class PlayerHUD : UdonSharpBehaviour
    {
        [SerializeField] private Transform targetObj;
        [SerializeField] private float speed;
        private void Update()
        {
            // targetObjに追従する
            var transform1 = transform;
            transform1.position = Vector3.Lerp(transform1.position, targetObj.position, speed * Time.deltaTime);
            transform1.rotation = Quaternion.Lerp(transform1.rotation, targetObj.rotation, speed * Time.deltaTime);
        }
    }

}