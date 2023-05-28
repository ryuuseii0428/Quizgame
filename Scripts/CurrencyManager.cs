using System;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    [SerializeField] private int currency = 0;
    [SerializeField] private int generationAmount = 1;
    [SerializeField] private float generationIntervalInSeconds = 10f;
    private DateTime lastPlayTime;
    private float remainingTime;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LoadData();
        StartCurrencyGeneration();
    }

    private void Update()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            UIManager.Instance.UpdateRemainingTimeText(remainingTime);
        }
    }

    private void OnDestroy()
    {
        SaveData();
    }

    private void StartCurrencyGeneration()
    {
        DateTime currentTime = DateTime.Now;
        TimeSpan timeSinceLastPlay = currentTime - lastPlayTime;

        float generationCount = (float)timeSinceLastPlay.TotalSeconds / generationIntervalInSeconds;
        int generatedCurrency = Mathf.FloorToInt(generationCount) * generationAmount;

        currency += generatedCurrency;
        lastPlayTime = currentTime;
        remainingTime = generationIntervalInSeconds - (float)timeSinceLastPlay.TotalSeconds % generationIntervalInSeconds;

        UIManager.Instance.UpdateCurrencyText(currency);
        UIManager.Instance.UpdateRemainingTimeText(remainingTime);
        SaveData();

        if (remainingTime <= 0)
        {
            GenerateCurrency();
            remainingTime = generationIntervalInSeconds;
        }

        Invoke(nameof(StartCurrencyGeneration), remainingTime);
    }

    private void GenerateCurrency()
    {
        currency += generationAmount;
        UIManager.Instance.UpdateCurrencyText(currency);
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt("Currency", currency);
        PlayerPrefs.SetString("LastPlayTime", lastPlayTime.ToString());
        PlayerPrefs.Save();
    }

    private void LoadData()
    {
        currency = PlayerPrefs.GetInt("Currency", 0);
        string lastPlayTimeString = PlayerPrefs.GetString("LastPlayTime", string.Empty);

        if (!string.IsNullOrEmpty(lastPlayTimeString) && DateTime.TryParse(lastPlayTimeString, out DateTime loadedTime))
        {
            lastPlayTime = loadedTime;
        }
        else
        {
            lastPlayTime = DateTime.Now;
        }

        remainingTime = generationIntervalInSeconds - (float)(DateTime.Now - lastPlayTime).TotalSeconds % generationIntervalInSeconds;
    }



}
