namespace Owner.Script.GameData
{
    using System.Collections;
    using System.Collections.Generic;
    using Assets.Owner.Script.GameData;

    public class CurrentSpecialItem : Singleton<CurrentSpecialItem>
    {
        public Dictionary<int, SpecialItemData> SpecialData = new();
    }
}