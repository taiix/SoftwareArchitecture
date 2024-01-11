using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamagable
{
    [SerializeField] private int _health;
    public GameObject floatingText;
    public int Health { get => _health; set => _health = value; }

    public void Damage(int damage)
    {
        Health -= damage;

        if (Health <= 0)
        {
            int gold = this.gameObject.GetComponent<Enemy>().gold;

            GameObject go = Instantiate(floatingText, gameObject.transform.position, Quaternion.identity);
            
            if (go.TryGetComponent<TextMesh>(out TextMesh goldText))
            {
                goldText.text = "+" + gold.ToString();
            }

            Destroy(gameObject);
            WaveManager.instance.enemies.Remove(gameObject);
            WaveManager.instance.OnEnemyListUpdated?.Invoke(WaveManager.instance.enemies);

            CurrencyManager.instance.UpdateGold(gold);
        }
    }
}
