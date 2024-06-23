using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using DG.Tweening;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CellColorManager : MonoBehaviour
{
    private CellColorData _cellColorsData;

    [SerializeField] private List<Image> cells = new List<Image>();
    public Queue<ColorAndTag> colorQueue = new Queue<ColorAndTag>();
    
    private void Awake()
    {
        _cellColorsData = (CellColorData) Resources.Load("CellColorsData");

    }

    // Start is called before the first frame update
    void Start()
    {
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
            // Make a copy of the original ColorAndTag
            ColorAndTag colorAndTag = originalColorAndTag.Copy();
        
            // Modify the color of the copy
            Color color = colorAndTag.color;
            color.a = 1f;
            colorAndTag.color = color;
            
            // Apply the color to the cell and enqueue the colorAndTag
            cells[i].color = color;
            colorAndTag.img = cells[i];
            cells[i].rectTransform.DOScale(Vector3.one, 0.5f);
            colorQueue.Enqueue(colorAndTag);
        }
        
       
    }
    
}
