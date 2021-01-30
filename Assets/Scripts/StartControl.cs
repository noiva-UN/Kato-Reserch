using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartControl : MonoBehaviour
{
    [SerializeField] private GameObject marsLock, earthLock, saturnLock, jupiterLock, sunLock;
    private GameObject[] locks;
    [SerializeField] private GameObject arrow;
    [SerializeField] private float arrowMove;

    [SerializeField] private GameObject How2EMS, ideaTrick1, ideaTrick2;
    private int nowArrow, nextPos;

    private Vector3 _defPos;

    private int unlock = 0;

    private GameObject _displaying = null;

    // Start is called before the first frame update
    void Awake()
    {
        nowArrow =  0;
        nextPos = 0;
        _defPos = arrow.transform.localPosition;

        locks = new GameObject[] { marsLock, earthLock, saturnLock, jupiterLock };

        How2EMS.SetActive(false);
        ideaTrick1.SetActive(false);
        ideaTrick2.SetActive(false);
        _displaying = null;

        ControlData.Initialized(ControlData.filetype.normal);
        unlock = ControlData.Unlock();

        for (int i = 0; i < unlock; i++)
        {
            locks[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_displaying == null)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                _displaying = DecideDisplay(nowArrow);
                if (_displaying != null)
                {
                    _displaying.SetActive(true);
                }

            } else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                nextPos--;
                if (nextPos < 0)
                {
                    nextPos = 0;
                    return;
                }
                
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                nextPos++;
                if (4 < nextPos)
                {
                    nextPos = 4;
                }
               
            }

            if (nextPos <= unlock)
            {
                nowArrow = nextPos;
                if (nextPos == 0)
                {
                    arrow.transform.localPosition = new Vector3(_defPos.x, _defPos.y - (nowArrow * arrowMove), _defPos.z);
                }
                else
                {
                    arrow.transform.localPosition = new Vector3(_defPos.x, _defPos.y - (nowArrow * arrowMove) - 21.5f, _defPos.z);
                }
                
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Return))
            {
                _displaying.SetActive(false);
                _displaying = null;
            }
        }
    }

    private GameObject DecideDisplay(int pos)
    {
        switch (pos)
        {
            case 0:
                string[] s1 = { " ", "0", ControlData.GetHeghScore().ToString(), " ", "start" };
                ControlData.CSVAddWrite(s1, ControlData.filetype.normal);


                ControlData.Initialized(ControlData.filetype.favorite);
                SceneManager.LoadScene("MainGame");
                break;
            
            case 1:

                ControlData.Initialized(ControlData.filetype.favorite);
                SceneManager.LoadScene("ReadingMode");
                break;
            
            case 2:

                return How2EMS;
       
            case 3:
                return ideaTrick1;
            
            case 4:
                return ideaTrick2;
            
        }
        return null;
    }
}
