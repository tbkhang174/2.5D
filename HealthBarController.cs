using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    public Animator healthBarAnimator;
    public float maxHealth = 100f;
    public float currentHealth;

    public GameObject healthBarObject;
    public float hideDelay = 5f;

    private float hideTimer;

    public float regenSpeed = 2f;
    public bool isRegenerating = false;

    void Start()
    {
        currentHealth = maxHealth;
        healthBarObject.SetActive(false);
        UpdateHealthUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReduceHealth(10f);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            PlusHealth(10f);
        }

        if (isRegenerating && currentHealth < maxHealth)
        {
            RegenerateHealth();
        }

        if (healthBarObject.activeSelf && hideTimer <= 0)
        {
            healthBarObject.SetActive(false);
        }
        else if (healthBarObject.activeSelf)
        {
            hideTimer -= Time.deltaTime;
        }
    }

    public void ReduceHealth(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);
        UpdateHealthUI();
        ShowHealthBar();
        isRegenerating = true;
    }

    public void PlusHealth(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UpdateHealthUI();
        ShowHealthBar();
    }

    void UpdateHealthUI()
    {
        float healthPercentage = currentHealth / maxHealth;
        healthBarAnimator.SetFloat("HealthPercentage", healthPercentage);
        Debug.Log("Current Health: " + currentHealth + ", Health Percentage: " + healthPercentage);
    }

    void ShowHealthBar()
    {
        healthBarObject.SetActive(true);
        hideTimer = hideDelay;
    }

    void RegenerateHealth()
    {
        currentHealth += regenSpeed * Time.deltaTime;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHealthUI();
        ShowHealthBar();

        if (currentHealth >= maxHealth)
        {
            isRegenerating = false;
        }
    }
}
