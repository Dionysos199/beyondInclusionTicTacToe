using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class gameManager
{
    private gameManager() { }
    private static gameManager instance = null;
    public static gameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new gameManager();
            }
            return instance;
        }
    }


}