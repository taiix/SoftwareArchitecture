using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// Manages the health and damage behavior of enemy GameObjects.
/// Implements the IDamagable interface for handling damage.
/// </summary>

public class EnemyHealth : MonoBehaviour, IDamagable
{
    public UnityAction<int> OnEnemyAttributesChanged;

    [SerializeField] private int maxHealth;     
    [SerializeField] private GameObject floatingText;  
    [SerializeField] private Image healthbar;        

    private int currentHealth;                      

    // Property to access and modify the current health
    public int Health { get => currentHealth; set => currentHealth = value; }

    void Awake() { 
    
        OnEnemyAttributesChanged += OnEnemyHealthIncreased;
    }

    void Start()
    {

        currentHealth = maxHealth;
    }

    private void OnEnemyHealthIncreased(int increaser)
    {
        int multiplier = 1;
        maxHealth += increaser + multiplier;
        Debug.Log(maxHealth);
    }

    private void Update()
    {
        healthbar.gameObject.transform.LookAt(Camera.main.transform);
    }

    /// <summary>
    /// Applies damage to the enemy and updates the health bar.
    /// </summary>
    /// <param name="damage"></param>
    public void Damage(int damage)
    {
        Health -= damage;   
        UpdateHealthbar(((float)currentHealth / (float)maxHealth));   

        if (Health <= 0)
        {
            int gold = this.gameObject.GetComponent<Enemy>().gold;  

            GameObject go = Instantiate(floatingText, gameObject.transform.position + Vector3.up, Quaternion.identity);

            if (go.TryGetComponent<TextMesh>(out TextMesh goldText))
            {
                goldText.text = "+" + gold.ToString();
            }

            Destroy(gameObject);

            WaveManager.instance?.enemies.Remove(gameObject);
            WaveManager.instance?.OnEnemyListUpdated?.Invoke(WaveManager.instance?.enemies);
            
            CurrencyManager.instance?.UpdateGold(gold);
        }
    }

    // Method to update the health bar display
    void UpdateHealthbar(float damage)
    {
        healthbar.fillAmount = damage;
    }
}
