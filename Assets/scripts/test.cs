using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class test : MonoBehaviour
{
    public TextMeshProUGUI testText;

    public void onMouseClick()
    {
        testText.text = "it works";
    }
}
