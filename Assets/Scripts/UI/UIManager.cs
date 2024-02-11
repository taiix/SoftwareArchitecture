using TMPro;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
///  Manages UI elements and updates them based on game events.
/// </summary>

public class UIManager : MonoBehaviour
{
    public static UnityAction<int, int> OnWaveChanged;
    public static UnityAction<int> OnGoldChanged;
    public static UnityAction<float> OnTimeChanged;
    public static UnityAction<int> OnHealthChanged;

    public TextMeshProUGUI waveText;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI healthText;

    private void OnEnable()
    {
        OnWaveChanged += WaveChange;
        OnGoldChanged += GoldChange;
        OnTimeChanged += TimeChange;
        OnHealthChanged += HealthChange;
    }
    private void OnDisable()
    {
        OnWaveChanged -= WaveChange;
        OnGoldChanged -= GoldChange;
        OnTimeChanged -= TimeChange;
        OnHealthChanged -= HealthChange;
    }

    private void WaveChange(int currentWave, int maxWaves)
    {
        waveText.text = "Wave: " + currentWave + "/" + maxWaves;
    }

    private void GoldChange(int gold)
    {
        goldText.text = "Gold: " + gold;
    }

    private void TimeChange(float time)
    {
        timeText.text = "Preparing State: " + time.ToString("0:00");
    }

    private void HealthChange(int health)
    {
        healthText.text = "Health: " + health;
    }
}
