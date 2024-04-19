using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "ScriptableObject/Cell Colors", fileName = "CellColorsData")]
    public class CellColorData : ScriptableObject
    {
        public List<ColorAndTag> cellColors = new List<ColorAndTag>();
    }

    [Serializable]
    public struct ColorAndTag
    {
        public Color color;
        [NonSerialized] public Image img;
        public string colorTag;
    }
}