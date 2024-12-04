using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FillBar : MonoBehaviour
{
    #region Variables

    [SerializeField] private Image fillBarImage; // The fill image for the bar
    [SerializeField] private TextMeshProUGUI barText; // The text display on the bar
    [SerializeField] private bool barCharge = false; // Determines if the bar is a charging bar
    [SerializeField] private string chargingMessage = string.Empty; // Message to display when charging

    private float maxBarValue = 0; // Maximum value of the bar
    private float actualBarValue = 0; // Current value of the bar

    public UnityEvent onBarReachesZero; // Event triggered when the bar reaches zero
    public UnityEvent onBarReachesMax; // Event triggered when the bar reaches maximum

    #endregion

    #region Public Methods

    /// <summary>
    /// Initializes the bar with a maximum value. Optionally sets it as a life bar.
    /// </summary>
    /// <param name="maxValue">Maximum value of the bar.</param>
    /// <param name="isLifeBar">True if the bar is a life bar, false if it's a charging bar.</param>
    public void InitBar(float maxValue, bool isLifeBar = true)
    {
        maxBarValue = maxValue;

        if (isLifeBar)
        {
            actualBarValue = maxValue;
        }

        UpdateBar(0, true);
    }

    /// <summary>
    /// Updates the bar based on the given value. Supports both damage and charge functionality.
    /// </summary>
    /// <param name="value">The value to update the bar with.</param>
    /// <param name="isDamage">True if the value represents damage, false if it represents a charge.</param>
    public void UpdateBar(float value, bool isDamage = true)
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
                actualBarValue = 0;
                onBarReachesMax?.Invoke();
            }

            if (barCharge)
            {
                barText.text = chargingMessage;
            }
        }

        UpdateBarScale();
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Updates the scale of the fill bar to match the current bar value.
    /// Ensures the scale is clamped between 0 and 1.
    /// </summary>
    private void UpdateBarScale()
    {
        // Clamp the actual value between 0 and maxBarValue
        float normalizedValue = Mathf.Clamp01(actualBarValue / maxBarValue);

        // Update the fill bar's scale
        fillBarImage.transform.localScale = new Vector3(normalizedValue, 1, 1);
    }

    #endregion
}
