using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public Game GM;
    public TanksConstructor TK;
    public Sience SC;

    [Header("Menu")]
    public GameObject[] OpenPan;

    [Header("Main")]
    public Text[] ResourceText;

    [Header("PremVisual")]
    public Button PremiumActiveBttn;
    public Text PremiumActiveText;

    [Header("StartGame")]
    public Button StartGameActiveBttn;
    public Text StartGameActiveText;
    public Image StartGameActiveImage;
    public Text NameLevelText;
    public Image ImageLevel;
    public Sprite[] ImageLevelSprite;

    [Header("WorkShop")]
    public Text[] TextSlot;
    public GameObject[] TrackTanks;
    public GameObject[] HeaderTanks;
    public GameObject[] PremTanksHeader;

    [Header("Science")]
    public Text SienceAvailableText;
    public Text[] ScienceScoreText;

    [Header("UpdatePartPan")]
    public GameObject UpdatePan;
    public Image[] UpdatePartImage;
    public Text UpdatePartText;

    [Header("Tasks")]
    public Button[] TaskPan;
    public Text[] TaskText;
    public Text[] TaskTextReward;
    public int[] Taskcount;
    public Animator TaskAnim;
    public bool ODTaskPan;
    public GameObject[] TaskBttnImage;

    [Header("DayBonus")]
    public GameObject DayBonusObj;
    public GameObject DayBonusPan;
    public Button[] DayBonusBttn;
    public GameObject[] DayBonusAwardsObj;
    public GameObject[] DayBonusAnim;

    [Header("Achives")]
    public List<Achives> Achives = new List<Achives>();
    public Sprite[] AwardsSprites;
    public bool ODAchivesPan;
    public Animator AchivesAnim;
    //public GameObject[] AchivesBttnImage;

    [Header("Audio")]
    public GameObject[] ActivDisMusic;
    public AudioSource UpItemSource;
    public AudioSource MenuBttnSource;
    public AudioSource PopUpSource;

    [Header("Trainer")]
    public GameObject[] TrainerPan;

    public GameObject SaveText;
    public GameObject LoadPan;

    private void Awake()
    {
        GM = Game.Instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (GM.Load)
        {
            ChoiceTask();
            UpdateText();
            LevelPart();
            SlotTankView();
            AvailableSience();
            AchivesUpdate();
            if (!GM.TakeBonus) { DayBonusObj.SetActive(true); }
            if (!GM.Trainer[0]) { TrainerPan[0].SetActive(true); TrainerPan[1].SetActive(true); }
            LoadPan.SetActive(false);
        }
        else { StartCoroutine(LoadGame()); }
    }

    void UpdateText()
    {
        for(int i = 0; i < 4; i++){ResourceText[i].text = GM.Resource[i].ToString();}
        for(int i = 0; i < 3; i++) { ScienceScoreText[i].text = GM.ScienceScore[i].ToString(); }
        if(GM.PremAll || GM.Prem >= 1) { PremiumActiveBttn.interactable = true; PremiumActiveText.text = "Премиум Активирован"; PremiumActiveText.color = new Color32(75,255,0,255);  }
        else { PremiumActiveBttn.interactable = false; PremiumActiveText.text = "Премиум не активирован!"; PremiumActiveText.color = new Color32(255, 0, 0, 255); }
        if(GM.LevelGameChoice == 0)
        {
            ImageLevel.sprite = ImageLevelSprite[GM.LevelGameChoice];
            if (GM.TankList[GM.SlotTankActive].TankDone) { StartGameActiveBttn.interactable = true; StartGameActiveText.text = "Танк готов к бою"; StartGameActiveImage.color = new Color32(0, 255, 0, 255); }
            else { StartGameActiveBttn.interactable = false; StartGameActiveText.text = "Танк не готов к бою"; StartGameActiveImage.color = new Color32(255, 0, 0, 255); }
            if (GM.ChapterDone[GM.LevelGameChoice]) { NameLevelText.text = "Глава " + (GM.LevelGameChoice + 1) + ": Пройдено"; }
            else { NameLevelText.text = "Уровень " + (GM.LevelGameChoice + 1) + ": Этап " + GM.LevelGameStage[GM.LevelGameChoice] + "/" + GM.LevelGameMaxStage[GM.LevelGameChoice]; }
        }
        else
        {
            ImageLevel.sprite = ImageLevelSprite[GM.LevelGameChoice];
            if (GM.ChapterDone[GM.LevelGameChoice-1])
            {
                if (GM.TankList[GM.SlotTankActive].TankDone) { StartGameActiveBttn.interactable = true; StartGameActiveText.text = "Танк готов к бою"; StartGameActiveImage.color = new Color32(0, 255, 0, 255); }
                else { StartGameActiveBttn.interactable = false; StartGameActiveText.text = "Танк не готов к бою"; StartGameActiveImage.color = new Color32(255, 0, 0, 255); }
                if (GM.ChapterDone[GM.LevelGameChoice]) { NameLevelText.text = "Глава " + (GM.LevelGameChoice + 1) + ": Пройдено"; }
                else { NameLevelText.text = "Глава " + (GM.LevelGameChoice + 1) + ": Этап " + GM.LevelGameStage[GM.LevelGameChoice] + "/" + GM.LevelGameMaxStage[GM.LevelGameChoice]; }
            }
            else{ StartGameActiveBttn.interactable = false; NameLevelText.text = "Чтобы открыть пройдите предыдущий уровень!";}
        }
        if (GM.ActiveAudio) { ActivDisMusic[0].SetActive(true); ActivDisMusic[1].SetActive(false); }
        else { ActivDisMusic[0].SetActive(false); ActivDisMusic[1].SetActive(true); }
    }
    public void NextLevel(){if ((GM.LevelGameChoice+1)< GM.LevelGameStage.Length){GM.LevelGameChoice++; UpdateText(); if (GM.ActiveAudio) { MenuBttnSource.Play(); } } }
    public void BackLevel() { if (GM.LevelGameChoice> 0) { GM.LevelGameChoice--; UpdateText(); if (GM.ActiveAudio) { MenuBttnSource.Play(); } } }

    public void OpenPanMenu(int index)
    {
        OpenPan[index].SetActive(true);
        for (int i = 0; i < TrackTanks.Length; i++) { TrackTanks[i].SetActive(false); }
        for (int i = 0; i < HeaderTanks.Length; i++) { HeaderTanks[i].SetActive(false); }
        for(int i = 0; i < PremTanksHeader.Length; i++) { PremTanksHeader[i].SetActive(false); }
        if (GM.ActiveAudio) { MenuBttnSource.Play(); } 
    }

    public void ClosedPanMenu(int index)
    {
        OpenPan[index].SetActive(false);
        UpdateText(); SlotTankView();
        GM.Save();
        if (GM.ActiveAudio) { MenuBttnSource.Play(); }
    }

    public void OpenShopIsConstructor()
    {
        OpenPan[0].SetActive(false); OpenPan[2].SetActive(true);
        if (GM.ActiveAudio) { MenuBttnSource.Play(); }
    }

    public void StartGame(){ GM.SceneChoice((GM.LevelGameChoice+1)); GM.ProgressAchives[1]++; }
    public void StartTestScene() { GM.LevelGameChoice = 6; GM.SceneChoice((6)); }

    void LevelPart()
    {
        for(int i = 0; i < 3; i++)
        {
            if (GM.UpdatePart[i])
            {
                if (i == 0 & GM.LevelTrack[GM.TankList[GM.SlotTankActive].TrackTanks] < GM.ColorA.Length - 1)
                {
                    int r = GM.TankList[GM.SlotTankActive].TrackTanks;
                    UpdatePartImage[0].sprite = TK.TrackTanksImage[r]; UpdatePartImage[0].color = new Color32((byte)GM.ColorA[GM.LevelTrack[r]], (byte)GM.ColorB[GM.LevelTrack[r]], (byte)GM.ColorC[GM.LevelTrack[r]], 255);
                    UpdatePartImage[1].sprite = TK.TrackTanksImage[r]; UpdatePartImage[1].color = new Color32((byte)GM.ColorA[GM.LevelTrack[r]], (byte)GM.ColorB[GM.LevelTrack[r]], (byte)GM.ColorC[GM.LevelTrack[r]], 255);
                    GM.LevelTrack[r]++;
                    UpdatePartImage[2].sprite = TK.TrackTanksImage[r]; UpdatePartImage[2].color = new Color32((byte)GM.ColorA[GM.LevelTrack[r]], (byte)GM.ColorB[GM.LevelTrack[r]], (byte)GM.ColorC[GM.LevelTrack[r]], 255);
                    UpdatePartText.text = "+10% к Скорости!";
                    GM.TrackUP[r] = 0; UpdatePan.SetActive(true); if (GM.ActiveAudio) { UpItemSource.Play(); }
                    GM.SaveLevelPart(0, r);
                }
                if (i == 1 & GM.LevelHead[GM.TankList[GM.SlotTankActive].HeaderTanks] < GM.ColorA.Length - 1)
                {
                    int r = GM.TankList[GM.SlotTankActive].HeaderTanks;
                    UpdatePartImage[0].sprite = TK.HeadTanksImage[r]; UpdatePartImage[0].color = new Color32((byte)GM.ColorA[GM.LevelHead[r]], (byte)GM.ColorB[GM.LevelHead[r]], (byte)GM.ColorC[GM.LevelHead[r]], 255);
                    UpdatePartImage[1].sprite = TK.HeadTanksImage[r]; UpdatePartImage[1].color = new Color32((byte)GM.ColorA[GM.LevelHead[r]], (byte)GM.ColorB[GM.LevelHead[r]], (byte)GM.ColorC[GM.LevelHead[r]], 255);
                    GM.LevelHead[r]++;
                    UpdatePartImage[2].sprite = TK.HeadTanksImage[r]; UpdatePartImage[2].color = new Color32((byte)GM.ColorA[GM.LevelHead[r]], (byte)GM.ColorB[GM.LevelHead[r]], (byte)GM.ColorC[GM.LevelHead[r]], 255);
                    UpdatePartText.text = "+10% к Броне!";
                    GM.HeadUP[r] = 0; UpdatePan.SetActive(true); if (GM.ActiveAudio) { UpItemSource.Play(); }
                    GM.SaveLevelPart(1, r);
                }
                if (i == 2 & GM.LevelGun[GM.TankList[GM.SlotTankActive].GunsTanks] < GM.ColorA.Length-1)
                {
                    int r = GM.TankList[GM.SlotTankActive].GunsTanks;
                    UpdatePartImage[0].sprite = TK.GunsTanksImage[r]; UpdatePartImage[0].color = new Color32((byte)GM.ColorA[GM.LevelGun[r]], (byte)GM.ColorB[GM.LevelGun[r]], (byte)GM.ColorC[GM.LevelGun[r]], 255);
                    UpdatePartImage[1].sprite = TK.GunsTanksImage[r]; UpdatePartImage[1].color = new Color32((byte)GM.ColorA[GM.LevelGun[r]], (byte)GM.ColorB[GM.LevelGun[r]], (byte)GM.ColorC[GM.LevelGun[r]], 255);
                    GM.LevelGun[r]++;
                    UpdatePartImage[2].sprite = TK.GunsTanksImage[r]; UpdatePartImage[2].color = new Color32((byte)GM.ColorA[GM.LevelGun[r]], (byte)GM.ColorB[GM.LevelGun[r]], (byte)GM.ColorC[GM.LevelGun[r]], 255);
                    UpdatePartText.text = "+10% к Урону!";
                    GM.GunUP[r] = 0; UpdatePan.SetActive(true); if (GM.ActiveAudio) { UpItemSource.Play(); }
                    GM.SaveLevelPart(2, r);
                }
                GM.UpdatePart[i] = false;
                break;
            }
        }
    }

    public void ClosedUpdaterPan(){ if (GM.ActiveAudio) { MenuBttnSource.Play(); } UpdatePan.SetActive(false); LevelPart();}

    public void OffOnMusic()
    {
        if (GM.ActiveAudio) { GM.ActiveAudio = false; ActivDisMusic[0].SetActive(false); ActivDisMusic[1].SetActive(true); }
        else { GM.ActiveAudio = true; ActivDisMusic[0].SetActive(true); ActivDisMusic[1].SetActive(false); }
    }

    public void AvailableSience()
    {
        for (int r = 0; r < GM.ScienceTrack.Length; r++)
        {
            if (!GM.ScienceTrack[r])
            {
                if (GM.ScienceScore[0] >= SC.CostTrack[r])
                {
                    SienceAvailableText.text = "Доступно исследование!"; SienceAvailableText.color = new Color32(30,255,0,255); return;
                }
                else { SienceAvailableText.text = "Проводи исследования и открывай новые виды танков и вооружения!"; SienceAvailableText.color = new Color32(255,255,255,255); }
                break;
            }
        }
        for (int r = 0; r < GM.ScienceTrack.Length; r++)
        {
            if (!GM.ScienceHead[r])
            {
                if (GM.ScienceScore[1] >= SC.CostHead[r])
                {
                    SienceAvailableText.text = "Доступно исследование!"; SienceAvailableText.color = new Color32(30, 255, 0, 255); return;
                }
                else { SienceAvailableText.text = "Проводи исследования и открывай новые виды танков и вооружения!"; SienceAvailableText.color = new Color32(255, 255, 255, 255); }
                break;
            }
        }
        for (int r = 0; r < GM.ScienceGun.Length; r++)
        {
            if (!GM.ScienceGun[r])
            {
                if (GM.ScienceScore[1] >= SC.CostGun[r])
                {
                    SienceAvailableText.text = "Доступно исследование!"; SienceAvailableText.color = new Color32(30, 255, 0, 255); return;
                }
                else { SienceAvailableText.text = "Проводи исследования и открывай новые виды танков и вооружения!"; SienceAvailableText.color = new Color32(255, 255, 255, 255); }
                break;
            }
        }
    }

    public void SlotTankView()
    {
        for(int i = 0; i < TrackTanks.Length; i++) { TrackTanks[i].SetActive(false); }
        for(int i = 0; i < HeaderTanks.Length; i++) { HeaderTanks[i].SetActive(false); }
        for(int i = 0; i < PremTanksHeader.Length; i++) { PremTanksHeader[i].SetActive(false); }
        TextSlot[0].text = "Выбран слот: " + (GM.SlotTankActive+1);
        if (GM.TankList[GM.SlotTankActive].TankDone)
        {
            TextSlot[1].text = "";
            if (!GM.TankList[GM.SlotTankActive].PremTanks)
            {
                TrackTanks[GM.TankList[GM.SlotTankActive].TrackTanks].SetActive(true);
                TrackTanks[GM.TankList[GM.SlotTankActive].TrackTanks].GetComponent<SpriteRenderer>().color = new Color32((byte)GM.ColorA[GM.LevelTrack[GM.TankList[GM.SlotTankActive].TrackTanks]], (byte)GM.ColorB[GM.LevelTrack[GM.TankList[GM.SlotTankActive].TrackTanks]], (byte)GM.ColorC[GM.LevelTrack[GM.TankList[GM.SlotTankActive].TrackTanks]], 255);
                HeaderTanks[GM.TankList[GM.SlotTankActive].HeaderTanks].SetActive(true);
                HeaderTanks[GM.TankList[GM.SlotTankActive].HeaderTanks].GetComponent<SpriteRenderer>().color = new Color32((byte)GM.ColorA[GM.LevelHead[GM.TankList[GM.SlotTankActive].HeaderTanks]], (byte)GM.ColorB[GM.LevelHead[GM.TankList[GM.SlotTankActive].HeaderTanks]], (byte)GM.ColorC[GM.LevelHead[GM.TankList[GM.SlotTankActive].HeaderTanks]], 255);
            }
            else { PremTanksHeader[GM.TankList[GM.SlotTankActive].HeaderTanks].SetActive(true); }
        }
        if(!GM.TankList[GM.SlotTankActive].SlotBuy){TextSlot[1].text = "Купите слот!";}
        if (GM.TankList[GM.SlotTankActive].SlotBuy & !GM.TankList[GM.SlotTankActive].TankDone) { TextSlot[1].text = "Создайте танк!"; }
    }

    public void NextSlotTank()
    {
        if(GM.SlotTankActive + 1 < GM.TankList.Capacity){ GM.SlotTankActive++; SlotTankView(); UpdateText(); if (GM.ActiveAudio) { MenuBttnSource.Play(); } }
    }

    public void BackSlotTank()
    {
        if (GM.SlotTankActive > 0){GM.SlotTankActive--; SlotTankView(); UpdateText(); if (GM.ActiveAudio) { MenuBttnSource.Play(); } }
    }

    //TRAINER!
    public void NextTrainer(int index)
    {
        if (index == 0) { TrainerPan[1].SetActive(false); TrainerPan[2].SetActive(true); if (GM.ActiveAudio) { MenuBttnSource.Play(); } }
        if (index == 1) { TrainerPan[2].SetActive(false); TrainerPan[3].SetActive(true); if (GM.ActiveAudio) { MenuBttnSource.Play(); } }
        if (index == 2) { TrainerPan[3].SetActive(false); TrainerPan[4].SetActive(true); if (GM.ActiveAudio) { MenuBttnSource.Play(); } }
        if (index == 3) { TrainerPan[0].SetActive(false); GM.Trainer[0] = true; }
    }

    //TASK!!!
    public void OpenClosedTaskPan()
    {
        if (!ODTaskPan) { ODTaskPan = true; TaskAnim.SetInteger("state", 1); UpdateTaskText(); if (GM.ActiveAudio) { PopUpSource.Play(); } TaskBttnImage[0].SetActive(false); TaskBttnImage[1].SetActive(true); }
        else { ODTaskPan = false; TaskAnim.SetInteger("state", 2); if (GM.ActiveAudio) { PopUpSource.Play(); } TaskBttnImage[0].SetActive(true); TaskBttnImage[1].SetActive(false); }
    }

    public void TakeAwardsTask(int index)
    {
        if(index == 0){ GM.Resource[0] += GM.TaskList[Taskcount[0]].AwardsTask; GM.TaskList[Taskcount[0]].CollectAwards = true; UpdateTaskText();}
        if (index == 1) { GM.Resource[1] += GM.TaskList[Taskcount[1]].AwardsTask; GM.TaskList[Taskcount[1]].CollectAwards = true; UpdateTaskText(); }
        if (index == 2) { GM.Resource[0] += GM.TaskList[Taskcount[2]].AwardsTask; GM.TaskList[Taskcount[2]].CollectAwards = true; UpdateTaskText(); }
        if (index == 3) { GM.Resource[2] += GM.TaskList[Taskcount[3]].AwardsTask; GM.TaskList[Taskcount[3]].CollectAwards = true; UpdateTaskText(); }
        if (index == 4) { GM.Resource[3]++; GM.AwardsDay = true; UpdateTaskText(); TaskText[4].text = "Выполнено!"; GM.Metrica(1); }
        for (int i = 0; i < 4; i++) { ResourceText[i].text = GM.Resource[i].ToString(); }
        TaskBttnImage[2].SetActive(false);
        if (GM.ActiveAudio) { UpItemSource.Play(); }
    }

    void ChoiceTask()
    {
        for (int r = 0; r < GM.TaskList.Capacity; r++) { GM.TaskList[r].ChoiceTask = false; }
        for (int i = 0; i < Taskcount.Length; i++)
        {
            for(int r = 0; r < GM.TaskList.Capacity; r++)
            {
                if(GM.TaskList[r].ActiveTask & !GM.TaskList[r].ChoiceTask)
                {
                    Taskcount[i] = r;
                    GM.TaskList[Taskcount[i]].ChoiceTask = true;
                    TaskText[i].text = GM.TaskList[Taskcount[i]].NameTask + ": " + GM.TaskList[Taskcount[i]].Perform + "/" + GM.TaskList[Taskcount[i]].NeedPerform;
                    TaskTextReward[i].text = GM.TaskList[Taskcount[i]].AwardsTask.ToString();
                    if(GM.TaskList[Taskcount[i]].Perform>= GM.TaskList[Taskcount[i]].NeedPerform & !GM.TaskList[Taskcount[i]].CollectAwards) { TaskBttnImage[2].SetActive(true); }
                    break;
                }
            }
        }
        if (!GM.TaskActive) { GM.TaskActive = true; GM.SaveTask(); }
    }

    void UpdateTaskText()
    {
        int r = 0;
        for(int i=0;i < Taskcount.Length; i++)
        {
            if(GM.TaskList[Taskcount[i]].Perform >= GM.TaskList[Taskcount[i]].NeedPerform)
            {
                if (!GM.TaskList[Taskcount[i]].CollectAwards){TaskPan[i].interactable = true; TaskText[i].text = "Собрать награду!"; TaskTextReward[i].text = GM.TaskList[Taskcount[i]].AwardsTask.ToString(); }
                else{TaskText[i].text = "Выполнено!"; TaskPan[i].interactable = false; TaskTextReward[i].text = GM.TaskList[Taskcount[i]].AwardsTask.ToString(); r++; }
                if(r == 4 & !GM.AwardsDay) { TaskPan[4].interactable = true; }
                if (GM.AwardsDay) { TaskPan[4].interactable = false; TaskText[4].text = "Выполнено!"; }
            }
            else { TaskText[i].text = GM.TaskList[Taskcount[i]].NameTask + ": " + GM.TaskList[Taskcount[i]].Perform + "/" + GM.TaskList[Taskcount[i]].NeedPerform; }
        }
    }

    //DAYBONUS!!
    public void OpenDayBonus(){DayBonusPan.SetActive(true);DayBonusBttn[GM.DayBonus].interactable = true; DayBonusAwardsObj[GM.DayBonus].SetActive(true); if (GM.ActiveAudio) { UpItemSource.Play(); }
        for (int i = 0; i < TrackTanks.Length; i++) { TrackTanks[i].SetActive(false); }
        for (int i = 0; i < HeaderTanks.Length; i++) { HeaderTanks[i].SetActive(false); }
    }

    public void TakeDayBonus(int index)
    {
        if (index == 0) { GM.Resource[0] += 100; DayBonusAnim[0].SetActive(true); DayBonusObj.SetActive(false); DayBonusPan.SetActive(false); }
        if (index == 1) { GM.Resource[2] += 100; DayBonusAnim[1].SetActive(true); DayBonusObj.SetActive(false); DayBonusPan.SetActive(false); }
        if (index == 2) { GM.Prem++; DayBonusObj.SetActive(false); DayBonusPan.SetActive(false); GM.SavePrem(); }
        if (index == 3) { GM.Resource[1] += 300; DayBonusAnim[2].SetActive(true); DayBonusObj.SetActive(false); DayBonusPan.SetActive(false); }
        if (index == 4) { GM.Resource[0] += 1000; DayBonusAnim[0].SetActive(true); DayBonusObj.SetActive(false); DayBonusPan.SetActive(false); }
        if (index == 5) { GM.ScienceScore[0]+=3; GM.ScienceScore[1] += 3; GM.ScienceScore[2] += 3; DayBonusAnim[3].SetActive(true); DayBonusObj.SetActive(false); DayBonusPan.SetActive(false); }
        if (index == 6) { GM.Resource[3] += 5; DayBonusAnim[4].SetActive(true); DayBonusObj.SetActive(false); DayBonusPan.SetActive(false); GM.Metrica(0); }
        GM.DayActive = true; GM.TakeBonus = true; if (GM.ActiveAudio) { UpItemSource.Play(); } UpdateText(); SlotTankView();
        GM.SaveEveryDayBonus();
    }


    //ACHIVES!!
    public void OpenClosedAchivesPan()
    {
        if (!ODAchivesPan) { ODAchivesPan = true; AchivesAnim.SetInteger("state", 1); AchivesUpdate(); if (GM.ActiveAudio) { PopUpSource.Play(); } }
        else { ODAchivesPan = false; AchivesAnim.SetInteger("state", 2); if (GM.ActiveAudio) { PopUpSource.Play(); } }
    }
    void AchivesUpdate()
    {
        for(int i = 0; i < Achives.Capacity; i++) 
        { 
            Achives[i].PanAchive.SetActive(false);
            if (GM.LevelAchives[i] <= Achives[i].NameAward.Length-1)
            {
                Achives[i].ImageAward.sprite = AwardsSprites[Achives[i].TypeAward[GM.LevelAchives[i]]];
                Achives[i].ProgressAwardImage.fillAmount = (float)GM.ProgressAchives[i] / Achives[i].NeedProgressAward[GM.LevelAchives[i]];
                Achives[i].AwardText.text = Achives[i].Award[GM.LevelAchives[i]].ToString();
                Achives[i].NameAwardText.text = Achives[i].NameAward[GM.LevelAchives[i]];
                Achives[i].ProgressAwardText.text = GM.ProgressAchives[i] + "/" + Achives[i].NeedProgressAward[GM.LevelAchives[i]];
                Achives[i].PanAchive.SetActive(true);
                if (GM.ProgressAchives[i] < Achives[i].NeedProgressAward[GM.LevelAchives[i]]){Achives[i].AwardBttn.interactable = false;}
                else { Achives[i].AwardBttn.interactable = true; }
            }
        }
    }

    public void CollectAchives(int index)
    {
        GM.Resource[Achives[index].TypeAward[GM.LevelAchives[index]]] += Achives[index].Award[GM.LevelAchives[index]];
        for (int i = 0; i < 4; i++) { ResourceText[i].text = GM.Resource[i].ToString(); }
        GM.LevelAchives[index]++;
        if (GM.LevelAchives[index] <= Achives[index].NameAward.Length)
        {
            Achives[index].ImageAward.sprite = AwardsSprites[Achives[index].TypeAward[GM.LevelAchives[index]]];
            Achives[index].ProgressAwardImage.fillAmount = (float)GM.ProgressAchives[index] / Achives[index].NeedProgressAward[GM.LevelAchives[index]];
            Achives[index].AwardText.text = Achives[index].Award[GM.LevelAchives[index]].ToString();
            Achives[index].NameAwardText.text = Achives[index].NameAward[GM.LevelAchives[index]];
            Achives[index].ProgressAwardText.text = GM.ProgressAchives[index] + "/" + Achives[index].NeedProgressAward[GM.LevelAchives[index]];
            Achives[index].PanAchive.SetActive(true);
            if (GM.ProgressAchives[index] < Achives[index].NeedProgressAward[GM.LevelAchives[index]]) { Achives[index].AwardBttn.interactable = false; }
            else { Achives[index].AwardBttn.interactable = true; }
        }
        else { Achives[index].PanAchive.SetActive(false); }
    }

    //SAVE!
    public void SaveGame() { GM.Save(); if (GM.ActiveAudio) { UpItemSource.Play(); } StartCoroutine(SaveTextView()); }

    IEnumerator SaveTextView()
    {
        SaveText.SetActive(true);
        yield return new WaitForSeconds(2);
        SaveText.SetActive(false);
        yield break;
    }

    public void DeleteSave() { GM.DeleteSave(); }

    IEnumerator LoadGame()
    {
        while (true)
        {
            if (!GM.Load) { yield return new WaitForSeconds(1); }
            else { Start(); yield break;  }
        }
    }
}

[Serializable]
public class Achives
{
    public GameObject PanAchive;
    public Button AwardBttn;
    public Image ImageAward;
    public Image ProgressAwardImage;
    public Text AwardText;
    public Text NameAwardText;
    public Text ProgressAwardText;
    public int[] Award;
    public int[] TypeAward;
    public int[] NeedProgressAward;
    public string[] NameAward;
}
