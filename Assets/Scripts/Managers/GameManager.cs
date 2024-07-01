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
        public GridManager gridManager;
        // public CellColorManager cellColorManager;
        public MatchFinder matchFinder;
        
        /// <summary>
        /// List of cells in container which are active, means they're not permanently placed yet and can be restored if player fails to place all the cells from the container.
        /// </summary>
        public Queue<ColorAndTag> activeColorsAndCell = new Queue<ColorAndTag>();
        
        /// <summary>
        /// list of cells to place on grid, stored in custom type with color, tag and img ref. 
        /// </summary>
        public Queue<ColorAndTag> colorAndCellsToPlace = new Queue<ColorAndTag>();  

        public void StartGame()
        {
            gridManager.CreateGrid();
            CellColorManager.Instance.PrepareCells();
            colorAndCellsToPlace = CellColorManager.Instance.GetNewColors();
            matchFinder = new MatchFinder(gridManager);
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

            colorAndCellsToPlace = CellColorManager.Instance.GetNewColors();
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

        public void CheckForTheMatch(List<Block> activeBlocks)
        {
            List<Block> matchedBlocks =  matchFinder.FindMatchingBlocks(activeBlocks);
            if (matchedBlocks != null && matchedBlocks.Count >= 4)
            {
                Debug.Log($"Matches found: {matchedBlocks.Count}");

                foreach (Block block in matchedBlocks)
                {
                    block.ResetBlock(0.5f);
                    ScoreManager.Instance.UpdateScore(10);
                }
            }
        }

        public void ExitGame()
        {
            gridManager.ClearGrid();
            colorAndCellsToPlace.Clear();
            activeColorsAndCell.Clear();
            matchFinder = null;
        }
    }
}