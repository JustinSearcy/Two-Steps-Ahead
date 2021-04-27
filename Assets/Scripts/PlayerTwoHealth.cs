using UnityEngine;
using System.Collections;
using TMPro;

public class PlayerTwoHealth : MonoBehaviour
{
    [SerializeField] int maxHealth = 20;
    [SerializeField] TextMeshPro p2Health;
    [SerializeField] GameObject healthDecrease;
    [SerializeField] GameObject healthIncrease;
    [SerializeField] float healthChangeDistance = 3f;
    [SerializeField] float healthChangeTime = 2f;

    int currentHealth = 0;
    void Start()
    {
        currentHealth = maxHealth;
        p2Health.text = currentHealth.ToString();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        InstantiateHealthDecrease(damage);
        UpdateHealth();
    }

    public void Heal(int healthGained)
    {
        currentHealth += healthGained;
        InstantiateHealthIncrease(healthGained);
        UpdateHealth();
    }

    private void UpdateHealth()
    {
        p2Health.text = currentHealth.ToString();
    }

    private void InstantiateHealthDecrease(int decrease)
    {
        GameObject health = Instantiate(healthDecrease, transform.position, Quaternion.identity);
        health.GetComponent<TextMeshPro>().text = "-" + decrease;
        LeanTween.moveLocalX(health, transform.position.x + healthChangeDistance, healthChangeTime).setEaseOutQuart();
        Destroy(health, healthChangeTime);
    }

    private void InstantiateHealthIncrease(int increase)
    {
        GameObject health = Instantiate(healthIncrease, transform.position, Quaternion.identity);
        health.GetComponent<TextMeshPro>().text = "+" + increase;
        LeanTween.moveLocalX(health, transform.position.x + healthChangeDistance, healthChangeTime).setEaseOutQuart();
        Destroy(health, healthChangeTime);
    }

    public int GetHealth() { return currentHealth; }
}
