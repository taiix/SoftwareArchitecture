using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

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
        timeText.text = "Time: " + time.ToString("0:00");
    }  
    
    private void HealthChange(int health)
    {
        healthText.text = "Health: " + health;
    }

    //public static void WaveUpdateUI()
    //{
    //    OnWaveChanged?.Invoke();
    //}
    //public static void GoldUpdateUI(string text, float value)
    //{
    //    OnGoldChanged?.Invoke(text, value);
    //}
    //public static void TimeUpdateUI(string text, float value)
    //{
    //    OnTimeChanged?.Invoke(text, value);
    //}
    //public static void HealthUpdateUI(string text, float value)
    //{
    //    OnHealthChanged?.Invoke(text, value);
    //}
}
