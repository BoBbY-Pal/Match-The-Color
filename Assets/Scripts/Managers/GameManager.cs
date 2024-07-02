using System;
using System.Collections.Generic;
using DG.Tweening;
using Script.Utils;
using ScriptableObjects;
using UnityEngine;

namespace Managers
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private GridManager gridManager;
        
        /// <summary>
        /// List of cells in container which are active, means they're not permanently placed yet and can be restored if player fails to place all the cells from the container.
        /// </summary>
        private Queue<ColorAndTag> activeColorsAndCell = new Queue<ColorAndTag>();
        
        /// <summary>
        /// list of cells to place on grid, stored in custom type with color, tag and img ref. 
        /// </summary>
        private Queue<ColorAndTag> colorAndCellsToPlace = new Queue<ColorAndTag>();  

        public bool isGameOver = false;
        public void StartGame()
        {
            gridManager.CreateGrid();
            isGameOver = false;
            CellsController.Instance.PrepareCells();
            colorAndCellsToPlace = CellsController.Instance.GetNewColors();
        }
        
        public ColorAndTag GetCellColorAndTag()
        {
            ColorAndTag colorAndTag = colorAndCellsToPlace.Dequeue();
            colorAndTag.img.rectTransform.DOScale(new Vector3(0.55f,0.55f, 0.55f), 0.5f) ;
            // Color color = colorAndTag.img.color;
            colorAndTag.img.DOFade(0.5f, 0.5f);
            // color.a = 0.5f;
            // colorAndTag.img.color = color;
            activeColorsAndCell.Enqueue(colorAndTag);
            return colorAndTag;
        }

        public void RestoreActiveCells()
        {
            foreach (ColorAndTag activeCell in activeColorsAndCell)
            {
                activeCell.img.rectTransform.DOScale(Vector3.one, 0.3f);
                Color color = activeCell.img.color;
                color.a = 1f;
                activeCell.img.color = color;
                Debug.Log("resetActiveCOlorCell");
            }
            activeColorsAndCell.Clear();
            colorAndCellsToPlace.Clear();

            colorAndCellsToPlace = CellsController.Instance.GetNewColors();
            Debug.Log($"Color Cells refilled: {colorAndCellsToPlace.Count}");
        }

        public void ResetActiveCells()
        {
            foreach (ColorAndTag activeCell in activeColorsAndCell)
            {
                activeCell.img.rectTransform.localScale = Vector3.zero;
                Color color = activeCell.img.color;
                color.a = 0f;
                activeCell.img.color = color;
                Debug.Log("Cell reset success", activeCell.img.gameObject);
            }
            
            colorAndCellsToPlace.Clear();
            activeColorsAndCell.Clear();
        }

      public void AssignNewColors(Queue<ColorAndTag> queue)
      {
          colorAndCellsToPlace = new Queue<ColorAndTag>(queue);
          Debug.Log($"Color Cells refilled: {colorAndCellsToPlace.Count}");
      }
        public bool IsAllTheCellsUtilised()
        {
            return colorAndCellsToPlace.Count == 0;
        }

     

        public void ExitGame()
        {
            gridManager.ClearGrid();
            colorAndCellsToPlace.Clear();
            activeColorsAndCell.Clear();
        }

        public int GetCurrentCellsCount()
        {
            return colorAndCellsToPlace.Count;
        }
    }
}