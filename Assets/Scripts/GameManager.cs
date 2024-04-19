using System.Collections.Generic;
using Script.Utils;
using ScriptableObjects;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameManager : Singleton<GameManager>
    {
        public GridManager gridManager;
        public CellColorManager cellColorManager;
        public Queue<Color> activeColors = new Queue<Color>();

        public void StartGame()
        {
            gridManager.CreateGrid();
            // cellColorManager.PrepareCells();
        }
        
        public ColorAndTag GetCellColorAndTag()
        {
            ColorAndTag colorAndTag = cellColorManager.colorQueue.Dequeue();
            activeColors.Enqueue(colorAndTag.color);
            return colorAndTag;
        }
    }
}