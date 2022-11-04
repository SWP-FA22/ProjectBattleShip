using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Owner.Script.GamePlay.SpecialItem
{
    public class HealthUpSI : BuffPlayerItem, IPlayerUseable
    {
        public float HealthScale { get; set; } = 1.0f;
        public float HealthFlat { get; set; } = 0.0f;

        public void Use()
        {
            Player.CustomCalcHP = (e) =>
            {
                return e * HealthScale + HealthFlat;
            };
        }
    }
}
