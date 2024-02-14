using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TanksConstructor : MonoBehaviour
{
    public Game GM;

    [Header("ResourcePan")]
    public Text[] ResourceText;

    [Header("TankConstructor")]
    public List<TrackConstructor> TrackConstructor = new List<TrackConstructor>();
    public List<HeadConstructor> HeadConstructor = new List<HeadConstructor>();
    public List<GunConstructor> GunConstructor = new List<GunConstructor>();
    public List<GunOptionalConstructor> GunOptionalConstructor = new List<GunOptionalConstructor>();
    public GameObject[] PremTanksHeader;
    public Text Weight_MaxWeightText;
    public Text[] SumConstructorCost;
    public Button BttnAssembly;
    public int SumWeight;
    public int[] costSum;
    //
    public GameObject[] BttnNextBackBuySlot;
    public Image BuySlotImage;
    public Text BuySlotText;

    [Header("PanelTankConstructor")]
    public List<PanelTankConstructor> PanelTankConstructor = new List<PanelTankConstructor>();
    public Sprite[] TrackTanksImage;
    public Sprite[] HeadTanksImage;
    public Sprite[] GunsTanksImage;
    public Sprite[] GunOptionImage;
    public int[] ChoicePanCount;

    [Header("GunOption")]
    public GameObject[] GunModule;
    public Text[] DynamicModuleText;
    public Text[] RocketModuleText;
    public Text[] LazerModuleText;
    public Text[] MachineGunModuleText;
    public Button[] GunOptionalBttn;
    public int[] SumCostGunOption;

    [Header("PremConstructor")]
    public GameObject[] PremTanksPan;
    public Text[] PremTanksText;
    public int ChoiceTankPrem;
    public List<PremTanksConstructor> PremTanksConstructor = new List<PremTanksConstructor>();

    [Header("DeleteConfig")]
    public GameObject DeleteConfigPan;
    public Text[] ReturnSumDeleteConfig;

    [Header("Trainer")]
    public GameObject[] TrainerPan;

    public AudioSource BttnSource;
    public AudioSource DoneSource;
    


    //Pool
    private List<GameObject> GunList = new List<GameObject>();
    private List<GameObject> GunOPtList = new List<GameObject>();

    private void Awake()
    {
        //Instance = this;
        GM = Game.Instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!GM.Trainer[1]) { TrainerPan[0].SetActive(true); TrainerPan[1].SetActive(true); }
    }

    private void OnEnable()
    {
        if (GM.TankList[GM.SlotTankActive].TankDone)
        {
            BttnNextBackBuySlot[2].SetActive(false);
            UpdatePanelTrackText(0);
            UpdatePanelHeadText(0);
            UpdatePanelGunText(0);
            UpdatePanelGunOptionText(0);
            UpdatePanTanksText();
        }
        else
        {
            GM.SlotTankActive = 1;
            UpdatePanTanksText();
            NextBackSlots(1);
        }
        PremTanksPan[0].SetActive(false);
    }

    void UpdatePanelTrackText(int index)
    {
        if(index == 0)
        {
            if (!GM.TankList[GM.SlotTankActive].PremTanks)
            {
                TrackConstructor[GM.TankList[GM.SlotTankActive].TrackTanks].TrackTankActive = true;
                TrackConstructor[GM.TankList[GM.SlotTankActive].TrackTanks].TrackTankObj.SetActive(true);
                TrackConstructor[GM.TankList[GM.SlotTankActive].TrackTanks].TrackTankObj.GetComponent<SpriteRenderer>().color = new Color32((byte)GM.ColorA[GM.LevelTrack[GM.TankList[GM.SlotTankActive].TrackTanks]], (byte)GM.ColorB[GM.LevelTrack[GM.TankList[GM.SlotTankActive].TrackTanks]], (byte)GM.ColorC[GM.LevelTrack[GM.TankList[GM.SlotTankActive].TrackTanks]], 255);
                PanelTankConstructor[0].ChoiceImage.sprite = TrackTanksImage[GM.TankList[GM.SlotTankActive].TrackTanks];
                PanelTankConstructor[0].ChoiceImage.color = new Color32((byte)GM.ColorA[GM.LevelTrack[GM.TankList[GM.SlotTankActive].TrackTanks]], (byte)GM.ColorB[GM.LevelTrack[GM.TankList[GM.SlotTankActive].TrackTanks]], (byte)GM.ColorC[GM.LevelTrack[GM.TankList[GM.SlotTankActive].TrackTanks]], 255);
                PanelTankConstructor[0].ChoiceText[1].text = TrackConstructor[GM.TankList[GM.SlotTankActive].TrackTanks].Name;
                PanelTankConstructor[0].ChoiceText[2].text = "Макс.Вес - " + TrackConstructor[GM.TankList[GM.SlotTankActive].TrackTanks].MaxWeightTrackTank + "кг.";
                PanelTankConstructor[0].ChoiceText[3].text = TrackConstructor[GM.TankList[GM.SlotTankActive].TrackTanks].CostTrackTank[1].ToString();
                PanelTankConstructor[0].ChoiceText[4].text = TrackConstructor[GM.TankList[GM.SlotTankActive].TrackTanks].CostTrackTank[2].ToString();
                costSum[0] = TrackConstructor[GM.TankList[GM.SlotTankActive].TrackTanks].CostTrackTank[0];
                costSum[1] = TrackConstructor[GM.TankList[GM.SlotTankActive].TrackTanks].CostTrackTank[1];
                costSum[2] = TrackConstructor[GM.TankList[GM.SlotTankActive].TrackTanks].CostTrackTank[2];
                PanelTankConstructor[0].PanBttn.interactable = true;
                PanelTankConstructor[1].PanBttn.interactable = true;
                PanelTankConstructor[0].ChoiceText[0].text = "";
                PanelTankConstructor[0].ChoiceObj.SetActive(true);
            }
            else
            {
                PanelTankConstructor[0].ChoiceText[0].text = "Выбрать шасси танка";
                PanelTankConstructor[0].ChoiceObj.SetActive(false);
                TrackConstructor[GM.TankList[GM.SlotTankActive].TrackTanks].TrackTankActive = false;
                TrackConstructor[GM.TankList[GM.SlotTankActive].TrackTanks].TrackTankObj.SetActive(false);
                costSum[0] = 0; costSum[1] = 0; costSum[2] = 0;
            }
        }
        if(index == 1)
        {
            PanelTankConstructor[0].ChoiceText[0].text = "Выбрать шасси танка";
            PanelTankConstructor[0].ChoiceObj.SetActive(false);
            PanelTankConstructor[1].PanBttn.interactable = false;
            TrackConstructor[GM.TankList[GM.SlotTankActive].TrackTanks].TrackTankActive = false;
            TrackConstructor[GM.TankList[GM.SlotTankActive].TrackTanks].TrackTankObj.SetActive(false);
            costSum[0] = 0; costSum[1] = 0; costSum[2] = 0;
            for(int i = 0; i < PremTanksHeader.Length; i++) { PremTanksHeader[i].SetActive(false); }
            UpdatePanelHeadText(1);
        }
    }

    void UpdatePanelHeadText(int index)
    {
        if (index == 0)
        {
            if (!GM.TankList[GM.SlotTankActive].PremTanks)
            {
                HeadConstructor[GM.TankList[GM.SlotTankActive].HeaderTanks].HeadTankActive = true;
                HeadConstructor[GM.TankList[GM.SlotTankActive].HeaderTanks].HeadTankObj.SetActive(true);
                HeadConstructor[GM.TankList[GM.SlotTankActive].HeaderTanks].HeadTankObj.GetComponent<SpriteRenderer>().color = new Color32((byte)GM.ColorA[GM.LevelHead[GM.TankList[GM.SlotTankActive].HeaderTanks]], (byte)GM.ColorB[GM.LevelHead[GM.TankList[GM.SlotTankActive].HeaderTanks]], (byte)GM.ColorC[GM.LevelHead[GM.TankList[GM.SlotTankActive].HeaderTanks]], 255);
                PanelTankConstructor[1].ChoiceImage.sprite = HeadTanksImage[GM.TankList[GM.SlotTankActive].HeaderTanks];
                PanelTankConstructor[1].ChoiceImage.color = new Color32((byte)GM.ColorA[GM.LevelHead[GM.TankList[GM.SlotTankActive].HeaderTanks]], (byte)GM.ColorB[GM.LevelHead[GM.TankList[GM.SlotTankActive].HeaderTanks]], (byte)GM.ColorC[GM.LevelHead[GM.TankList[GM.SlotTankActive].HeaderTanks]], 255);
                PanelTankConstructor[1].ChoiceText[1].text = HeadConstructor[GM.TankList[GM.SlotTankActive].HeaderTanks].Name;
                PanelTankConstructor[1].ChoiceText[2].text = "Вес - " + HeadConstructor[GM.TankList[GM.SlotTankActive].HeaderTanks].WeightHeadTank + "кг.";
                PanelTankConstructor[1].ChoiceText[3].text = HeadConstructor[GM.TankList[GM.SlotTankActive].HeaderTanks].CostHeadTank[1].ToString();
                PanelTankConstructor[1].ChoiceText[4].text = HeadConstructor[GM.TankList[GM.SlotTankActive].HeaderTanks].CostHeadTank[2].ToString();
                costSum[0] += HeadConstructor[GM.TankList[GM.SlotTankActive].HeaderTanks].CostHeadTank[0];
                costSum[1] += HeadConstructor[GM.TankList[GM.SlotTankActive].HeaderTanks].CostHeadTank[1];
                costSum[2] += HeadConstructor[GM.TankList[GM.SlotTankActive].HeaderTanks].CostHeadTank[2];
                SumWeight = HeadConstructor[GM.TankList[GM.SlotTankActive].HeaderTanks].WeightHeadTank;
                PanelTankConstructor[2].PanBttn.interactable = true;
                if (GM.TankList[GM.SlotTankActive].TankDone) { PanelTankConstructor[3].PanBttn.interactable = true; }
                PanelTankConstructor[1].ChoiceText[0].text = "";
                PanelTankConstructor[1].ChoiceObj.SetActive(true);
            }
            else
            {
                SumWeight = 0;
                PanelTankConstructor[1].ChoiceText[0].text = "Выбрать башню танка";
                PanelTankConstructor[1].ChoiceObj.SetActive(false);
                PanelTankConstructor[3].PanBttn.interactable = false;
                PremTanksHeader[GM.TankList[GM.SlotTankActive].HeaderTanks].SetActive(true);
            }
        }
        if (index == 1)
        {
            if (costSum[0] > 0 & HeadConstructor[GM.TankList[GM.SlotTankActive].HeaderTanks].HeadTankActive)
            {
                costSum[0] -= HeadConstructor[GM.TankList[GM.SlotTankActive].HeaderTanks].CostHeadTank[0];
                costSum[1] -= HeadConstructor[GM.TankList[GM.SlotTankActive].HeaderTanks].CostHeadTank[1];
                costSum[2] -= HeadConstructor[GM.TankList[GM.SlotTankActive].HeaderTanks].CostHeadTank[2];
            }
            SumWeight = 0;
            PanelTankConstructor[1].ChoiceText[0].text = "Выбрать башню танка";
            PanelTankConstructor[1].ChoiceObj.SetActive(false);
            PanelTankConstructor[2].PanBttn.interactable = false;
            PanelTankConstructor[3].PanBttn.interactable = false;
            HeadConstructor[GM.TankList[GM.SlotTankActive].HeaderTanks].HeadTankActive = false;
            HeadConstructor[GM.TankList[GM.SlotTankActive].HeaderTanks].HeadTankObj.SetActive(false);
            UpdatePanelGunText(1);
            UpdatePanelGunOptionText(1); UpdatePanelGunOptionText(2);
        }
    }

    void UpdatePanelGunText(int index)
    {
        if (index == 0)
        {
            if (!GM.TankList[GM.SlotTankActive].PremTanks)
            {
                for (int i = 0; i < HeadConstructor[GM.TankList[GM.SlotTankActive].HeaderTanks].PointHeader.Length; i++)
                {
                    GunConstructor[GM.TankList[GM.SlotTankActive].GunsTanks].GunTankActive = true;
                    GameObject obj = Instantiate(GunConstructor[GM.TankList[GM.SlotTankActive].GunsTanks].GunTankObj); GunList.Add(obj);
                    obj.transform.position = HeadConstructor[GM.TankList[GM.SlotTankActive].HeaderTanks].PointHeader[i].transform.position; obj.transform.parent = HeadConstructor[GM.TankList[GM.SlotTankActive].HeaderTanks].HeadTankObj.transform;
                    obj.GetComponentInChildren<SpriteRenderer>().color = new Color32((byte)GM.ColorA[GM.LevelGun[GM.TankList[GM.SlotTankActive].GunsTanks]], (byte)GM.ColorB[GM.LevelGun[GM.TankList[GM.SlotTankActive].GunsTanks]], (byte)GM.ColorC[GM.LevelGun[GM.TankList[GM.SlotTankActive].GunsTanks]], 255);
                    PanelTankConstructor[2].ChoiceImage.sprite = GunsTanksImage[GM.TankList[GM.SlotTankActive].GunsTanks];
                    PanelTankConstructor[2].ChoiceImage.color = new Color32((byte)GM.ColorA[GM.LevelGun[GM.TankList[GM.SlotTankActive].GunsTanks]], (byte)GM.ColorB[GM.LevelGun[GM.TankList[GM.SlotTankActive].GunsTanks]], (byte)GM.ColorC[GM.LevelGun[GM.TankList[GM.SlotTankActive].GunsTanks]], 255);
                    PanelTankConstructor[2].ChoiceText[1].text = GunConstructor[GM.TankList[GM.SlotTankActive].GunsTanks].Name;
                    if (HeadConstructor[GM.TankList[GM.SlotTankActive].HeaderTanks].PointHeader.Length == 1)
                    {
                        PanelTankConstructor[2].ChoiceText[2].text = "Вес - " + GunConstructor[GM.TankList[GM.SlotTankActive].GunsTanks].WeightGunTank + "кг.";
                        PanelTankConstructor[2].ChoiceText[3].text = GunConstructor[GM.TankList[GM.SlotTankActive].GunsTanks].CostGunTank[1].ToString();
                        PanelTankConstructor[2].ChoiceText[4].text = GunConstructor[GM.TankList[GM.SlotTankActive].GunsTanks].CostGunTank[2].ToString();
                        costSum[0] += GunConstructor[GM.TankList[GM.SlotTankActive].GunsTanks].CostGunTank[0];
                        costSum[1] += GunConstructor[GM.TankList[GM.SlotTankActive].GunsTanks].CostGunTank[1];
                        costSum[2] += GunConstructor[GM.TankList[GM.SlotTankActive].GunsTanks].CostGunTank[2];
                        SumWeight += GunConstructor[GM.TankList[GM.SlotTankActive].GunsTanks].WeightGunTank;
                    }
                    if (HeadConstructor[GM.TankList[GM.SlotTankActive].HeaderTanks].PointHeader.Length > 1)
                    {
                        PanelTankConstructor[2].ChoiceText[2].text = "Вес - " + GunConstructor[GM.TankList[GM.SlotTankActive].GunsTanks].WeightGunTank * 2 + "кг.";
                        PanelTankConstructor[2].ChoiceText[3].text = (GunConstructor[GM.TankList[GM.SlotTankActive].GunsTanks].CostGunTank[1] * 2).ToString();
                        PanelTankConstructor[2].ChoiceText[4].text = (GunConstructor[GM.TankList[GM.SlotTankActive].GunsTanks].CostGunTank[2] * 2).ToString();
                        costSum[0] += GunConstructor[GM.TankList[GM.SlotTankActive].GunsTanks].CostGunTank[0] * 2;
                        costSum[1] += GunConstructor[GM.TankList[GM.SlotTankActive].GunsTanks].CostGunTank[1] * 2;
                        costSum[2] += GunConstructor[GM.TankList[GM.SlotTankActive].GunsTanks].CostGunTank[2] * 2;
                        SumWeight += GunConstructor[GM.TankList[GM.SlotTankActive].GunsTanks].WeightGunTank * 2;
                    }
                    PanelTankConstructor[2].ChoiceText[0].text = "";
                    PanelTankConstructor[2].ChoiceObj.SetActive(true);
                }
            }
            else
            {
            }
        }
        if(index == 1)
        {
            for(int i = 0; i < HeadConstructor[GM.TankList[GM.SlotTankActive].HeaderTanks].PointHeader.Length; i++)
            {
                if (costSum[0] > 0 & GunConstructor[GM.TankList[GM.SlotTankActive].GunsTanks].GunTankActive)
                {
                    if(HeadConstructor[GM.TankList[GM.SlotTankActive].HeaderTanks].PointHeader.Length == 1)
                    {
                        costSum[0] -= GunConstructor[GM.TankList[GM.SlotTankActive].GunsTanks].CostGunTank[0];
                        costSum[1] -= GunConstructor[GM.TankList[GM.SlotTankActive].GunsTanks].CostGunTank[1];
                        costSum[2] -= GunConstructor[GM.TankList[GM.SlotTankActive].GunsTanks].CostGunTank[2];
                    }
                    if (HeadConstructor[GM.TankList[GM.SlotTankActive].HeaderTanks].PointHeader.Length > 1)
                    {
                        costSum[0] -= GunConstructor[GM.TankList[GM.SlotTankActive].GunsTanks].CostGunTank[0]*2;
                        costSum[1] -= GunConstructor[GM.TankList[GM.SlotTankActive].GunsTanks].CostGunTank[1]*2;
                        costSum[2] -= GunConstructor[GM.TankList[GM.SlotTankActive].GunsTanks].CostGunTank[2]*2;
                    }
                }
                if(SumWeight > 0 & GunConstructor[GM.TankList[GM.SlotTankActive].GunsTanks].GunTankActive)
                {
                    if(SumWeight > HeadConstructor[GM.TankList[GM.SlotTankActive].HeaderTanks].WeightHeadTank)
                    {
                        if (HeadConstructor[GM.TankList[GM.SlotTankActive].HeaderTanks].PointHeader.Length == 1) { SumWeight -= GunConstructor[GM.TankList[GM.SlotTankActive].GunsTanks].WeightGunTank; }
                        if (HeadConstructor[GM.TankList[GM.SlotTankActive].HeaderTanks].PointHeader.Length > 1) { SumWeight -= GunConstructor[GM.TankList[GM.SlotTankActive].GunsTanks].WeightGunTank * 2; }
                    }
                }
            }
            foreach (GameObject obj in GunList)
            {
                Destroy(obj);
            }
            GunList.Clear();
            PanelTankConstructor[2].ChoiceText[0].text = "Выбрать пушку танка";
            PanelTankConstructor[2].ChoiceObj.SetActive(false);
            GunConstructor[GM.TankList[GM.SlotTankActive].GunsTanks].GunTankActive = false;
        }
    }

    void UpdatePanelGunOptionText(int index)
    {
        if (index == 0)
        {
            if (!GM.TankList[GM.SlotTankActive].PremTanks)
            {
                for (int i = 0; i < 3; i++) { SumCostGunOption[i] = 0; }
                if (GM.TankList[GM.SlotTankActive].LazerTanks)
                {
                    GunOptionalConstructor[0].GunOptionalActive = true;
                    GameObject obj = Instantiate(GunOptionalConstructor[0].GunOptionalObj); GunOPtList.Add(obj);
                    obj.transform.position = HeadConstructor[GM.TankList[GM.SlotTankActive].HeaderTanks].PointLazer.transform.position; obj.transform.parent = HeadConstructor[GM.TankList[GM.SlotTankActive].HeaderTanks].HeadTankObj.transform;
                    for (int i = 0; i < 3; i++) { SumCostGunOption[i] += GunOptionalConstructor[0].CostGunOptionalTank[i]; }
                    PanelTankConstructor[3].ChoiceText[0].text = "";
                    PanelTankConstructor[3].ChoiceObj.SetActive(true);
                }
                if (GM.TankList[GM.SlotTankActive].RocketTanks)
                {
                    GunOptionalConstructor[1].GunOptionalActive = true;
                    GameObject obj = Instantiate(GunOptionalConstructor[1].GunOptionalObj); GunOPtList.Add(obj);
                    obj.transform.position = HeadConstructor[GM.TankList[GM.SlotTankActive].HeaderTanks].PointRocket.transform.position; obj.transform.parent = HeadConstructor[GM.TankList[GM.SlotTankActive].HeaderTanks].HeadTankObj.transform;
                    for (int i = 0; i < 3; i++) { SumCostGunOption[i] += GunOptionalConstructor[1].CostGunOptionalTank[i]; }
                    PanelTankConstructor[3].ChoiceText[0].text = "";
                    PanelTankConstructor[3].ChoiceObj.SetActive(true);
                }
                PanelTankConstructor[3].ChoiceText[3].text = SumCostGunOption[1].ToString();
                PanelTankConstructor[3].ChoiceText[4].text = SumCostGunOption[2].ToString();
            }
            else
            {
            }
        }
        if (index == 1)
        {
            foreach (GameObject obj in GunOPtList)
            {
                Destroy(obj);
            }
            GunOPtList.Clear();
            if(!GM.TankList[GM.SlotTankActive].RocketTanks)
            {
                PanelTankConstructor[3].ChoiceText[0].text = "Выбрать дополнительный модуль танка";
                PanelTankConstructor[3].ChoiceObj.SetActive(false);
            }
            else
            {
                for (int i = 0; i < 3; i++) { SumCostGunOption[i] -= GunOptionalConstructor[0].CostGunOptionalTank[i]; }
                PanelTankConstructor[3].ChoiceText[3].text = SumCostGunOption[1].ToString();
                PanelTankConstructor[3].ChoiceText[4].text = SumCostGunOption[2].ToString();
            }
            GM.TankList[GM.SlotTankActive].LazerTanks = false;
            GunOptionalConstructor[0].GunOptionalActive = false; 
        }
        if(index == 2)
        {
            foreach (GameObject obj in GunOPtList)
            {
                Destroy(obj);
            }
            GunOPtList.Clear();
            if (!GM.TankList[GM.SlotTankActive].LazerTanks)
            {
                PanelTankConstructor[3].ChoiceText[0].text = "Выбрать дополнительный модуль танка";
                PanelTankConstructor[3].ChoiceObj.SetActive(false);
            }
            else
            {
                for (int i = 0; i < 3; i++) { SumCostGunOption[i] -= GunOptionalConstructor[0].CostGunOptionalTank[i]; }
                PanelTankConstructor[3].ChoiceText[3].text = SumCostGunOption[1].ToString();
                PanelTankConstructor[3].ChoiceText[4].text = SumCostGunOption[2].ToString();
            }
            GM.TankList[GM.SlotTankActive].RocketTanks = false;
            GunOptionalConstructor[1].GunOptionalActive = false;
        }
    }

    void UpdateChoiceTrackPan()
    {
        if (GM.ScienceTrack[ChoicePanCount[0]])
        {
            PanelTankConstructor[0].ImageObj.sprite = TrackTanksImage[ChoicePanCount[0]]; PanelTankConstructor[0].ImageObj.color = new Color32((byte)GM.ColorA[GM.LevelTrack[ChoicePanCount[0]]], (byte)GM.ColorB[GM.LevelTrack[ChoicePanCount[0]]], (byte)GM.ColorC[GM.LevelTrack[ChoicePanCount[0]]], 255);
            PanelTankConstructor[0].ChoiceTextObj[0].text = TrackConstructor[ChoicePanCount[0]].Name;
            PanelTankConstructor[0].ChoiceTextObj[1].text = "Макс.вес - " + TrackConstructor[ChoicePanCount[0]].MaxWeightTrackTank + "\n" + "Скорость: " + TrackConstructor[ChoicePanCount[0]].BaseSpeed * 10;
            PanelTankConstructor[0].ChoiceTextObj[2].text = TrackConstructor[ChoicePanCount[0]].TrackDescription;
            PanelTankConstructor[0].ChoiceTextObj[3].text = TrackConstructor[ChoicePanCount[0]].CostTrackTank[0].ToString();
            PanelTankConstructor[0].ChoiceTextObj[4].text = TrackConstructor[ChoicePanCount[0]].CostTrackTank[1].ToString();
            PanelTankConstructor[0].ChoiceTextObj[5].text = TrackConstructor[ChoicePanCount[0]].CostTrackTank[2].ToString();
            PanelTankConstructor[0].NeedSienceText.text = "";
            PanelTankConstructor[0].ChoiceTankConstructorBttn.interactable = true;
            if (TrackConstructor[ChoicePanCount[0]].TrackTankActive) { PanelTankConstructor[0].ChoiceTextObj[6].text = "Убрать"; }
            else { PanelTankConstructor[0].ChoiceTextObj[6].text = "Выбрать"; }
        }
        else
        {
            PanelTankConstructor[0].ImageObj.sprite = PanelTankConstructor[0].SpriteNeedSience;
            PanelTankConstructor[0].NeedSienceText.text = "Исследуйте, чтобы открыть!";
            PanelTankConstructor[0].ChoiceTankConstructorBttn.interactable = false;
            for(int i = 0; i < 5; i++)
            {
                PanelTankConstructor[0].ChoiceTextObj[i].text = "";
            }
        }
    }

    void UpdateChoiceHeadPan()
    {
        if (GM.ScienceHead[ChoicePanCount[1]])
        {
            PanelTankConstructor[1].ImageObj.sprite = HeadTanksImage[ChoicePanCount[1]]; PanelTankConstructor[1].ImageObj.color = new Color32((byte)GM.ColorA[GM.LevelHead[ChoicePanCount[1]]], (byte)GM.ColorB[GM.LevelHead[ChoicePanCount[1]]], (byte)GM.ColorC[GM.LevelHead[ChoicePanCount[1]]], 255);
            PanelTankConstructor[1].ChoiceTextObj[0].text = HeadConstructor[ChoicePanCount[1]].Name;
            PanelTankConstructor[1].ChoiceTextObj[1].text = "Вес - " + HeadConstructor[ChoicePanCount[1]].WeightHeadTank + "\n" + "Броня: " + HeadConstructor[ChoicePanCount[1]].LifeSheildTank;
            PanelTankConstructor[1].ChoiceTextObj[2].text = HeadConstructor[ChoicePanCount[1]].HeadDescription;
            PanelTankConstructor[1].ChoiceTextObj[3].text = HeadConstructor[ChoicePanCount[1]].CostHeadTank[0].ToString();
            PanelTankConstructor[1].ChoiceTextObj[4].text = HeadConstructor[ChoicePanCount[1]].CostHeadTank[1].ToString();
            PanelTankConstructor[1].ChoiceTextObj[5].text = HeadConstructor[ChoicePanCount[1]].CostHeadTank[2].ToString();
            PanelTankConstructor[1].NeedSienceText.text = "";
            PanelTankConstructor[1].ChoiceTankConstructorBttn.interactable = true;
            if (HeadConstructor[ChoicePanCount[1]].HeadTankActive) { PanelTankConstructor[1].ChoiceTextObj[6].text = "Убрать"; }
            else { PanelTankConstructor[1].ChoiceTextObj[6].text = "Выбрать"; }
        }
        else
        {
            PanelTankConstructor[1].ImageObj.sprite = PanelTankConstructor[1].SpriteNeedSience;
            PanelTankConstructor[1].NeedSienceText.text = "Исследуйте, чтобы открыть!";
            PanelTankConstructor[1].ChoiceTankConstructorBttn.interactable = false;
            for (int i = 0; i < 5; i++)
            {
                PanelTankConstructor[1].ChoiceTextObj[i].text = "";
            }
        }
    }

    void UpdateChoiceGunPan()
    {
        if (GM.ScienceGun[ChoicePanCount[2]])
        {
            PanelTankConstructor[2].ImageObj.sprite = GunsTanksImage[ChoicePanCount[2]]; PanelTankConstructor[2].ImageObj.color = new Color32((byte)GM.ColorA[GM.LevelGun[ChoicePanCount[2]]], (byte)GM.ColorB[GM.LevelGun[ChoicePanCount[2]]], (byte)GM.ColorC[GM.LevelGun[ChoicePanCount[2]]], 255);
            PanelTankConstructor[2].ChoiceTextObj[0].text = GunConstructor[ChoicePanCount[2]].Name;
            if (HeadConstructor[GM.TankList[GM.SlotTankActive].HeaderTanks].PointHeader.Length == 1)
            {
                PanelTankConstructor[2].ChoiceTextObj[1].text = "Вес - " + GunConstructor[ChoicePanCount[2]].WeightGunTank + "\n" + "Урон: " + GunConstructor[ChoicePanCount[2]].BaseDamage;
                PanelTankConstructor[2].ChoiceTextObj[3].text = GunConstructor[ChoicePanCount[2]].CostGunTank[0].ToString();
                PanelTankConstructor[2].ChoiceTextObj[4].text = GunConstructor[ChoicePanCount[2]].CostGunTank[1].ToString();
                PanelTankConstructor[2].ChoiceTextObj[5].text = GunConstructor[ChoicePanCount[2]].CostGunTank[2].ToString();
            }
            if (HeadConstructor[GM.TankList[GM.SlotTankActive].HeaderTanks].PointHeader.Length > 1)
            {
                PanelTankConstructor[2].ChoiceTextObj[1].text = "Вес(х2) - " + GunConstructor[ChoicePanCount[2]].WeightGunTank * 2 + "\n" + "Урон(х2): " + GunConstructor[ChoicePanCount[2]].BaseDamage;
                PanelTankConstructor[2].ChoiceTextObj[3].text = (GunConstructor[ChoicePanCount[2]].CostGunTank[0] * 2).ToString();
                PanelTankConstructor[2].ChoiceTextObj[4].text = (GunConstructor[ChoicePanCount[2]].CostGunTank[1] * 2).ToString();
                PanelTankConstructor[2].ChoiceTextObj[5].text = (GunConstructor[ChoicePanCount[2]].CostGunTank[2] * 2).ToString();
            }
            PanelTankConstructor[2].ChoiceTextObj[2].text = GunConstructor[ChoicePanCount[2]].GunDescription;
            PanelTankConstructor[2].NeedSienceText.text = "";
            PanelTankConstructor[2].ChoiceTankConstructorBttn.interactable = true;
            if (GunConstructor[ChoicePanCount[2]].GunTankActive) { PanelTankConstructor[2].ChoiceTextObj[6].text = "Убрать"; }
            else { PanelTankConstructor[2].ChoiceTextObj[6].text = "Выбрать"; }
        }
        else
        {
            PanelTankConstructor[2].ImageObj.sprite = PanelTankConstructor[2].SpriteNeedSience;
            PanelTankConstructor[2].NeedSienceText.text = "Исследуйте, чтобы открыть!";
            PanelTankConstructor[2].ChoiceTankConstructorBttn.interactable = false;
            for (int i = 0; i < 5; i++)
            {
                PanelTankConstructor[2].ChoiceTextObj[i].text = "";
            }
        }
    }

    void UpdateChoiceGunOptionPan()
    {
        if (HeadConstructor[GM.TankList[GM.SlotTankActive].HeaderTanks].PointLazer != null)
        {
            GunModule[0].SetActive(true);
            if (!GunOptionalConstructor[0].GunOptionalActive) { LazerModuleText[3].text = "Выбрать"; }
            else { LazerModuleText[3].text = "Убрать"; }
            if(LazerModuleText[3].text == "Выбрать") { for (int i = 0; i < 3; i++) { LazerModuleText[i].text = GunOptionalConstructor[0].CostGunOptionalTank[i].ToString(); } }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    if (GunOptionalConstructor[0].CostGunOptionalTank[i] > 0){LazerModuleText[i].text = (GunOptionalConstructor[0].CostGunOptionalTank[i]/2).ToString();}
                    else { LazerModuleText[i].text = 0.ToString(); }
                }
            }
        }
        else { GunModule[0].SetActive(false); }
        if (HeadConstructor[GM.TankList[GM.SlotTankActive].HeaderTanks].PointRocket != null)
        {
            GunModule[1].SetActive(true);
            if (!GunOptionalConstructor[1].GunOptionalActive) { RocketModuleText[3].text = "Выбрать"; }
            else { RocketModuleText[3].text = "Убрать"; }
            if (RocketModuleText[3].text == "Выбрать") { for (int i = 0; i < 3; i++) { RocketModuleText[i].text = GunOptionalConstructor[1].CostGunOptionalTank[i].ToString(); } }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    if (GunOptionalConstructor[1].CostGunOptionalTank[i] > 0) { RocketModuleText[i].text = (GunOptionalConstructor[1].CostGunOptionalTank[i] / 2).ToString(); }
                    else { RocketModuleText[i].text = 0.ToString(); }
                }
            }
        }
        else { GunModule[1].SetActive(false); }
        if (HeadConstructor[GM.TankList[GM.SlotTankActive].HeaderTanks].CanDynamicSheild)
        {
            GunModule[2].SetActive(true);
            DynamicModuleText[3].text = "Кол-во: " + GM.TankList[GM.SlotTankActive].Sheild;
            if (GM.TankList[GM.SlotTankActive].Sheild < HeadConstructor[GM.TankList[GM.SlotTankActive].HeaderTanks].DynamicSheild) { DynamicModuleText[4].text = "Добавить"; GunOptionalBttn[0].interactable = true; }
            else { DynamicModuleText[4].text = "Максимум!"; GunOptionalBttn[0].interactable = false; }
            for (int i = 0; i < 3; i++) { DynamicModuleText[i].text = (GunOptionalConstructor[2].CostGunOptionalTank[i] * (GM.TankList[GM.SlotTankActive].Sheild + 1)).ToString(); }
        }
        else { GunModule[2].SetActive(false); }
        if (HeadConstructor[GM.TankList[GM.SlotTankActive].HeaderTanks].MachineGun>0)
        {
            GunModule[3].SetActive(true);
            MachineGunModuleText[3].text = "Кол-во: " + GM.TankList[GM.SlotTankActive].MachineGunTanks;
            if(GM.TankList[GM.SlotTankActive].MachineGunTanks < HeadConstructor[GM.TankList[GM.SlotTankActive].HeaderTanks].MachineGun) { MachineGunModuleText[4].text = "Добавить"; GunOptionalBttn[1].interactable = true; }
            else { MachineGunModuleText[4].text = "Максимум!"; GunOptionalBttn[1].interactable = false; }
            for (int i = 0; i < 3; i++) { MachineGunModuleText[i].text = (GunOptionalConstructor[3].CostGunOptionalTank[i] * (GM.TankList[GM.SlotTankActive].MachineGunTanks+1)).ToString(); }
        }
        else { GunModule[3].SetActive(false); }
    }

    public void UpdatePanTanksText()
    {
        Weight_MaxWeightText.text = "Вес/Макс.вес " + SumWeight + "/" + TrackConstructor[GM.TankList[GM.SlotTankActive].TrackTanks].MaxWeightTrackTank + "кг.";
        if(SumWeight > TrackConstructor[GM.TankList[GM.SlotTankActive].TrackTanks].MaxWeightTrackTank) { Weight_MaxWeightText.color = new Color32(255,0,0,255); }
        else { Weight_MaxWeightText.color = new Color32(0, 255, 0, 255); }
        for(int i = 0; i < 4; i++) { ResourceText[i].text = GM.Resource[i].ToString(); }
        for(int i = 0; i < 3; i++)
        {
            SumConstructorCost[i].text = costSum[i].ToString();
            if(GM.Resource[i] >= costSum[i]) { SumConstructorCost[i].color = new Color32(0, 255, 0, 255); }
            else { SumConstructorCost[i].color = new Color32(255, 0, 0, 255); }
        }
        if (GM.TankList[GM.SlotTankActive].TankDone) { PanelTankConstructor[3].PanBttn.interactable = true; }
        else { PanelTankConstructor[3].PanBttn.interactable = false; }
        if(!GM.TankList[GM.SlotTankActive].TankDone & GunConstructor[GM.TankList[GM.SlotTankActive].GunsTanks].GunTankActive & TrackConstructor[GM.TankList[GM.SlotTankActive].TrackTanks].MaxWeightTrackTank >= SumWeight & GM.Resource[0] >= costSum[0] & GM.Resource[1] >= costSum[1] & GM.Resource[2] >= costSum[2])
        {
            BttnAssembly.interactable = true;
        }
        else { BttnAssembly.interactable = false; }
    }
    
    
    //   OPEN_CLOSED_PAN!
    public void OpenChoicePan(int index)
    {
        if (GM.TankList[GM.SlotTankActive].TankDone & index !=3) { UpdateDeleteConfigPan(); DeleteConfigPan.SetActive(true); BttnNextBackBuySlot[0].SetActive(false); BttnNextBackBuySlot[1].SetActive(false); }
        else
        {
            if (index == 0) { UpdateChoiceTrackPan(); PanelTankConstructor[0].PanChoice.SetActive(true); BttnNextBackBuySlot[0].SetActive(false); BttnNextBackBuySlot[1].SetActive(false); }
            if (index == 1) { UpdateChoiceHeadPan(); PanelTankConstructor[1].PanChoice.SetActive(true); BttnNextBackBuySlot[0].SetActive(false); BttnNextBackBuySlot[1].SetActive(false); }
            if (index == 2) { UpdateChoiceGunPan(); PanelTankConstructor[2].PanChoice.SetActive(true); BttnNextBackBuySlot[0].SetActive(false); BttnNextBackBuySlot[1].SetActive(false); }
            if (index == 3) { UpdateChoiceGunOptionPan(); PanelTankConstructor[3].PanChoice.SetActive(true); BttnNextBackBuySlot[0].SetActive(false); BttnNextBackBuySlot[1].SetActive(false); }
        }
        AudioBttn();
    }

    public void ClosedChoicePan(int index)
    {
        PanelTankConstructor[index].PanChoice.SetActive(false);
        BttnNextBackBuySlot[0].SetActive(true); BttnNextBackBuySlot[1].SetActive(true);
        UpdatePanTanksText(); AudioBttn();
    }

    public void ChoiceBttn(int index)
    {
        if (index == 0)
        {
            if(PanelTankConstructor[0].ChoiceTextObj[6].text == "Выбрать")
            {
                UpdatePanelTrackText(1);
                GM.TankList[GM.SlotTankActive].TrackTanks = ChoicePanCount[0];
                UpdatePanelTrackText(0);
                PanelTankConstructor[0].ChoiceTextObj[6].text = "Убрать";
            }
            else
            {
                UpdatePanelTrackText(1);
                PanelTankConstructor[0].ChoiceTextObj[6].text = "Выбрать";
            }
        }
        if (index == 1)
        {
            if (PanelTankConstructor[1].ChoiceTextObj[6].text == "Выбрать")
            {
                UpdatePanelHeadText(1);
                GM.TankList[GM.SlotTankActive].HeaderTanks = ChoicePanCount[1];
                UpdatePanelHeadText(0);
                PanelTankConstructor[1].ChoiceTextObj[6].text = "Убрать";
            }
            else
            {
                UpdatePanelHeadText(1);
                PanelTankConstructor[1].ChoiceTextObj[6].text = "Выбрать";
            }
        }
        if (index == 2)
        {
            if (PanelTankConstructor[2].ChoiceTextObj[6].text == "Выбрать")
            {
                UpdatePanelGunText(1);
                GM.TankList[GM.SlotTankActive].GunsTanks = ChoicePanCount[2];
                UpdatePanelGunText(0);
                PanelTankConstructor[2].ChoiceTextObj[6].text = "Убрать";
            }
            else
            {
                UpdatePanelGunText(1);
                PanelTankConstructor[2].ChoiceTextObj[6].text = "Выбрать";
            }
        }
        if (index == 3)
        {
            if (LazerModuleText[3].text == "Выбрать" & GM.Resource[0] >= GunOptionalConstructor[0].CostGunOptionalTank[0] & GM.Resource[1] >= GunOptionalConstructor[0].CostGunOptionalTank[1] & GM.Resource[2] >= GunOptionalConstructor[0].CostGunOptionalTank[2])
            {
                for (int i = 0; i < 3; i++) { GM.Resource[i] -= GunOptionalConstructor[0].CostGunOptionalTank[i]; }
                GM.TankList[GM.SlotTankActive].LazerTanks = true;
                UpdatePanelGunOptionText(0);
                UpdateChoiceGunOptionPan();
                UpdatePanTanksText();
            }
            else
            {
                UpdatePanelGunOptionText(1);
                UpdateChoiceGunOptionPan();
                UpdatePanTanksText();
            }
        }
        if (index == 4)
        {
            if(RocketModuleText[3].text == "Выбрать" & GM.Resource[0] >= GunOptionalConstructor[1].CostGunOptionalTank[0] & GM.Resource[1] >= GunOptionalConstructor[1].CostGunOptionalTank[1] & GM.Resource[2] >= GunOptionalConstructor[1].CostGunOptionalTank[2])
            {
                for(int i = 0; i < 3; i++) { GM.Resource[i] -= GunOptionalConstructor[1].CostGunOptionalTank[i]; }
                GM.TankList[GM.SlotTankActive].RocketTanks = true;
                UpdatePanelGunOptionText(0);
                UpdateChoiceGunOptionPan();
            }
            else
            {
                UpdatePanelGunOptionText(2);
                UpdateChoiceGunOptionPan();
                UpdatePanTanksText();
            }
        }
        if(index == 5 & GM.Resource[0] >= GunOptionalConstructor[2].CostGunOptionalTank[0]* (HeadConstructor[GM.TankList[GM.SlotTankActive].HeaderTanks].DynamicSheild + 1) & GM.Resource[1] >= GunOptionalConstructor[2].CostGunOptionalTank[1]* (HeadConstructor[GM.TankList[GM.SlotTankActive].HeaderTanks].DynamicSheild + 1) & GM.Resource[2] >= GunOptionalConstructor[2].CostGunOptionalTank[2]* (HeadConstructor[GM.TankList[GM.SlotTankActive].HeaderTanks].DynamicSheild + 1))
        {
            for(int i = 0; i < 3; i++) { GM.Resource[i] -= GunOptionalConstructor[2].CostGunOptionalTank[i] * (GM.TankList[GM.SlotTankActive].Sheild+ 1); }
            GM.TankList[GM.SlotTankActive].Sheild++;
            UpdateChoiceGunOptionPan();
            UpdatePanTanksText();
        }
        if (index == 6 & GM.Resource[0] >= GunOptionalConstructor[3].CostGunOptionalTank[0] * (GM.TankList[GM.SlotTankActive].MachineGunTanks+1) & GM.Resource[1] >= GunOptionalConstructor[3].CostGunOptionalTank[1] * (GM.TankList[GM.SlotTankActive].MachineGunTanks + 1) & GM.Resource[2] >= GunOptionalConstructor[3].CostGunOptionalTank[2] * (GM.TankList[GM.SlotTankActive].MachineGunTanks + 1))
        {
            for (int i = 0; i < 3; i++) { GM.Resource[i] -= GunOptionalConstructor[3].CostGunOptionalTank[i] * (GM.TankList[GM.SlotTankActive].MachineGunTanks+1); }
            GM.TankList[GM.SlotTankActive].MachineGunTanks++;
            UpdateChoiceGunOptionPan();
            UpdatePanTanksText();
        }
        AudioBttn();
    }

    public void NextChoicePan(int index)
    {
        if (index == 0 & ChoicePanCount[0] < TrackConstructor.Capacity - 1) { ChoicePanCount[0]++; UpdateChoiceTrackPan(); }
        if (index == 1 & ChoicePanCount[1] < HeadConstructor.Capacity - 1) { ChoicePanCount[1]++; UpdateChoiceHeadPan(); }
        if (index == 2 & ChoicePanCount[2] < GunConstructor.Capacity - 1) { ChoicePanCount[2]++; UpdateChoiceGunPan(); }
        AudioBttn();
    }

    public void BackChoicePan(int index)
    {
        if (index == 0 & ChoicePanCount[0] > 0) { if (!GM.ScienceTrack[ChoicePanCount[0] - 1]) { ChoicePanCount[0] = 0; } else { ChoicePanCount[0]--; } UpdateChoiceTrackPan(); }
        if (index == 1 & ChoicePanCount[1] > 0) { if (!GM.ScienceHead[ChoicePanCount[1] - 1]) { ChoicePanCount[1] = 0; } else { ChoicePanCount[1]--; } UpdateChoiceHeadPan(); }
        if (index == 2 & ChoicePanCount[2] > 0) { if (!GM.ScienceHead[ChoicePanCount[2] - 1]) { ChoicePanCount[2] = 0; } else { ChoicePanCount[2]--; }  UpdateChoiceGunPan(); }
        AudioBttn();
    }

    public void ClosedSaleDeleteConfigPan(int index)
    {
        if(index == 0){DeleteConfigPan.SetActive(false);}
        if (index == 1)
        {
            for(int i = 0; i < 3; i++)
            {
                if(costSum[i] != 0){GM.Resource[i] += costSum[i] / 2;}
            }
            GM.TankList[GM.SlotTankActive].TankDone = false;
            GM.TankList[GM.SlotTankActive].PremTanks = false;
            GM.TankList[GM.SlotTankActive].MachineGunTanks = 0; GM.TankList[GM.SlotTankActive].Sheild = 0;
            UpdatePanelTrackText(1);
            UpdatePanTanksText();
            DeleteConfigPan.SetActive(false);
        }
        BttnNextBackBuySlot[0].SetActive(true); BttnNextBackBuySlot[1].SetActive(true);
    }

    void UpdateDeleteConfigPan()
    {
        for(int i = 0; i < 3; i++)
        {
            if(costSum[i] != 0)
            {
                ReturnSumDeleteConfig[i].text = (costSum[i] / 2).ToString();
            }
            else { ReturnSumDeleteConfig[i].text = 0.ToString(); }
        }
    }

    public void TankDone()
    {
        for(int i = 0; i < 3; i++) 
        { 
            GM.Resource[i] -= costSum[i]; 
        }
        GM.TankList[GM.SlotTankActive].LifeTank = HeadConstructor[GM.TankList[GM.SlotTankActive].HeaderTanks].LifeSheildTank;
        GM.TankList[GM.SlotTankActive].SpeedTank = TrackConstructor[GM.TankList[GM.SlotTankActive].TrackTanks].BaseSpeed;
        GM.TankList[GM.SlotTankActive].DamageGun = GunConstructor[GM.TankList[GM.SlotTankActive].GunsTanks].BaseDamage;
        if(GM.TankList[GM.SlotTankActive].TrackTanks < 3) { GM.TankList[GM.SlotTankActive].SizeTank = 0; }
        if (GM.TankList[GM.SlotTankActive].TrackTanks >= 3 & GM.TankList[GM.SlotTankActive].TrackTanks < 6) { GM.TankList[GM.SlotTankActive].SizeTank = 1; }
        if (GM.TankList[GM.SlotTankActive].TrackTanks >= 6) { GM.TankList[GM.SlotTankActive].SizeTank = 2; }
        GM.TankList[GM.SlotTankActive].TankDone = true;
        UpdatePanTanksText();
        GM.ProgressAchives[0]++;
        if (GM.ActiveAudio) { DoneSource.Play(); }
    }

    //SLOTS!
    public void BuySlot()
    {
        if (GM.TankList[GM.SlotTankActive].SlotCost[0] > 0 & GM.Resource[0] >= GM.TankList[GM.SlotTankActive].SlotCost[0])
        {
            GM.Resource[0] -= GM.TankList[GM.SlotTankActive].SlotCost[0];
            GM.TankList[GM.SlotTankActive].SlotBuy = true;
            BttnNextBackBuySlot[2].SetActive(false);
            PanelTankConstructor[0].PanBttn.interactable = true;
            UpdatePanTanksText();
            if (GM.ActiveAudio) { DoneSource.Play(); }
        }
        if(GM.TankList[GM.SlotTankActive].SlotCost[1] > 0 & GM.Resource[3] >= GM.TankList[GM.SlotTankActive].SlotCost[1])
        {
            GM.Resource[3] -= GM.TankList[GM.SlotTankActive].SlotCost[1];
            GM.TankList[GM.SlotTankActive].SlotBuy = true;
            BttnNextBackBuySlot[2].SetActive(false);
            PanelTankConstructor[0].PanBttn.interactable = true;
            UpdatePanTanksText();
            if (GM.ActiveAudio) { DoneSource.Play(); }
        }
    }

    public void NextBackSlots(int index)
    {
        if(index==0 & GM.SlotTankActive < GM.TankList.Capacity-1)
        {
            if (!GM.TankList[GM.SlotTankActive+1].SlotBuy) 
            {
                PanelTankConstructor[0].PanBttn.interactable = false;
                UpdatePanelTrackText(1);
                BttnNextBackBuySlot[2].SetActive(true);
                GM.SlotTankActive++;
                if (GM.TankList[GM.SlotTankActive].SlotCost[0] > 0)
                {
                    BuySlotImage.sprite = GM.ResourceSprite[0];
                    BuySlotText.text = GM.TankList[GM.SlotTankActive].SlotCost[0].ToString();
                }
                else
                {
                    BuySlotImage.sprite = GM.ResourceSprite[1];
                    BuySlotText.text = GM.TankList[GM.SlotTankActive].SlotCost[1].ToString();
                }
                UpdatePanTanksText();
            }
            else
            {
                PanelTankConstructor[0].PanBttn.interactable = true;
                BttnNextBackBuySlot[2].SetActive(false);
                if (GM.TankList[GM.SlotTankActive+1].TankDone)
                {
                    UpdatePanelTrackText(1);
                    GM.SlotTankActive++;
                    UpdatePanelTrackText(0);
                    UpdatePanelHeadText(0);
                    UpdatePanelGunText(0);
                    UpdatePanelGunOptionText(0);
                    UpdatePanTanksText();
                }
                else
                {
                    UpdatePanelTrackText(1);
                    GM.SlotTankActive++;
                    UpdatePanTanksText();
                }
            }
        }
        if(index ==1 & GM.SlotTankActive > 0)
        {
            if (!GM.TankList[GM.SlotTankActive-1].SlotBuy)
            {
                PanelTankConstructor[0].PanBttn.interactable = false;
                UpdatePanelTrackText(1);
                BttnNextBackBuySlot[2].SetActive(true);
                GM.SlotTankActive--;
                if (GM.TankList[GM.SlotTankActive].SlotCost[0] > 0)
                {
                    BuySlotImage.sprite = GM.ResourceSprite[0];
                    BuySlotText.text = GM.TankList[GM.SlotTankActive].SlotCost[0].ToString();
                }
                else
                {
                    BuySlotImage.sprite = GM.ResourceSprite[1];
                    BuySlotText.text = GM.TankList[GM.SlotTankActive].SlotCost[1].ToString();
                }
                UpdatePanTanksText();
            }
            else
            {
                PanelTankConstructor[0].PanBttn.interactable = true;
                BttnNextBackBuySlot[2].SetActive(false);
                if (GM.TankList[GM.SlotTankActive-1].TankDone)
                {
                    UpdatePanelTrackText(1);
                    GM.SlotTankActive--;
                    UpdatePanelTrackText(0);
                    UpdatePanelHeadText(0);
                    UpdatePanelGunText(0);
                    UpdatePanelGunOptionText(0);
                }
                else
                {
                    UpdatePanelTrackText(1);
                    GM.SlotTankActive--;
                    UpdatePanTanksText();
                }
            }
        }
        AudioBttn();
    }

    // PREM TANKS!
    public void OpenPremTanksPan()
    {
        if (!PremTanksPan[0].activeInHierarchy)
        {
            bool Active = false;
            for (int i = 0; i < GM.TanksPrem.Length; i++)
            {
                if (GM.TanksPrem[i]) { Active = true; ChoiceTankPrem = i; break; }
            }
            if (Active) { UpdateTankPrem(); }
            else
            {
                for (int i = 0; i < GM.TanksPrem.Length; i++) { PremTanksConstructor[i].TanksObj.SetActive(false); }
                for (int i = 0; i < 3; i++) { PremTanksText[i].text = ""; }
                PremTanksPan[1].SetActive(true); PremTanksPan[2].SetActive(false); PremTanksPan[3].SetActive(false);
            }
            PremTanksPan[0].SetActive(true); AudioBttn();
        }
        else { PremTanksPan[0].SetActive(false); AudioBttn(); }
    }

    void UpdateTankPrem()
    {
        for (int i = 0; i < GM.TanksPrem.Length; i++) { PremTanksConstructor[i].TanksObj.SetActive(false); }
        PremTanksPan[3].SetActive(true); PremTanksPan[1].SetActive(false);
        PremTanksConstructor[ChoiceTankPrem].TanksObj.SetActive(true);
        PremTanksText[0].text = "Скорость: " + (PremTanksConstructor[ChoiceTankPrem].SpeedTank * 10);
        PremTanksText[1].text = "Броня: " + PremTanksConstructor[ChoiceTankPrem].HealthTank;
        PremTanksText[2].text = "Урон: " + PremTanksConstructor[ChoiceTankPrem].DamageTank;
    }

    public void AddTankPremSlot()
    {
        if (GM.TankList[GM.SlotTankActive].TankDone || !GM.TankList[GM.SlotTankActive].SlotBuy) { PremTanksConstructor[ChoiceTankPrem].TanksObj.SetActive(false); PremTanksPan[2].SetActive(true); }
        else
        {
            GM.TankList[GM.SlotTankActive].TankDone = true;
            GM.TankList[GM.SlotTankActive].PremTanks = true;
            GM.TankList[GM.SlotTankActive].TrackTanks = 5;
            GM.TankList[GM.SlotTankActive].HeaderTanks = ChoiceTankPrem;
            GM.TankList[GM.SlotTankActive].LifeTank = PremTanksConstructor[ChoiceTankPrem].HealthTank;
            GM.TankList[GM.SlotTankActive].SpeedTank = PremTanksConstructor[ChoiceTankPrem].SpeedTank;
            GM.TankList[GM.SlotTankActive].DamageGun = PremTanksConstructor[ChoiceTankPrem].DamageTank;
            GM.TankList[GM.SlotTankActive].Sheild = PremTanksConstructor[ChoiceTankPrem].DynamicSheild;
            GM.TankList[GM.SlotTankActive].MachineGunTanks = PremTanksConstructor[ChoiceTankPrem].GunMachine;
            GM.TankList[GM.SlotTankActive].SizeTank = 2;
            UpdatePanelHeadText(0); if (GM.ActiveAudio) { DoneSource.Play(); }
        }
    }

    public void ClosedSlotEmployed(){PremTanksPan[2].SetActive(false); PremTanksConstructor[ChoiceTankPrem].TanksObj.SetActive(true); AudioBttn(); }

    public void NextBackPremTank(int index)
    {
        if (index == 0 & (ChoiceTankPrem + 1) < GM.TanksPrem.Length) { if (GM.TanksPrem[ChoiceTankPrem + 1]) { ChoiceTankPrem++; UpdateTankPrem(); } }
        if (index == 1 & (ChoiceTankPrem - 1) >= 0) { if (GM.TanksPrem[ChoiceTankPrem - 1]) { ChoiceTankPrem--; UpdateTankPrem(); }  }
        AudioBttn();
    }

    //Trainer!
    public void NextTrainer(int index)
    {
        if (index == 0) { TrainerPan[1].SetActive(false); TrainerPan[2].SetActive(true); AudioBttn(); }
        if (index == 1) { TrainerPan[2].SetActive(false); TrainerPan[3].SetActive(true); AudioBttn(); }
        if (index == 2) { TrainerPan[3].SetActive(false); TrainerPan[4].SetActive(true); AudioBttn(); }
        if (index == 3) { TrainerPan[0].SetActive(false); GM.Trainer[1] = true; }
    }

    //AUDIO
    void AudioBttn(){if (GM.ActiveAudio) { BttnSource.Play(); }}

}

[Serializable]
public class TrackConstructor
{
    public string Name;
    public GameObject TrackTankObj;
    public bool TrackTankActive;
    public int[] CostTrackTank;
    public int MaxWeightTrackTank;
    public float BaseSpeed;
    public string TrackDescription;
}

[Serializable]
public class HeadConstructor
{
    public string Name;
    public GameObject HeadTankObj;
    public GameObject[] PointHeader;
    public GameObject PointLazer;
    public GameObject PointRocket;
    public bool HeadTankActive;
    public int[] CostHeadTank;
    //public int MaxGunsLevel
    public int WeightHeadTank;
    public int LifeSheildTank;
    public int MachineGun;
    public bool CanDynamicSheild;
    public int DynamicSheild;
    public string HeadDescription;
}

[Serializable]
public class GunConstructor
{
    public string Name;
    public GameObject GunTankObj;
    public bool GunTankActive;
    public int[] CostGunTank;
    public int WeightGunTank;
    public int BaseDamage;
    public string GunDescription;
}

[Serializable]
public class GunOptionalConstructor
{
    public string Name;
    public GameObject GunOptionalObj;
    public bool GunOptionalActive;
    public int[] CostGunOptionalTank;
    public string GunOptionalDescription;
}
[Serializable]
public class PanelTankConstructor
{
    public string Name;
    public Text[] ChoiceText;
    public Button PanBttn;
    public GameObject ChoiceObj;
    public Image ChoiceImage;
    //
    public GameObject PanChoice;
    public Image ImageObj;
    public Text[] ChoiceTextObj;
    public GameObject[] ChoiceBttn;
    public Sprite SpriteNeedSience;
    public Text NeedSienceText;
    public Button ChoiceTankConstructorBttn;
}
[Serializable]
public class PremTanksConstructor
{
    public GameObject TanksObj;
    public int HealthTank;
    public int DamageTank;
    public float SpeedTank;
    public int DynamicSheild;
    public int GunMachine;
}

