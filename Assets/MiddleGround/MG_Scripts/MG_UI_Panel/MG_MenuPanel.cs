using MiddleGround.Save;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace MiddleGround.UI
{
    public class MG_MenuPanel : MG_UIBase
    {
        public Button btn_Setting;
        public Button btn_Wheel;
        public Button btn_Sign;
        public Button btn_Scratch;
        public Button btn_Dice;
        public Button btn_Slots;
        public Button btn_shop;
        public Button btn_Gold;
        public Button btn_Cash;
        public Button btn_SpecialToken;

        public Text text_Gold;
        public Text text_Cash;
        public Text text_ScratchTicketNum;
        public Text text_SpecialToken;

        public Image img_SpecialToken;
        public Image img_CashIcon;
        public GameObject go_SpecialToken;

        public RectTransform rect_Top;
        public Transform trans_guidMask;
        public Transform trans_guidBase;
        public Image img_guidBG;
        public Image img_guidIcon;
        public Text text_guidDes;

        public GameObject go_wheelRP;
        public GameObject go_signRP;
        public GameObject go_scratchRP;
        public GameObject go_cashoutTips_cash;
        public GameObject go_cashoutTips_special;

        Sprite sp_scratchOn;
        Sprite sp_scratchOff;
        Sprite sp_diceOn;
        Sprite sp_diceOff;
        Sprite sp_slotsOn;
        Sprite sp_slotsOff;
        Image img_scratchbutton;
        Image img_dicebutton;
        Image img_slotsbutton;

        SpriteAtlas MenuAtlas;
        protected override void Awake()
        {
            base.Awake();
            img_scratchbutton = btn_Scratch.image;
            img_dicebutton = btn_Dice.image;
            img_slotsbutton = btn_Slots.image;

            btn_Setting.onClick.AddListener(OnSettingButtonClick);
            btn_Wheel.onClick.AddListener(OnWheelButtonClick);
            btn_Sign.onClick.AddListener(OnSignButtonClick);
            btn_Scratch.onClick.AddListener(OnScratchButtonClick);
            btn_Dice.onClick.AddListener(OnDiceButtonClick);
            btn_Slots.onClick.AddListener(OnSlotsButtonClick);
            btn_shop.onClick.AddListener(OnShopButtonClick);
            btn_Gold.onClick.AddListener(OnGoldButtonClick);
            btn_Cash.onClick.AddListener(OnCashButtonClick);
            btn_SpecialToken.onClick.AddListener(OnSpecialButtonClick);
            trans_guidMask.GetComponent<Button>().onClick.AddListener(OnMaskButtonClick);


            float lwr = Screen.height / Screen.width;
            if (lwr > 4 / 3f)
            {
                rect_Top.anchoredPosition = new Vector2(0, -87);
                f_guidY = 600;
            }
            else
                f_guidY = 513;
            trans_guidBase.localPosition = new Vector2(0, f_guidY);
            packB = MG_Manager.Instance.Get_Save_PackB();

            MenuAtlas = MG_UIManager.Instance.GetMenuSpriteAtlas();

            sp_diceOff = MenuAtlas.GetSprite("MG_Sprite_Menu_DiceOff");
            sp_diceOn = MenuAtlas.GetSprite("MG_Sprite_Menu_DiceOn");
            sp_scratchOff = MenuAtlas.GetSprite("MG_Sprite_Menu_ScratchOff");
            sp_scratchOn = MenuAtlas.GetSprite("MG_Sprite_Menu_ScratchOn");
            sp_slotsOff = MenuAtlas.GetSprite("MG_Sprite_Menu_SlotsOff");
            sp_slotsOn = MenuAtlas.GetSprite("MG_Sprite_Menu_SlotsOn");

            sp_ScratchToken = MenuAtlas.GetSprite("MG_Sprite_Menu_ScratchToken");
            sp_SlotsToken = MenuAtlas.GetSprite("MG_Sprite_Menu_SlotsToken");
            sp_DiceToken = MenuAtlas.GetSprite("MG_Sprite_Menu_DiceToken");
            img_CashIcon.sprite = MenuAtlas.GetSprite("MG_Sprite_Menu_Cash" + (packB ? "B" : "A"));
            packB = MG_Manager.Instance.Get_Save_PackB();

            go_cashoutTips_cash.SetActive(packB);
            go_cashoutTips_special.SetActive(packB);
            btn_shop.gameObject.SetActive(packB);

            dic_flytarget_transform.Add((int)MG_MenuFlyTarget.OneGold, btn_Gold.transform);
            dic_flytarget_transform.Add((int)MG_MenuFlyTarget.Cash, btn_Cash.transform);
            dic_flytarget_transform.Add((int)MG_MenuFlyTarget.Amazon, btn_SpecialToken.transform);
            dic_flytarget_transform.Add((int)MG_MenuFlyTarget.ScratchTicket, btn_Scratch.transform);
            dic_flytarget_transform.Add((int)MG_MenuFlyTarget.SlotsSpecialToken, btn_SpecialToken.transform);
            dic_flytarget_transform.Add((int)MG_MenuFlyTarget.Scratch, btn_Scratch.transform);
            dic_flytarget_transform.Add((int)MG_MenuFlyTarget.Slots, btn_Slots.transform);
            dic_flytarget_transform.Add((int)MG_MenuFlyTarget.SSS, btn_SpecialToken.transform);
            dic_flytarget_transform.Add((int)MG_MenuFlyTarget.Diamond, btn_SpecialToken.transform);
            go_scratchRP.SetActive(false);
        }
        bool packB = false;
        void OnSettingButtonClick()
        {
            MG_Manager.Play_ButtonClick();
            if (!MG_Manager.Instance.canChangeGame) return;
            MG_UIManager.Instance.ShowPopPanelAsync(MG_PopPanelType.SettingPanel);
        }
        void OnWheelButtonClick()
        {
            MG_Manager.Play_ButtonClick();
            if (!MG_Manager.Instance.canChangeGame) return;
            MG_UIManager.Instance.ShowPopPanelAsync(MG_PopPanelType.WheelPanel);
        }
        void OnSignButtonClick()
        {
            MG_Manager.Play_ButtonClick();
            if (!MG_Manager.Instance.canChangeGame) return;
            MG_UIManager.Instance.ShowPopPanelAsync(MG_PopPanelType.SignPanel);
        }
        public void OnScratchButtonClick()
        {
            MG_Manager.Play_ButtonClick();
            if (!MG_Manager.Instance.canChangeGame) return;
            if (go_scratchRP.activeSelf)
                go_scratchRP.SetActive(false);
            MG_UIManager.Instance.ShowGamePanel(MG_GamePanelType.ScratchPanel);
            UpdateBottomButtonState(MG_GamePanelType.ScratchPanel);
            SetSpecialToken(MG_SpecialTokenType.ScratchToken);
        }
        public void OnDiceButtonClick()
        {
            MG_Manager.Play_ButtonClick();
            if (!MG_Manager.Instance.canChangeGame) return;
            MG_UIManager.Instance.ShowGamePanel(MG_GamePanelType.DicePanel);
            UpdateBottomButtonState(MG_GamePanelType.DicePanel);
            SetSpecialToken(MG_SpecialTokenType.DiceToken);
        }
        public void OnSlotsButtonClick()
        {
            MG_Manager.Play_ButtonClick();
            if (!MG_Manager.Instance.canChangeGame) return;
            MG_UIManager.Instance.ShowGamePanel(MG_GamePanelType.SlotsPanel);
            UpdateBottomButtonState(MG_GamePanelType.SlotsPanel);
            SetSpecialToken(MG_SpecialTokenType.SlotsToken);
        }
        void OnShopButtonClick()
        {
            MG_Manager.Play_ButtonClick();
            if (!MG_Manager.Instance.canChangeGame) return;
            MG_UIManager.Instance.ShowPopPanelAsync(MG_PopPanelType.ShopPanel);
        }
        void OnGoldButtonClick()
        {
            MG_Manager.Play_ButtonClick();
            OnMaskButtonClick();
            if (MG_Manager.Instance.isGuid) return;
            if (!MG_Manager.Instance.canChangeGame) return;
            if (packB)
                MG_UIManager.Instance.ShowPopPanelAsync(MG_PopPanelType.ShopPanel);
        }
        void OnCashButtonClick()
        {
            MG_Manager.Play_ButtonClick();
            OnMaskButtonClick();
            if (MG_Manager.Instance.isGuid) return;
            if (!MG_Manager.Instance.canChangeGame) return;
            if (packB)
                MG_UIManager.Instance.ShowPopPanelAsync(MG_PopPanelType.ShopPanel);
        }
        void OnSpecialButtonClick()
        {
            MG_Manager.Play_ButtonClick();
            OnMaskButtonClick();
            if (MG_Manager.Instance.isGuid) return;
            if (!MG_Manager.Instance.canChangeGame) return;
            if (packB)
                MG_UIManager.Instance.ShowPopPanelAsync(MG_PopPanelType.ShopPanel);
        }
        void UpdateAllContent()
        {
            UpdateGoldText();
            UpdateCashText();
            UpdateScratchTicketText();
            UpdateSpecialTokenText();
            UpdateWheelRP();
            UpdateSignRP();
        }
        Sprite sp_ScratchToken = null;
        Sprite sp_SlotsToken = null;
        Sprite sp_DiceToken = null;
        void SetSpecialToken(MG_SpecialTokenType _SpecialTokenType)
        {
            switch (_SpecialTokenType)
            {
                case MG_SpecialTokenType.ScratchToken:
                    if(!packB)
                        go_SpecialToken.SetActive(false);
                    else
                    {
                        go_SpecialToken.SetActive(true);
                        img_SpecialToken.sprite = sp_ScratchToken;
                        text_SpecialToken.text = MG_Manager.Instance.Get_Save_777().ToString();
                    }
                    break;
                case MG_SpecialTokenType.SlotsToken:
                    img_SpecialToken.sprite = sp_SlotsToken;
                    text_SpecialToken.text = MG_Manager.Instance.Get_Save_Diamond().ToString();
                    go_SpecialToken.SetActive(true);
                    break;
                case MG_SpecialTokenType.DiceToken:
                    if (!packB)
                        go_SpecialToken.SetActive(false);
                    else
                    {
                        go_SpecialToken.SetActive(true);
                        img_SpecialToken.sprite = sp_DiceToken;
                        text_SpecialToken.text = MG_Manager.Instance.Get_Save_Amazon().ToString();
                    }
                    break;
                case MG_SpecialTokenType.Null:
                    break;
            }
        }
        public void FlyToTarget(Vector3 startPos,MG_MenuFlyTarget flyTarget,int num)
        {
            MG_Manager.Instance.MG_Fly.FlyToTarget(startPos, GetFlyTargetPos(flyTarget), num, flyTarget, FlyToTargetCallback);
        }
        public void UpdateCashText()
        {
            text_Cash.text = MG_Manager.Get_CashShowText(MG_Manager.Instance.Get_Save_Cash());
        }
        public void UpdateGoldText()
        {
            text_Gold.text = MG_Manager.Instance.Get_Save_Gold().ToString();
        }
        public void UpdateScratchTicketText()
        {
            text_ScratchTicketNum.text = MG_Manager.Instance.Get_Save_ScratchTicket().ToString();
        }
        public void UpdateWheelRP()
        {
            go_wheelRP.SetActive(MG_Manager.Instance.Get_Save_WheelTickets() > 0);
        }
        public void UpdateSignRP()
        {
            go_signRP.SetActive(MG_Manager.Instance.Get_Save_WetherSign());
        }
        public void UpdateSpecialTokenText()
        {
            int panelIndex = MG_SaveManager.Current_GamePanel;
            if (panelIndex == (int)MG_GamePanelType.ScratchPanel)
            {
                SetSpecialToken(MG_SpecialTokenType.ScratchToken);
            }
            else if (panelIndex == (int)MG_GamePanelType.DicePanel)
            {
                SetSpecialToken(MG_SpecialTokenType.DiceToken);
            }
            else if (panelIndex == (int)MG_GamePanelType.SlotsPanel)
            {
                SetSpecialToken(MG_SpecialTokenType.SlotsToken);
            }
        }
        void UpdateBottomButtonState(MG_GamePanelType clickbuttonType)
        {
            switch (clickbuttonType)
            {
                case MG_GamePanelType.DicePanel:
                    img_scratchbutton.sprite = sp_scratchOff;
                    img_dicebutton.sprite = sp_diceOn;
                    img_slotsbutton.sprite = sp_slotsOff;
                    break;
                case MG_GamePanelType.ScratchPanel:
                    img_scratchbutton.sprite = sp_scratchOn;
                    img_dicebutton.sprite = sp_diceOff;
                    img_slotsbutton.sprite = sp_slotsOff;
                    break;
                case MG_GamePanelType.SlotsPanel:
                    img_scratchbutton.sprite = sp_scratchOff;
                    img_dicebutton.sprite = sp_diceOff;
                    img_slotsbutton.sprite = sp_slotsOn;
                    break;
            }
        }
        public readonly Dictionary<int, Transform> dic_flytarget_transform = new Dictionary<int, Transform>();
        Vector3 GetFlyTargetPos(MG_MenuFlyTarget _flyTarget)
        {
            if(dic_flytarget_transform.TryGetValue((int)_flyTarget,out Transform trans_Target))
            {
                return trans_Target.position;
            }
            return Vector3.zero;
        }
        void FlyToTargetCallback(MG_MenuFlyTarget _flyTarget)
        {
            switch (_flyTarget)
            {
                case MG_MenuFlyTarget.WheelTicket:
                    MG_UIManager.Instance.UpdateWheelTicketText();
                    return;
                case MG_MenuFlyTarget.Orange:
                case MG_MenuFlyTarget.Cherry:
                case MG_MenuFlyTarget.Watermalen:
                    MG_UIManager.Instance.UpdateSlotsPanel_FruitText();
                    return;
            }
            StopCoroutine("ExpandTarget");
            StartCoroutine("ExpandTarget", _flyTarget);
        }
        IEnumerator ExpandTarget(MG_MenuFlyTarget _flyTarget)
        {
            if (!dic_flytarget_transform.TryGetValue((int)_flyTarget, out Transform tempTrans))
                yield break;
            bool toBiger = true;
            while (true)
            {
                yield return null;
                if (toBiger)
                {
                    tempTrans.localScale += Vector3.one * Time.unscaledDeltaTime * 3;
                    if (tempTrans.localScale.x >= 1.3f)
                    {
                        toBiger = false;
                        switch (_flyTarget)
                        {
                            case MG_MenuFlyTarget.OneGold:
                                UpdateGoldText();
                                break;
                            case MG_MenuFlyTarget.Cash:
                                UpdateCashText();
                                break;
                            case MG_MenuFlyTarget.Diamond:
                                UpdateSpecialTokenText();
                                break;
                            case MG_MenuFlyTarget.Scratch:
                            case MG_MenuFlyTarget.ScratchTicket:
                                MG_UIManager.Instance.Update_ScratchTicketText();
                                break;
                            default:
                                UpdateSpecialTokenText();
                                break;
                        }
                    }
                }
                else
                {
                    tempTrans.localScale -= Vector3.one * Time.unscaledDeltaTime * 3;
                    if (tempTrans.localScale.x <= 1f)
                        break;
                }
            }
            if (_flyTarget == MG_MenuFlyTarget.ScratchTicket && MG_SaveManager.Current_GamePanel != (int)MG_GamePanelType.ScratchPanel)
            {
                if (!go_scratchRP.activeSelf)
                    go_scratchRP.SetActive(true);
            }
            yield return null;
            tempTrans.localScale = Vector3.one;
        }
        public override IEnumerator OnEnter()
        {
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
            UpdateAllContent();
            if (MG_SaveManager.FirstCome)
            {
                MG_Manager.Instance.Random_DiceOrExtraReward(MG_PopRewardPanel_RewardType.Extra);
            }
            UpdateBottomButtonState((MG_GamePanelType)MG_SaveManager.Current_GamePanel);
            yield return null;
        }

        public override IEnumerator OnExit()
        {
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
            yield return null;
        }

        public override void OnPause()
        {

        }

        public override void OnResume()
        {
        }
        public void CheckGuid()
        {
            if (MG_Manager.Instance.next_GuidType != MG_Guid_Type.Null)
            {
                MG_Manager.Instance.isGuid = true;
                if (!MG_Manager.Instance.Get_Save_PackB())
                {
                    MG_Manager.Instance.isGuid = false;
                    return;
                }
                switch (MG_Manager.Instance.next_GuidType)
                {
                    case MG_Guid_Type.DiceGuid:
                        trans_guidMask.gameObject.SetActive(true);
                        trans_guidMask.SetParent(rect_Top);
                        trans_guidMask.SetAsLastSibling();
                        btn_Cash.transform.SetAsLastSibling();
                        trans_guidBase.localScale = new Vector3(-1, 1, 1);
                        text_guidDes.transform.localScale = new Vector3(-1, 1, 1);
                        img_guidIcon.transform.localScale = new Vector3(-1, 1, 1);
                        img_guidIcon.sprite = MenuAtlas.GetSprite(str_CashoutSP_name);
                        img_guidBG.sprite = MenuAtlas.GetSprite(str_BlueBgSP_name);
                        text_guidDes.text = str_guidCashout;
                        MG_SaveManager.GuidDice = true;
                        StartCoroutine(WaitForClickDiceGuid());
                        break;
                    case MG_Guid_Type.ScratchGuid:
                        trans_guidMask.gameObject.SetActive(true);
                        trans_guidMask.SetParent(rect_Top);
                        trans_guidMask.SetAsLastSibling();
                        btn_SpecialToken.transform.SetAsLastSibling();
                        trans_guidBase.localScale = Vector3.one;
                        text_guidDes.transform.localScale = Vector3.one;
                        img_guidIcon.transform.localScale = Vector3.one;
                        img_guidIcon.sprite = MenuAtlas.GetSprite(str_GiftSP_name);
                        img_guidBG.sprite = MenuAtlas.GetSprite(str_BlueBgSP_name);
                        text_guidDes.text = str_guid7;
                        MG_SaveManager.GuidScratch = true;
                        StartCoroutine(WaitForClickScratchGuid());
                        break;
                    case MG_Guid_Type.SlotsGuid:
                        trans_guidMask.gameObject.SetActive(true);
                        trans_guidMask.SetParent(rect_Top);
                        trans_guidMask.SetAsLastSibling();
                        btn_SpecialToken.transform.SetAsLastSibling();
                        trans_guidBase.localScale = Vector3.one;
                        text_guidDes.transform.localScale = Vector3.one;
                        img_guidIcon.transform.localScale = Vector3.one;
                        img_guidIcon.sprite = MenuAtlas.GetSprite(str_GiftSP_name);
                        img_guidBG.sprite = MenuAtlas.GetSprite(str_GreenBgSP_name);
                        text_guidDes.text = str_guidDimond;
                        MG_SaveManager.GuidSlots = true;
                        StartCoroutine(WaitForClickScratchGuid());
                        break;
                }
            }
        }
        const string str_guidCashout = "<size=70><color=#FFF408>You'have won cash!</color></size>\nOnce you get up to specified amount\nyou can cash out with PayPal.";
        float f_guidY = 0;
        const string str_guidAmazon = "<size=70><color=#FFF408>Win the big prize</color></size>\nOnce you meet the requirements,\nyou can get a huge bonus";
        const string str_guid7 = "<size=70><color=#FFF408>Lucky Seven Day</color></size>\nCollect lucky 7 to redeem cash\nrewards";
        const string str_guidDimond = "<size=70><color=#FFF408>Redeem gifts</color></size>\nYou can use coins to redeem\nGift Cards and more!";

        const string str_CashoutSP_name = "MG_Sprite_Menu_Guid_Paypal";
        const string str_AmazonSP_name = "MG_Sprite_Menu_Guid_Amazon";
        const string str_GiftSP_name = "MG_Sprite_Menu_Guid_Cashout";
        const string str_BlueBgSP_name = "MG_Sprite_Menu_Guid_B";
        const string str_GreenBgSP_name = "MG_Sprite_Menu_Guid_G";
        const string str_OrangeBgSP_name = "MG_Sprite_Menu_Guid_O";
        bool hasClickGuid = false;
        bool canClickButton = false;
        IEnumerator WaitForClickDiceGuid()
        {
            hasClickGuid = false;
            canClickButton = false;
            yield return new WaitForSeconds(Time.timeScale);
            canClickButton = true;
            while (true)
            {
                if (hasClickGuid)
                    break;
                yield return null;
            }
            trans_guidMask.SetAsLastSibling();
            btn_SpecialToken.transform.SetAsLastSibling();
            trans_guidBase.localScale = Vector3.one;
            text_guidDes.transform.localScale = Vector3.one;
            img_guidIcon.transform.localScale = Vector3.one;
            img_guidIcon.sprite = MenuAtlas.GetSprite(str_AmazonSP_name);
            img_guidBG.sprite = MenuAtlas.GetSprite(str_OrangeBgSP_name);
            text_guidDes.text = str_guidAmazon;
            hasClickGuid = false;
            canClickButton = false;
            yield return new WaitForSeconds(Time.timeScale);
            canClickButton = true;
            while (true)
            {
                yield return null;
                if (hasClickGuid)
                    break;
            }
            trans_guidMask.SetParent(transform);
            trans_guidMask.gameObject.SetActive(false);
            MG_Manager.Instance.isGuid = false;
            MG_Manager.Instance.next_GuidType = MG_Guid_Type.Null;
        }
        IEnumerator WaitForClickScratchGuid()
        {
            hasClickGuid = false;
            canClickButton = false;
            yield return new WaitForSeconds(Time.timeScale);
            canClickButton = true;
            while (true)
            {
                if (hasClickGuid)
                    break;
                yield return null;
            }
            trans_guidMask.SetParent(transform);
            trans_guidMask.gameObject.SetActive(false);
            MG_Manager.Instance.isGuid = false;
            MG_Manager.Instance.next_GuidType = MG_Guid_Type.Null;
        }
        void OnMaskButtonClick()
        {
            if (canClickButton)
                hasClickGuid = true;
        }
    }
}
