using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class _CustomFlexibleGridLayout : LayoutGroup
{
    [SerializeField] private int _rows;
    [SerializeField] private int _columns;
    [SerializeField] private Vector2 _cellSize;
    [SerializeField] private Vector2 _spacing;

    public override void CalculateLayoutInputVertical()
    {
        base.CalculateLayoutInputHorizontal();
        
        float sqrRt = Mathf.Sqrt(transform.childCount);
        _rows = Mathf.CeilToInt(sqrRt);
        _columns = Mathf.CeilToInt(sqrRt);

        float parentWidth = rectTransform.rect.width;
        float parentHeight = rectTransform.rect.height;

        float cellWidth = (parentWidth / (float)_columns) - ((_spacing.x / (float)_columns) * 2) - (padding.left / (float) _columns) - padding.right / (float)_columns;
        float cellHeight = (parentHeight / (float)_rows) - ((_spacing.y / (float)_rows) * 2) - (padding.top / (float) _rows) - (padding.bottom / (float)_rows);

        _cellSize.x = cellWidth;
        _cellSize.y = cellHeight;

        int columnCount = 0;
        int rowCount = 0;

        for (int i = 0; i < rectChildren.Count; i++)
        {
            rowCount = i / _columns;
            columnCount = i % _columns;

            var item = rectChildren[i];

            var xPosition = (_cellSize.x * columnCount) + (_spacing.x * columnCount) + padding.left;
            var yPosition = (_cellSize.y * rowCount) + (_spacing.y * rowCount) + padding.top;
            
            
            SetChildAlongAxis(item, 0, xPosition, _cellSize.x);
            SetChildAlongAxis(item, 1, yPosition, _cellSize.y);

        } 
        
    }


    public override void SetLayoutHorizontal()
    {
    }

    public override void SetLayoutVertical()
    {
        
    }
}
