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
    private bool _changeNum = false, _interruption = false;

    private bool end = false;

    private bool inputing = true;

    // [SerializeField] private GameObject _navi;

    public void Initialized()
    {
        //_navi.SetActive(false);
        ideas = new string[4] { "", "", "", "" };
        IdeasInitialized();
    }

    private void IdeasInitialized()
    {
        for(int i = 0; i < ideas.Length; i++)
        {
            ideas[i] = "";
        }

        for(int i = 0; i < fields.Length; i++)
        {
            fields[i].text = "";
        }
        _entryNum = 0;
        gameObject.SetActive(false);
    }

    public IEnumerator InputControl(Action<bool, string[]> action)
    {
        end = false;
        while (true)
        {
            inputing = true;
            yield return StartCoroutine(InputIdea(_entryNum));

            if (end)
            {
                yield break;
            }

            if (_interruption)
            {
                _interruption = false;
                action(false, null);
                yield return null;

                IdeasInitialized();
                //gameObject.SetActive(false);
                yield break;
            }

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
            action(false,ideas);

            yield return null;

            IdeasInitialized();

            //gameObject.SetActive(false);

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

        //Debug.Log(ideasNum);

        while (true)
        {
            if (end)
            {
                fields[ideasNum].DeactivateInputField();
                yield break;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                yield return null;
                _interruption = true;
                yield break;
            }


            if (inputing == false)
            {
                yield return null;
                //fields[ideasNum].DeactivateInputField();
                ideas[ideasNum] = fields[ideasNum].text;
                /*
                string debug = "";
                for(int i = 0; i < ideas.Length; i++)
                {
                    debug += i + "番目に" + ideas[i] + "が、　";
                }
                Debug.Log(debug);
                */
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
                fields[ideasNum].DeactivateInputField();
                yield break;
            }
            yield return null;
        }
    }
    public void TimeUp()
    {
        end = true;
    }


    public void EndInput()
    {
        inputing = false;
    }
}
