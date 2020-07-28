using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MiddleGround.UI
{
    public class MG_PopPanel_Shop_Item : MonoBehaviour
    {
        public Image img_icon;
        public Image img_rewardIcon;
        public Text text_des;
        public Text text_progress;
        public Slider slider_progress;
        public void Init(Sprite icon,Sprite rewardicon,string des)
        {
            img_icon.sprite = icon;
            img_rewardIcon.sprite = rewardicon;
            text_des.text = des;
        }
        public void RefreshProgress(string currentNum,string targetNum)
        {
            text_progress.text = "<color=#FF780E>" + currentNum + "</color>/" + targetNum;
            slider_progress.value = float.Parse(currentNum) / float.Parse(targetNum);
        }
    }
}
