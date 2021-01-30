using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainControl : MonoBehaviour
{

    //public string userName = "noiva";
    private string _filePath;

    [SerializeField] private GameObject inputCanvas, dispayCanvas, optionCanvas, 
        speechCanvas, resultCanvas, countDownWin, mainRuleWin, endWin, ReviewRule;
    private InputText _inputText;
    private DisplayIdeas _diplayIdeas;
    private SpeechBalloons _speechBalloons;
    private Result _result;
    [SerializeField] private GameObject reviewWindow;
    [SerializeField] private Image _optionArrow;
    [SerializeField] private float arrowMove = 137;
    private int _optionNum = 1;
    [SerializeField] private int countDown = 3, EndWinTime = 4;
    [SerializeField] private Text countText, highText, nextTime;

    private bool _inputing = false;
    private string[] _inputIdea= new string[4];

    [SerializeField] private float gameTimeSec = 300, addBonusTime = 5;
    private bool oneMinute = false, halfMinute = false;
    private float _mathTime = 0;
    [SerializeField] private Text timeText, starText, scoreText, ideaNumText;

    private int _star = 0, _Score = 0, _idea = 0;
    [SerializeField] private int addScore=10;


    private enum MainState
    {
        opning,
        idle,
        inputing,
        waiting,
        mainend,
        review,
        result,
        nextchoice
    }
    private MainState _mainState = MainState.opning;


    public void setName(string name)
    {
        //userName = name;
    }

      // Start is called before the first frame update
    void Start()
    {
       // ControlData.Initialized(ControlData.filetype.normal);

        //ControlData.Initialized(ControlData.filetype.favorite);
        _star = ControlData.FavoriteIdeNum();
        _Score = 0;
        _idea = 0;
        //Debug.Log(_star);

        reviewWindow.SetActive(false);

        optionCanvas.SetActive(false);
        //_optionArrow.gameObject.SetActive(false);

        _inputText = inputCanvas.GetComponent<InputText>();
        _inputText.Initialized();
        //inputCanvas.SetActive(false);
        //StartCoroutine(Inputidea());

        ScoresUpdate();

        _diplayIdeas = dispayCanvas.GetComponent<DisplayIdeas>();
        _diplayIdeas.Initialized();

        _speechBalloons = speechCanvas.GetComponent<SpeechBalloons>();

        _result = resultCanvas.GetComponent<Result>();
        resultCanvas.SetActive(false);

        _mathTime = 0;
        mainRuleWin.SetActive(true);

        _mainState = MainState.opning;

        //_speechBalloons.CharaActive(false);
    }

    // Update is called once per frame
    void Update()
    {


        switch (_mainState)
        {
            case MainState.opning:
                Opning();
                break;

            case MainState.idle:
                IdleUpdate();

                break;
            case MainState.inputing:
                InputingUpdate();

                break;
            case MainState.waiting:
                WaitngUpdate();

                break;

            case MainState.mainend:
                MainEnd();

                break;
            case MainState.review:
                ReviewUpdate();

                break;
            case MainState.result:
                ResultUpdate();

                break;
            case MainState.nextchoice:
                NextChoiceUpdate();

                break;

            default:
                break;

        }

    }

    #region opning

    private void Opning()
    {
        if (mainRuleWin.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                mainRuleWin.SetActive(false);
                countDownWin.SetActive(true);
                highText.text = "ハイスコア:" + ControlData.GetHeghScore();
                _mathTime = countDown;
            }
        }
        else
        {
            if (_mathTime <= 0)
            {
                _mathTime = 0;
                countDownWin.SetActive(false);
                _speechBalloons.CharaActive(true);
                _mainState = MainState.idle;
            }
            else
            {
                _mathTime -= Time.deltaTime;
                countText.text = Mathf.CeilToInt(_mathTime).ToString();
            }
        }
    }

    #endregion

    #region idle
    private void IdleUpdate()
    {
        if (gameTimeSec - _mathTime <= 30 && halfMinute == false)
        {
            _speechBalloons.CommentDisable();
            _speechBalloons.Comment(SpeechBalloons.comentType.halfMinute, false);
            //Debug.Log("30");
            halfMinute = true;
        }
        else if (gameTimeSec - _mathTime <= 60 && oneMinute == false)
        {
            _speechBalloons.CommentDisable();
            _speechBalloons.Comment(SpeechBalloons.comentType.oneMinute, false);
            //Debug.Log("60");
            oneMinute = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _mainState = MainState.waiting;
            optionCanvas.SetActive(true);
            _optionArrow.gameObject.SetActive(true);
            _speechBalloons.CommentDisable();
        }else if (Input.GetKeyDown(KeyCode.Return))
        {
            _mainState = MainState.inputing;
            inputCanvas.SetActive(true);
            _inputing = true;
            _speechBalloons.CommentDisable();
            _speechBalloons.CharaActive(false);
            StartCoroutine(_inputText.InputControl((r, s) => (_inputing, _inputIdea) = (r, s)));

        }else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _diplayIdeas.BackPageIdeaDisplay();
        }else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _diplayIdeas.NextPageIdeaDisplay();
        }
        TimeMath();
    }
    #endregion

    #region input
    private void InputingUpdate()
    {
        if(gameTimeSec-_mathTime <= 30 && halfMinute == false)
        {
            _speechBalloons.CommentDisable();
            _speechBalloons.Comment(SpeechBalloons.comentType.halfMinute, true);
            halfMinute = true;
        }else if (gameTimeSec - _mathTime <= 60 && oneMinute == false)
        {
            _speechBalloons.CommentDisable();
            _speechBalloons.Comment(SpeechBalloons.comentType.oneMinute, true);
            oneMinute = true;
        }

        if (!_inputing)
        {

            _speechBalloons.CommentDisable();

            if (_inputIdea == null)
            {
                _mainState = MainState.idle;
                _speechBalloons.CharaActive(true);
                //inputCanvas.SetActive(false);
                return;
            }                  

            //データの保存、スコア・星の加算判断、表示
            if (ControlData.CheckIdeaOverlapInGame(_inputIdea))
            {
                //ゲーム中のアイデアと被った処理
                _speechBalloons.Comment(SpeechBalloons.comentType.advice, false);
                
            }
            else if (ControlData.CheckIdeaOverlapInCSV(_inputIdea))
            {
                //過去のアイデアと被った処理
                _speechBalloons.Comment(SpeechBalloons.comentType.advice, false);
            }
            else
            {
                //新しいアイデアを出した処理
                _speechBalloons.Comment(SpeechBalloons.comentType.praise, false);

            }

            AddTime(addBonusTime);
            _Score += addScore;
            _idea++;

            ControlData.CSVAddWrite(_inputIdea, ControlData.filetype.normal);
            _diplayIdeas.SetIdeaData(_inputIdea[0], _inputIdea[1], _inputIdea[2], _inputIdea[3]);
            
            ScoresUpdate();

            _mainState = MainState.idle;
            _speechBalloons.CharaActive(true);
            
            if(30 <= gameTimeSec - _mathTime)
            {
                halfMinute = false;
            }
            if (60 <= gameTimeSec - _mathTime)
            {
                oneMinute = false;
            }
            //inputCanvas.SetActive(false);

        }
        TimeMath();
    }

    #endregion

    #region waitng
    private void WaitngUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _mainState = MainState.idle;
            optionCanvas.SetActive(false);
            _optionArrow.gameObject.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            switch (_optionNum)
            {
                case 2:
                    //タイトル行きの処理
                    ControlData.CSVAddWrite(0, "", ControlData.filetype.normal);
                    SceneManager.LoadScene("Start");
                    //ControlData.Initialized(ControlData.filetype.normal);
                    
                    break;
                case 1:
                    //ギブアップの処理
                    optionCanvas.SetActive(false);

                    _mainState = MainState.mainend;
                    endWin.SetActive(true);
                    _mathTime = EndWinTime;

                    _star = 0;
                    reviewWindow.SetActive(true);
                    starText.text = _star.ToString();
                    scoreText.gameObject.SetActive(false);
                    timeText.gameObject.SetActive(false);
                    _diplayIdeas.ReviewSetUp();

                    Debug.Log("giv");
                    break;

                case 0:
                    _mainState = MainState.idle;
                    optionCanvas.SetActive(false);
                    _optionArrow.gameObject.SetActive(false);
                    break;
            }

        }
        else
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                var move = 0f;
                _optionNum--;
                if (_optionNum < 0)
                {
                    _optionNum = 2;
                    move = -arrowMove * 2;
                }
                else
                {
                    move = arrowMove;
                }
                _optionArrow.gameObject.transform.localPosition = new Vector3(_optionArrow.gameObject.transform.localPosition.x, _optionArrow.gameObject.transform.localPosition.y - move, _optionArrow.gameObject.transform.localPosition.z);
            }else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                var move = 0f;
                _optionNum++;
                if (2 < _optionNum)
                {
                    _optionNum = 0;
                    move = arrowMove * 2;
                }
                else
                {
                    move = -arrowMove;
                }
                _optionArrow.gameObject.transform.localPosition = new Vector3(_optionArrow.gameObject.transform.localPosition.x, _optionArrow.gameObject.transform.localPosition.y - move, _optionArrow.gameObject.transform.localPosition.z);
            }
        }        
    }

    private void MainEnd()
    {
        if (endWin.activeSelf)
        {
            if (_mathTime <= 0)
            {
                endWin.SetActive(false);

                ReviewRule.SetActive(true);

                inputCanvas.SetActive(false);
                return;
            }
            _mathTime -= Time.deltaTime;
            nextTime.text ="Next " + _mathTime.ToString("n2");
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                ReviewRule.SetActive(false);
                _mainState = MainState.review;
                
            }
        }
    }

    private void ReviewUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            _diplayIdeas.EndReview();

            _mainState = MainState.result;

            resultCanvas.SetActive(true);
            _result.Initialized();
            _Score += _star * 100;
            _result.ResultView(_Score, _star, _idea);

            return;
        }
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            if (_diplayIdeas.DecideFavo())
            {
                _star++;
                starText.text = _star.ToString();
            }
            else
            {
                _star--;
                starText.text = _star.ToString();
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _diplayIdeas.ArrowUp();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _diplayIdeas.ArrowDown();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _diplayIdeas.BackPageIdeaDisplay();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _diplayIdeas.NextPageIdeaDisplay();
        }

    }

    #endregion


    #region result

    private void ResultUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            _result.ChoiceNext();
            _mainState = MainState.nextchoice;
        }
        
    }
    private void NextChoiceUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            _result.DecideNext();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _result.UpArrow();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _result.DownArrow();
        }
    }


    #endregion

    private void TimeMath()
    {
        if (gameTimeSec <= _mathTime)
        {
            StopAllCoroutines();
            //終わり

            if (_mainState == MainState.inputing)
            {
                _inputText.TimeUp();
                
            }

            

            
            _mainState = MainState.mainend;
            endWin.SetActive(true);
            _mathTime = EndWinTime;


            _star = 0;
            reviewWindow.SetActive(true);
            starText.text = _star.ToString();
            scoreText.gameObject.SetActive(false);
            timeText.gameObject.SetActive(false);
            _diplayIdeas.ReviewSetUp();

            return;
        }
        else
        {
            _mathTime += Time.deltaTime;
        }
        TimeUpdate();
    }
    private void AddTime(float arg)
    {
        _mathTime -= arg;
        if (30 < _mathTime)
        {
            halfMinute = false;
        }else if(60 < _mathTime)
        {
            oneMinute = false;
        }
        TimeUpdate();
    }

    private void TimeUpdate()
    {
        var time = gameTimeSec - _mathTime;

        var sec = (time % 60f);
        int min = (int)((time - sec) / 60f);

        timeText.text = min + ":" + string.Format("{0:00}", (int)sec);
    }

    private void ScoresUpdate()
    {
        starText.text = _star.ToString();
        scoreText.text = _Score.ToString("N0");
        ideaNumText.text = _idea.ToString();
    }
}
