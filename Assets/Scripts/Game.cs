using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public class Game : MonoBehaviour
{
    public static Game Instance;

    [Header("Resource")]
    public int[] Resource;
    public Sprite[] ResourceSprite;

    [Header("TankList")]
    public List<TankList> TankList = new List<TankList>();
    public int SlotTankActive;

    [Header("TanksPart")]
    public int[] LevelTrack;
    public int[] TrackUP; // Время в бою
    public int[] LevelHead;
    public int[] HeadUP; // Попадания по танку для повышения уровня
    public int[] LevelGun;
    public int[] GunUP; // Уничтожение врагов для повышения уровня
    public bool[] UpdatePart;
    //
    public int[] ColorA;
    public int[] ColorB;
    public int[] ColorC;

    [Header("Science")]
    public int[] ScienceScore;
    public bool[] ScienceTrack;
    public bool[] ScienceHead;
    public bool[] ScienceGun;

    [Header("MainGameMode")]
    public bool[] ChapterDone;
    public int[] LevelGameMaxStage;
    public int[] LevelGameStage;
    public int[] SaveLevelStage;
    public int LevelGameChoice;

    [Header("PRem")]
    public int Prem;
    public bool PremAll;
    public bool[] TanksPrem;

    [Header("Task")]
    public bool TaskActive;
    public List<TaskList> TaskList = new List<TaskList>();
    public bool AwardsDay;

    [Header("DayBonus")]
    public bool DayActive;
    public int DayBonus;
    public bool TakeBonus;

    public bool SaveActive1;
    public bool Load;

    [Header("Trainer")]
    public bool[] Trainer;

    [Header("Achives")]
    public int[] LevelAchives;
    public int[] ProgressAchives;

    [Header("ADS")]
    public bool OffAds;

    [Header("Audio")]
    public bool ActiveAudio;

    public GameObject Pool;

    public Vector3 PlayerPosition;
    public string DeviceGame;

    private void OnEnable() 
    {
        YandexGame.PurchaseSuccessEvent += Success;
        YandexGame.GetDataEvent += GetLoad;
    }

    private void OnDisable()
    {
        YandexGame.GetDataEvent -= GetLoad;
    }

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (YandexGame.SDKEnabled == true)
        {
            GetLoad();
        }
    }

    void Success(string id)
    {
        if (id == "tank2")
        {
            TanksPrem[1] = true; Save();
            YandexMetrica.Send("tank2");
        }
        if (id == "tank3")
        {
            TanksPrem[2] = true; Save(); 
            YandexMetrica.Send("tank3");
        }
        if (id == "diamond")
        {
            Resource[3] += 60; Save(); 
            YandexMetrica.Send("diamond");
        }
        if (id == "prem1")
        {
            Prem++; SavePrem(); Save();
            YandexMetrica.Send("prem");
        }
        if (id == "premall")
        {
            PremAll = true; Save();
            YandexMetrica.Send("premall");
        }
        if (id == "offads")
        {
            OffAds = true; Save(); YandexGame.StickyAdActivity(false);
            YandexMetrica.Send("offads"); //AdsPan.SetActive(false);
        }
    }

    public void GetLoad()
    {
        SaveActive1 = YandexGame.savesData.SaveActive1;
        if (SaveActive1)
        {
            for (int i = 0; i < 4; i++) { Resource[i] = YandexGame.savesData.Resource[i]; }
            for (int i = 0; i < 3; i++)
            {
                ScienceScore[i] = YandexGame.savesData.ScienceScore[i];
                TanksPrem[i] = YandexGame.savesData.TanksPrem[i];
            }
            for(int i = 0; i < 9; i++)
            {
                LevelTrack[i] = YandexGame.savesData.LevelTrack[i];
                TrackUP[i] = YandexGame.savesData.TrackUP[i];
                LevelGun[i] = YandexGame.savesData.LevelGun[i];
                GunUP[i] = YandexGame.savesData.GunUP[i];
                ScienceTrack[i] = YandexGame.savesData.ScienceTrack[i];
                ScienceGun[i] = YandexGame.savesData.ScienceGun[i];
            }
            for(int i = 0; i < 13; i++)
            {
                LevelHead[i] = YandexGame.savesData.LevelHead[i];
                HeadUP[i] = YandexGame.savesData.HeadUP[i];
                ScienceHead[i] = YandexGame.savesData.ScienceHead[i];
            }
            for(int i = 0; i < 5; i++)
            {
                ChapterDone[i] = YandexGame.savesData.ChapterDone[i];
                LevelGameStage[i] = YandexGame.savesData.LevelGameStage[i];
                SaveLevelStage[i] = YandexGame.savesData.SaveLevelStage[i];
                TankList[i].SlotBuy = YandexGame.savesData.SlotBuy[i];
                if (TankList[i].SlotBuy)
                {
                    TankList[i].TankDone = YandexGame.savesData.TankDone[i];
                    if (TankList[i].TankDone)
                    {
                        TankList[i].PremTanks = YandexGame.savesData.PremTanks[i];
                        TankList[i].TrackTanks = YandexGame.savesData.TrackTanks[i];
                        TankList[i].HeaderTanks = YandexGame.savesData.HeaderTanks[i];
                        TankList[i].MachineGunTanks = YandexGame.savesData.MachineGunTanks[i];
                        TankList[i].GunsTanks = YandexGame.savesData.GunsTanks[i];
                        TankList[i].LazerTanks = YandexGame.savesData.LazerTanks[i];
                        TankList[i].RocketTanks = YandexGame.savesData.RocketTanks[i];
                        TankList[i].Sheild = YandexGame.savesData.Sheild[i];
                        TankList[i].SizeTank = YandexGame.savesData.SizeTank[i];
                        TankList[i].LifeTank = YandexGame.savesData.LifeTank[i];
                        TankList[i].SpeedTank = YandexGame.savesData.SpeedTank[i];
                        TankList[i].DamageGun = YandexGame.savesData.DamageGun[i];
                    }
                }
            }
            for(int i = 0; i < 6; i++) { LevelAchives[i] = YandexGame.savesData.LevelAchives[i]; ProgressAchives[i] = YandexGame.savesData.ProgressAchives[i]; }
            for (int i = 0; i < 8; i++)
            {
                TaskList[i].ActiveTask = YandexGame.savesData.ActiveTask[i];
                TaskList[i].CollectAwards = YandexGame.savesData.CollectAwards[i];
                TaskList[i].NeedPerform = YandexGame.savesData.NeedPerform[i];
                TaskList[i].Perform = YandexGame.savesData.Perform[i];
                TaskList[i].AwardsTask = YandexGame.savesData.AwardsTask[i];
            }
            TaskActive = YandexGame.savesData.TaskActive;
            AwardsDay = YandexGame.savesData.AwardsDay;
            Prem = YandexGame.savesData.Prem;
            DayActive = YandexGame.savesData.DayActive;
            DayBonus = YandexGame.savesData.DayBonus;
            TakeBonus = YandexGame.savesData.TakeBonus;
            ActiveAudio = YandexGame.savesData.ActiveAudio;
            LevelGameChoice = YandexGame.savesData.LevelGameChoice;
            //TanksPrem[0] = YandexGame.savesData.TanksPrem[0];
            Trainer[0] = YandexGame.savesData.Trainer[0]; Trainer[1] = YandexGame.savesData.Trainer[1];
            if (Prem > 0)
            {
                DateTime bon = new DateTime(YandexGame.savesData.PremTime[0], YandexGame.savesData.PremTime[1], YandexGame.savesData.PremTime[2], YandexGame.savesData.PremTime[3], YandexGame.savesData.PremTime[4], YandexGame.savesData.PremTime[5]);
                TimeSpan tsBon = DateTime.Now - bon;
                int BonusAd = (int)tsBon.TotalHours;
                if (BonusAd >= 24 & BonusAd < 48) { Prem--; }
                if (BonusAd >= 48 & BonusAd < 72) { Prem -= 2; }
                if (BonusAd >= 72) { Prem -= 3; }
                if (Prem < 0) { Prem = 0; }
            }
            if (TaskActive)
            {
                DateTime bon = new DateTime(YandexGame.savesData.DayTaskTime[0], YandexGame.savesData.DayTaskTime[1], YandexGame.savesData.DayTaskTime[2], YandexGame.savesData.DayTaskTime[3], YandexGame.savesData.DayTaskTime[4], YandexGame.savesData.DayTaskTime[5]);
                TimeSpan tsBon = DateTime.Now - bon;
                int BonusAd = (int)tsBon.TotalHours;
                if (BonusAd >= 20) { UpdateTask(); TaskActive = false; }
            }
            if (DayActive)
            {
                DateTime bon = new DateTime(YandexGame.savesData.DayBonusTime[0], YandexGame.savesData.DayBonusTime[1], YandexGame.savesData.DayBonusTime[2], YandexGame.savesData.DayBonusTime[3], YandexGame.savesData.DayBonusTime[4], YandexGame.savesData.DayBonusTime[5]);
                TimeSpan tsBon = DateTime.Now - bon;
                int BonusAd = (int)tsBon.TotalHours;
                if (BonusAd >= 20) { TakeBonus = false; DayBonus++; }
                if (DayBonus > 6) { DayBonus = 0; }
            }
            OffAds = YandexGame.savesData.OffAds;
            if (OffAds) { YandexGame.StickyAdActivity(false); }
            PremAll = YandexGame.savesData.PremAll;
        }
        else { UpdateTask(); }
        DeviceGame = YandexGame.EnvironmentData.deviceType;
        Load = true;
    }

    public void SceneChoice(int index)
    {
        if(index == 0)
        {
            SceneManager.LoadScene("Menu");
            Save();
            FullscreenAds();
            if (DeviceGame == "mobile" || DeviceGame == "tablet" & !OffAds) { YandexGame.StickyAdActivity(true); }
        }
        if (index == 1)
        {
            SceneManager.LoadScene("Chapter1");
            if(DeviceGame == "mobile" || DeviceGame == "tablet") { YandexGame.StickyAdActivity(false); }
        }
        if (index == 2)
        {
            SceneManager.LoadScene("Chapter2");
            FullscreenAds();
            if (DeviceGame == "mobile" || DeviceGame == "tablet") { YandexGame.StickyAdActivity(false); }
        }
        if (index == 3)
        {
            SceneManager.LoadScene("Chapter3");
            FullscreenAds();
            if (DeviceGame == "mobile" || DeviceGame == "tablet") { YandexGame.StickyAdActivity(false); }
        }
        if (index == 4)
        {
            SceneManager.LoadScene("Chapter4");
            FullscreenAds();
            if (DeviceGame == "mobile" || DeviceGame == "tablet") { YandexGame.StickyAdActivity(false); }
        }
        if (index == 5)
        {
            SceneManager.LoadScene("Chapter5");
            FullscreenAds();
            if (DeviceGame == "mobile" || DeviceGame == "tablet") { YandexGame.StickyAdActivity(false); }
        }
    }

    void UpdateTask()
    {
        AwardsDay = false;
        for (int i = 0; i < TaskList.Capacity; i++) { TaskList[i].ActiveTask = false; TaskList[i].CollectAwards = false; TaskList[i].Perform = 0; }
        for(int i = 0; i < 4; i++)
        {
            int r = UnityEngine.Random.Range(0, TaskList.Capacity);
            if (TaskList[r].ActiveTask)
            {
                while (TaskList[r].ActiveTask) { r = UnityEngine.Random.Range(0, TaskList.Capacity); }
            }
            TaskList[r].ActiveTask = true; TaskList[r].NeedPerform = UnityEngine.Random.Range(3, 10); TaskList[r].AwardsTask = UnityEngine.Random.Range(5, 50); if (r == 0) { TaskList[r].NeedPerform = 0; }
        }
    }

    public void SaveTask()
    {
        YandexGame.savesData.DayTaskTime[0] = DateTime.Now.Year; YandexGame.savesData.DayTaskTime[1] = DateTime.Now.Month; YandexGame.savesData.DayTaskTime[2] = DateTime.Now.Day; YandexGame.savesData.DayTaskTime[3] = DateTime.Now.Hour; YandexGame.savesData.DayTaskTime[4] = DateTime.Now.Minute; YandexGame.savesData.DayTaskTime[5] = DateTime.Now.Second;
        YandexGame.savesData.TaskActive = TaskActive;
        YandexGame.SaveProgress();
    }

    public void SaveEveryDayBonus()
    {
        YandexGame.savesData.DayBonusTime[0] = DateTime.Now.Year; YandexGame.savesData.DayBonusTime[1] = DateTime.Now.Month; YandexGame.savesData.DayBonusTime[2] = DateTime.Now.Day; YandexGame.savesData.DayBonusTime[3] = DateTime.Now.Hour; YandexGame.savesData.DayBonusTime[4] = DateTime.Now.Minute; YandexGame.savesData.DayBonusTime[5] = DateTime.Now.Second;
        YandexGame.savesData.DayBonus = DayBonus;
        YandexGame.savesData.DayActive = DayActive;
        YandexGame.savesData.TakeBonus = TakeBonus;
        YandexGame.SaveProgress();
    }

    public void SavePrem()
    {
        YandexGame.savesData.PremTime[0] = DateTime.Now.Year; YandexGame.savesData.PremTime[1] = DateTime.Now.Month; YandexGame.savesData.PremTime[2] = DateTime.Now.Day; YandexGame.savesData.PremTime[3] = DateTime.Now.Hour; YandexGame.savesData.PremTime[4] = DateTime.Now.Minute; YandexGame.savesData.PremTime[5] = DateTime.Now.Second;
        YandexGame.savesData.Prem = Prem;
        YandexGame.SaveProgress();
    }

    public void SaveLevelPart(int index,int id)
    {
        if (index == 0) { YandexGame.savesData.LevelTrack[id] = LevelTrack[id]; YandexGame.savesData.TrackUP[id] = TrackUP[id]; }
        if (index == 1) { YandexGame.savesData.LevelHead[id] = LevelHead[id]; YandexGame.savesData.HeadUP[id] = HeadUP[id]; }
        if (index == 1) { YandexGame.savesData.LevelGun[id] = LevelGun[id]; YandexGame.savesData.GunUP[id] = GunUP[id]; }
        YandexGame.SaveProgress();
    }

    public void Save()
    {
        for (int i = 0; i < 4; i++) { YandexGame.savesData.Resource[i] = Resource[i]; }
        for (int i = 0; i < 3; i++)
        {
            YandexGame.savesData.ScienceScore[i] = ScienceScore[i];
            YandexGame.savesData.TanksPrem[i] = TanksPrem[i];
        }
        for (int i = 0; i < 9; i++)
        {
            YandexGame.savesData.LevelTrack[i] = LevelTrack[i];
            YandexGame.savesData.TrackUP[i] = TrackUP[i];
            YandexGame.savesData.LevelGun[i]= LevelGun[i];
            YandexGame.savesData.GunUP[i] = GunUP[i];
            YandexGame.savesData.ScienceTrack[i] = ScienceTrack[i];
            YandexGame.savesData.ScienceGun[i] = ScienceGun[i];
        }
        for (int i = 0; i < 13; i++)
        {
            YandexGame.savesData.LevelHead[i] = LevelHead[i];
            YandexGame.savesData.HeadUP[i] = HeadUP[i];
            YandexGame.savesData.ScienceHead[i] = ScienceHead[i];
        }
        for (int i = 0; i < 5; i++)
        {
            YandexGame.savesData.ChapterDone[i] = ChapterDone[i];
            YandexGame.savesData.LevelGameStage[i] = LevelGameStage[i];
            YandexGame.savesData.SaveLevelStage[i] = SaveLevelStage[i];
            YandexGame.savesData.SlotBuy[i] = TankList[i].SlotBuy;
            if (TankList[i].SlotBuy)
            {
                YandexGame.savesData.TankDone[i] = TankList[i].TankDone;
                if (TankList[i].TankDone)
                {
                    YandexGame.savesData.PremTanks[i] = TankList[i].PremTanks;
                    YandexGame.savesData.TrackTanks[i] = TankList[i].TrackTanks;
                    YandexGame.savesData.HeaderTanks[i] = TankList[i].HeaderTanks;
                    YandexGame.savesData.MachineGunTanks[i] = TankList[i].MachineGunTanks;
                    YandexGame.savesData.GunsTanks[i] = TankList[i].GunsTanks;
                    YandexGame.savesData.LazerTanks[i] = TankList[i].LazerTanks;
                    YandexGame.savesData.RocketTanks[i] = TankList[i].RocketTanks;
                    YandexGame.savesData.Sheild[i] = TankList[i].Sheild;
                    YandexGame.savesData.SizeTank[i] = TankList[i].SizeTank;
                    YandexGame.savesData.LifeTank[i] = TankList[i].LifeTank;
                    YandexGame.savesData.SpeedTank[i] = TankList[i].SpeedTank;
                    YandexGame.savesData.DamageGun[i] = TankList[i].DamageGun;
                }
            }
        }
        for(int i = 0; i < 6; i++) { YandexGame.savesData.LevelAchives[i] = LevelAchives[i]; YandexGame.savesData.ProgressAchives[i] = ProgressAchives[i]; }
        for (int i = 0; i < 8; i++)
        {
            YandexGame.savesData.ActiveTask[i] = TaskList[i].ActiveTask;
            YandexGame.savesData.CollectAwards[i] = TaskList[i].CollectAwards;
            YandexGame.savesData.NeedPerform[i] = TaskList[i].NeedPerform;
            YandexGame.savesData.Perform[i] = TaskList[i].Perform;
            YandexGame.savesData.AwardsTask[i] = TaskList[i].AwardsTask;
        }
        YandexGame.savesData.AwardsDay = AwardsDay;
        YandexGame.savesData.ActiveAudio = ActiveAudio;
        YandexGame.savesData.LevelGameChoice = LevelGameChoice;
        YandexGame.savesData.Trainer[0] = Trainer[0]; YandexGame.savesData.Trainer[1] = Trainer[1];
        SaveActive1 = true;
        YandexGame.savesData.SaveActive1 = SaveActive1;
        YandexGame.savesData.OffAds = OffAds;
        YandexGame.savesData.PremAll = PremAll;
        YandexGame.NewLeaderboardScores("LeaderBoard", ProgressAchives[2]);
        YandexGame.SaveProgress(); //
    }

    public void DeleteSave()
    {
        YandexGame.ResetSaveProgress();
        //YandexGame.DeleteAllPurchases();
        SaveActive1 = false;
        YandexGame.savesData.SaveActive1 = SaveActive1;
        YandexGame.SaveProgress();
        SceneManager.LoadScene(0);
        Destroy(Pool); Destroy(this.gameObject);
    }

    //ADS
    void FullscreenAds()
    {
        if (!OffAds) { YandexGame.FullscreenShow(); }
    }

    //METRICA
    public void Metrica(int index)
    {
        if (index == 0) { YandexMetrica.Send("bonusday7"); }
        if (index == 1) { YandexMetrica.Send("daytask"); }
        if (index == 2) { YandexMetrica.Send("boss1"); }
        if (index == 3) { YandexMetrica.Send("boss2"); }
        if (index == 4) { YandexMetrica.Send("boss3"); }
        if (index == 5) { YandexMetrica.Send("boss4"); }
        if (index == 6) { YandexMetrica.Send("boss5"); }
    }
}

[Serializable]
public class TankList
{
    public bool TankDone;
    public bool PremTanks;
    public int TrackTanks;
    public int HeaderTanks;
    public int MachineGunTanks;
    public int GunsTanks;
    public bool LazerTanks;
    public bool RocketTanks;
    public int Sheild;
    public int SizeTank;
    //
    public int LifeTank;
    public float SpeedTank;
    public int DamageGun;
    //
    public bool SlotBuy;
    public int[] SlotCost;

}
[Serializable]
public class TaskList
{
    public string NameTask;
    public bool ActiveTask;
    public bool ChoiceTask;
    public bool CollectAwards;
    public int NeedPerform;
    public int Perform;
    public int AwardsTask;
}
