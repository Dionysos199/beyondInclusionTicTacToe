using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

[System.Serializable]
public class cellClicked : UnityEvent<Vector2, string>
{ }
public class Player : MonoBehaviour
{
    cell cell;
    public cellClicked cellClicked;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                //suppose i have two objects here named obj1 and obj2.. how do i select obj1 to be transformed 
                if (hit.transform.gameObject.tag == "cell")
                {
                    cell = hit.transform.gameObject.GetComponent<cell>();
                    if (!cell.Checked)
                    {
                        if (!tictactoe._gameOver)
                        {

                            //Debug.Log("cell"+cell.i+"  "+cell.j+"is clicked");
                            //Debug.Log("turn" + tictactoe.turn);
                            // if(!tictactoe.gameOver)
                            cellClicked.Invoke(new Vector2(cell.i, cell.j), "player1");
                        }

                    }
                }
            }
        }
    }
}
