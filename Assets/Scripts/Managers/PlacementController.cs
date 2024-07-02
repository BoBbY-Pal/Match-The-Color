using System;
using System.Collections;
using System.Collections.Generic;

using DG.Tweening;
using Managers;
using ScriptableObjects;
using UnityEngine;


public class PlacementController : MonoBehaviour
{
    private Block currentBlock;
    [SerializeField] private GridManager gridManager;

    /// <summary>
    /// List of blocks on grid which are active, means they're not permanently placed yet and can be restored if player fails to place all the cells from the container.
    /// </summary>
    public List<Block> activeGridBlocks = new List<Block>();
    
    public MatchFinder matchFinder;

    private readonly float inBetweenDelay = 0.1f;
    private WaitForSeconds clearBlockRoutine;

    private void Awake()
    {
        clearBlockRoutine = new WaitForSeconds(inBetweenDelay);
    }

    private void OnEnable()
    {
        matchFinder = new MatchFinder(gridManager);
    }

    private void Update()
    {
        if (GameManager.Instance.isGameOver)
            return;
        
        if (UnityEngine.Input.GetMouseButtonDown(0))
        {
            OnPointerDown();
        }
        else if (UnityEngine.Input.GetMouseButton(0))
        {
            OnDrag();
        }
        else if (UnityEngine.Input.GetMouseButtonUp(0))
        {
            OnPointerUp();
        }
    }

    public void OnPointerDown()
    {
        Debug.Log("ONPointerDown");
        CheckBlock( true);
    }

    public void OnDrag()
    {
        CheckBlock(false);
    }

    public void OnPointerUp()
    {
        Debug.Log("ONPointerUp");
        currentBlock = null; // Reset current block reference when the user stops dragging.

        
        if (GameManager.Instance.IsAllTheCellsUtilised())
        {
            StartCoroutine(CheckForTheMatch(activeGridBlocks));
            CellsController.Instance.RefillColorCells();
            activeGridBlocks.Clear();
        }
        else
        {
            Debug.Log("BLocks not utilised");
            
            // Prevent playing sound when user has not placed any single cell.
            if (activeGridBlocks.Count > 0)
            {
                SoundManager.Instance.Play(SoundTypes.CellRestore);
            }

            GameManager.Instance.RestoreActiveCells();
            ResetActiveBlocks();
        }

        CanPlaceCells();
    }

    private void CanPlaceCells()
    {
        bool canPlace = gridManager.CheckAdjacentSpaces(GameManager.Instance.GetCurrentCellsCount());
        if (!canPlace)
        {
            UiManager.Instance.Gameover();
        }
    }

    private void CheckBlock( bool initialize = false)
    {
        if (GameManager.Instance.IsAllTheCellsUtilised())
                    return;
        
        Vector2 mousePosition = Input.mousePosition;
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, 1f);
        
        if (hit.collider != null)
        {
            Debug.Log($"Hit {hit.collider.gameObject.name}", hit.collider.gameObject);
            Block block = hit.collider.GetComponent<Block>();
            if (block != null)
            {
                if (initialize)
                {
                    currentBlock = block; // Set the initial block from where dragging starts.
                }
                // Check if the current block is the same as the initially touched block or adjacent to it
                if (block == currentBlock || (IsAdjacent(currentBlock, block) ))
                {
                    if (block.isOccupied)
                        return;
                    
                    block.isOccupied = true;
                    ColorAndTag colorAndTag = GameManager.Instance.GetCellColorAndTag();
                    block.colorTag = colorAndTag.colorTag;
                    Color color = colorAndTag.color;
                    color.a = 1f;
                    block.blockImage.color = color; // Change color to green if it's the current or an adjacent block.
                    SoundManager.Instance.Play(SoundTypes.CellPlace);

                    block.blockImage.rectTransform.DOScale(Vector3.one, 0.5f);
                    currentBlock = block; // Update the current block to the new one.
                    activeGridBlocks.Add(block);
                }
                else
                {
                    Debug.Log("Not adjacent");
                }
            }
        }
    }

    private bool IsAdjacent(Block first, Block second)
    {
        if (first == null || second == null)
            return false;

        // Calculate the difference in row and column positions.
        int dx = Mathf.Abs(first.RowId - second.RowId);
        int dy = Mathf.Abs(first.ColumnId - second.ColumnId);

        // Check for horizontal adjacency where dy == 0 and dx == 1, or vertical adjacency where dx == 0 and dy == 1.
        return (dx == 1 && dy == 0) || (dx == 0 && dy == 1);
    }

    public void ResetActiveBlocks()
    {
        foreach (Block activeBlock in activeGridBlocks)
        {
            activeBlock.ResetBlock(0.01f);
        }
        activeGridBlocks.Clear();
    }
    
    private bool IsDiagonal(Block first, Block second)
    {
        if (first == null || second == null)
            return false;

        // Calculate the difference in row and column positions.
        int dx = Mathf.Abs(first.RowId - second.RowId);
        int dy = Mathf.Abs(first.ColumnId - second.ColumnId);

        // Check for diagonal adjacency where dx == 1 and dy == 1.
        return dx == 1 && dy == 1;
    }
    
    public IEnumerator CheckForTheMatch(List<Block> activeBlocks)
    {
        List<Block> matchedBlocks =  matchFinder.FindMatchingBlocks(activeBlocks);
        if (matchedBlocks != null && matchedBlocks.Count >= 4)
        {
            Debug.Log($"Matches found: {matchedBlocks.Count}");

            SoundManager.Instance.Play(SoundTypes.CellClear);
            foreach (Block block in matchedBlocks)
            {
                block.ResetBlock(1f);
                ScoreManager.Instance.UpdateScore(10);
                yield return clearBlockRoutine;
            }
        }
    }
    private void OnDisable()
    {
        Debug.Log("On Disable");
        currentBlock = null;
        activeGridBlocks.Clear();
        matchFinder = null;
    }
}
