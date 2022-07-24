using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public sealed class tictactoe : MonoBehaviour
{
    public static tictactoe Instance = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != null)
        {
            Destroy(gameObject);
        }
    }

    public static GameObject[][] cellsArray = new GameObject[3][];


    public static bool _gameOver;


    public GameObject player1Mark;
    public GameObject player2Mark;
    public string NamePlayerOne;
    public string NamePlayerTwo;
    string winner;
    public List<Vector2> playersMoves { get; set; }

   

    [HideInInspector]
    public static int turn;

    public static int n { get; private set; }
    public static string[][] gridChart = new string[3][];

    enum players { player1, player2, AI }

    


    // Start is called before the first frame updateGrid
    void Start()
    {
        n = 3;
        _gameOver = false;
        playersMoves = new List<Vector2>();
        resetGame();
    
    }

    // updateGrid is called once per frame
    public void player(Vector2 coord, string player)
    {

        int i = (int)coord.x;
        int j = (int)coord.y;

        GameObject cellObject = cellsArray[i][j];
        if (gridChart[i][j] == null)
        {
            gridChart[i][j] = NamePlayerOne;
            playersMoves.Add(new Vector2(i, j));
            Instantiate(player1Mark, cellObject.transform.position, Quaternion.identity);
            turn++;
        }
    }
    public static void resetGame()
    {

        for (int i = 0; i < n; i++)
        {
            gridChart[i] = new string[n];
            for (int j = 0; j < n; j++)
            {
                gridChart[i][j] = null;
            }
        }
        GameObject[] allMarks = GameObject.FindGameObjectsWithTag("mark");
        foreach (var mark in allMarks)
        {
            Destroy(mark);
        }
        _gameOver = false;
        turn = 0;

    }
}
  