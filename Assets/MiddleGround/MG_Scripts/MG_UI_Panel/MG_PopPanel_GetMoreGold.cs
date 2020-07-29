using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MiddleGround.UI
{
    public class MG_PopPanel_GetMoreGold : MG_UIBase
    {
        public Button btn_ad;
        public Button btn_is;
        public Button btn_close;
        public Transform trans_Icon;
        public Transform trans_Rot;
        Image img_close;
        protected override void Awake()
        {
            base.Awake();
            img_close = btn_close.image;
            btn_ad.onClick.AddListener(OnAdClick);
            btn_is.onClick.AddListener(OnIsClick);
            btn_close.onClick.AddListener(() => { MG_UIManager.Instance.ClosePopPanelAsync(MG_PopPanelType.GetMoreGoldPanel); });
        }
        int clickTime = 0;
        void OnAdClick()
        {
            MG_Manager.Play_ButtonClick();
            clickTime++;
            MG_Manager.ShowRV(AdCallback, clickTime, "Get more gold");
        }
        void AdCallback()
        {
            clickTime = 0;
            MG_UIManager.Instance.ClosePopPanelAsync(MG_PopPanelType.GetMoreGoldPanel);
            MG_Manager.Instance.Add_Save_Gold(1000);
            MG_UIManager.Instance.FlyEffectTo_MenuTarget(trans_Icon.position, MG_MenuFlyTarget.OneGold, 1000);
        }
        void OnIsClick()
        {
            MG_Manager.Play_ButtonClick();
            if (Ads._instance.ShowOfferwallAd())
            {
                MG_UIManager.Instance.ClosePopPanelAsync(MG_PopPanelType.GetMoreGoldPanel);
            }
        }
        public override IEnumerator OnEnter()
        {
            if (!Ads._instance.CheckOfferwall())
            {
                trans_Rot.gameObject.SetActive(true);
                StartCoroutine("Roate");
            }
            else
                trans_Rot.gameObject.SetActive(false);
            img_close.color = Color.clear;
            img_close.raycastTarget = false;
            StartCoroutine("WaitShowNothanks");

            Transform transAll = transform.GetChild(1);
            transAll.localScale = new Vector3(0.8f, 0.8f, 1);
            canvasGroup.alpha = 0.8f;
            canvasGroup.blocksRaycasts = true;
            while (transAll.localScale.x < 1)
            {
                yield return null;
                float addValue = Time.unscaledDeltaTime * 2;
                transAll.localScale += new Vector3(addValue, addValue);
                canvasGroup.alpha += addValue;
            }
            transAll.localScale = Vector3.one;
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
        }

        public override IEnumerator OnExit()
        {
            clickTime = 0;
            Transform transAll = transform.GetChild(1);
            canvasGroup.interactable = false;
            while (transAll.localScale.x > 0.8f)
            {
                yield return null;
                float addValue = Time.unscaledDeltaTime * 2;
                transAll.localScale -= new Vector3(addValue, addValue);
                canvasGroup.alpha -= addValue;
            }
            transAll.localScale = new Vector3(0.8f, 0.8f, 1);
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
            StopCoroutine("Roate");
            StopCoroutine("WaitShowNothanks");
        }

        public override void OnPause()
        {
        }

        public override void OnResume()
        {
        }
        IEnumerator Roate()
        {
            while (true)
            {
                yield return null;
                if (Ads._instance.CheckOfferwall())
                {
                    trans_Rot.gameObject.SetActive(false);
                    yield break;
                }
                else
                    trans_Rot.Rotate(Vector3.forward * Time.unscaledDeltaTime * 400);
            }
        }
        IEnumerator WaitShowNothanks()
        {
            if (img_close.color.a > 0)
                yield break;
            yield return new WaitForSeconds(Time.timeScale);
            float progress = 0;
            while (progress < 1)
            {
                yield return null;
                progress += Time.unscaledDeltaTime * 2;
                img_close.color = new Color(1, 1, 1, progress);
            }
            img_close.color = Color.white;
            img_close.raycastTarget = true;
        }
    }
}
