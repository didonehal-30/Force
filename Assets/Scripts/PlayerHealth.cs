using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] Image health;
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject healthCanvas;
    [SerializeField] float maxHealth = 100f;
    private float currentHealth;
    void Start()
    {
        currentHealth = maxHealth;   
    }

    void Update()
    {

    }
    public void SetHealthUIActive(bool isActive)
    {
        if (healthCanvas != null)
            healthCanvas.SetActive(isActive);
    }
    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }


    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);

        //end game if health is 0
        if (currentHealth == 0)
            gameManager.GameOver();

        health.fillAmount = currentHealth / 100f;
    }
}

