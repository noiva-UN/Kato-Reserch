using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayIdeas : MonoBehaviour
{
    private List<string[]> _displatData = new List<string[]>();
    private List<string[]> _favoDatas = new List<string[]>();

    [SerializeField] private Text[] _texts = new Text[6];

    [SerializeField] private Text _testText;
    [SerializeField] private Image _Arrow;
    [SerializeField] private float _arrowMove;
    private float _textWidth;


    private int maxPage = 0, nowPage = 0;
    private int nextLine = 0, nowArrow = 0;
    private Vector3 _defArrowPos;
    private bool _reviewing = false;

    [SerializeField] private Color defColor, favoColor;

    public void Initialized()
    {
        _textWidth = _testText.rectTransform.rect.width;

        for (int i = 0; i < _texts.Length; i++)
        {
            _texts[i].text = "";
        }
        _displatData = new List<string[]>();              
        _displatData.Add(new string[_texts.Length]);
        _favoDatas = new List<string[]>();
        //_favoDatas.Add(new string[] { "", "", "", ""});

        maxPage = 0;
        nowPage = 0;
        nextLine = 0;

        _reviewing = false;
        _defArrowPos = _Arrow.gameObject.transform.localPosition;
        _Arrow.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        //Initialized();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetIdeaData(string some1, string do1, string some2, string do2)
    {
        if (_texts.Length <= nextLine)
        {
            nextLine = 0;
            maxPage++;
            //nowPage = maxPage;
            _displatData.Add(new string[_texts.Length]);
        }

        var idea = "「" + some1 + "」を「" + do1 + "」「" + some2 + "」を「" + do2 + "」ゲーム";

        _testText.text = idea;

        if (_testText.preferredWidth < _textWidth)
        {
            _displatData[maxPage][nextLine] = idea;
            nextLine++;
        }
        else
        {
            if (_texts.Length - 1 <= nextLine)
            {
                _displatData[maxPage][nextLine] = "";
                nextLine = 0;
                maxPage++;
                //nowPage = maxPage;
                _displatData.Add(new string[_texts.Length]);
            }
            var idea1 = "「" + some1 + "」を「" + do1 + "」";
            var idea2 = "「" + some2 + "」を「" + do2 + "」ゲーム";

            _displatData[maxPage][nextLine] = idea1;
            nextLine++;
            _displatData[maxPage][nextLine] = idea2;
            nextLine++;
            
        }
        nowPage = maxPage;
        IdeaDisplayUpdate(nowPage);
    }

    private void IdeaDisplayUpdate(int page)
    {
        for (int i = 0; i < _texts.Length; i++)
        {

            _texts[i].text = _displatData[page][i];
            if (!_reviewing) continue;
            var idea = GetFullIdea(nowPage, nowArrow);

            if (CheckFavo(idea))
            {
                _texts[i].color = favoColor;
            }
            else
            {
                _texts[i].color = defColor;
            }
        }
    }

    public void Reading()
    {
        _reviewing = true;
    }

    public void BackPageIdeaDisplay()
    {
        nowPage--;
        if (nowPage < 0) nowPage = 0;

        IdeaDisplayUpdate(nowPage);

        if (_reviewing)
        {
            for (; nowArrow > 0; nowArrow--)
            {
                ArrowUp();
            }
            nowArrow = 0;
        }
    }
    public void NextPageIdeaDisplay()
    {
        nowPage++;
        if (maxPage < nowPage) nowPage = maxPage;
        IdeaDisplayUpdate(nowPage);

        if (_reviewing)
        {
            for (; nowArrow > 0; nowArrow--)
            {
                ArrowUp();
            }
            nowArrow = 0;
        }
    }

    private bool CheckFavo(string idea)
    {

        for (int i = 0; i < _favoDatas.Count; i++)
        {
            var favo = "「" + _favoDatas[i][0] + "」を「" + _favoDatas[i][1] + "」「" + _favoDatas[i][2] + "」を「" + _favoDatas[i][3] + "」ゲーム";

            if (idea == favo)
            {
                return true;
            }
        }

        return false;

    }

    private string GetFullIdea (int page, int num)
    {
        var idea = _displatData[page][num];

        if (idea.Substring(idea.Length - 2) == "ム")
        {//最後が～ゲームで終わっていて
            if (num == 0)
            {//一行目→1行で完結している

            }
            if (_displatData[page][num - 1].Substring(idea.Length - 2) == "ム")
            { //上の行も～ゲームで終わっている→1行で完結している


            }
            else
            {//2行になってる
                idea = _displatData[page][num - 1] + _displatData[page][num];

            }
        }
        else
        {//2行になっている
            idea += _displatData[page][num + 1];
        }

        return idea;
    }
    public void ReviewSetUp()
    {
        nowPage = 0;

        IdeaDisplayUpdate(nowPage);

        _Arrow.gameObject.SetActive(true);
        nowArrow = 0;
        _reviewing = true;
    }

    public void ArrowUp()
    {
        nowArrow--;
        if (nowArrow < 0)
        {
            nowArrow = 0;
            return;
        }
        var pos = _Arrow.gameObject.transform.localPosition;
        _Arrow.gameObject.transform.localPosition = new Vector3(pos.x, pos.y + _arrowMove, pos.z); 
    }

    public void ArrowDown()
    {
        nowArrow++;
        if (_displatData[nowPage].Length - 1 < nowArrow)
        {
            nowArrow = _displatData[nowPage].Length - 1;
            return;
        }
        if (_displatData[nowPage][nowArrow] == "")
        {
            nowArrow--;
            return;
        }
        var pos = _Arrow.gameObject.transform.localPosition;
        _Arrow.gameObject.transform.localPosition = new Vector3(pos.x, pos.y - _arrowMove, pos.z);
    }

    public bool DecideFavo()
    {
        int gyo = 0;
        Debug.Log(nowPage + "  " + nowArrow);
        var fullidea =  _displatData[nowPage][nowArrow];

        if (fullidea.Substring(fullidea.Length - 1) == "ム")
        {//最後が～ゲームで終わっていて
            if (nowArrow == 0)
            {//一行目→1行で完結している
                gyo = 0;
            }else if (_displatData[nowPage][nowArrow - 1].Substring(_displatData[nowPage][nowArrow - 1].Length - 1) == "ム")
            { //上の行も～ゲームで終わっている→1行で完結している

                gyo = 0;
            }
            else
            {//2行になってる
                fullidea = _displatData[nowPage][nowArrow - 1] + _displatData[nowPage][nowArrow];
                gyo = -1;
            }
        }
        else
        {//2行になっている
            fullidea += _displatData[nowPage][nowArrow + 1];
            gyo = 1;
        }

        string[] ideas = fullidea.Split('」');
        var some1 = ideas[0].Substring(1);
        var do1 = ideas[1].Substring(2);
        var some2 = ideas[2].Substring(1);
        var do2 = ideas[3].Substring(2);

        //Debug.Log(some1 + " " + do1 + " " + some2 + " " + do2);

        if (CheckFavo(fullidea))
        {
            

            var match = _favoDatas.FindIndex(idea =>{
                if (some1 == idea[0] && do1 == idea[1] && some2 == idea[2] && do2 == idea[3]) { 
                    
                    
                    return true; 
                
                }
               
                return false;
            });
            _favoDatas.RemoveAt(match);

            if (gyo == -1)
            {
                _texts[nowArrow].color = defColor;
                _texts[nowArrow - 1].color = defColor;
            }
            else if (gyo == 1)
            {
                _texts[nowArrow].color = defColor;
                _texts[nowArrow + 1].color = defColor;
            }
            else
            {
                _texts[nowArrow].color = defColor;
            }

            return false;
        }
        else
        {
            if (gyo == -1)
            {
                _texts[nowArrow].color = favoColor;
                _texts[nowArrow - 1].color = favoColor;
            }
            else if (gyo == 1)
            {
                _texts[nowArrow].color = favoColor;
                _texts[nowArrow + 1].color = favoColor;
            }
            else
            {
                _texts[nowArrow].color = favoColor;
            }
            _favoDatas.Add(new string[] { some1, do1, some2, do2 });

            return true;
        }
    }
    public void EndReview()
    {
        for(int i = 0; i < _favoDatas.Count; i++)
        {
            ControlData.CSVAddWrite(_favoDatas[i], ControlData.filetype.favorite);
        }
    }
}
