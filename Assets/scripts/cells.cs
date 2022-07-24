using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cells : MonoBehaviour
{
    public GameObject cell;


    // Start is called before the first frame update
    void Start()
    {

        tictactoe tictactoe = tictactoe.Instance;

        float x = cell.transform.localScale.x*1.1f;
        float z = cell.transform.localScale.z*1.1f;
        for (int i = 0; i < 3; i++)
        {
          tictactoe. cellsArray [i]= new GameObject[3];
            for (int j = 0; j < 3; j++)
            {

               GameObject newCell= Instantiate(cell, new Vector3(x*i,0,z*j), Quaternion.identity);
              tictactoe. cellsArray[i][j] = newCell;
               cell cellScript = newCell.GetComponent<cell>();
               cellScript.i = i;
               cellScript.j = j;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
