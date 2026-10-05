using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public GameManager gameManager;
    public int boardSize = 10;
    public GameObject boardCellPrefab;

    public void CreatBoard()
    {
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                GameObject newCell = Instantiate(boardCellPrefab);
                newCell.transform.position = new Vector3(i, 0.5f, j);
                newCell.transform.SetParent(this.transform);
            }
        }
    }
}
