using MiddleGround;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowOfferwallAds : MonoBehaviour
{
    int rewardOfferwall = 0;
    int rewardCount = 0;
    void OnEnable()
    {
        IronSourceEvents.onOfferwallClosedEvent += OfferwallClosedEvent;
        IronSourceEvents.onOfferwallOpenedEvent += OfferwallOpenedEvent;
        IronSourceEvents.onOfferwallShowFailedEvent += OfferwallShowFailedEvent;
        IronSourceEvents.onOfferwallAdCreditedEvent += OfferwallAdCreditedEvent;
        IronSourceEvents.onGetOfferwallCreditsFailedEvent += GetOfferwallCreditsFailedEvent;
        IronSourceEvents.onOfferwallAvailableEvent += OfferwallAvailableEvent;
    }
    void OfferwallAvailableEvent(bool canShowOfferwall)
    {
        MG_Manager.Instance.canShowOfferwall = canShowOfferwall;
    }
    void OfferwallOpenedEvent()
    {
        rewardOfferwall = 0;
        rewardCount = 0;
    }
    void OfferwallShowFailedEvent(IronSourceError error)
    {
        Debug.LogError("luckyclub--------show offerwall failed : " + error);
    }
    void OfferwallAdCreditedEvent(Dictionary<string, object> dict)
    {
        //Debug.Log("I got OfferwallAdCreditedEvent, current credits = "+dict["credits"] + "totalCredits = " + dict["totalCredits"]);
        rewardOfferwall += (int)float.Parse(dict["credits"].ToString());
        rewardCount++;
    }
    void GetOfferwallCreditsFailedEvent(IronSourceError error)
    {
        Debug.LogError("luckyclub--------get offerwall credits failed : " + error);
    }
    void OfferwallClosedEvent()
    {
        if (rewardOfferwall > 0)
        {
            MG_Manager.Instance.Show_MostRewardPanel(MG_RewardPanelType.FreeClaim, MG_RewardType.Gold, rewardOfferwall);
            MG_Manager.Instance.SendAdjustOfferwallEvent(rewardCount, rewardOfferwall);
        }
    }
}
