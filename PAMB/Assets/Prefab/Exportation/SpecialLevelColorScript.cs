using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpecialLevelColorScript : MonoBehaviour
{
    public TextMeshProUGUI Number;
    public Image Circuit;
    public Image background;
    public List<Color> StateColor;


    //0 for not completed, 1 for completed and 2 for selected
    public void CardState(int state)
    {
        Number.color = Color.white;
        Circuit.color = Color.white;
        if (state >= 1)
        {
            Number.color = Color.black;
            Circuit.color = Color.black;
        }

        background.color = StateColor[state];
    }


}
