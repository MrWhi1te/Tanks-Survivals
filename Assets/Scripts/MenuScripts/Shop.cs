using UnityEngine;
using UnityEngine.UI;
using YG;

public class Shop : MonoBehaviour
{
    public Game GM;
    public Menu MN;

    [Header("Main")]
    public Text[] ResourceText;

    [Header("PremTanks")]
    public GameObject[] PremPan; 
    public GameObject[] PremTanksPan;
    public GameObject[] PremTanksBttn;

    public GameObject[] SucessFailedTankPan;
    public GameObject[] SucessFailedPurchPan;

    public GameObject[] BuyBttn;

    private void Awake()
    {
        GM = Game.Instance;
    }

    private void OnEnable()
    {
        UpdateText();
        YandexGame.PurchaseSuccessEvent += Success;
        YandexGame.PurchaseFailedEvent += FailedP;
        if (GM.PremAll) { BuyBttn[0].SetActive(false); }
        if (GM.OffAds) { BuyBttn[1].SetActive(false); }
    }

    private void OnDisable()
    {
        YandexGame.PurchaseSuccessEvent += Success;
        YandexGame.PurchaseFailedEvent += FailedP;
    }

    void Success(string id)
    {
        if (id == "tank2") 
        { 
            GM.TanksPrem[1] = true; GM.Save(); PremTanksBttn[1].SetActive(false); if (GM.ActiveAudio) { MN.UpItemSource.Play(); }
            SucessFailedTankPan[0].SetActive(true); 
            //YandexMetrica.Send("tank2");
        }
        if (id == "tank3")
        {
            GM.TanksPrem[2] = true; GM.Save(); PremTanksBttn[2].SetActive(false); if (GM.ActiveAudio) { MN.UpItemSource.Play(); }
            SucessFailedTankPan[0].SetActive(true); 
            //YandexMetrica.Send("tank3");
        }
        if (id == "diamond")
        {
            //GM.Resource[3] += 60; 
            GM.Save(); UpdateText(); if (GM.ActiveAudio) { MN.UpItemSource.Play(); }
            PremPan[0].SetActive(false); PremPan[2].SetActive(true); SucessFailedPurchPan[0].SetActive(true); 
            //YandexMetrica.Send("diamond");
        }
        if (id == "prem1")
        {
            //GM.Prem++; 
            GM.SavePrem(); GM.Save(); if (GM.ActiveAudio) { MN.UpItemSource.Play(); }
            PremPan[0].SetActive(false); PremPan[2].SetActive(true); SucessFailedPurchPan[0].SetActive(true); 
            //YandexMetrica.Send("prem");
        }
        if (id == "premall")
        {
            GM.PremAll = true; GM.Save(); if (GM.ActiveAudio) { MN.UpItemSource.Play(); }
            PremPan[0].SetActive(false); PremPan[2].SetActive(true); SucessFailedPurchPan[0].SetActive(true); BuyBttn[0].SetActive(false); 
            //YandexMetrica.Send("premall");
        }
        if (id == "offads")
        {
            GM.OffAds = true; GM.Save(); YandexGame.StickyAdActivity(false); if (GM.ActiveAudio) { MN.UpItemSource.Play(); }
            PremPan[0].SetActive(false); PremPan[2].SetActive(true); SucessFailedPurchPan[0].SetActive(true); BuyBttn[1].SetActive(false); 
            //YandexMetrica.Send("offads");
        }
    }

    void FailedP(string id) 
    {
        if (id == "tank2" || id == "tank3") { SucessFailedTankPan[1].SetActive(true); }
        if (id == "diamond" || id == "prem" || id == "premall" || id == "offads") { PremPan[0].SetActive(false); PremPan[2].SetActive(true); SucessFailedPurchPan[1].SetActive(true); }
    }

    void UpdateText()
    {
        for(int i = 0; i < 4; i++) { ResourceText[i].text = GM.Resource[i].ToString(); }
    }

    public void OpenTankMore(int index)
    {
        for(int i = 0; i < PremTanksPan.Length; i++) { PremTanksPan[i].SetActive(false); if (GM.TanksPrem[i]) { PremTanksBttn[i].SetActive(false); } }
        PremPan[0].SetActive(false); PremPan[1].SetActive(true);
        PremTanksPan[index].SetActive(true);
        if (GM.ActiveAudio) { MN.MenuBttnSource.Play(); }
    }

    public void ClosedTankMore()
    {
        PremPan[1].SetActive(false); PremPan[0].SetActive(true);
        UpdateText(); if (GM.ActiveAudio) { MN.MenuBttnSource.Play(); }
    }

    public void BuyTankPrem()
    {
        if(GM.Resource[3] >= 120) { GM.Resource[3] -= 120; GM.TanksPrem[0] = true; PremTanksBttn[0].SetActive(false); GM.Save(); if (GM.ActiveAudio) { MN.UpItemSource.Play(); } }
    }

    public void BuyDiamondPrem(int index)
    {
        if (index == 0 & GM.Resource[3] >= 20) { GM.Resource[3] -= 20; GM.Prem++; UpdateText(); GM.SavePrem(); if (GM.ActiveAudio) { MN.UpItemSource.Play(); } }
        if (index == 1 & GM.Resource[3] >= 159) { GM.Resource[3] -= 159; GM.PremAll = true; UpdateText(); GM.SavePrem(); if (GM.ActiveAudio) { MN.UpItemSource.Play(); } }
    }

    public void ExchangeCurr(int index)
    {
        if (GM.Resource[3]>=1) { GM.Resource[3]--; GM.Resource[index] += 100; UpdateText(); if (GM.ActiveAudio) { MN.UpItemSource.Play(); } }
    }

    public void ClosedSFPurchasePan(int index)
    {
        SucessFailedTankPan[index].SetActive(false); if (GM.ActiveAudio) { MN.MenuBttnSource.Play(); }
    }

    public void ClosedSucessFailedPurchPan()
    {
        PremPan[0].SetActive(true); PremPan[2].SetActive(false); SucessFailedPurchPan[0].SetActive(false); SucessFailedPurchPan[1].SetActive(false);
    }
}
