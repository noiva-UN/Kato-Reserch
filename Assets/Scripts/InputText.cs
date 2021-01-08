using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputText : MonoBehaviour
{
    private string[] ideas;

    public InputField[] fields = new InputField[4];

    private int _entryNum = 0;
    private bool _changeNum = false;

    [SerializeField] private GameObject _navi;

    public void Initialized()
    {
        _navi.SetActive(false);
        IdeasInitialized();
    }

    private void IdeasInitialized()
    {
        ideas = new string[4] { "", "", "", "" };
        for(int i = 0; i < fields.Length; i++)
        {
            fields[i].text = "";
        }
        _entryNum = 0;
    }

    public IEnumerator InputControl()
    {
        while (true)
        {

            yield return StartCoroutine(InputIdea(_entryNum));

            if (_changeNum)
            {
                _changeNum = false;
                continue;
            }

            bool b = false;
            //入力完了か判断して完了なら 
            for (int i = 0; i < ideas.Length; i++)
            {
                if (ideas[i] == "")
                {
                    _entryNum = i;
                    b = true;
                    break;
                }
            }

            if (b) {
                continue;
            }

            //入力完了後の処理追加
            IdeasInitialized();
            yield break;
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="field"></param>
    /// <param name="ideasNum"></param>
    /// <param name="callback">全て入力されているか</param>
    /// <returns></returns>
    private IEnumerator InputIdea(int ideasNum)
    {
        fields[ideasNum].ActivateInputField();
        if (ideas[ideasNum] != "")
        {
            fields[ideasNum].text = ideas[ideasNum];
        }

        Debug.Log(ideasNum);

        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                yield return null;
                fields[ideasNum].DeactivateInputField();
                ideas[ideasNum] = fields[ideasNum].text;

                string debug = "";
                for(int i = 0; i < ideas.Length; i++)
                {
                    debug += i + "番目に" + ideas[i] + "が、　";
                }
                Debug.Log(debug);
                yield break;       
            }

            /*
            if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
            {
                _navi.SetActive(false);
            }
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
            {
                _navi.SetActive(true);
            }
            */

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                yield return null;

                _entryNum++;
                if (ideas.Length <= _entryNum)
                {
                    _entryNum = 0;
                }
                _changeNum = true;
                yield break;
            }


            /*
            if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl))
            {
                _navi.SetActive(false);
            }
            if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
            {
                _navi.SetActive(true);
            }

            if (_navi.activeSelf)
            {
                if (Input.GetKeyDown("1"))
                {
                    _entryNum = 0;
                    _navi.SetActive(false);
                    _changeNum = true;
                    yield break;

                }
                else if (Input.GetKeyDown("2"))
                {
                    _entryNum = 1;
                    _navi.SetActive(false);
                    _changeNum = true;
                    yield break;

                }
                else if (Input.GetKeyDown("3"))
                {
                    _entryNum = 2;
                    _navi.SetActive(false);
                    _changeNum = true;
                    yield break;
                }
                else if (Input.GetKeyDown("4"))
                {
                    _entryNum = 3;
                    _navi.SetActive(false);
                    _changeNum = true;
                    yield break;

                }
            }*/
            yield return null;
        }
    }
}
