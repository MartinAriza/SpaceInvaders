using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class rankingDisplay : MonoBehaviour
{
    [SerializeField] Text names;
    [SerializeField] Text scores;
    private void OnEnable()
    {
        if (!names) names = GameObject.Find("names").GetComponent<Text>();
        if (!scores) scores = GameObject.Find("scores").GetComponent<Text>();
        if (names && scores)
        {
            names.text = "";
            scores.text = "";
            string[] lineas = rankingController.getRanking();
            for(int i = 0; i < rankingController.maxPlayersInRanking; i++)
            {
                if (i < lineas.Length)
                {
                    names.text += lineas[i][0];
                    names.text += lineas[i][1];
                    names.text += lineas[i][2];

                    for (int j = 3; j < lineas[i].Length; j++)
                    {
                        scores.text += lineas[i][j];
                    }
                    names.text += "\n";
                    scores.text += "\n";
                } else
                {
                    names.text += "---" + "\n";
                    scores.text += "-----" + "\n";
                }
            }
        }
    }
}
