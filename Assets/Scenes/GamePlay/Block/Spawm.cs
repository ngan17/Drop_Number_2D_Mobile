using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Spawm : MonoBehaviour
{
    public Block blockPrefab;
    public Column2D column;
    public RectTransform bg;
    private int[] row = new int[5];
    public Board board;


    void Start()
    {
        // Initialize the bottom position for each column
        for (int i = 0; i < 5; i++)
        {
            row[i] = 0; // Bottom of the background
        }
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            TrySpawn(Mouse.current.position.ReadValue());
        }

        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            TrySpawn(Touchscreen.current.primaryTouch.position.ReadValue());
        }
    }

    void TrySpawn(Vector2 screenPos)
    {
        if (bg == null)
        {
            Debug.LogError("Background RectTransform (bg) is null!");
            return;
        }

        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(bg, screenPos, Camera.main, out localPos);

        int col = GetColumnByPosition(screenPos);
        if (col != -1)
        {
            SpawnBlock(col);
        }
    }

    int GetColumnByPosition(Vector2 screenPos)
    {
        float colWidth = bg.rect.width / 5;
        Debug.Log(colWidth+" "+screenPos.x);
        int x = Mathf.FloorToInt((screenPos.x-80) / colWidth);
        if (x >= 0 && x <= 4)
            return x;
        return -1;
    }

    void SpawnBlock(int colIndex)
    {
        // Validate references
        if (blockPrefab == null)
        {
            Debug.LogError("blockPrefab is null!");
            return;
        }
        if (column == null || column.background == null)
        {
            Debug.LogError("Column or column.background is null!");
            return;
        }
        if (board == null)
        {
            Debug.LogError("Board is null!");
            return;
        }

        // Instantiate a new block
        Block newBlock = Instantiate(blockPrefab, column.background);

        // Verify RectTransform
        RectTransform blockRect = newBlock.GetComponent<RectTransform>();
        if (blockRect == null)
        {
            Debug.LogError("Block prefab does not have a RectTransform!");
            Destroy(newBlock.gameObject);
            return;
        }

        // Get the cell position for the block
        Vector2 cellPos = column.GetCellPos(colIndex + 1, 0);
        float spawnX = cellPos.x;
        float spawnY = bg.rect.height / 2; // Start at the top

        // Set the block's initial position
        blockRect.anchoredPosition = new Vector2(spawnX, spawnY);

        // Assign references to the block
        newBlock.column = column;
        newBlock.colIndex = colIndex;
        newBlock.bg = bg;
        newBlock.getBoard(board);
        newBlock.changeRandom(2);
        Debug.Log(bg.rect.height);

        newBlock.setPos(bg.rect.height,board.rowCount(colIndex));
        row[colIndex]++;
    
        newBlock.getBoard(board);
        column.RegisterBlock(newBlock, board.rowCount(colIndex)-1, colIndex);
        
    }

}