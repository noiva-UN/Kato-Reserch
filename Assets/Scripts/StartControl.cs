using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartControl : MonoBehaviour
{
    [SerializeField] private string name;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if(Input.GetKeyDown(KeyCode.Space))
        {
            //SceneManager.sceneLoaded += GameSceneLoaded;

            // シーン切り替え
            SceneManager.LoadScene("MainGame");
        } 
    }


    private void GameSceneLoaded(Scene next, LoadSceneMode mode)
    {
        // シーン切り替え後のスクリプトを取得
        var gameManager = GameObject.FindWithTag("GameController").GetComponent<MainControl>();

        // データを渡す処理
        gameManager.setName(name);

        // イベントから削除
        SceneManager.sceneLoaded -= GameSceneLoaded;
    }
}
