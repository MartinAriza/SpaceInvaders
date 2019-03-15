using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class rankingController : MonoBehaviour
{

    public static string fileName = "ranking.txt";
    public static int maxPlayersInRanking = 10;

    private void Start()
    {
        insertPlayer(1000, "PCM");
        insertPlayer(3000, "IVG");
        displayRankingInConsole();
        StartCoroutine("addPlayer");
    }

    IEnumerator addPlayer()
    {
        yield return new WaitForSeconds(2f);
        insertPlayer(2000, "AVP");
        displayRankingInConsole();
    }

    public static void displayRankingInConsole()
    {
        string[] ran = getRanking();
        for(int i = 0; i < ran.Length; i++)
        {
            print(ran[i]);
        }
    }

    public static string[] getRanking()
    {
        string[] lineas = new string[maxPlayersInRanking];
        StreamReader fichero;
        if (!File.Exists(fileName))
            File.CreateText(fileName);

        fichero = File.OpenText(fileName);
        int index = 0;
        for (int i = 0; i < maxPlayersInRanking; i++)
        {
            lineas[i] = fichero.ReadLine();
            if (lineas[i] == null) i = maxPlayersInRanking;
            else index++;
        }
        string[] newLineas = new string[index];
        for(int i = 0; i < newLineas.Length; i++)
        {
            newLineas[i] = lineas[i];
        }
        fichero.Close();
        return newLineas;
    }

    public static void insertPlayer(int score, string name)
    {
        int[] previousScores = sortRanking();
        string[] lineas = getRanking();
        string line = name + score;

        bool exists = false;
        for(int i = 0; i < lineas.Length; i++)
        {
            if (lineas[i] == line) exists = true;
        }

        if (!exists)
        {

            //int[] newScores = new int[previousScores.Length + 1];
            bool condition = false;
            if (previousScores.Length > 0)
            {
                if (score > previousScores[previousScores.Length - 1]) condition = true;
            }
            else
            {
                condition = true;
            }
            if (condition)
            {
                if (lineas.Length < maxPlayersInRanking)
                {
                    string[] newLineas = new string[lineas.Length + 1];
                    for (int i = 0; i < lineas.Length; i++)
                    {
                        newLineas[i] = lineas[i];
                    }
                    newLineas[newLineas.Length - 1] = line;
                    setRanking(newLineas);
                }
                else
                {
                    lineas[lineas.Length - 1] = line;
                    setRanking(lineas);
                }
            }
            sortRanking();
        }
    }

    public static void setRanking(string[] lineas)
    {
        StreamWriter fichero;

        if (!File.Exists(fileName))
            File.CreateText(fileName);

        //fichero = File.AppendText(fileName);
        fichero = new StreamWriter(fileName);

        for (int i = 0; i < lineas.Length; i++)
        {
            fichero.WriteLine(lineas[i]);
        }
        fichero.Close();
    }

    public static int[] getScores()
    {
        string[] lineas = getRanking();
        int[] scores = new int[lineas.Length];

        for (int i = 0; i < lineas.Length; i++)
        {
            string number = "";
            string resultString = lineas[i];
            for (int j = 0; j < resultString.Length; j++)
            {
                if (char.IsDigit(resultString[j]))
                {
                    number += resultString[j];
                }
            }
            if (number.Length > 0) scores[i] = int.Parse(number);
        }
        return scores;
    }

    public static int[] sortRanking()
    {
        string[] lineas = getRanking();
        int[] scores = getScores();
        
        for(int j = 0; j < lineas.Length; j++)
        {
            int higherScore = 0;
            string higherScoreString = "";
            int index = 0;
            for (int i = j; i < lineas.Length; i++)
            {
                if (scores[i] > higherScore)
                {
                    higherScore = scores[i];
                    higherScoreString = lineas[i];
                    index = i;
                }
            }
            for(int w = index; w > j; w--)
            {
                scores[w] = scores[w - 1];
                lineas[w] = lineas[w - 1];
            }
            scores[j] = higherScore;
            lineas[j] = higherScoreString;
        }
        setRanking(lineas);
        return scores;
    }
}