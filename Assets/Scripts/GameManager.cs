using System;
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
        public Queue<ColorAndTag> activeColorsAndCell = new Queue<ColorAndTag>();


        private void Start()
        {
            StartGame();
        }

        public void StartGame()
        {
            gridManager.CreateGrid();
        }
        
        public ColorAndTag GetCellColorAndTag()
        {
            ColorAndTag colorAndTag = cellColorManager.colorQueue.Dequeue();
            colorAndTag.img.rectTransform.localScale = Vector3.one * 0.6f;
            Color color = colorAndTag.img.color;
            color.a = 0.5f;
            colorAndTag.img.color = color;
            activeColorsAndCell.Enqueue(colorAndTag);
            return colorAndTag;
        }

        public void ResetActiveColors()
        {
            foreach (ColorAndTag activeColorCell in activeColorsAndCell)
            {
                // ColorAndTag colorAndTag = activeColorsAndCell.Dequeue();
                activeColorCell.img.rectTransform.localScale = Vector3.one;
                Color color = activeColorCell.img.color;
                color.a = 1f;
                activeColorCell.img.color = color;
                cellColorManager.colorQueue.Enqueue(activeColorCell);
                Debug.Log("resetActiveCOlorCell");
            }
            
            activeColorsAndCell.Clear();
        }

        public void RefillColorCells()
        {
            cellColorManager.PrepareCells();
            activeColorsAndCell.Clear();
        }
        
        public bool IsAllTheCellsUtilised()
        {
            return cellColorManager.colorQueue.Count == 0;
        }
    }
}