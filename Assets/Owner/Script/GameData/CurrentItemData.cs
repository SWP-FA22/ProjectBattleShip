namespace Owner.Script.GameData
{
    using System.Collections.Generic;
    using Assets.Owner.Script.GameData;

    public class CurrentItemData : Singleton<CurrentItemData>
    {
        public List<ItemData> Items = new();
    }
}