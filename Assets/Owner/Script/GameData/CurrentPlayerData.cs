namespace Owner.Script.GameData
{
    using System.Collections.Generic;

    public class CurrentPlayerData : Singleton<CurrentPlayerData>
    {
        public float        ATK;
        public float        BaseHP;
        public float        Speed;
        public float        Rotate;
        public float        Rate;
        public List<string> SpecialItems;
        public int          Score;
    }
}