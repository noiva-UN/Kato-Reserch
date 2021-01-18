using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    [SerializeField] private Text scoreText, starText, tiemText, ideaNumText;
    [SerializeField] private GameObject mars, earth, saturn, jupiter, sun;
    [SerializeField] private GameObject lowCom, heghCom, newrecCom, newmodeCom,newrecord;

    [SerializeField] private int marsLimit, earthLimit, saturnLimit, jupiterLimit;

    [SerializeField] private GameObject next, Arrow;
    [SerializeField] private float ArrowMove;
    private int nowArrow = 0;

    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Initialized()
    {
        mars.SetActive(false);
        earth.SetActive(false);
        saturn.SetActive(false);
        jupiter.SetActive(false);
        sun.SetActive(false);

        lowCom.SetActive(false);
        heghCom.SetActive(false);
        newrecCom.SetActive(false);
        newmodeCom.SetActive(false);

        newrecord.SetActive(false);

        next.SetActive(false);
        Arrow.SetActive(false);
    }

    public void ResultView(int score, int star, bool record, int ideaNum)
    {
        scoreText.text = score.ToString("N0");
        starText.text = star.ToString();
        var time = 300 + ideaNum * 7;
        var sec = (time % 60f);
        int min = (int)((time - sec) / 60f);
        tiemText.text = min + ":" + string.Format("{0:00}", (int)sec);
        ideaNumText.text = ideaNum.ToString();

        GameObject grade;
        var g = 1;
        if (score <= marsLimit)
        {
            grade = mars;
            g = 2;
        } else if (score <= earthLimit)
        {
            grade = earth;
            g = 2;
        } else if (score <= saturnLimit)
        {
            grade= saturn;
            g = 2;
        } else if (score <= jupiterLimit)
        {
            grade= jupiter;
            g = 3;
        }
        else
        {
            grade = sun;
            g = 4;
        }

        grade.SetActive(true);

        var last = ControlData.GetLastScore();

        if (ControlData.Unlock(g))
        {
            newmodeCom.SetActive(true);
            newrecord.SetActive(true);
        } else if (record)
        {
            newrecord.SetActive(true);
            newrecCom.SetActive(true);

        }else if(last< score)
        {
            heghCom.SetActive(true);
        }
        else
        {
            lowCom.SetActive(true);
        }

        ControlData.CSVAddWrite(score, grade.name, ControlData.filetype.normal);
    }

    public void ChoiceNext()
    {
        next.SetActive(true);
        Arrow.SetActive(true);
        nowArrow = 0;
    }
    public void UpArrow()
    {
        nowArrow--;
        if (nowArrow < 0)
        {
            nowArrow = 0;
            return;
        }
        Arrow.transform.localPosition = new Vector3(Arrow.transform.localPosition.x, Arrow.transform.localPosition.y + ArrowMove, Arrow.transform.localPosition.z);
    }
    public void DownArrow()
    {
        nowArrow++;
        if (1<nowArrow)
        {
            nowArrow = 1;
            return;
        }
        Arrow.transform.localPosition = new Vector3(Arrow.transform.localPosition.x, Arrow.transform.localPosition.y - ArrowMove, Arrow.transform.localPosition.z);
    }
    public void DecideNext()
    {
        if (nowArrow == 1)
        {
            SceneManager.LoadScene("MainGame");
        }
        else if (nowArrow == 0)
        {
            SceneManager.LoadScene("Start");
        }
    }
}