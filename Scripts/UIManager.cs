using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private Text currencyText;
    [SerializeField] private Text remainingTimeText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateCurrencyText(int currency)
    {
        currencyText.text = currency.ToString();
    }

    public void UpdateRemainingTimeText(float remainingTime)
    {
        remainingTimeText.text = Mathf.CeilToInt(remainingTime).ToString();
    }
}
