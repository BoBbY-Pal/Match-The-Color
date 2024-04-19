using System;
using System.Collections.Generic;
using DefaultNamespace;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    public CellColorManager cellColorManager;
 
    private Block currentBlock;
    public List<Block> activeBlocks = new List<Block>();

  
    void Start()
    {
    }

    private void Update()
    {
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
        Debug.Log("ONDrag");

        CheckBlock(false);
    }

    public void OnPointerUp()
    {
        Debug.Log("ONPointerUp");
        if (GameManager.Instance.IsAllTheCellsUtilised())
        {
            GameManager.Instance.RefillColorCells();
            activeBlocks.Clear();
        }
        else
        {
            Debug.Log("BLocks not utilised");
            GameManager.Instance.ResetActiveColors();
            ResetActiveBlocks();
        }
        currentBlock = null; // Reset current block reference when the user stops dragging.
    }
    
    private void CheckBlock( bool initialize = false)
    {  
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
                    currentBlock = block; // Update the current block to the new one.
                    activeBlocks.Add(block);
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
        foreach (Block activeBlock in activeBlocks)
        {
            activeBlock.ResetBlock();
        }
        activeBlocks.Clear();
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

}
