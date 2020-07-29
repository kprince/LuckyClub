using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MiddleGround.UI
{
    public class MG_PopPanel_Offerwall : MG_UIBase
    {
        public Button btn_is;
        public Transform trans_Rot;
        public Button btn_close;
        Image img_close;
        protected override void Awake()
        {
            base.Awake();
            img_close = btn_close.image;
            btn_is.onClick.AddListener(OnIsClick);
            btn_close.onClick.AddListener(() => { MG_UIManager.Instance.ClosePopPanelAsync(MG_PopPanelType.OfferwallPanel); });
        }
        void OnIsClick()
        {
            MG_Manager.Play_ButtonClick();
            if (Ads._instance.ShowOfferwallAd())
            {
                MG_UIManager.Instance.ClosePopPanelAsync(MG_PopPanelType.OfferwallPanel);
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
