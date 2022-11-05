namespace Owner.Script.Signals
{
    using Assets.Owner.Script.GameData;
    using Owner.Script.GameData;
    using UnityEngine;

    public class ShowPopupSignal
    {
        public Vector3         Position;
        public BattleShipData  BattleShipData;
        public ItemData        ItemData;
        public SpecialItemData SpecialItemData;
    }
}