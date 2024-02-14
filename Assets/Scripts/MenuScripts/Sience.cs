using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sience : MonoBehaviour
{
    public Game GM;
    public TanksConstructor TK;

    [Header("Main")]
    public Text[] ScienceResourceText;

    [Header("InfoPan")]
    public GameObject InfoPan;
    public Button ScienceBttn;
    public Text[] NameCostScience;
    public Image InfoScienceImage;
    public Sprite[] ScienceInfoSprite;

    [Header("ScienceObj")]
    public Image[] ImageTrack;
    public Image[] ImageHead;
    public Image[] ImageGun;
    public int[] CostTrack;
    public int[] CostHead;
    public int[] CostGun;
    int[] ScienceChoice = new int[2];

    public AudioSource DoneSource;

    private void Awake()
    {
        GM = Game.Instance;
    }
    private void OnEnable()
    {
        OpenSciencePan();
        UpdateText(); InfoPan.SetActive(false);
    }
    void UpdateText()
    {
        for(int i = 0; i < 3; i++) { ScienceResourceText[i].text = GM.ScienceScore[i].ToString(); }
    }
    void OpenSciencePan()
    {
        for(int i = 0; i < GM.ScienceTrack.Length; i++) { if (GM.ScienceTrack[i]) { ImageTrack[i].color = new Color32(255, 255, 255, 255); } }
        for (int i = 0; i < GM.ScienceHead.Length; i++) { if (GM.ScienceHead[i]) { ImageHead[i].color = new Color32(255, 255, 255, 255); } }
        for (int i = 0; i < GM.ScienceGun.Length; i++) { if (GM.ScienceGun[i]) { ImageGun[i].color = new Color32(255, 255, 255, 255); } }
    }
    public void OpenInfoPanTrack(int index)
    {
        ScienceChoice[0] = 0; ScienceChoice[1] = index;
        InfoScienceImage.sprite = ScienceInfoSprite[0];
        NameCostScience[0].text = TK.TrackConstructor[index].Name;
        NameCostScience[1].text = CostTrack[index].ToString();
        if (index > 0)
        {
            if(GM.ScienceTrack[index - 1])
            {
                if (!GM.ScienceTrack[index])
                {
                    if(GM.ScienceScore[0] >= CostTrack[index]){NameCostScience[2].text = "Исследовать"; ScienceBttn.interactable = true;}
                    else { NameCostScience[2].text = "Не хватает очков!"; ScienceBttn.interactable = false; }
                }
                else { NameCostScience[2].text = "Уже открыто!"; ScienceBttn.interactable = false; }
            }
            else{NameCostScience[2].text = "Откройте предыдущий!"; ScienceBttn.interactable = false;}
        }
        else { NameCostScience[2].text = "Уже открыто!"; ScienceBttn.interactable = false; }
        InfoPan.SetActive(true);
    }
    public void OpenInfoPanHead(int index)
    {
        ScienceChoice[0] = 1; ScienceChoice[1] = index;
        InfoScienceImage.sprite = ScienceInfoSprite[1];
        NameCostScience[0].text = TK.HeadConstructor[index].Name;
        NameCostScience[1].text = CostHead[index].ToString();
        if (index > 0)
        {
            if (GM.ScienceHead[index - 1])
            {
                if (!GM.ScienceHead[index])
                {
                    if (GM.ScienceScore[1] >= CostHead[index]) { NameCostScience[2].text = "Исследовать"; ScienceBttn.interactable = true; }
                    else { NameCostScience[2].text = "Не хватает очков!"; ScienceBttn.interactable = false; }
                }
                else { NameCostScience[2].text = "Уже открыто!"; ScienceBttn.interactable = false; }
            }
            else { NameCostScience[2].text = "Откройте предыдущий!"; ScienceBttn.interactable = false; }
        }
        else { NameCostScience[2].text = "Уже открыто!"; ScienceBttn.interactable = false; }
        InfoPan.SetActive(true);
    }
    public void OpenInfoPanGun(int index)
    {
        ScienceChoice[0] = 2; ScienceChoice[1] = index;
        InfoScienceImage.sprite = ScienceInfoSprite[2];
        NameCostScience[0].text = TK.GunConstructor[index].Name;
        NameCostScience[1].text = CostGun[index].ToString();
        if (index > 0)
        {
            if (GM.ScienceGun[index - 1])
            {
                if (!GM.ScienceGun[index])
                {
                    if (GM.ScienceScore[2] >= CostGun[index]) { NameCostScience[2].text = "Исследовать"; ScienceBttn.interactable = true; }
                    else { NameCostScience[2].text = "Не хватает очков!"; ScienceBttn.interactable = false; }
                }
                else { NameCostScience[2].text = "Уже открыто!"; ScienceBttn.interactable = false; }
            }
            else { NameCostScience[2].text = "Откройте предыдущий!"; ScienceBttn.interactable = false; }
        }
        else { NameCostScience[2].text = "Уже открыто!"; ScienceBttn.interactable = false; }
        InfoPan.SetActive(true);
    }
    public void BuyScience()
    {
        if (ScienceChoice[0] == 0) {GM.ScienceScore[0] -= CostTrack[ScienceChoice[1]]; GM.ScienceTrack[ScienceChoice[1]] = true; ImageTrack[ScienceChoice[1]].color = new Color32(255, 255, 255, 255); UpdateText(); if (GM.ActiveAudio) { DoneSource.Play(); } }
        if (ScienceChoice[0] == 1) { GM.ScienceScore[1] -= CostHead[ScienceChoice[1]]; GM.ScienceHead[ScienceChoice[1]] = true; ImageHead[ScienceChoice[1]].color = new Color32(255, 255, 255, 255); UpdateText(); if (GM.ActiveAudio) { DoneSource.Play(); } }
        if (ScienceChoice[0] == 2) { GM.ScienceScore[2] -= CostGun[ScienceChoice[1]]; GM.ScienceGun[ScienceChoice[1]] = true; ImageGun[ScienceChoice[1]].color = new Color32(255, 255, 255, 255); UpdateText(); if (GM.ActiveAudio) { DoneSource.Play(); } }
        InfoPan.SetActive(false);
    }
}
