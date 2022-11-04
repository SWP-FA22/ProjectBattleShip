using Owner.Script.GameData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;

namespace Assets.Owner.Script.GamePlay.SpecialItem
{
    public class SpecialItemManage : IPlayerUseable
    {
        public Dictionary<Type, IPlayerUseable> SIContainer { get; set; } = new();

        public void Add(IPlayerUseable playerUseable)
        {
            if (playerUseable == this)
                SIContainer.Add(playerUseable.GetType(), playerUseable);
        }

        public void Remove(IPlayerUseable playerUseable)
        {
            SIContainer.Remove(playerUseable.GetType());
        }

        public void Clear()
        {
            SIContainer.Clear();
        }

        public void AddRange(IEnumerable<IPlayerUseable> playerUseables)
        {
            SIContainer.AddRange(playerUseables.Select(e => new KeyValuePair<Type, IPlayerUseable>(e.GetType(), e)));
        }

        public void Use()
        {
            foreach (var item in SIContainer.Values)
            {
                if (item != this)
                    item.Use();
            }
        }
    }
}
