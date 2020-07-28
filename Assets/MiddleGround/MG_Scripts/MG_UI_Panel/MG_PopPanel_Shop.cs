using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace MiddleGround.UI
{
    public class MG_PopPanel_Shop : MG_UIBase
    {
        public RectTransform rect_Top;
        MG_PopPanel_Shop_Item item0;
        MG_PopPanel_Shop_Item item1;
        MG_PopPanel_Shop_Item item2;
        MG_PopPanel_Shop_Item item3;
        MG_PopPanel_Shop_Item item4;

        public Button btn_back;
        public Button btn_get0;
        public Button btn_get1;
        public Button btn_get2;
        public Button btn_get3;
        public Button btn_get4;

        SpriteAtlas shopAtlas;
        Sprite sp_cash;
        Sprite sp_diamond;
        Sprite sp_amazon;
        Sprite sp_sss;
        Sprite sp_fruit;
        Sprite sp_amazon50;
        Sprite sp_amazon100;
        Sprite sp_amazon500;
        Sprite sp_amazon1000;
        Sprite sp_amazon10000;
        protected override void Awake()
        {
            base.Awake();
            float lwr = Screen.height / Screen.width;
            if (lwr > 4 / 3f)
                rect_Top.anchoredPosition = new Vector2(0, 0);
            btn_back.onClick.AddListener(OnBackButtonClick);

            shopAtlas = MG_UIManager.Instance.GetSpriteAtlas((int)MG_PopPanelType.ShopPanel);
            sp_cash = shopAtlas.GetSprite("MG_Sprite_Shop_Cash");
            sp_diamond = shopAtlas.GetSprite("MG_Sprite_Shop_Diamond");
            sp_amazon = shopAtlas.GetSprite("MG_Sprite_Shop_Amazon");
            sp_sss = shopAtlas.GetSprite("MG_Sprite_Shop_SSS");
            sp_fruit = shopAtlas.GetSprite("MG_Sprite_Shop_Fruits");
            sp_amazon50 = shopAtlas.GetSprite("MG_Sprite_Shop_Paypal50");
            sp_amazon100 = shopAtlas.GetSprite("MG_Sprite_Shop_Paypal100");
            sp_amazon500 = shopAtlas.GetSprite("MG_Sprite_Shop_AmazonCard500");
            sp_amazon1000 = shopAtlas.GetSprite("MG_Sprite_Shop_AmazonCard1000");
            sp_amazon10000 = shopAtlas.GetSprite("MG_Sprite_Shop_AmazonCard10000");

            item0 = btn_get0.GetComponent<MG_PopPanel_Shop_Item>();
            item1 = btn_get1.GetComponent<MG_PopPanel_Shop_Item>();
            item2 = btn_get2.GetComponent<MG_PopPanel_Shop_Item>();
            item3 = btn_get3.GetComponent<MG_PopPanel_Shop_Item>();
            item4 = btn_get4.GetComponent<MG_PopPanel_Shop_Item>();

            item0.Init(sp_diamond, sp_amazon50, "5M Gems=$50");
            item1.Init(sp_cash, sp_amazon100, "100 Dollar=$100");
            item2.Init(sp_amazon, sp_amazon500, "100 Cards=$500");
            item3.Init(sp_sss, sp_amazon1000, "150 Lukcy=$1000");
            item4.Init(sp_fruit, sp_amazon10000, "200 Fruits=$10,000");

            btn_get0.onClick.AddListener(OnGetButtonClick);
            btn_get1.onClick.AddListener(OnGetButtonClick);
            btn_get2.onClick.AddListener(OnGetButtonClick);
            btn_get3.onClick.AddListener(OnGetButtonClick);
            btn_get4.onClick.AddListener(OnGetButtonClick);
        }
        void OnBackButtonClick()
        {
            MG_Manager.Play_ButtonClick();
            MG_UIManager.Instance.ClosePopPanelAsync(MG_PopPanelType.ShopPanel);
        }
        void OnGetButtonClick()
        {
            MG_Manager.Play_ButtonClick();
            MG_Manager.Instance.Show_PopTipsPanel("Not enough money to exchange.");
        }
        public override IEnumerator OnEnter()
        {
            yield return null;
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
            item0.RefreshProgress(MG_Manager.Instance.Get_Save_Diamond().ToString(), "5000000");
            item1.RefreshProgress(MG_Manager.Get_CashShowText(MG_Manager.Instance.Get_Save_Cash()), "100");
            item2.RefreshProgress(MG_Manager.Instance.Get_Save_Amazon().ToString(), "100");
            item3.RefreshProgress(MG_Manager.Instance.Get_Save_777().ToString(), "150");
            item4.RefreshProgress(MG_Manager.Instance.Get_Save_Fruits().ToString(), "200");
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
    }
}
