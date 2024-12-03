using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FillBar : MonoBehaviour
{
    [SerializeField] private Image fillBarImage; // The fill image for the bar
    [SerializeField] private TextMeshProUGUI barText; // The text display on the bar
    [SerializeField] private bool barCharge = false;
    [SerializeField] private string chargingMessage = string.Empty;

    private int maxBarValue = 0; // Maximum value of the bar
    private int actualBarValue = 0; // Current value of the bar

    public UnityEvent onBarReachesZero; // Event triggered when the bar reaches zero
    public UnityEvent onBarReachesMax; // Event triggered when the bar reaches maximum

    /// <summary>
    /// Initializes the bar with a maximum value. Optionally sets it as a life bar.
    /// </summary>
    public void InitBar(int maxValue, bool isLifeBar = true)
    {
        maxBarValue = maxValue;

        if (isLifeBar)
        {
            actualBarValue = maxValue;           
        }
    }

    /// <summary>
    /// Updates the bar based on the given value. Supports both damage and charge functionality.
    /// </summary>
    public void UpdateBar(int value, bool isDamage = true)
    {
        if (isDamage)
        {
            actualBarValue -= value;

            if (actualBarValue <= 0)
            {
                actualBarValue = 0;
                onBarReachesZero?.Invoke();
            }

            barText.text = $"{actualBarValue}/{maxBarValue}";
        }
        else
        {
            actualBarValue += value;
           
            if (actualBarValue >= maxBarValue)
            {
                actualBarValue = maxBarValue;
                onBarReachesMax?.Invoke();
            }

            if (barCharge)
            {
                barText.text = chargingMessage;
            }
        }

        UpdateBarScale();
    }

    /// <summary>
    /// Updates the scale of the fill bar to match the current bar value.
    /// Ensures the scale is clamped between 0 and 1.
    /// </summary>
    private void UpdateBarScale()
    {
        // Clamp the actual value between 0 and maxBarValue
        float normalizedValue = Mathf.Clamp01((float)actualBarValue / maxBarValue);

        // Update the fill bar's scale
        fillBarImage.transform.localScale = new Vector3(normalizedValue, 1, 1);

        
    }
}
