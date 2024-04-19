using System;
using System.Collections;
using System.Collections.Generic;
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
       
    }

    // Start is called before the first frame update
    void Start()
    {
        _cellColorsData = (CellColorData) Resources.Load("CellColorsData");
        PrepareCells();
    }

    public void PrepareCells()
    {
        colorQueue.Clear();
        int cellsToPrepare = Random.Range(0, cells.Count);
        for (int i = 0; i < cellsToPrepare; i++)
        {
            int randomColor = Random.Range(0, _cellColorsData.cellColors.Count);
            ColorAndTag colorAndTag = _cellColorsData.cellColors[randomColor];
            
            Color color = _cellColorsData.cellColors[randomColor].color;
            color.a = 1f;
            cells[i].color = color;
            colorAndTag.img = cells[i];
            colorAndTag.img.rectTransform.localScale = Vector3.one;
            colorQueue.Enqueue(colorAndTag);
        }
        
    }

    
}
