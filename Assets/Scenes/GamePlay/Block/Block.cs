using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using NUnit.Framework;
using UnityEngine.Experimental.GlobalIllumination;
public class Block : MonoBehaviour
{
    public RectTransform bg;
    public float fallSpeed = 100000000f; // pixel/giây
    public Column2D column;        // tham chiếu đến ColumnUI
    public int colIndex = 0;       // block thuộc cột nào
    private RectTransform rect;
    private float posEnd;
    private Board board;
    private bool isFalling = true;
    private int currentRow = 0;
    private long value = 2;
    private int row = 0, col = 0;
    public TextMeshProUGUI valueShow;

    void Awake()
    {
        rect = GetComponent<RectTransform>(); value = 2;
    }
    void Start()
    {
        rect.SetParent(column.background, false);


        posEnd = -bg.rect.height;

    }

    public void changeRandom(long num)
    {
        this.value = num;
        valueShow.text = num.ToString();
    }




    public long getvalue()
    {
        return this.value;
    }

    void falling_Block(float pos, int r)
    {
        if (isFalling)
        {
            rect.anchoredPosition -= new Vector2(0, fallSpeed * Time.deltaTime);

            if ((rect.anchoredPosition.y - rect.rect.height / 2) <= pos - ((pos * r) / 10))
            {
                isFalling = false;

                // Khi block dừng thì kiểm tra gộp
                TryMerge();
            }
        }
    }

   void TryMerge()
{
    if (board == null) return;

    // Merge dọc
    TryMergeVertical();

    // Merge ngang
    TryMergeHorizontal();
}

void TryMergeHorizontal()
{
    if (board == null) return;

    bool merged = true;

    // Lặp cho đến khi không merge được nữa
    while (merged)
    {
        merged = false;
        long curVal = board.getValue(row, col);
        if (curVal == 0) break;

        // Check bên trái
        if (col > 0 && board.getValue(row, col - 1) == curVal)
        {
            MergeWith(row, col - 1);
            col = col - 1; // sau khi merge thì block "di chuyển" về bên trái
            merged = true;
        }
        // Check bên phải
        else if (col < 4 && board.getValue(row, col + 1) == curVal)
        {
            MergeWith(row, col + 1);
            // Không đổi col, vì block đang giữ vị trí trái
            merged = true;
        }
    }
}


void MergeWith(int targetRow, int targetCol)
{
    long curVal = board.getValue(row, col);
    long newValue = curVal * 2;

    // ⭐ LẤY ĐÚNG CỘT
    Column2D targetColumn = board.GetColumn(targetCol);
    if (targetColumn == null)
    {
        Debug.LogError($"Không tìm thấy cột {targetCol}");
        return;
    }

    // Update block ở cột target
    Block targetBlock = targetColumn.GetBlockAt(targetRow, targetCol);
    if (targetBlock != null)
    {
        targetBlock.changeRandom(newValue);
    }

    // Update Board
    board.updateBoard(targetRow, targetCol, newValue);
    board.clearCell(row, col);

    // Xóa block hiện tại
    Block curBlock = column.GetBlockAt(row, col);
    if (curBlock != null)
    {
        Destroy(curBlock.gameObject);
        column.ClearBlock(row, col);
    }

    Debug.Log($"Merge horizontal [{row},{col}] + [{targetRow},{targetCol}]: {curVal}+{curVal} -> {newValue}");
}

void TryMergeVertical()
{
    int r = row;
    int c = col;

    while (r > 0)
    {
        long curVal = board.getValue(r, c);
        long belowVal = board.getValue(r - 1, c);

        if (curVal != 0 && curVal == belowVal)
        {
            long newValue = curVal * 2;

            Block belowBlock = column.GetBlockAt(r - 1, c);
            if (belowBlock != null)
                belowBlock.changeRandom(newValue);

            board.updateBoard(r - 1, c, newValue);
            board.clearCell(r, c);

            Block curBlock = column.GetBlockAt(r, c);
            if (curBlock != null)
            {
                Destroy(curBlock.gameObject);
                column.ClearBlock(r, c);
            }

            r -= 1; // tiếp tục check xuống dưới
        }
        else break;
    }
}



    void Update()
    {
        falling_Block(posEnd, row);
    }


    public void setPos(float pos, int row)
    {
        this.posEnd = pos;
        Debug.Log(row + " " + col);
        this.row = row;
        this.col = colIndex;
        board.updateBoard(row, col, value);

    }

    public void getBoard(Board bo)
    {
        this.board = bo;
    }
    public void mergerNumber()
    {

    }




}
