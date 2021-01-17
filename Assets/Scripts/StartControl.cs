using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartControl : MonoBehaviour
{
    [SerializeField] private GameObject marsLock, earthLock, saturnLock, jupiterLock, sunLock;
    private GameObject[] locks;
    [SerializeField] private GameObject arrow;
    [SerializeField] private Vector2 arrowMove;

    [SerializeField] private GameObject How2EMS, ideaTrick1, ideaTrick2, ideaTrick3;
    private Vector2 nowArrow, nextPos;

    private Vector3 _defPos;

    private int unlock = 0;

    private GameObject _disolaying = null;

    // Start is called before the first frame update
    void Awake()
    {
        nowArrow = new Vector2(0, 0);
        nextPos = new Vector2(0, 0);
        _defPos = arrow.transform.localPosition;

        locks = new GameObject[] { marsLock, earthLock, saturnLock, jupiterLock, sunLock };

        How2EMS.SetActive(false);
        ideaTrick1.SetActive(false);
        ideaTrick2.SetActive(false);
        ideaTrick3.SetActive(false);
        _disolaying = null;

        

        ControlData.Initialized(ControlData.filetype.normal);

        ControlData.Initialized(ControlData.filetype.favorite);

        unlock = ControlData.Unlock();
        Debug.Log(unlock);

        for(int i = 0; i < unlock; i++)
        {
            locks[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_disolaying == null)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                _disolaying = DecideDisplay(nowArrow);

            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                nextPos.x++;
                nextPos.x = nextPos.x % 2;
                nextPos.y = 0;                
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                nextPos.y--;
                if (nextPos.y < 0)
                {
                    nextPos.y = 0;
                    return;
                }
                
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                nextPos.y++;
                if (nextPos.x == 0 && 1 < nextPos.y)
                {
                    nextPos.y = 1;
                    return;
                }
                else if (nextPos.x == 1 && 3 < nextPos.y)
                {
                    nextPos.y = 3;
                }
               
            }

            var num = (int)nextPos.y + (int)nextPos.x * 2;
            if (num <= unlock)
            {
                nowArrow = nextPos;
                arrow.transform.localPosition = new Vector3(_defPos.x + (nowArrow.x * arrowMove.x), _defPos.y - (nowArrow.y * arrowMove.y)+nowArrow.x*100, _defPos.z);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Return))
            {
                _disolaying.SetActive(false);
                _disolaying = null;
            }
        }
    }

    private GameObject DecideDisplay(Vector2 pos)
    {
        int dis = (int)pos.y + (int)pos.x * 2;

        switch (dis)
        {
            case 0:
                SceneManager.LoadScene("MainGame");
                break;
            
            case 1:
                //閲覧モード
                break;
            
            case 2:

                return How2EMS;

            
            case 3:
                return ideaTrick1;
            
            case 4:
                return ideaTrick2;
            
            case 5:
                return ideaTrick3;
            
        }
        return null;
    }

}
