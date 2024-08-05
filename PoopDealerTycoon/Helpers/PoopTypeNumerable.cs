using System.Collections;
using System.Collections.Generic;
using System;

namespace Chameleon.Game.ArcadeIdle.Helpers
{
    public class PoopTypeNumerable : IEnumerable
    {
        private static bool _isFirstCall = true;
        public static List<PoopType> poopTypes = new List<PoopType>();

        private void InitializeTypesList()
        {
            _isFirstCall = false;
            PoopType poopType = PoopType.NormalPoop;;
            int poopTypeCount = Enum.GetValues(poopType.GetType()).Length;
            for(int i = 1; i < poopTypeCount; i++)
            {
                PoopType typeToAdd = (PoopType)Enum.GetValues(poopType.GetType()).GetValue(i);
                poopTypes.Add(typeToAdd);
            }
        }

        public IEnumerator GetEnumerator()
        {
            if(_isFirstCall)
                InitializeTypesList();
            return poopTypes.GetEnumerator();
        }

    }

}