using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class AI_v1 : MonoBehaviour
{
    public GameObject shape;
    public Vector2 lastMove { get; set; }
    int n = 3;


   [SerializeField] private  TextMeshProUGUI result;
    // Start is called before the first frame update
    void Start()
    {

    }

    //we call playForWin when
    // the player is not one move away from wining and when we can not win immediatly
    //*the player's last move does align with his previous move
    //*the player's last move aligns with his previous move, but the third cell in the lign is not vacant

    // the AI tries to win by :

    public Vector2 playForWin()
    {
        Debug.Log("last Move " + lastMove);
        Vector2 nextMove = new Vector2();
        int a = (int)lastMove.x;
        int b = (int)lastMove.y;

        //if the AI's last move is on any of the diagonals
        // we look through all the cells surrounding the last move
        // just like the king in chess moves

        if (belongsToDiagonal(a, b) || belongsTo_Diagonal(a, b))
        {
            int y = 0;
            //to look all around we need the for loops to be nested
            // so it's a multiplication 
            for (int i = a - 1; i <= a + 1; i++)
            {
                for (int j = b - 1; j <= b + 1; j++)
                {
                    y++;
                    // we need to make sure the values are inside the grid
                    //for example if the last move is (2,2)
                    // the surounding cells to check are: (1,1)(0,1)(1,0)

                    //**** this is not the best strategy because the line should not contain any cell marked by the player

                    if ((i >= 0 && i < 3) && (j >= 0 && j < 3))
                    {

                        if (tictactoe.gridChart[i][j] == null)
                        {
                            nextMove = new Vector2(i, j);
                        }
                    }
                }
            }
        }
        else
        {
            //else if the last move is not on any diagonal
            // we only need to check vertically
            // and horizontally
            // for a vacant cell 
            for (int i = a - 1; i < a + 1; i++)
            {
                if ((i >= 0 && i < 3))
                {
                    if (tictactoe.gridChart[i][b] == null)
                    {
                        nextMove = new Vector2(i, b);
                    }
                }
            }
            for (int j = b - 1; j < b + 1; j++)
            {
                if ((j >= 0 && j < 3) && (j >= 0 && j < 3))
                {
                    if (tictactoe.gridChart[a][j] == null)
                    {
                        nextMove = new Vector2(a, j);
                    }
                }
            }
        }


        return nextMove;
    }

    public Vector2 firstMove(Vector2 coord)
    {

        int i = (int)coord.x;
        int j = (int)coord.y;
        Vector2 AIMove = new Vector2();


        if (j == 1 && i == 1)
        {
            Vector2[] corners = new Vector2[4];
            corners = new Vector2[] { new Vector2(0, 0), new Vector2(0, 2), new Vector2(2, 0), new Vector2(2, 2) };
            int num = (int)Random.Range(0, 4);
            AIMove = corners[num];
        }
        else
        {
            AIMove = new Vector2(1, 1);
        }

        return AIMove;

    }
    public void AIUpdate(Vector2 coord, string player)
    {
        int i = (int)coord.x;
        int j = (int)coord.y;
        int t = tictactoe.turn;

        if (t == 1)
        {
            Vector2 cellPos = firstMove(coord);
      

            Debug.Log("i " + i + " j " + j);
            updateGrid(cellPos, "AI");
        }
        if (t == 3)
        {
            Debug.Log("hihi");
            List<Vector2>[] allLines = extractAllLines(i, j, tictactoe.gridChart, player);
            if (!atLeastOneLineHasMoreThanOne(allLines,2))
            {
                Vector2 U = playForWin();
              
                updateGrid(U, "AI");
            }
            else if (atLeastOneLineHasMoreThanOne(allLines,2))
            {
                List<int> _LineWhichContainsTwo = LinesWhichContainsTwo(allLines);
                (Vector2 cellPos, bool containsVacantCell) = vacantCell(i, j, _LineWhichContainsTwo[0]);

                if (containsVacantCell)
                {
                    Debug.Log("prevent losing");
                    
                    updateGrid(cellPos, "AI");
                }
                else
                {
                    Debug.Log("trying to win " + i + " " + j);
                    Vector2 U = playForWin();
                 
                    updateGrid(U, "AI");
                }
            }
        }
        if (t > 3 & tictactoe.turn < 8)
        {
            // AI strategy
            //checks first if it has a win, if it is one mark away from winning
            // based on the last cell checked by the AI
            // it evaluates all the lines that contains this cell
            // if any of them contains two
            // then looks for the third cell and checks if it's vacant

            //first we check if the player has filled three cells in a row
            //then it's gameover

            List<Vector2>[] allLines = extractAllLines(i, j, tictactoe.gridChart, player);
            if (atLeastOneLineHasMoreThanOne(allLines, 3))
            {
                tictactoe._gameOver = true;
                displayGameOver(true,false);
            }
            else

            {
                (Vector2 cellPos, bool containsVacantCell) = VacantCellToFill(lastMove, "AI");
                if (containsVacantCell)
                {
                    Debug.Log("immediate win");
                    updateGrid(cellPos, "AI");

                    tictactoe._gameOver = true;
                  displayGameOver(false,false);
                }
                else
                {

                    (cellPos, containsVacantCell) = VacantCellToFill(coord, player);
                    if (containsVacantCell)
                    {
                        Debug.Log("We prevent losing");
                        updateGrid(cellPos, "AI");
                    }
                    else
                    {
                        cellPos = playForWin();

                        Debug.Log("try to cumulate"+" fill cell "+cellPos);
                        updateGrid(cellPos, "AI");

                    }
                }
            }
        }
        if (t==9)
        {
            displayGameOver(false, true);
        }

        tictactoe.turn++;
    }


    (Vector2, bool) VacantCellToFill(Vector2 coord, string player)
    {
        int i = (int)coord.x;
        int j = (int)coord.y;

        Vector2 cellPos = new Vector2();
        bool containsVacantCell = false;
        // we are not trying to win but to prevent losing
        // which means just like we did in the previous move
        // to check if the last move by the player aligns with his previous move
        // so again we check first for doubles in lines
        // list the lines that contain them
        // use the vacantCell function to determine which cell is vacant if there's any
        // the function will keep the last one in cse there are more than one
        // Here also it doesn't matter which one we pick
        // But this time because we can not block both at the same time


        List<Vector2>[] allLines = extractAllLines(i, j, tictactoe.gridChart, player);
        //But before that
        // What if the player has no
        if (!atLeastOneLineHasMoreThanOne(allLines,2))
        {
            //Vector2 U = playForWin();
            //i = (int)U.x;
            //j = (int)U.y;
            //updateGrid(i, j, "AI");
        }
        else if (atLeastOneLineHasMoreThanOne(allLines,2))
        {
            List<int> _LinesWhichContainsTwo = LinesWhichContainsTwo(allLines);
         
            int index = 0;
            while (!containsVacantCell && index < _LinesWhichContainsTwo.Count)
            {
                (cellPos, containsVacantCell) = vacantCell(i, j, _LinesWhichContainsTwo[index]);
                index++;
            }

        }

        return (cellPos, containsVacantCell);
    }
    void updateGrid(Vector2 cellPos, string player)
    {
        int i = (int)cellPos.x;
        int j = (int)cellPos.y;
        lastMove = new Vector2(i, j);
        tictactoe.gridChart[i][j] = player;
        Instantiate(shape, tictactoe.cellsArray[i][j].transform.position, Quaternion.identity);
    }

    bool belongsToDiagonal(int i, int j)
    {
        bool b = false;
        if (i == j) b = true;
        return b;
    }
    bool belongsTo_Diagonal(int i, int j)
    {
        bool b = false;
        if (i + j == 2) b = true;
        return b;
    }
    enum lines { row, column, diag, _diag }


    //following the last move by the designated player
    // the function checks the line to which the clicked cell belongs
    // then the column
    // then we check if the clicked cell belongs to a diagonal


    //returns the list of cells belonging to the same row

    List<Vector2> row(int j, string[][] array, string player)
    {

        List<Vector2> line = new List<Vector2>();
        for (int k = 0; k < n; k++)
        {
            if (array[k][j] == player)
            {
                line.Add(new Vector2(k, j));

            }

        }
        return line;
    }
    List<Vector2> column(int i, string[][] array, string player)
    {
        List<Vector2> line = new List<Vector2>();
        for (int k = 0; k < n; k++)
        {
            if (array[i][k] == player)
            {
                line.Add(new Vector2(i, k));

            }
        }
        return line;

    }
    List<Vector2> diagonal(string[][] array, string player)
    {
        List<Vector2> line = new List<Vector2>();
        for (int k = 0; k < n; k++)
        {
            if (array[k][k] == player)
            {
                line.Add(new Vector2(k, k));

            }

        }
        return line;
    }

    List<Vector2> _diagonal(string[][] array, string player)
    {
        List<Vector2> line = new List<Vector2>();
        for (int k = 0; k < n; k++)
        {
            if (array[k][2 - k] == player)
            {
                line.Add(new Vector2(k, 2 - k));
            }
        }
        return line;
    }


    //extracts all the lines relative to the played move
    // if (2,2) is played
    //the extracted lines for evaluation are row(k,2) column(2,k) and the normal diagonal
    List<Vector2>[] extractAllLines(int i, int j, string[][] array, string player)
    {
        List<Vector2>[] allLines = new List<Vector2>[4];
        for (int s = 0; s < 4; s++)
        {
            allLines[s] = new List<Vector2>();
        }
        allLines[(int)lines.row] = row(j, array, player);

        allLines[(int)lines.column] = column(i, array, player);

        if (belongsToDiagonal(i, j))
        {
            allLines[(int)lines.diag] = diagonal(array, player);
        }

        if (belongsTo_Diagonal(i, j))
        {
            allLines[(int)lines._diag] = _diagonal(array, player);
            Debug.Log("antiDigonal");
        }
        return allLines;
    }

    // for each extracted line related to the player's move
    //we check if the player has cumulated two marks
    //because if not the main strategie for the AI is cumulate itself 3 straight marks 
    bool atLeastOneLineHasMoreThanOne(List<Vector2>[] extractedLines, int value)
    {
        bool hasTwoMarks = false;
        // Debug.Log("number of extracted Lines is "+extractedLines.Length);
        for (int k = 0; k < extractedLines.Length; k++)
        {
            if (extractedLines != null)
            {
                if (extractedLines[k].Count == value)
                {
                    hasTwoMarks = true;
                }
            }
        }
        return hasTwoMarks;
    }


    //Then we select in the list of lines we extracted, the index of the line which contains 2 marks by the player 
    //which in the enum Lines corresponds to either the row, the column, the diagonal, or the anti Diagonal
    List<int> LinesWhichContainsTwo(List<Vector2>[] extractedLines)
    {

        List<int> lineNumber = new List<int>();
        if (atLeastOneLineHasMoreThanOne(extractedLines,2))
        {
            for (int k = 0; k < extractedLines.Length; k++)
            {
                if (extractedLines[k].Count == 2)
                {
                    lineNumber.Add(k);
                }
            }
        }
        return lineNumber;
    }

    //the variable m corresponds to i or j depending on which forLoop we are using 
    (Vector2, bool) vacantCell(int i, int j, int num)
    {
        Vector2 cellCoord = new Vector2();
        bool ThirdCellIsVacant = false;
        switch (num)
        {
            case (int)lines.row:
                for (int k = 0; k < n; k++)
                {
                    if (tictactoe.gridChart[k][j] == null)
                    {
                        cellCoord = new Vector2(k, j);
                        ThirdCellIsVacant = true;
                    }
                }
                break;

            case (int)lines.column:
                for (int k = 0; k < n; k++)
                {
                    if (tictactoe.gridChart[i][k] == null)
                    {
                        cellCoord = new Vector2(i, k);
                        ThirdCellIsVacant = true;

                    }
                }
                break;
            case (int)lines.diag:
                for (int k = 0; k < n; k++)
                {
                    if (tictactoe.gridChart[k][k] == null)
                    {
                        cellCoord = new Vector2(k, k);
                        ThirdCellIsVacant = true;

                    }
                }
                break;
            case (int)lines._diag:
                for (int k = 0; k < n; k++)
                {
                    if (tictactoe.gridChart[k][2 - k] == null)
                    {
                        cellCoord = new Vector2(k, 2 - k);
                        ThirdCellIsVacant = true;
                        Debug.Log("_diag");
                    }
                }
                break;
        }
        return (cellCoord, ThirdCellIsVacant);
    }

      void displayGameOver(bool win, bool draw)
    {


        if (win)
        {
            result.text = "YOU WON";
            result.color = Color.green;
        }
        else
        {
            if (draw)
            {

                result.text = "It's a draw";
                result.color = Color.yellow;
            }
            else
            {
                result.text = "YOU LOST MY DEAR";
                result.color = Color.red;
            }
        }
    }

    public void resetText()
    {
        result.text = "";
    }

}


