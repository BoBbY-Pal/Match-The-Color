using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    public int columnSize = 6;
    public int rowSize = 9;
    public float blockSize = 100;
    public float blockSpace = 2;
    public GameObject blockPrefab;
    public Transform gridParent;

    private Block[,] gridArray;

    void Start()
    {
        
    }

    public void CreateGrid()
    {
            gridArray = new Block[rowSize, columnSize];

            // Starting points represents point from where block shape grid should start inside block shape.
            float startPointX = GetStartPointX(blockSize, columnSize);
            float startPointY = GetStartPointY(blockSize, rowSize);

            // Will keep updating with iterations.
            float currentPositionX = startPointX;
            float currentPositionY = startPointY;
            
            
            // Iterates through all rows and columns to generate grid.
            for (int row = 0; row < rowSize; row++)
            {

                for (int column = 0; column < columnSize; column++)
                {
                    // Spawn a block instance and prepares it.
                    RectTransform blockElement = GetBlockInsideGrid();
                    blockElement.localPosition = new Vector3(currentPositionX, currentPositionY, 0);
                    currentPositionX += (blockSize + blockSpace);
                    blockElement.sizeDelta = Vector3.one * blockSize;
                    blockElement.GetComponent<Image>().color = Color.gray;
                    blockElement.GetComponent<BoxCollider2D>().size = (Vector3.one * blockSize * 0.6f);;
                    blockElement.name = "block-" + row + "" + column;

                    // Sets blocks logical position inside grid and its default sprite.
                    Block block = blockElement.GetComponent<Block>();
                    block.blockImage.rectTransform.sizeDelta = (Vector3.one * blockSize * 0.9f);
                    block.SetBlockLocation(row, column);
                    block.gameObject.SetActive(true);
                    gridArray[row, column] = block;
                }
                currentPositionX = startPointX;
                currentPositionY -= (blockSize + blockSpace);

      
            }

    }
                
    public RectTransform GetBlockInsideGrid()
    {
        GameObject block = (GameObject)(Instantiate(blockPrefab, gridParent)) as GameObject;
        block.transform.localScale = Vector3.one;
        return block.GetComponent<RectTransform>();
    }
    
    /// <summary>
    /// Horizontal position from where block grid will start.
    /// </summary>
    public float GetStartPointX(float blockSize, int rowSize)
    {
        float totalWidth = (blockSize * rowSize) + ((rowSize - 1) * blockSpace);
        return -((totalWidth / 2) - (blockSize / 2));
    }

    /// <summary>
    /// Vertical position from where block grid will start.
    /// </summary>
    public float GetStartPointY(float blockSize, int columnSize)
    {
        float totalHeight = (blockSize * columnSize) + ((columnSize - 1) * blockSpace);
        return ((totalHeight / 2) - (blockSize / 2));
    }
}