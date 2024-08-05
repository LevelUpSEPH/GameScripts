using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chameleon.Game.ArcadeIdle
{
    public class ScrapStashBehavior : MonoBehaviour
    {
        private List<Scrap> _scrapsInZone = new List<Scrap>();

        public bool TryCollectScrapFromZone(out Scrap scrapToCollect)
        {
            if(!GetHasScrap())
            {
                scrapToCollect = null;
                return false;   
            }
            
            scrapToCollect = _scrapsInZone[0];
            _scrapsInZone.RemoveAt(0);

            if(scrapToCollect != null)
            {
                scrapToCollect.SetCanBeCollected(false);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void PlaceScrapInZone(Scrap scrapToPlace)
        {
            _scrapsInZone.Add(scrapToPlace);
        }
        
        public bool GetHasScrap()
        {
            return _scrapsInZone.Count > 0;
        }
        
    }
}