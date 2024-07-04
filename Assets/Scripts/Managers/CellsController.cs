using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Managers;
using Script.Utils;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CellsController : Singleton<CellsController>
{
    private CellColorData _cellColorsData;

    [SerializeField] private List<Image> cells = new List<Image>();
    public Queue<ColorAndTag> colorQueue = new Queue<ColorAndTag>();
    
    private void OnEnable()
    {
        _cellColorsData = (CellColorData) Resources.Load("CellColorsData");

    }

    private void OnDisable()
    {
        colorQueue.Clear();
    }

    public void PrepareCells()
    {
        Debug.Log("Refilling color cells");
        colorQueue.Clear();
        
        int cellsToPrepare = Random.Range(0, cells.Count);
        for (int i = 0; i <= cellsToPrepare; i++)
        {
            int randomColor = Random.Range(0, _cellColorsData.cellColors.Count);
            

            ColorAndTag originalColorAndTag = _cellColorsData.cellColors[randomColor];
            // Make a copy of the original ColorAndTag so that it doesn't modify the original scriptable object's data.
            ColorAndTag colorAndTag = originalColorAndTag.Copy();
            
            // Apply the color to the cell and enqueue the colorAndTag
            cells[i].color =  colorAndTag.color;
            
            // Set it's alpha value.
            Color color = cells[i].color;
            color.a = 1f;
            cells[i].color = color;

            colorAndTag.img = cells[i];
            cells[i].rectTransform.DOScale(Vector3.one, 0.5f);
            colorQueue.Enqueue(colorAndTag);
            // yield return inBetweenWait;
        }
        SoundManager.Instance.Play(SoundTypes.CellRefill);
    }
    
    public void RefillColorCells()
    {
        GameManager.Instance.ResetActiveCells();
        // StartCoroutine(CellColorManager.Instance.PrepareCells());
        PrepareCells();
        GameManager.Instance.AssignNewColors(new Queue<ColorAndTag>(colorQueue));
    }
    
    public Queue<ColorAndTag> GetNewColors( )
    {
       return new Queue<ColorAndTag>(colorQueue);
    }
}
