using MiddleGround.Save;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace MiddleGround.UI
{
    public class MG_LoadingPanel : MonoBehaviour
    {
        public Text text_loading;
        public Text text_notice;
        public Slider slider_loading;
        private void Start()
        {
            slider_loading.value = 0;
            text_loading.text = "Loading...0%";
            StartCoroutine("WaitLoad");
        }
        IEnumerator WaitLoad()
        {
            string noticeContent;
#if UNITY_ANDROID
            noticeContent = "Google is not a sponsor nor is involved in any way\n  with these contests or sweepstakes.";
#elif UNITY_IOS
            noticeContent = "Apple is not a sponsor nor is involved in any way\n  with these contests or sweepstakes.";
#endif
            Coroutine getCor = null;
            if (!MG_Manager.Instance.Get_Save_PackB())
                getCor = StartCoroutine(WaitFor());
            int progress = 0;
            int speed = 1;
            float maxWaitTime = 5;
            bool hasB = false;
            while (true)
            {
                yield return null;
                if (slider_loading.value < 1)
                {
                    progress += 1 * speed;
                    progress = Mathf.Clamp(progress, 0, 1000);
                    slider_loading.value = progress * 0.001f;
                    text_loading.text = "Loading..." + progress / 10 + "%";
                    if (!hasB && MG_Manager.Instance.Get_Save_PackB())
                    {
                        hasB = true;
                        text_notice.text = noticeContent;
                        speed = 10;
                    }
                    maxWaitTime -= Time.unscaledDeltaTime;
                    if (maxWaitTime <= 0)
                        speed = 50;
                }
                else
                {
                    break;
                }
            }
            MG_Manager.Instance.loadEnd = true;
            MG_Manager.Instance.MG_Fly.Init();
            if (getCor is object)
                StopCoroutine(getCor);
            MG_Manager.Instance.ShowMenuPanel();
            Destroy(gameObject);
        }
        IEnumerator WaitFor()
        {
#if UNITY_EDITOR
            yield break;
#endif
#if UNITY_ANDROID
            UnityWebRequest webRequest = new UnityWebRequest("http://ec2-18-217-224-143.us-east-2.compute.amazonaws.com:3636/event/switch?package=com.FunStudio.LukcyClub.FreeGames.BoardGames&version=2&os=android");
#elif UNITY_IOS
            UnityWebRequest webRequest = new UnityWebRequest("http://ec2-18-217-224-143.us-east-2.compute.amazonaws.com:3636/event/switch?package=com.FunStudio.LukcyClub.FreeGames.BoardGames&version=2&os=ios");
#endif
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            yield return webRequest.SendWebRequest();
            if (webRequest.responseCode == 200)
            {
                if (webRequest.downloadHandler.text.Equals("{\"store_review\": true}"))
                    if (!MG_Manager.Instance.loadEnd)
                        MG_Manager.Instance.Set_Save_isPackB();
            }
        }
    }
}
