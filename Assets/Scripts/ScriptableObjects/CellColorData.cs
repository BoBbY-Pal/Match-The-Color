using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "ScriptableObject/Cell Colors", fileName = "CellColorsData")]
    public class CellColorData : ScriptableObject
    {
        public List<ColorAndTag> cellColors = new List<ColorAndTag>();
    }

    public struct ColorAndTag
    {
        public Color color;
        public string colorTag;
    }
}