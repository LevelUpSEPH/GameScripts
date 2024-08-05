using System.Collections;
using System.Collections.Generic;

namespace Chameleon.Game.ArcadeIdle
{
    public class CustomerNeeds
    {
        private List<PoopType> _needs = new List<PoopType>();

        public void ResetNeeds()
        {
            _needs.Clear();
        }

        public void AddToNeeds(PoopType poopType)
        {
            _needs.Add(poopType);
        }

        public void RemoveFromNeeds(PoopType poopType)
        {
            _needs.Remove(poopType);
        }

        public PoopType GetNextNeed()
        {
            for(int i = 0; i < _needs.Count; i++)
            {
                if(_needs[i] != PoopType.None)
                    return _needs[i];
            }
            return PoopType.None;
        }

        public List<PoopType> GetNeeds()
        {
            return _needs;
        }
    }
}