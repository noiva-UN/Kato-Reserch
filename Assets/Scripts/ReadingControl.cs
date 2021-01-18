using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReadingControl : MonoBehaviour
{
    [SerializeField] private GameObject displayCanvas;
    private DisplayIdeas _displayIdeas;

    [SerializeField] Text _ideaNum;

    private List<string[]> _data, _favo;

    // Start is called before the first frame update
    void Awake()
    {
        _data = new List<string[]>();
        _data = ControlData.GetIdeas();
        _favo = new List<string[]>();
        _favo = ControlData.GetFavo();
        _displayIdeas = displayCanvas.GetComponent<DisplayIdeas>();
        _displayIdeas.Initialized();
        _displayIdeas.Reading();
        _displayIdeas.SetFavoData(_favo);

        var a = 0;
        for(int i = 0; i < _data.Count; i++)
        {
            if (_data[i].Length <= 4)
            {
                _displayIdeas.SetIdeaData(_data[i][0], _data[i][1], _data[i][2], _data[i][3]);
                a++;
            }

            
        }
        Debug.Log(a);
        _ideaNum.text = a.ToString();
        _displayIdeas.NextPageIdeaDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Start");
        }else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _displayIdeas.BackPageIdeaDisplay();
        }else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _displayIdeas.NextPageIdeaDisplay();
        }
    }
}
