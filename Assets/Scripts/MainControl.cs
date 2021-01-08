using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainControl : MonoBehaviour
{
    public string userName = "noiva";
    private string _filePath;

    [SerializeField] private GameObject InputCanvas;
    private InputText _inputText;

    private int situation = 0;

    // Start is called before the first frame update
    void Start()
    {
        _filePath = ControlData.Initialized(userName);
        _inputText = InputCanvas.GetComponent<InputText>();
        _inputText.Initialized();
        StartCoroutine(Input());
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    private IEnumerator Input()
    {
        while (true)
        {
            yield return StartCoroutine(_inputText.InputControl());
        }
    }
}
