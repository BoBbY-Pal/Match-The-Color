using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Block : MonoBehaviour
{
    
    // Returns rowId 
    public int RowId
    {
        get
        {
            return _rowId;
        }
        private set
        {
            _rowId = value;
        }
    }
    
    //Returns columnId
    public int ColumnId
    {
        get
        {
            return _columnId;
        }
        private set
        {
            _columnId = value;
        }
    }
    // Represents row id of block in the grid.
    private int _rowId;

    // Represents columnId id of block in the grid.
    private int _columnId;

    public bool isOccupied = false;

    public string colorTag = "";
    // Box collide attached to this block.
    public BoxCollider2D thisCollider { get; private set; }
    
    // Image component on the block. Assigned from Inspector.
    [SerializeField] public Image blockImage;
    private void Awake()
    {
        thisCollider = gameObject.GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        
    }


    public void ResetBlock(float delay)
    {
        isOccupied = false;
        colorTag = "";
        blockImage.rectTransform.DOScale(Vector3.zero, delay);
        // Color color = blockImage.color;
        // color.a = 0f;
        // blockImage.color = color;

    }
    
    public void SetBlockLocation(int rowIndex, int columnIndex)
    {
        RowId = rowIndex;
        ColumnId = columnIndex;
    }
}
