
namespace YG
{
    [System.Serializable]
    public class SavesYG
    {
        // "Технические сохранения" для работы плагина (Не удалять)
        public int idSave;
        public bool isFirstSession = true;
        public string language = "ru";
        public bool promptDone;

        public int[] Resource = new int[4];
        //
        public int[] LevelTrack = new int[9];
        public int[] TrackUP = new int[9]; // Время в бою/ либо количество боев/ либо пройдено расстояние, но х\з
        public int[] LevelHead = new int[13];
        public int[] HeadUP = new int[13]; // Попадания по танку для повышения уровня
        public int[] LevelGun = new int[9];
        public int[] GunUP = new int[9]; // Уничтожение врагов для повышения уровня
        //
        public int[] ScienceScore = new int[3];
        public bool[] ScienceTrack = new bool[9];
        public bool[] ScienceHead = new bool[13];
        public bool[] ScienceGun = new bool[9];
        //
        public bool[] ChapterDone = new bool[5];
        public int[] LevelGameStage = new int[5];
        public int[] SaveLevelStage = new int[5];
        public int LevelGameChoice;
        //
        public int Prem;
        public int[] PremTime = new int[6];
        public bool[] TanksPrem = new bool[3];
        
        //TankLIST
        public bool[] TankDone = new bool[5];
        public bool[] PremTanks = new bool[5];
        public int[] TrackTanks = new int[5];
        public int[] HeaderTanks = new int[5];
        public int[] MachineGunTanks = new int[5];
        public int[] GunsTanks = new int[5];
        public bool[] LazerTanks = new bool[5];
        public bool[] RocketTanks = new bool[5];
        public int[] Sheild = new int[5];
        public int[] SizeTank = new int[5];
        public int[] LifeTank = new int[5];
        public float[] SpeedTank = new float[5];
        public int[] DamageGun = new int[5];
        public bool[] SlotBuy = new bool[5];

        //TASK!
        public bool TaskActive;
        public bool AwardsDay;
        public int[] DayTaskTime = new int[6];
        //TaskList
        public bool[] ActiveTask = new bool[8];
        public bool[] CollectAwards = new bool[8];
        public int[] NeedPerform = new int[8];
        public int[] Perform = new int[8];
        public int[] AwardsTask = new int[8];
        //
        public bool DayActive;
        public int DayBonus;
        public bool TakeBonus;
        public int[] DayBonusTime = new int[6];
        //
        public int[] LevelAchives = new int[6];
        public int[] ProgressAchives = new int[6];

        public bool[] Trainer = new bool[2];
        public bool ActiveAudio;
        public bool SaveActive1;
        public bool OffAds;
        public bool PremAll;
    }
}
