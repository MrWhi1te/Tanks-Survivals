using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class Fortune : MonoBehaviour
{
    public Player PL;
    public LevelManager LM;
    public Game GM;

    public int[] SectorValue;
    public string[] TextRewarder;
    public int[] RewarderValue;
    int f;
    public float Value;

    public int LevelChoice;

    [Header("UI")]
    public Text[] TextRewardSectorText;
    public Text TextReward;
    public GameObject[] FortuneBttn;

    // Start is called before the first frame update
    void Start()
    {
        GM = Game.Instance;
        PL = Player.Instance;
        LM = LevelManager.Instance;
        LevelChoice = GM.LevelGameChoice;
    }
    private void OnEnable()
    {
        YandexGame.RewardVideoEvent += Rewarded;
        FortuneBttn[1].SetActive(true); FortuneBttn[2].SetActive(false); FortuneBttn[0].SetActive(false);
        RewarderValue[0] = Random.Range(30, 100)* (LevelChoice+1); RewarderValue[2] = Random.Range(50, 160)* (LevelChoice + 1); RewarderValue[4] = Random.Range(80, 220)* (LevelChoice + 1);
        RewarderValue[1] = Random.Range(20, 60)* (LevelChoice + 1); RewarderValue[5] = Random.Range(40, 100)* (LevelChoice + 1);
        RewarderValue[3] = Random.Range(10, 30)* (LevelChoice + 1);
        for(int i = 0; i < SectorValue.Length; i++) { TextRewardSectorText[i].text = RewarderValue[i].ToString(); }
        TextReward.text = "";
    }

    private void OnDisable() => YandexGame.RewardVideoEvent -= Rewarded;

    public void Spin()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        StartCoroutine(SpinFortune()); FortuneBttn[1].SetActive(false); FortuneBttn[2].SetActive(false); FortuneBttn[0].SetActive(false);
    }

    IEnumerator SpinFortune()
    {
        float Speed = 0.02f;
        int NumberSector = Random.Range(24, 36);

        for(int i = 0; i < NumberSector; i++)
        {
            transform.Rotate(0, 0, 60f);
            if (i > Mathf.RoundToInt(NumberSector * 0.5f)){Speed = 0.05f;}
            if (i > Mathf.RoundToInt(NumberSector * 0.6f)) { Speed = 0.09f; }
            if (i > Mathf.RoundToInt(NumberSector * 0.8f)) { Speed = 0.1f; }
            yield return new WaitForSeconds(Speed);
        }
        if (Mathf.RoundToInt(transform.eulerAngles.z) % 60 != 0) { transform.Rotate(0, 0, 60f); }
        //for(int i = 0; i < SectorValue.Length; i++)
        //{
        //    if(Mathf.RoundToInt(transform.eulerAngles.z) == SectorValue[i]) { f = i; }
        //}
        Value = Mathf.RoundToInt(transform.eulerAngles.z);
        switch (Value)
        {
            case 0:
                f = 0;
                break;
            case 60:
                f = 1;
                break;
            case 120:
                f = 2;
                break;
            case 180:
                f = 3;
                break;
            case 240:
                f = 4;
                break;
            case 300:
                f = 5;
                break;
        }
        TextReward.text = "Награда!" + "\n" + TextRewarder[f] + " " + RewarderValue[f];
        if (f == 0 || f == 2 || f == 4) { PL.Resource[0] += RewarderValue[f]; }
        if (f == 1 || f == 5) { PL.Resource[1] += RewarderValue[f]; }
        if (f == 3) { PL.Resource[2] += RewarderValue[f]; }
        PL.ResourceUpdate();
        for (int i = 0; i < RewarderValue.Length; i++) { RewarderValue[i] /= 2; TextRewardSectorText[i].text = RewarderValue[i].ToString(); }
        FortuneBttn[2].SetActive(true); FortuneBttn[0].SetActive(true);
        yield break;
    }

    //ADS!
    void Rewarded(int id)
    {
        if (id == 0) //
        {
            Spin();
        }
        if (id == 1)
        {

        }
    }
    public void ExampleOpenRewardAd(int id)
    {
        // Вызываем метод открытия видео рекламы
        YandexGame.RewVideoShow(id);
    }
}
