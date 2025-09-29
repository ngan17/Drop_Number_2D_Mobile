using System;
using UnityEngine;

public class Board : MonoBehaviour
{
    private long[,] BoardPlay;
    private int[] ColCount = new int[5]; // 5 cột
    public Column2D[] columns;

    void Start()
    {
        Array.Clear(ColCount, 0, ColCount.Length);
        BoardPlay = new long[5, 7]; // 5 cột × 7 hàng (đúng thứ tự)
    }

    public long getValue(int col, int row)
    {
        return BoardPlay[col, row];
    }

    public void clearCell(int col, int row)
    {
        BoardPlay[col, row] = 0;
        ColCount[col]--; // Giảm counter khi xóa
        if (ColCount[col] < 0) ColCount[col] = 0;
    }

    public Column2D GetColumn(int colIndex)
    {
        if (colIndex >= 0 && colIndex < columns.Length)
            return columns[colIndex];
        return null;
    }

    // FIXED: Dùng ColCount thay vì loop
    public void setPos(int col, long value)
    {
        if (col < 0 || col >= 5) return;
        if (ColCount[col] >= 7) return; // Cột đầy

        BoardPlay[col, ColCount[col]] = value;
        ColCount[col]++;
    }

    // FIXED: Trả về trực tiếp từ ColCount - O(1) thay vì O(n)
    public int rowCount(int col)
    {
        if (col < 0 || col >= 5) return 0;
        return ColCount[col];
    }

    public void updateBoard(int col, int row, long value)
    {
        if (col < 0 || col >= 5 || row < 0 || row >= 7) return;
        BoardPlay[col, row] = value;
    }
}