using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using TMPro;

namespace AX.SystemLogger
{
    public class SystemText : UdonSharpBehaviour
    {
        [Tooltip("次の文字表示までの秒数")]
        public float nextCharSec = 0.005f;

        [Tooltip("表示完了後、自壊するまでの時間")]
        public float completeDelayDestroySec = 4f;

        [SerializeField, Tooltip("自分が最後のテキストの場合のみ自壊するか")]
        private bool isSelfDestroyOnlyLastText = true;

        [Tooltip("文字表示時の音声再生間隔")]
        public int SoundPlayCharDelay = 2;

        [SerializeField, Tooltip("音声再生用のAudioSource")]
        private AudioSource audioSource;

        [Tooltip("再生する音源")]
        public AudioClip audioClip;

        private float beforeTime = 0;
        public bool isCompleted = false;

        [SerializeField]
        private int maxVisibleCharacter;

        [SerializeField]
        private int view;

        private TextMeshProUGUI tmp;
        private int playCountRemain = 0;

        private void Start()
        {
            tmp = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            beforeTime += Time.deltaTime;
            if (!isCompleted && beforeTime >= nextCharSec)
            {
                maxVisibleCharacter++;
                tmp.maxVisibleCharacters = maxVisibleCharacter;
                if (tmp.maxVisibleCharacters >= tmp.textInfo.characterCount)
                {
                    isCompleted = true;
                }
                else
                {
                    playCountRemain--;
                    if (playCountRemain <= 0)
                    {
                        PlaySound();
                        playCountRemain = SoundPlayCharDelay;
                    }
                }

                beforeTime = 0;
                tmp.ForceMeshUpdate();
            }
            else if (
                isCompleted && beforeTime >= completeDelayDestroySec // 表示完了後の一定時間経過
            )
            {
                if (isSelfDestroyOnlyLastText && transform.GetSiblingIndex() != transform.parent.childCount - 1)
                {
                    return;
                }

                Destroy(gameObject);
                beforeTime = 0;
            }
        }

        private void PlaySound()
        {
            if (audioSource != null && audioClip != null) audioSource.PlayOneShot(audioClip);
        }
    }
}