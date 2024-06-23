using System;
using System.Collections.Generic;
using DG.Tweening;
using Script.Utils;
using ScriptableObjects;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameManager : Singleton<GameManager>
    {
        public GridManager gridManager;
        public CellColorManager cellColorManager;
        public MatchFinder matchFinder;
        
        /// <summary>
        /// List of cells in container which are active, means they're not permanently placed yet and can be restored if player fails to place all the cells from the container.
        /// </summary>
        public Queue<ColorAndTag> activeColorsAndCell = new Queue<ColorAndTag>();
        
        /// <summary>
        /// list of cells to place on grid, stored in custom type with color, tag and img ref. 
        /// </summary>
        public Queue<ColorAndTag> colorAndCellsToPlace = new Queue<ColorAndTag>();  

        private void Start()
        {
            StartGame();
        }

        public void StartGame()
        {
            gridManager.CreateGrid();
            cellColorManager.PrepareCells();
            colorAndCellsToPlace = new Queue<ColorAndTag>(cellColorManager.colorQueue);
            matchFinder = new MatchFinder(gridManager);
        }
        
        public ColorAndTag GetCellColorAndTag()
        {
            ColorAndTag colorAndTag = colorAndCellsToPlace.Dequeue();
            colorAndTag.img.rectTransform.DOScale(new Vector3(0.55f,0.55f, 0.55f), 0.5f) ;
            Color color = colorAndTag.img.color;
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

            colorAndCellsToPlace = new Queue<ColorAndTag>(cellColorManager.colorQueue);
            Debug.Log($"Color Cells refilled: {colorAndCellsToPlace.Count}");
        }

        public void RefillColorCells()
        {
            ResetActiveCells();
            colorAndCellsToPlace.Clear();
            activeColorsAndCell.Clear();
            cellColorManager.PrepareCells();
            colorAndCellsToPlace = new Queue<ColorAndTag>(cellColorManager.colorQueue);
            Debug.Log($"Color Cells refilled: {colorAndCellsToPlace.Count}");
        }

        private void ResetActiveCells()
        {
            foreach (ColorAndTag activeCell in activeColorsAndCell)
            {
                activeCell.img.rectTransform.localScale = Vector3.zero;
                Color color = activeCell.img.color;
                color.a = 0f;
                activeCell.img.color = color;
            }
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

                foreach (var block in matchedBlocks)
                {
                    block.ResetBlock();
                    ScoreManager.Instance.UpdateScore(10);
                }
            }
        }
    }
}