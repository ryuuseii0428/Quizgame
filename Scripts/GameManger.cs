using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManger : MonoBehaviour
{
    int ClickCount = 0;


    public string[] langu;
    public int langNum = 0;
    public int ranNum = 0;
    public int num =20;
    private float time = 0;
    private float bestTime = 0;
    public Text bestScore;
    public Text nowLangu;
    public Text nowTime;
    //public GameObject obj1;
    public GameObject obj2;
    // Start is called before the first frame update
     private void Awake() 
    {
        ranNum = Random.Range(0, langNum);


        DontDestroyOnLoad(gameObject);
        obj2.SetActive(false);

        /*float besttime = PlayerPrefs.GetFloat("besttime");
        if(besttime > 0 )
        {
            bestTime = besttime;
        }*/
        //bestScore.text = bestTime.ToString();
        nowLangu.text = langu[ranNum];
        nowTime.text = time.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClickCount++;
            if (!IsInvoking("DoubleClick"))
                Invoke("DoubleClick", 1.0f);
       
        }
        else if (ClickCount == 1)
        {
            num = 20;
            time = 0;
            ranNum = Random.Range(0, langNum);
            obj2.SetActive(false);

        }
        else if (ClickCount == 2)
        {
            CancelInvoke("DoubleClick");
            Application.Quit();
        }
       /* if(bestTime <= time)
            {
                obj1.SetActive(true);
                obj2.SetActive(true);
                
            }
        if(time < bestTime)
            {
                bestTime = time;
                
            } */
        
        if(num ==0)
        {
            obj2.SetActive(true);
        }
        if( 0 < num && num < 20)
        {
            obj2.SetActive(false);
            Score();
        }
        //bestScore.text = bestTime.ToString();
        nowLangu.text = langu[ranNum];
        nowTime.text = time.ToString();


        //PlayerPrefs.SetFloat("besttime", bestTime);
       //PlayerPrefs.Save();
    }

    public void OnClick()
    {
        if(num > 0)
        {   
            ranNum = Random.Range(0, langNum);
            Debug.Log(langu[ranNum]);
            num--;
        }
     
    }

    void Score()
    {
        time += 1 * Time.deltaTime;
    }

    void DoubleClick()
    {
        ClickCount = 0;
    }
    
}
