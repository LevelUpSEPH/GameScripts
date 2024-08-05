using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RocketUtils.SerializableDictionary;

namespace Chameleon.Game.Scripts.Model
{
    public class HexagonMaterials : Singleton<HexagonMaterials>
    {
        [SerializeField] private HexagonMaterialByColor _frogMaterialByColor = new HexagonMaterialByColor();
        [SerializeField] private HexagonMaterialByColor _fruitMaterialByColor = new HexagonMaterialByColor();
        [SerializeField] private HexagonMaterialByColor _hexMaterialByColor = new HexagonMaterialByColor();
        [SerializeField] private ArrowImageByColor _arrowImageByColor = new ArrowImageByColor();

        public Material GetHexMaterialOfColor(GridColor gridColor)
        {
            return GetMaterialOfColorFromDictionary(gridColor, _hexMaterialByColor);
        }

        public Material GetFrogMaterialOfColor(GridColor gridColor)
        {
            return GetMaterialOfColorFromDictionary(gridColor, _frogMaterialByColor);
        }

        public Material GetFruitMaterialOfColor(GridColor gridColor)
        {
            return GetMaterialOfColorFromDictionary(gridColor, _fruitMaterialByColor);
        }

        public Sprite GetArrowIconOfColor(GridColor gridColor)
        {
            if(_arrowImageByColor.TryGetValue(gridColor, out Sprite arrowImage))
                return arrowImage;
            Debug.LogError("Arrow image not found for color: " + gridColor);
            return null;
        }

        private Material GetMaterialOfColorFromDictionary(GridColor gridColor, HexagonMaterialByColor targetDictionary)
        {
            if(targetDictionary.TryGetValue(gridColor, out Material gridMaterial))
                return gridMaterial;
            return null;
        }
    }

    [System.Serializable]
    public class HexagonMaterialByColor : SerializableDictionary<GridColor, Material>{}

    [System.Serializable]
    public class ArrowImageByColor : SerializableDictionary<GridColor, Sprite>{}

}