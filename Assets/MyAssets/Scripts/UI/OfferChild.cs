//using DG.Tweening;
//using Do.Scripts.Tools;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Plugins;
//using TMPro;

//public class OfferChild : MonoBehaviour
//{
//   // public PackId packId;
//    public int coin;
//    public int undo;
//    public int fill;
//    public int addHol;
//    public int heart;
//    public TextMeshProUGUI price, salePrice;
//    private void OnEnable()
//    {
//        //price.text = InAppManager.Instance.GetLocalPrice(packId);
//        //if (packId != PackId.WeeklyDeals && packId != PackId.RemoveAds)
//        //{
//        //    salePrice.text = InAppManager.Instance.GetLocalPrice((PackId) ((int) packId + 6));
//        //}
//    }
//    public void OnClickBuy()
//    {
//        if (CheckCantBuy()) return;
       
//        PackId salePack = (packId != PackId.WeeklyDeals && packId != PackId.RemoveAds) ? (PackId)((int)packId + 6) : packId;
//        InAppManager.Instance.BuyItem(salePack, check =>
//        {
//            if (check)
//            {
//                GameUtils.Iap_Count++;
//                if (undo > 0)
//                {
                   
//                }
//                if (fill > 0)
//                {
                  
//                }
//                if (fill > 0)
//                {
                    
//                }
//                if (addHol > 0)
//                {
                    
//                }
//                if (heart >= 0)
//                {
//                    GameUtils.Heart += heart;
                  
//                    HomeManager.Instance.UpdateHeart();
//                }

//                switch (packId)
//                {
//                    case PackId.RemoveAds:
//                        GameUtils.IsRemoveAds = true;
//                        break;
//                    case PackId.WeeklyDeals:

//                        if(Subscription_Manager.Instance)
//                        Subscription_Manager.Instance.Set_Is_Buy_Pack(packId);

//                        GameUtils.Is_Buy_Weekly_Bundle = true;
//                        GameUtils.Weekly_Bundle_Count = 6;
//                        GameUtils.IsRemoveAdsWeekly = true;
//                        TimeManager.Instance.UnlimitedHeartCall(604798);
//                        transform.SetAsLastSibling();
//                        DOVirtual.DelayedCall(0.2f, () =>
//                        {
//                            transform.parent.GetComponent<RectTransform>().DOSizeDelta(new Vector2(1000, 455), 0.5f).SetEase(Ease.InBack);
//                            transform.DOScale(0, 0.5f).SetEase(Ease.InBack).OnComplete(() => gameObject.SetActive(false));
//                        });
//                        break;
//                    case PackId.NoAdsBundle:
//                        GameUtils.Is_Buy_NoAds_Bundle = true;
//                        GameUtils.IsRemoveAds = true;
//                        break;
//                    case PackId.StarterPack:
//                        GameUtils.Is_Buy_Starter_Pack = true;
//                        break;
//                    case PackId.SpecialOffer:
//                        GameUtils.Is_Buy_Special_Offer = true;
//                        GameUtils.IsRemoveAds = true;
//                        break;
//                }
//                transform.GetChild(0).gameObject.SetActive(false);
//                transform.GetChild(1).gameObject.SetActive(true);
//                if (packId != PackId.StarterPack_Sale)
//                {
//                    HomeManager.Instance.DisableRemoveAds();
//                }
//                if (packId != PackId.WeeklyDeals)
//                {
//                    GameUtils.Coin += coin;
//                    GameUtils.Undo_Booster += undo;
//                    GameUtils.Fill_Hol_Booster += fill;
//                    GameUtils.Add_Hol_Booster += addHol;
//                }
//                if (coin > 0)
//                {
//                    HomeManager.Instance.UpdateCoin();
                   
//                }



//            }
//        });

//    }
//    public bool CheckCantBuy()
//    {
//        return ((packId == PackId.RemoveAds && GameUtils.IsRemoveAds) || (packId == PackId.WeeklyDeals && GameUtils.Is_Buy_Weekly_Bundle) ||
//            ((packId == PackId.SpecialOffer && GameUtils.Is_Buy_Special_Offer)) || (packId == PackId.NoAdsBundle && GameUtils.Is_Buy_NoAds_Bundle) ||
//           (packId == PackId.StarterPack && GameUtils.Is_Buy_Starter_Pack));
//    }
//}
