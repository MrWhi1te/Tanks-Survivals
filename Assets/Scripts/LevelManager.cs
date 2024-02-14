using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public Game GM;
    public Player PL;

    public GameObject Players;
    public Vector2 PlayerPosition;
    public bool ActiveShoot;
    public bool ActiveMovePlayer;

    public int Level;
    public TextMesh LevelTextVisual;
    int LevelPlayer = 1;
    int xp;
    public int[] CountEnemy;
    public int[] CountFortune; //
    int CounterFortune;

    public GameObject[] EnemyObj;
    public GameObject[] WorldObj;
    public GameObject[] Gate;

    [Header("UI")]
    public Text LevelText;
    public Text LevelPlayerText;
    public Image levelPlayerImage;
    public GameObject FortunePan;
    public GameObject ReadyPan;
    public GameObject[] EndGamePan;
    public Text[] ResourceText;
    public GameObject[] ResourceItemTake;

    [Header("UPPlayer")]
    public GameObject UpPan;
    public Image[] UpImage;
    public Sprite[] UpSprite;
    public int[] UPBttnIndex;
    //
    public bool ExtraLife;
    public bool ExtraShoot;
    public bool ReboundBullet;
    public int ChanceFire;
    public int ChanceFreeze;
    public float SpeedShoot;
    public bool ReturnGame;

    public AudioSource LevelUpSource;
    public AudioSource DeadEnemySource;
    public AudioSource BttnSource;
    public AudioSource EndGameSource;
    public AudioSource WinGameSource;
    public AudioSource FortuneSource;
    public AudioSource DamageTank;

    public AudioSource Music;
    public AudioClip[] MusicObj;
    public AudioClip BossClip;
    public GameObject[] ActivDisMusic;


    private void OnEnable() => YandexGame.RewardVideoEvent += Rewarded;

    // Отписываемся от события открытия рекламы в OnDisable
    private void OnDisable() => YandexGame.RewardVideoEvent -= Rewarded;

    private void Awake()
    {
        GM = Game.Instance;
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        PL = Player.Instance;
        PlayerPosition = Players.transform.position;
        Gate[0].SetActive(true); Gate[1].SetActive(false); 
        if (GM.ChapterDone[GM.LevelGameChoice] & GM.LevelGameStage[GM.LevelGameChoice] >= GM.LevelGameMaxStage[GM.LevelGameChoice]) { Level = 0; }
        else { if (GM.LevelGameStage[GM.LevelGameChoice] > 0) { Level = GM.LevelGameStage[GM.LevelGameChoice] * 5; CounterFortune = GM.LevelGameStage[GM.LevelGameChoice]; } }
        if (GM.SaveLevelStage[GM.LevelGameChoice] > 0) { Level = GM.SaveLevelStage[GM.LevelGameChoice]; }
        StartCoroutine(ReadyGo()); 
        if (GM.ActiveAudio) { if (Level == 15) { Music.clip = BossClip; } else { Music.clip = MusicObj[Random.Range(0, MusicObj.Length)]; } Music.Play(); ActivDisMusic[0].SetActive(true); ActivDisMusic[1].SetActive(false); }
        else { ActivDisMusic[0].SetActive(false); ActivDisMusic[1].SetActive(true); }
        if (GM.TaskList[1].ActiveTask) { GM.TaskList[1].Perform++; }
        if (GM.TaskList[4].ActiveTask) { StartCoroutine(TimeinBattle()); }
    }

    //// Update is called once per frame
    //void Update()
    //{
    //}
    IEnumerator ReadyGo()
    {
        ActiveShoot = false; ActiveMovePlayer = false;
        LevelTextVisual.text = (Level+1).ToString();
        LevelText.text = "Раунд: " + (Level + 1) + "/" + (CountEnemy.Length + 1);
        ReadyPan.SetActive(true);
        yield return new WaitForSeconds(2);
        ReadyPan.SetActive(false);
        CreatWorld();
        yield break; 
    }
    void CreatWorld()
    {
        Gate[0].SetActive(true); Gate[1].SetActive(false);
        Players.transform.position = PlayerPosition;
        if (Level > 0){WorldObj[Level - 1].SetActive(false); EnemyObj[Level - 1].SetActive(false);}
        WorldObj[Level].SetActive(true); EnemyObj[Level].SetActive(true);
        ActiveShoot = true; ActiveMovePlayer = true;
        if(GM.DeviceGame == "mobile" || GM.DeviceGame == "tablet") { PL.AIM.SetActive(true); }
    }

    public void CheckEnemy()
    {
        if (GM.ActiveAudio) { DeadEnemySource.Play(); }
        if (GM.TaskList[2].ActiveTask) { GM.TaskList[2].Perform++; }
        xp++; LevelUP();
        if (CountEnemy[Level] <= 0)
        {
            Gate[0].SetActive(false); Gate[1].SetActive(true); ActiveShoot = false; PL.AIM.SetActive(false);
        }
    }
    IEnumerator TimeinBattle()
    {
        while (true)
        {
            if(GM.TaskList[4].Perform < GM.TaskList[4].NeedPerform){GM.TaskList[4].Perform++; yield return new WaitForSeconds(60);}
            else { yield break; }
        }
    }
    public void ClosedFortune()
    {
        FortunePan.SetActive(false); if (GM.ActiveAudio) { BttnSource.Play(); Music.Play(); }
        StartCoroutine(ReadyGo());
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Level++;
            if (GM.LevelGameStage[GM.LevelGameChoice] >= GM.LevelGameMaxStage[GM.LevelGameChoice]) { }
            else { 
                if (Level >= 5 & Level < 10) { GM.LevelGameStage[GM.LevelGameChoice] = 1; }
                if (Level >= 10 & Level < 15) { GM.LevelGameStage[GM.LevelGameChoice] = 2; }
                if (Level >= 15) { GM.LevelGameStage[GM.LevelGameChoice] = 3; }
            }
            if(Level == 5) { if (GM.ActiveAudio) { Music.clip = MusicObj[Random.Range(0, MusicObj.Length)]; Music.Play(); } }
            if (Level == 10) { if (GM.ActiveAudio) { Music.clip = MusicObj[Random.Range(0, MusicObj.Length)]; Music.Play(); } }
            if (Level == 15) { if (GM.ActiveAudio) { Music.clip = BossClip; Music.Play(); } }
            if (Level < CountEnemy.Length) 
            {
                if (Level == CountFortune[CounterFortune])
                {
                    Music.Pause(); if (GM.ActiveAudio) { FortuneSource.Play(); }
                    FortunePan.SetActive(true); ActiveMovePlayer = false;
                    CounterFortune++;
                }
                else { StartCoroutine(ReadyGo());}
            }
            else { Result(0); EndGamePan[0].SetActive(true); Time.timeScale = 0; }
        }
    }


    //UPLEVEL
    void OpenUP()
    {
        //int r = Random.Range(0, UpSprite.Length);
        //if(r == 0 & ExtraLife) { OpenUP(index); }
        //if(r == 1 & ExtraShoot) { OpenUP(index); }
        //if (r == 2 & ReboundBullet) { OpenUP(index); }
        //if (r == 5 & ChanceFire<100) { OpenUP(index); }
        //if (r == 6 & ChanceFreeze < 100) { OpenUP(index); }
        //if (r == 8 & PL.Life<PL.MaxLife) { OpenUP(index); }
        //UPBttnIndex[index] = r;
        //UpImage[index].sprite = UpSprite[r];
        for(int i = 0; i < 3; i++)
        {
            for(int q = 0; q < 2; q++)
            {
                int r = Random.Range(0, UpSprite.Length);
                if (r == 1 & ExtraShoot) { continue; }
                if (r == 2 & ReboundBullet) { continue; }
                if (r == 4 & ExtraLife || ReturnGame == true) { continue; }
                if (r == 5 & ChanceFire >= 100) { continue; }
                if (r == 6 & ChanceFreeze >= 100) { continue; }
                if (r == 8 & PL.Life >= PL.MaxLife) { continue; }
                UPBttnIndex[i] = r;
                UpImage[i].sprite = UpSprite[r];
                break;
            }
        }
        UpPan.SetActive(true); Music.Pause(); if (GM.ActiveAudio) { LevelUpSource.Play(); } Time.timeScale = 0;
    }
    public void UpLevelChoice(int index)
    {
        LevelChoice(UPBttnIndex[index]); if (GM.ActiveAudio) { BttnSource.Play(); Music.Play(); } 
    }
    void LevelChoice(int index)
    {
        if (index == 0) { PL.MaxLife += PL.MaxLife / 100 * 10; PL.LifeSlider.fillAmount = (float)PL.Life / PL.MaxLife; UpPan.SetActive(false); Time.timeScale = 1; }
        if (index == 1) { ExtraShoot = true; UpPan.SetActive(false); Time.timeScale = 1; }
        if (index == 2) { ReboundBullet = true; UpPan.SetActive(false); Time.timeScale = 1; }
        if (index == 3) 
        {
            if (PL.Damage < 100) { PL.Damage += 8; UpPan.SetActive(false); }
            else { PL.Damage += PL.Damage / 100 * 10; UpPan.SetActive(false); }
            Time.timeScale = 1; 
        }
        if (index == 4) { ExtraLife = true; UpPan.SetActive(false); Time.timeScale = 1; }
        if (index == 5) { ChanceFire+=30; UpPan.SetActive(false); Time.timeScale = 1; }
        if (index == 6) { ChanceFreeze += 30; UpPan.SetActive(false); Time.timeScale = 1; }
        if (index == 7) { SpeedShoot+=0.1f; UpPan.SetActive(false); Time.timeScale = 1; }
        if (index == 8) { PL.Life = PL.MaxLife; PL.CheckHealth(); UpPan.SetActive(false); Time.timeScale = 1; }
        if (index == 9) { PL.SpeedPlayer += 0.1f; UpPan.SetActive(false); Time.timeScale = 1; }
    }
    void LevelUP()
    {
        if (xp >= LevelPlayer * 3) { LevelPlayer++; xp = 0; OpenUP(); } //for (int i = 0; i < 3; i++) { OpenUP(i); } UpPan.SetActive(true); Time.timeScale = 0; }
            LevelPlayerText.text = "Уровень: " + LevelPlayer;
        levelPlayerImage.fillAmount = (float)xp /(LevelPlayer*3);
    }


    //END GAME!
    public void Result(int index)
    {
        if(index == 0) { ResourceText[9].text = "Глава пройдена!"; Music.Stop(); if (GM.ActiveAudio) { WinGameSource.Play(); } EndGamePan[1].SetActive(false); EndGamePan[2].SetActive(false); GM.ChapterDone[GM.LevelGameChoice] = true; GM.SaveLevelStage[GM.LevelGameChoice] = 0; if (GM.ProgressAchives[4] < GM.LevelGameChoice) { GM.ProgressAchives[4] = GM.LevelGameChoice;} }
        if (index == 1) { ResourceText[9].text = "Игра окончена"; Music.Stop(); if (GM.ActiveAudio) { EndGameSource.Play(); } GM.SaveLevelStage[GM.LevelGameChoice] = 0; EndGamePan[2].SetActive(false); if (!ReturnGame) { EndGamePan[1].SetActive(true); } else { EndGamePan[1].SetActive(false); } }
        if(index == 2) { ResourceText[9].text = "Остановимся здесь?"; Music.Pause(); GM.SaveLevelStage[GM.LevelGameChoice] = Level; EndGamePan[1].SetActive(false); EndGamePan[2].SetActive(true); }
        for (int i = 0; i < 4; i++) { ResourceText[i].text = PL.Resource[i].ToString(); }
        for (int i = 4; i < 8; i++) { ResourceText[i].text = (PL.Resource[i-4]*2).ToString(); }
        if (GM.Prem > 0) { ResourceText[8].text = "С премиум вы получаете:"; for (int i = 0; i < 4; i++) { GM.Resource[i] += PL.Resource[i]*2; } }
        else { ResourceText[8].text = "С премиум вы бы получили:"; for (int i = 0; i < 4; i++) { GM.Resource[i] += PL.Resource[i]; } }
    }
    public void ExitMenu(){ PL.OffObject(); if (GM.ActiveAudio) { BttnSource.Play(); } GM.SceneChoice(0); Time.timeScale = 1;}
    public void Menu() { Result(2); if (GM.ActiveAudio) { BttnSource.Play(); } EndGamePan[0].SetActive(true); Time.timeScale = 0; }
    public void Resume() { EndGamePan[0].SetActive(false); if (GM.ActiveAudio) { BttnSource.Play(); Music.Play(); } Time.timeScale = 1; }
    public void ResumeAD() { }

    public void OffOnMusic()
    {
        if (GM.ActiveAudio){ GM.ActiveAudio = false; ActivDisMusic[0].SetActive(false); ActivDisMusic[1].SetActive(true); Music.Stop(); }
        else { GM.ActiveAudio = true; ActivDisMusic[0].SetActive(true); ActivDisMusic[1].SetActive(false); Music.clip = MusicObj[Random.Range(0, MusicObj.Length)]; Music.Play(); }
    }

    //ADS!
    void Rewarded(int id)
    {
        if (id == 1) //
        {
            EndGamePan[1].SetActive(false); EndGamePan[2].SetActive(true); ReturnGame = true;
            PL.Life = PL.MaxLife; PL.CheckHealth(); Time.timeScale = 0;
        }
    }
    public void ExampleOpenRewardAd(int id)
    {
        // Вызываем метод открытия видео рекламы
        YandexGame.RewVideoShow(id);
    }
}
