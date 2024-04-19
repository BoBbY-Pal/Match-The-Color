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
        public MatchFinder matchFinder;
        
        
        public Queue<ColorAndTag> activeColorsAndCell = new Queue<ColorAndTag>();
        public Queue<ColorAndTag> colorAndCells = new Queue<ColorAndTag>();

        private void Start()
        {
            StartGame();
        }

        public void StartGame()
        {
            gridManager.CreateGrid();
            // StartCoroutine(cellColorManager.PrepareCells(0));
            cellColorManager.PrepareCells(0);
            colorAndCells = new Queue<ColorAndTag>(cellColorManager.colorQueue);
            matchFinder = new MatchFinder(gridManager);
        }
        
        public ColorAndTag GetCellColorAndTag()
        {
            ColorAndTag colorAndTag = colorAndCells.Dequeue();
            colorAndTag.img.rectTransform.localScale = Vector3.one * 0.6f;
            Color color = colorAndTag.img.color;
            color.a = 0.5f;
            colorAndTag.img.color = color;
            activeColorsAndCell.Enqueue(colorAndTag);
            return colorAndTag;
        }

        public void RestoreActiveColorCells()
        {
            foreach (ColorAndTag activeColorCell in activeColorsAndCell)
            {
                activeColorCell.img.rectTransform.localScale = Vector3.one;
                Color color = activeColorCell.img.color;
                color.a = 1f;
                activeColorCell.img.color = color;
                Debug.Log("resetActiveCOlorCell");
            }
            activeColorsAndCell.Clear();
            colorAndCells.Clear();

            colorAndCells = new Queue<ColorAndTag>(cellColorManager.colorQueue);
            Debug.Log($"Color Cells refilled: {colorAndCells.Count}");
        }

        public void RefillColorCells()
        {
            ResetActiveCells();
            colorAndCells.Clear();
            activeColorsAndCell.Clear();
            // StartCoroutine(cellColorManager.PrepareCells(0.3f));
            cellColorManager.PrepareCells(0.3f);
            colorAndCells = new Queue<ColorAndTag>(cellColorManager.colorQueue);
            Debug.Log($"Color Cells refilled: {colorAndCells.Count}");
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
            return colorAndCells.Count == 0;
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