using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public static class ControlData
{
    private static string path, UserName;
    private static int count = 1;

    private static List<string[]> csvDatas = new List<string[]>();
    private static List<string[]> inGameDatas = new List<string[]>();

    public static string Initialized(string name)
    {
        UserName = name;
        path = Application.dataPath + "/Resources";
        Directory.CreateDirectory(path);

        path += "/" + UserName + ".csv";

        SetCSVandData("start", path);

        return path;
    }
    static void SetCSVandData(string data, string filePath)
    {
        if (File.Exists(filePath))
        {
            CSVRead(filePath);
            count = 1;
            for(int i = 0; i < csvDatas.Count; i++)
            {
                if (csvDatas[i][0] == " ")
                {
                    count++;
                }
            }

            var sw = new StreamWriter(filePath, true, Encoding.GetEncoding("UTF-8"));
            string[] s1 = {" ", "第" + count + "回", UserName, data };
            count++;
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

            string[]s1 = {" ", "第1回", UserName, data };
            count = 2;
            var s2 = string.Join(",", s1);
            sw.WriteLine(s2);
            sw.Close();
            Debug.Log("CreateCSV Completed");
        }
    }

    public static void CSVAddWrite(string data, string filePath)
    {
        string[] s1 = { data, data };
        var s2 = string.Join(",", s1);
        inGameDatas.Add(s2.Split(',')); // , 区切りでリストに追加

        var sw = new StreamWriter(filePath, true, Encoding.GetEncoding("UTF-8"));


        sw.WriteLine(s2);
        sw.Close();

        Debug.Log("Save Completed");
    }
    public static void CSVAddWrite(string data1, string data2, string filePath)
    {
        string[] s1 = {data1, data2, data1, data2 };
        var s2 = string.Join(",", s1);
        inGameDatas.Add(s2.Split(',')); // , 区切りでリストに追加

        var sw = new StreamWriter(filePath, true, Encoding.GetEncoding("UTF-8"));

        sw.WriteLine(s2);
        sw.Close();

        Debug.Log("Save Completed");
    }

    private static void CSVRead(string filePath)
    {
        TextAsset csvFile = Resources.Load(UserName) as TextAsset; // Resouces下のCSV読み込み
        StringReader reader = new StringReader(csvFile.text);

        // , で分割しつつ一行ずつ読み込み
        // リストに追加していく
        while (reader.Peek() != -1) // reader.Peaekが-1になるまで
        {
            string line = reader.ReadLine(); // 一行ずつ読み込み
            csvDatas.Add(line.Split(',')); // , 区切りでリストに追加
        }
        /*
        string str = "";
        for(int i = 0; i < csvDatas.Count; i++)
        {
            for(int j = 0; j < csvDatas[i].Length; j++)
            {
                str += csvDatas[i][j] + " ";
            }
            str += "\n";
        }

        Debug.Log(str);
        */
    }

    public static bool CheckIdeaOverlapInGame(string some1, string do1, string some2, string do2)
    {
        string[] idea = { some1, do1, some2, do2 };

        for(int i = 0; i < inGameDatas.Count; i++)
        {
            if(inGameDatas[i][0] == " ")
            {
                break;
            }

            for(int j=0;j< idea.Length; j++)
            {
                if (inGameDatas[i][j] != idea[j])
                {
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

    public static bool CheckIdeaOverlapInCSV(string some1, string do1, string some2, string do2)
    {
        string[] idea = { some1, do1, some2, do2 };

        for (int i = 0; i < csvDatas.Count; i++)
        {

            if (csvDatas[i][0] == " ") continue;

            for (int j = 0; j < idea.Length; j++)
            {
                if (csvDatas[i][j] != idea[j])
                {
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
}
