using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Owner.Script.GamePlay.SpecialItem
{
    public class SpeedUpSI : BuffPlayerItem, IPlayerUseable
    {
        public float SpeedScale { get; set; } = 1.0f;
        public float SpeedFlat { get; set; } = 0.0f;

        public void Use()
        {
            Player.CustomCalcSpeed = (e) =>
            {
                return e * SpeedScale + SpeedFlat;
            };
        }
    }
}
