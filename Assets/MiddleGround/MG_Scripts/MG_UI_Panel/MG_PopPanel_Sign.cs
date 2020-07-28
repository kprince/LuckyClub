using MiddleGround.Save;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace MiddleGround.UI
{
    public class MG_PopPanel_Sign : MG_UIBase
    {
        public List<MG_PopPanel_Sign_Day> list_alldays = new List<MG_PopPanel_Sign_Day>();
        readonly int[] rewards = new int[7] { 2000, 100, 2000, 2000, 100, 100, 100 };
        readonly bool[] isGold = new bool[7] { true, false, true, true, false, false, false };
        readonly float[] rewardmutiples = new float[7] { 3, 1.5f, 1.5f, 5, 1.5f, 1.5f, 5 };
        Sprite sp_gold;
        Sprite sp_cash;
        Sprite sp_reawrd7;
        Sprite sp_today7Bg;
        Sprite sp_tomorrow7Bg;
        Sprite sp_scratchTicket;
        Sprite sp_yestodayBg;
        Sprite sp_todayBg;
        Sprite sp_tomorrowBg;
        SpriteAtlas signSA;
        public Button btn_Sign;
        public Button btn_Nothanks;
        public Button btn_Close;
        public CanvasGroup cg_nothanks;
        Image img_close;
        protected override void Awake()
        {
            base.Awake();
            signSA = MG_UIManager.Instance.GetSpriteAtlas((int)MG_PopPanelType.SignPanel);
            sp_gold = signSA.GetSprite("MG_Sprite_Sign_Gold");
            sp_cash = signSA.GetSprite("MG_Sprite_Sign_Cash");
            sp_scratchTicket = signSA.GetSprite("MG_Sprite_Sign_ScratchTicket");
            sp_yestodayBg = signSA.GetSprite("MG_Sprite_Sign_Yestoday");
            sp_todayBg = signSA.GetSprite("MG_Sprite_Sign_Today");
            sp_tomorrowBg = signSA.GetSprite("MG_Sprite_Sign_Tomorrow");
            sp_reawrd7 = signSA.GetSprite("MG_Sprite_Sign_DayFinalReward");
            sp_today7Bg = signSA.GetSprite("MG_Sprite_Sign_Today7");
            sp_tomorrow7Bg = signSA.GetSprite("MG_Sprite_Sign_Tomorrow7");
            btn_Sign.onClick.AddListener(OnSignButtonClick);
            btn_Nothanks.onClick.AddListener(OnNothanksClick);
            btn_Close.onClick.AddListener(OnCloseClick);
            img_close = btn_Close.image;
        }
        int clickTime = 0;
        void OnSignButtonClick()
        {
            MG_Manager.Play_ButtonClick();
            if (MG_Manager.Instance.Get_Save_WetherSign())
            {
                clickTime++;
                MG_Manager.ShowRV(OnAdCallback, clickTime, "signin ad");
            }
            else
            {
                MG_UIManager.Instance.ClosePopPanelAsync(MG_PopPanelType.SignPanel);
                MG_Manager.Instance.Show_PopTipsPanel("You have signed today.");
            }
        }
        void OnAdCallback()
        {
            clickTime = 0;
            int day = MG_Manager.Instance.Get_Save_NextSignDay();
            MG_UIManager.Instance.ClosePopPanelAsync(MG_PopPanelType.SignPanel);
            MG_SaveManager.LastSignDate = System.DateTime.Now;
            if (day < 7)
            {
                if (isGold[day])
                    MG_Manager.Instance.Show_MostRewardPanel(MG_RewardPanelType.FreeMutipleClaim, MG_RewardType.Gold, rewards[day], rewardmutiples[day]);
                else
                    MG_Manager.Instance.Show_CashRewardPanel(MG_RewardPanelType.FreeMutipleClaim, rewards[day], rewardmutiples[day]);
            }
            else
                MG_Manager.Instance.Show_MostRewardPanel(MG_RewardPanelType.FreeMutipleClaim, MG_RewardType.ScratchTicket, 1, 5);
            MG_UIManager.Instance.UpdateSignRP();
            day %= 7;
            MG_SaveManager.SignState = MG_SaveManager.SignState.Remove(day, 1).Insert(day, "1");
        }
        void OnNothanksClick()
        {
            MG_Manager.Play_ButtonClick();
            if (MG_Manager.Instance.Get_Save_WetherSign())
            {
                int day = MG_Manager.Instance.Get_Save_NextSignDay();
                MG_UIManager.Instance.ClosePopPanelAsync(MG_PopPanelType.SignPanel);
                MG_SaveManager.LastSignDate = System.DateTime.Now;
                if (day < 7)
                {
                    if (isGold[day])
                        MG_Manager.Instance.Show_MostRewardPanel(MG_RewardPanelType.FreeMutipleClaim, MG_RewardType.Gold, rewards[day]);
                    else
                        MG_Manager.Instance.Show_CashRewardPanel(MG_RewardPanelType.FreeMutipleClaim, rewards[day], 1);
                }
                else
                    MG_Manager.Instance.Show_MostRewardPanel(MG_RewardPanelType.FreeMutipleClaim, MG_RewardType.ScratchTicket, 1, 1);
                MG_UIManager.Instance.UpdateSignRP();
                day %= 7;
                MG_SaveManager.SignState = MG_SaveManager.SignState.Remove(day, 1).Insert(day, "0");
            }
            else
            {
                MG_UIManager.Instance.ClosePopPanelAsync(MG_PopPanelType.SignPanel);
                MG_Manager.Instance.Show_PopTipsPanel("You have signed today.");
            }
        }
        void OnCloseClick()
        {
            MG_UIManager.Instance.ClosePopPanelAsync(MG_PopPanelType.SignPanel);
        }
        public override IEnumerator OnEnter()
        {
            clickTime = 0;
            bool canSign = MG_Manager.Instance.Get_Save_WetherSign();
            if (list_alldays.Count != rewards.Length)
                Debug.LogError("Set MG_Sign Reward Error : exist is not match config.");
            else
            {
                int lastSignDay = MG_Manager.Instance.Get_Save_NextSignDay();
                string signState = MG_SaveManager.SignState;
                bool changeScratchTicket = false;
                if (lastSignDay >= 7)
                {
                    changeScratchTicket = true;
                    lastSignDay %= 7;
                }
                for (int i = 0; i < 7; i++)
                {
                    Sprite bg;
                    Sprite reward;
                    string numDes;
                    char state = signState[i];
                    bool getAd = state == '1';
                    bool hasGet;
                    if (i == lastSignDay && canSign)
                    {
                        bg = i == 6 ? sp_today7Bg : sp_todayBg;
                        hasGet = false;
                    }
                    else if (i < lastSignDay)
                    {
                        bg = sp_yestodayBg;
                        hasGet = true;
                    }
                    else
                    {
                        bg = i == 6 ? sp_tomorrow7Bg : sp_tomorrowBg;
                        hasGet = false;
                    }

                    if (changeScratchTicket)
                    {
                        numDes = i < lastSignDay ? (getAd ? "5" : "1") : "?";
                        reward = sp_scratchTicket;
                    }
                    else
                    {
                        reward = (i == 6 ? sp_reawrd7 : (isGold[i] ? sp_gold : sp_cash));
                        if (isGold[i])
                            numDes = i < lastSignDay && getAd ? (rewards[i] * rewardmutiples[i]).ToString() : rewards[i].ToString();
                        else
                            numDes = i < lastSignDay ? (MG_Manager.Get_CashShowText(getAd ? (int)(rewards[i] * rewardmutiples[i]) : (int)rewards[i])) : "?";
                    }
                    list_alldays[i].SetDay(i + 1, hasGet, bg, reward, numDes);

                }
            }
            if (canSign)
            {
                cg_nothanks.alpha = 0;
                cg_nothanks.blocksRaycasts = false;
                img_close.color = Color.clear;
                img_close.raycastTarget = false;
            }
            else
            {
                cg_nothanks.alpha = 1;
                cg_nothanks.blocksRaycasts = true;
                img_close.color = Color.white;
                img_close.raycastTarget = true;
            }

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

            if (canSign)
                StartCoroutine("WaitShowNothanks");
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
            MG_UIManager.Instance.UpdateWheelRP();
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
            StopCoroutine("WaitShowNothanks");
        }

        public override void OnPause()
        {
        }

        public override void OnResume()
        {
        }
        IEnumerator WaitShowNothanks()
        {
            if (cg_nothanks.alpha > 0 || img_close.color.a > 0)
                yield break;
            yield return new WaitForSeconds(Time.timeScale);
            while (cg_nothanks.alpha < 1)
            {
                yield return null;
                float offset = Time.unscaledDeltaTime * 2;
                cg_nothanks.alpha += offset;
                img_close.color += Color.white * offset;
            }
            cg_nothanks.alpha = 1;
            cg_nothanks.blocksRaycasts = true;
            img_close.color = Color.white;
            img_close.raycastTarget = true;
        }
    }
}
