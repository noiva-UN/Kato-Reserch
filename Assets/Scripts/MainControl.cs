using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainControl : MonoBehaviour
{
    public string userName = "noiva";
    private string _filePath;

    // Start is called before the first frame update
    void Start()
    {
        _filePath = ControlData.Initialized(userName);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            ControlData.CSVAddWrite("a", "a",_filePath);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            ControlData.CSVAddWrite("a","aa", _filePath);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("game " + ControlData.CheckIdeaOverlapInGame("a", "a", "a", "a"));
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("CSV   "+ ControlData.CheckIdeaOverlapInCSV("a", "a", "a", "a"));
        }
    }
}
