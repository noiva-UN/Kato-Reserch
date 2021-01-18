using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public static class ControlData
{
    private static string path, UserName;
    //private static int count;//第n回か
    private static int highScore;

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
            if (csvDatas.Count <= 0)
            {
                CSVRead(type);
            }
            var hegh = 0;
            for(int i = 1; i < csvDatas.Count; i++)
            {
                if (csvDatas[i][0] == " ")
                {
                    if (type == filetype.normal)
                    {
                        i++;
                        if (csvDatas.Count <= i) break; 
                        if (Int32.TryParse(csvDatas[i][2], out int h))
                        {
                            if (hegh < h)
                            {
                                hegh = h;
                            }
                        }                      
                    }
                }                
            }
            highScore = hegh;

            var sw = new StreamWriter(filePath, true, Encoding.GetEncoding("UTF-8"));
            string[] s1 = { " ", "0", hegh.ToString(), " ",data };
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
        var sw = new StreamWriter(path + "/" + type.ToString() + ".csv", true, Encoding.GetEncoding("UTF-8"));


        sw.WriteLine(s2);
        sw.Close();

        Debug.Log("Save Completed");
    }
    public static void CSVAddWrite(int data1, string data2, filetype type)
    {
        var s2 = " ," + data1.ToString() + "," + data2 +","+ ",end";

        inGameDatas.Add(s2.Split(','));
        var sw = new StreamWriter(path + "/" + type.ToString() + ".csv", true, Encoding.GetEncoding("UTF-8"));


        sw.WriteLine(s2);
        sw.Close();

        Debug.Log("Save Completed");

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
                //Debug.Log(line.Split(','));
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

        for (int i = 0; i < favoDatas.Count; i++)
        {
            if (csvDatas[i][0] == " " || csvDatas[i][0] == "○○が")
            {
                excl++;
            }
        }

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
        return highScore;
    }

    public static int Unlock()
    {
        if (csvDatas.Count <= 0) return 0;

        var num = 0;
        var hegh = 0;
        for (int i = 0; i < csvDatas.Count; i++)
        {
            if (csvDatas[i].Length <= 5)
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
                return 2;

            case "jupiter":
                return 3;

            case "sun":
                return 4;

            default:
                return 0;
        }
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
