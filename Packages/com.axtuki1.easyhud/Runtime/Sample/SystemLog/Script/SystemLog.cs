using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using TMPro;

namespace AX.SystemLogger
{
    public class SystemLog : UdonSharpBehaviour
    {
        public GameObject textTmp;

        [Tooltip("一度に表示するテキストの最大数")]
        public int maxText = 10;

        [Tooltip("前メッセージ表示しきった後の待機時間")]
        public float delay = 0.25f;

        [Tooltip("省略時の削除感覚")]
        public float ommisionDelay = 0.125f;

        public string[] queue = new string[0];

        [SerializeField]
        private float beforeTime = 0, ommisionTime = 0;

        private Animator animator;

        private void Start()
        {
            textTmp.SetActive(false);
            animator = GetComponent<Animator>();
        }

        public void Log(string text)
        {
            var tmp = new string[queue.Length + 1];
            queue.CopyTo(tmp, 0);
            tmp[tmp.Length - 1] = text;
            queue = tmp;
        }

        private void Update()
        {
            if (
                transform.childCount == 0 || // 表示中テキストがない もしくは
                transform.GetChild(transform.childCount - 1).GetComponent<SystemText>().isCompleted // 最後のテキストが表示完了している
            )
            {
                beforeTime += Time.deltaTime;
                if (
                    beforeTime >= delay // 一定時間経過
                )
                {
                    if (queue.Length != 0) // キューが空でない
                    {
                        LogProcess();
                        beforeTime = 0;
                        ommisionTime = 0;
                    }
                    else if (transform.childCount > 1) // 表示中テキストが2つ以上の場合省略を行う
                    {
                        ommisionTime += Time.deltaTime;
                        if (ommisionTime >= ommisionDelay)
                        {
                            animator.SetTrigger("Blink");
                            Destroy(transform.GetChild(0).gameObject);
                            ommisionTime = 0;
                        }
                    }
                }
            }
        }

        public void LogProcess()
        {
            string text = queue[0];
            if (transform.childCount >= maxText)
            {
                Destroy(transform.GetChild(0).gameObject);
            }

            GameObject obj = Instantiate(textTmp);
            obj.transform.SetParent(transform, false);
            obj.SetActive(true);
            obj.GetComponent<TextMeshProUGUI>().text = text;
            obj.GetComponent<TextMeshProUGUI>().maxVisibleCharacters = 0;

            // キューから削除
            var tmp = new string[queue.Length - 1];
            for (int i = 1; i < queue.Length; i++)
            {
                tmp[i - 1] = queue[i];
            }

            queue = tmp;
        }
    }
}