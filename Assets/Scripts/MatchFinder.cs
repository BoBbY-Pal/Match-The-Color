using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

// using UnityEngine;

public class MatchFinder 
{
    private GridManager gridManager; // Assume this is set up correctly elsewhere

    // Constructor to set the GridManager
    public MatchFinder(GridManager gridManager)
    {
        this.gridManager = gridManager;
    }

    public List<Block> FindMatchingBlocks(List<Block> blocks)
    {
        Debug.Log("Finding matches");
        List<Block> matchedBlocks = new List<Block>();
        HashSet<Block> visited = new HashSet<Block>();

        foreach (Block block in blocks)
        {
            if (!visited.Contains(block))
            {
                List<Block> currentMatches = new List<Block>();
                FindAdjacentMatches(block, block.colorTag, currentMatches, visited);
                if (currentMatches.Count >= 4)
                {
                    matchedBlocks.AddRange(currentMatches);
                }
            }
        }

        return matchedBlocks;
    }

    private void FindAdjacentMatches(Block current, string matchColorTag, List<Block> matchedBlocks, HashSet<Block> visited)
    {
        if (current == null || visited.Contains(current) || current.colorTag != matchColorTag)
            return;

        // Mark this block as visited and add to matched list
        visited.Add(current);
        matchedBlocks.Add(current);

        // Check in all four directions
        Block up = gridManager.GetBlockAt(current.RowId - 1, current.ColumnId);
        Block down = gridManager.GetBlockAt(current.RowId + 1, current.ColumnId);
        Block left = gridManager.GetBlockAt(current.RowId, current.ColumnId - 1);
        Block right = gridManager.GetBlockAt(current.RowId, current.ColumnId + 1);

        FindAdjacentMatches(up, matchColorTag, matchedBlocks, visited);
        FindAdjacentMatches(down, matchColorTag, matchedBlocks, visited);
        FindAdjacentMatches(left, matchColorTag, matchedBlocks, visited);
        FindAdjacentMatches(right, matchColorTag, matchedBlocks, visited);
    }
}