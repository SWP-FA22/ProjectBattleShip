using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Owner.Script.GameData
{
    internal class ListCurrentPlayers:Singleton<ListCurrentPlayers>
    {
        public List<GameObject> listPlayer = new List<GameObject>();
    }
}
