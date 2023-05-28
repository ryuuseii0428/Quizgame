using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Networking;



public class QuizGameManager : MonoBehaviour
{
    public string fileName = "quizdata";
    public List<Button> choiceButtonList;
    public int fileIndex = 2;
   
    public float currentTime = 0;
    public float totalCount = 0;
    public float correctCount = 0;
    public float currentCount = 0;
    public float timeLimit = 3;
    public float fillSpeed = 0.5f; // �������� ���� �ӵ�

    public const float exitTimeThreshold = 2.0f; // �� ��° ��ư�� ����������� �ð� ����(��)
    public int clickCount = 0; // �̽������� ��ư Ŭ�� Ƚ��
    public float lastEscapeTime = 0.0f; // ������ �̽������� ��ư�� ���� �ð�

    public float lastResetTime;
    public float resetTimeThreshold = 0.5f; // ���� �ð� (��) �Ŀ� Ŭ��ī��Ʈ �ʱ�ȭ


    public bool isReset = false;


    public Text questionText;
    public Text currentCountText;
    public Text correctCountText;
   
    private bool isPause;

    public Image timeGauge;
    public Image corretGauge;
    public Image currentGauge;

    






    private List<QuizData> quizDataList = new List<QuizData>();
    private int currentQuestionIndex;




    void Awake()
    {
        Time.fixedDeltaTime = 1f / 60f;
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadStreamingAsset(fileName, fileIndex));


        // ���� ����
        currentQuestionIndex = 0;
        SetCurrentQuestion();
        isPause = false;
        totalCount = 20;
        currentCount = totalCount;
        currentTime = timeLimit;

    }

    void FixedUpdate()
    {


        Gauge();


    }

    private void Update()
    {

        ClickCount();
        GameResult();
    }

    
    
    
    
    
    
    
    
    
    
    
    
    
    
    // ���� ���� �����ֱ�
    private void SetCurrentQuestion()
    {
        QuizData quizData = quizDataList[currentQuestionIndex];
        questionText.text = quizData.question;

        // ����� �Բ� ������ �ε����� �����ϰ� ����
        List<string> choices = quizData.choices;
        int answerIndex = quizData.answerIndex;

        for (int i = 0; i < choiceButtonList.Count; i++)
        {
            int randomIndex = Random.Range(i, choiceButtonList.Count);
            string temp = choices[i];
            choices[i] = choices[randomIndex];
            choices[randomIndex] = temp;

            if (i == answerIndex)
            {
                answerIndex = randomIndex;
            }
            else if (randomIndex == answerIndex)
            {
                answerIndex = i;
            }

            choiceButtonList[i].GetComponentInChildren<Text>().text = choices[i];
        }

        quizData.answerIndex = answerIndex;
    }

    // ���� ��ư Ŭ���� ����Ǵ� �Լ�
    public void OnClickChoiceButton(int choiceIndex)
    {
        QuizData quizData = quizDataList[currentQuestionIndex];
        if (quizData.answerIndex == choiceIndex && currentTime > 0 && currentQuestionIndex < totalCount)
        {
            Debug.Log("�����Դϴ�!");
            Debug.Log(quizData.answerIndex);
            Debug.Log(choiceIndex);
            Debug.Log("Ŭ��ī��Ʈ"+clickCount);
            correctCount++;




        }
        else
        {

            Debug.Log("�����Դϴ�!");
            Debug.Log(quizData.answerIndex);
            Debug.Log(choiceIndex);
        }



        if (currentQuestionIndex < totalCount && currentTime > 0) 
        {
            currentQuestionIndex++;
            currentCount--;
            currentTime = timeLimit;

            

            Debug.Log("���� ������ :" );

            SetCurrentQuestion();
        }
        else
        {

        }
    }

    public void GameResult()
    {

        currentCountText.text = currentCount.ToString();
        correctCountText.text = correctCount.ToString();







        if (currentTime < 0 || currentCount == 0) 
        {
            currentTime = 0;
            Gauge();
            Time.timeScale = 0;


            Debug.Log("���� ����");
            return;
        }
        else if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;

        }


    }

    public void Gauge() 
    {
        float timeFillAmount = currentTime / timeLimit;
        float currentCountFillAmount = currentCount / totalCount;
        float correctCountFillAmount = correctCount / totalCount;

        timeGauge.fillAmount = timeFillAmount;

        if (currentGauge.fillAmount != currentCountFillAmount)
        {
            currentGauge.fillAmount = Mathf.Lerp(currentGauge.fillAmount, currentCountFillAmount, fillSpeed);
        }

        if (corretGauge.fillAmount != correctCountFillAmount)
        {
            corretGauge.fillAmount = Mathf.Lerp(corretGauge.fillAmount, correctCountFillAmount, fillSpeed);
        }
    }

    void DoubleClick()
    {
        clickCount = 0;
    }

    void ResetTime()
    {
        isReset = false;
    }

    public void ClickCount()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.R))
        {
            clickCount++;
            isReset = true;

            if (!IsInvoking("DoubleClick"))
                Invoke("DoubleClick", 1f);

            if (!IsInvoking("ResetTime"))
                Invoke("ResetTime", 0.01f);
        }


        if(clickCount == 1 && isReset == true) 
        {
            Reset();
            StartCoroutine(LoadStreamingAsset(fileName, fileIndex));
        }

        else if (clickCount == 2)
        {
            CancelInvoke("DoubleClick");
            Application.Quit();
        }

        else if(clickCount > 2) 
        {
            clickCount = 2;
            DoubleClick();
        }

    }

    IEnumerator LoadStreamingAsset(string fileName, int fileIndex)
    {
        TextAsset asset = Resources.Load<TextAsset>(fileName + fileIndex);

        // ���� �ε�
        using (StringReader reader = new StringReader(asset.text))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                // ���� ������ ó��
                List<QuizData> rawDataList = new List<QuizData>();
                while (reader.Peek() != -1)
                {
                    line = reader.ReadLine();
                    var values = line.Split(',');

                    QuizData quizData = new QuizData();
                    quizData.question = values[0];
                    quizData.choices = new List<string>() { values[1], values[2], values[3], values[4] };
                    quizData.answerIndex = int.Parse(values[5]) - 1;

                    rawDataList.Add(quizData);
                }

                // ������ ���� ������ ����Ʈ�� �����ϰ� �߰�
                while (rawDataList.Count > 0)
                {
                    int randomIndex = Random.Range(0, rawDataList.Count);
                    QuizData quizData = rawDataList[randomIndex];
                    quizDataList.Add(quizData);
                    rawDataList.RemoveAt(randomIndex);
                }

                if (clickCount == 1 && isReset == true)
                {
                    Reset();

                    while (quizDataList.Count > 0)
                    {
                        int randomIndex = Random.Range(0, quizDataList.Count);
                        rawDataList.Add(quizDataList[randomIndex]);
                        quizDataList.RemoveAt(randomIndex);
                    }

                    while (rawDataList.Count > 0)
                    {
                        int randomIndex = Random.Range(0, rawDataList.Count);
                        QuizData quizData = rawDataList[randomIndex];
                        quizDataList.Add(quizData);
                        rawDataList.RemoveAt(randomIndex);
                    }
                }
            }
        }
        yield break;
    }



    // CSV ���Ͽ��� ������ ���� ������ �ҷ�����


    public void Reset()
    {
        currentQuestionIndex = 0;
        SetCurrentQuestion();
        isPause = false;
        totalCount = 20;
        currentCount = totalCount;
        currentTime = timeLimit;
        correctCount = 0;
        Time.timeScale = 1;
    }






}
