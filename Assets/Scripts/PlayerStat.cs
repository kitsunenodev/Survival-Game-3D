using UnityEngine;
using UnityEngine.UI;

public class PlayerStat : MonoBehaviour
{
    [Header("REFERENCES")] 
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private MoveBehaviour playerMovement;
    
    [Header("Health")]
    [SerializeField]
    private float maxHealth = 100;
    public float currentHealth;
    [SerializeField]
    private Image healthBarFill;
    
    [Header("Hunger")]
    [SerializeField]
    private float maxHunger = 100;
    public float currentHunger;
    [SerializeField]
    private Image hungerBarFill;
    [SerializeField]
    private float hungerBarDecreaseRate;
    
    [Header("Thirst")]
    [SerializeField]
    private float maxThirst = 100;
    public float currentThirst;
    [SerializeField]
    private Image thirstBarFill;
    [SerializeField]
    private float thirstBarDecreaseRate;

    public float armorPoints;
    public bool isDead;
    
    // Start is called before the first frame update
    void Awake()
    {
        currentHealth = maxHealth;
        currentHunger = maxHunger;
        currentThirst = maxThirst;
    }

    // Update is called once per frame
    void Update()
    {
        HungerAndThirst();
    }

    public void TakeDamage(float damage, bool overTime = false)
    {
        if (overTime)
        {
            currentHealth -= damage * Time.deltaTime;
        }
        else
        {
           currentHealth -= damage * (1 - (armorPoints/100)); 
        }

        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
        
        
        UpdateHealthBarFill();
    }

    private void Die()
    {
        isDead = true;
        playerMovement.canMove = false;
        hungerBarDecreaseRate = thirstBarDecreaseRate = 0;
        animator.SetTrigger("Die");
    }

    public void UpdateHealthBarFill()
    {
        healthBarFill.fillAmount = currentHealth / maxHealth;
    }
    private void UpdateHungerAndThirstBarFill()
    {
        hungerBarFill.fillAmount = currentHunger / maxHunger;
        thirstBarFill.fillAmount = currentThirst / maxThirst;
    }
    

    private void HungerAndThirst()
    {
        currentHunger -= hungerBarDecreaseRate * Time.deltaTime;
        currentThirst -= thirstBarDecreaseRate * Time.deltaTime;

        currentHunger = currentHunger < 0 ? 0 : currentHunger;
        currentThirst = currentThirst < 0 ? 0 : currentThirst;

        if (currentHunger <= 0 || currentThirst <= 0)
        {
            TakeDamage(0.1f, true);
        }
        UpdateHungerAndThirstBarFill();
    }

    public void ConsumeItem(float health, float hunger, float thirst)
    {
        currentHealth += health;
        currentHealth = currentHealth > maxHealth ? maxHealth : currentHealth;
        UpdateHealthBarFill();
        currentHunger += hunger;
        currentHunger = currentHunger > maxHunger ? maxHunger : currentHunger;
        currentThirst += thirst;
        currentThirst = currentThirst > maxThirst ? maxThirst : currentThirst;
    }
}
