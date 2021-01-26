﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public static class ControlData
{
    private static string path, UserName;
    //private static int count;//第n回か
    private static int highScore=0, lastScore = 0;

    private static List<string[]> csvDatas = new List<string[]>();
    private static List<string[]> favoDatas = new List<string[]>();
    private static List<string[]> inGameDatas = new List<string[]>();

    public enum filetype
    {
        normal,
        favorite
    }

    public static string Initialized(filetype type)
    {

        path = Application.dataPath + "/Resources";
        Directory.CreateDirectory(path);

        //path += "/" + UserName + ".csv";

        SetCSVandData("start", type);

        return path;
    }

    static void SetCSVandData(string data,filetype type)
    {
        var filePath = path + "/" + type.ToString() + ".csv";

        if (File.Exists(filePath))
        {
            if (csvDatas.Count <= 0 && type==filetype.normal)
            {
                CSVRead(type);
            }else if (favoDatas.Count <= 0 && type == filetype.favorite)
            {
                CSVRead(type);
            }
            for(int i = 1; i < csvDatas.Count; i++)
            {
                if (5 <= csvDatas[i].Length)
                {
                    if (type == filetype.normal)
                    { 
                        if (Int32.TryParse(csvDatas[i][1], out int h))
                        {
                            if (highScore < h)
                            {
                                highScore = h;
                            }
                            lastScore = h;
                        }                      
                    }
                }                
            }

            var sw = new StreamWriter(filePath, true, Encoding.GetEncoding("UTF-8"));
            string[] s1 = { " ", "0", highScore.ToString(), " ",data };
            inGameDatas.Add(s1);
            var s2 = string.Join(",", s1);
            sw.WriteLine(s2);
            sw.Close();
            
            Debug.Log("SaveCSV Completed");
        }
        else
        {
            var sw = new StreamWriter(filePath, true, Encoding.GetEncoding("UTF-8"));
            string fixedFormText = "○○が,××して,□□を,△△する,ゲーム";
            sw.WriteLine(fixedFormText);

            string[]s1 = {" ", "0", "0", " ",data };
            inGameDatas.Add(s1);
            var s2 = string.Join(",", s1);
            sw.WriteLine(s2);
            sw.Close();
            Debug.Log("CreateCSV Completed");
        }
    }
    public static void CSVAddWrite(string[] data, filetype type)
    {
        var s2 = string.Join(",", data);

        if (type == filetype.normal)
        {
            inGameDatas.Add(s2.Split(',')); // , 区切りでリストに追加
        }
        else
        {
            favoDatas.Add(s2.Split(','));
        }
        var sw = new StreamWriter(path + "/" + type.ToString() + ".csv", true, Encoding.GetEncoding("UTF-8"));

        sw.WriteLine(s2);
        sw.Close();

       // Debug.Log("Save Completed");
    }
    public static void CSVAddWrite(int data1, string data2, filetype type)
    {
        var s2 = " ," + data1.ToString() + "," + data2 +","+ ",end";
        lastScore = data1;
        if (highScore < data1)
        {
            highScore = data1;
        }
        inGameDatas.Add(s2.Split(','));
        var sw = new StreamWriter(path + "/" + type.ToString() + ".csv", true, Encoding.GetEncoding("UTF-8"));


        sw.WriteLine(s2);
        sw.Close();

       // Debug.Log("Save Completed");

        EndGame();
    }
    private static void CSVRead(filetype type)
    {
        TextAsset csvFile = Resources.Load(type.ToString()) as TextAsset; // Resouces下のCSV読み込み

        StringReader reader = new StringReader(csvFile.text);

        // , で分割しつつ一行ずつ読み込み
        // リストに追加していく

        if (type == filetype.normal)
        {
            while (reader.Peek() != -1) // reader.Peaekが-1になるまで
            {
                string line = reader.ReadLine(); // 一行ずつ読み込み
                csvDatas.Add(line.Split(',')); // , 区切りでリストに追加
                //Debug.Log(line.Split(',')[1]);
            }
        }
        else
        {
            while (reader.Peek() != -1) // reader.Peaekが-1になるまで
            {
                string line = reader.ReadLine(); // 一行ずつ読み込み
                favoDatas.Add(line.Split(',')); // , 区切りでリストに追加
            }
        }
    }

    public static int FavoriteIdeNum()
    {
        var excl = 0;

        //Debug.Log(favoDatas.Count);
        for (int i = 0; i < favoDatas.Count; i++)
        {
            if (5<= csvDatas[i].Length)
            {
                excl++;
            }
            
        }
        //Debug.Log(excl);
        return favoDatas.Count - excl;
    }

    /// <summary>
    /// ゲーム中に出したアイデアと被っているか
    /// </summary>
    /// <param name="idea"></param>
    /// <returns></returns>
    public static bool CheckIdeaOverlapInGame(string[] idea)
    {
        for(int i = 0; i < inGameDatas.Count; i++)
        {
            if(inGameDatas[i][0] == " ")
            {
                break;
            }

            for(int j=0;j< idea.Length; j++)
            {
                if (inGameDatas[i][j] != idea[j])
                {//どこか１ヵ所でも違いがあれば被っていない判定
                    break;
                }
                if (j == idea.Length - 1)
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// 過去に出したアイデアと被っているか
    /// </summary>
    /// <param name="idea"></param>
    /// <returns></returns>
    public static bool CheckIdeaOverlapInCSV(string[] idea)
    {
        for (int i = 0; i < csvDatas.Count; i++)
        {
            if (csvDatas[i][0] == " ") continue;

            for (int j = 0; j < idea.Length; j++)
            {
                if (csvDatas[i][j] != idea[j])
                {//どこか１ヵ所でも違いがあれば被っていない判定
                    break;
                }
                if (j == idea.Length - 1)
                {
                    return true;
                }
            }
        }

        return false;
    }
    public static bool CheckIdeaOverlapInFavo(string[] idea)
    {
        for (int i = 0; i < favoDatas.Count; i++)
        {
            if (favoDatas[i][0] == " ")
            {
                break;
            }

            for (int j = 0; j < idea.Length; j++)
            {
                if (inGameDatas[i][j] != idea[j])
                {//どこか１ヵ所でも違いがあれば被っていない判定
                    break;
                }
                if (j == idea.Length - 1)
                {
                    return true;
                }
            }
        }

        return false;
    }
    public static int GetHeghScore()
    {
       // Debug.Log(highScore);
        return highScore;
    }

    public static List<string[]> GetIdeas()
    {
        return csvDatas;
    }

    public static List<String[]> GetFavo()
    {
        return favoDatas;
    }
    public static int GetLastScore()
    {
        return lastScore;
    }

    public static int Unlock()
    {
        if (csvDatas.Count <= 0) return 0;

        var num = 0;
        var hegh = 0;
        for (int i = 0; i < csvDatas.Count; i++)
        {
            if (5 <= csvDatas[i].Length)
            {
                if(Int32.TryParse(csvDatas[i][1],out int re))
                {
                    if (hegh < re)
                    {
                        hegh = re;
                        num = i;
                    }
                }
            }
        }

        var grade = csvDatas[num][2];
        //Debug.Log(grade);
        switch (grade)
        {
            case "mars":
                return 2;

            case "earth":
                return 2;

            case "saturn":
                return 3;

            case "jupiter":
                return 3;

            case "sun":
                return 4;

            default:
                return 0;
        }
    }

    public static bool Unlock(int score)
    {
        if (csvDatas.Count <= 0) return true;

        var num = 0;
        var hegh = 0;
        for (int i = 0; i < csvDatas.Count; i++)
        {
            if (5 <= csvDatas[i].Length)
            {
                if (Int32.TryParse(csvDatas[i][1], out int re))
                {
                    if (hegh < re)
                    {
                        hegh = re;
                        num = i;
                    }
                }
            }
        }

        var grade = csvDatas[num][2];
        var now = 0;

        switch (grade)
        {
            case "mars":
                now = 2;
                break;
            case "earth":
                now = 2;
                break;
            case "saturn":
                now = 2;
                break;
            case "jupiter":
                now = 3;
                break;
            case "sun":
                now = 4;
                break;
            default:
                now = 0;
                break;
        }

        return now < score;
    }

    public static void EndGame()
    {
        for (int i = 0; i < inGameDatas.Count; i++)
        {
            csvDatas.Add(inGameDatas[i]);
        }
        inGameDatas.Clear();
    }

}
