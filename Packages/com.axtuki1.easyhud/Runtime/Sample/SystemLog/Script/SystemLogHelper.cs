using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace AX.SystemLogger
{
    public class SystemLogHelper : UdonSharpBehaviour
    {
        [SerializeField]
        private SystemLog logger;

        [SerializeField]
        private bool isPlayerJoinNotice = false;
        
        [SerializeField]
        private string joinText = "<color=#00de1e>[Join]</color> {0}が参加";
        
        [SerializeField]
        private string leaveText = "<color=#de0b00>[Left]</color> {0}が退出";
        
        [SerializeField]
        private string leaveTextLocal = "<color=#de0b00>[Left]</color> またねー!";
        
        [SerializeField]
        private string[] debugTexts = new string[0];

        public void Log(string text)
        {
            logger.Log(text);
        }

        public override void OnPlayerJoined(VRCPlayerApi player)
        {
            if (isPlayerJoinNotice) logger.Log(string.Format(joinText, player.displayName));
        }

        public override void OnPlayerLeft(VRCPlayerApi player)
        {
            if (!isPlayerJoinNotice) return;
            if (player == null || player.isLocal)
            {
                logger.Log(leaveTextLocal);
            }
            else
            {
                logger.Log(string.Format(leaveText, player.displayName));
            }
        }

        public void SendTestText()
        {
            if (debugTexts.Length > 0)
            {
                logger.Log(debugTexts[Random.Range(0, debugTexts.Length)]);
            }
            else
            {
                logger.Log("TestingTasting...");
            }
        }
    }
}