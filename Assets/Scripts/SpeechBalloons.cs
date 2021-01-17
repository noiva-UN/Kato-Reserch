using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechBalloons : MonoBehaviour
{
    [SerializeField] private GameObject[] _praiseComments;
    [SerializeField] private GameObject[] _adviceComments;
    [SerializeField] private GameObject[] _oneMinuteComent;
    [SerializeField] private GameObject[] _halfMinuteComent;

    private GameObject _nowComent;
    private float _mathTime;
    [SerializeField] private float _displayTime = 7f;

    //private Vector3 _defPos, _inputingPos;

    public enum comentType
    {
        praise,
        advice,
        oneMinute,
        halfMinute
    }

    // Start is called before the first frame update
    void Awake()
    {
        for(int i = 0; i < _praiseComments.Length; i++)
        {
            _praiseComments[i].SetActive(false);
        }

        for (int i = 0; i < _adviceComments.Length; i++)
        {
            _adviceComments[i].SetActive(false);
        }

        for (int i = 0; i < _oneMinuteComent.Length; i++)
        {
            _oneMinuteComent[i].SetActive(false);
        }
        for (int i = 0; i < _halfMinuteComent.Length; i++)
        {
            _halfMinuteComent[i].SetActive(false);
        }

        _nowComent = null;
        _mathTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (_nowComent == null) return;

        if (_displayTime <= _mathTime)
        {
            CommentDisable();
        }
        else
        {
            _mathTime += Time.deltaTime;
        }
    }

    public void Comment(comentType type, bool inputing)
    {
        CommentDisable();
        switch (type)
        {
            case comentType.praise:
 
                _nowComent = _praiseComments[Random.Range(0, _praiseComments.Length)];

                break;
            case comentType.advice:

                _nowComent = _adviceComments[Random.Range(0, _adviceComments.Length)];            
                
                break;
            case comentType.oneMinute:
                if (!inputing)
                {
                    _nowComent = _oneMinuteComent[0];
                }
                else
                {
                    _nowComent = _oneMinuteComent[1];
                }
                break;
            case comentType.halfMinute:

                if (!inputing)
                {
                    _nowComent = _halfMinuteComent[0];
                }
                else
                {
                    _nowComent = _halfMinuteComent[1];
                }
                break;
        }

        _nowComent.SetActive(true);
        _mathTime = 0;
    }

    public void CommentDisable()
    {
        if (_nowComent == null) return;

        _nowComent.SetActive(false);
        _nowComent = null;
    }
}
