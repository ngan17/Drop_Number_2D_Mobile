using UnityEngine;

[ExecuteAlways]
public class Column2D : MonoBehaviour
{
    public RectTransform background;
    public int column = 5;
    private Vector2[] cellPositions;
    [HideInInspector] public Vector2 cellSize;
    private Block[,] blocks = new Block[7, 5];

public void RegisterBlock(Block block, int row, int col)
{
    blocks[row, col] = block;
}

public Block GetBlockAt(int row, int col)
{
    if (row >= 0 && row < 7 && col >= 0 && col < 5)
        return blocks[row, col];
    return null;
}

public void ClearBlock(int row, int col)
{
    blocks[row, col] = null;
}

    public void InitCellSize(RectTransform blockPrefab)
    {
        cellSize = blockPrefab.sizeDelta;
        if (background == null || column <= 0) return;

        Vector2 sizeBG = background.rect.size; // kích thước panel (pixel)
        float widthCol = sizeBG.x / column;

        cellPositions = new Vector2[column];
        for (int i = 0; i < column; i++)
        {
            // tính localPosition theo pivot (giữa = 0,0)
            cellPositions[i].x = -sizeBG.x / 2 + widthCol * (i + 0.5f);
            cellPositions[i].y = -sizeBG.y / 2 + widthCol * 0.5f;
        }
    }

    void Update()
    {
        if (background == null || column <= 0) return;

        Vector2 sizeBG = background.rect.size; // kích thước panel (pixel)
        float widthCol = sizeBG.x / column;

        cellPositions = new Vector2[column];
        for (int i = 0; i < column; i++)
        {
            // tính localPosition theo pivot (giữa = 0,0)
            cellPositions[i].x = -sizeBG.x / 2 + widthCol * (i + 0.5f);
            cellPositions[i].y = -sizeBG.y / 2 + widthCol * 0.5f;
        }
    }
    /// <summary>
    /// Callback to draw gizmos that are pickable and always drawn.
    /// </summary>


    
    public Vector2 GetCellPos(int col, int row = 0)
    {
        Vector2 sizeBG = background.rect.size;
        float widthCol = sizeBG.x / column;
        float cellHeight = widthCol;

        float x = -sizeBG.x / 2 + widthCol * (col + 2f);
        float y = -sizeBG.y / 2 + cellHeight * (row);

        return new Vector2(x, y);
    }
}
