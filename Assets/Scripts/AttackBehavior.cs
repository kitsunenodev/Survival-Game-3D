using UnityEngine;

public class AttackBehavior : MonoBehaviour
{
    [Header("REFERENCES")] 
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private Equipment equipment;

    [SerializeField]
    private UIManager uiManager;
    [SerializeField]
    private InteractBehavior interactBehavior;
    
    [Header("PARAMETERS")]
    private bool isAttacking;
    
    [SerializeField]
    private float attackRange;

    [SerializeField]
    private LayerMask layers;

    [SerializeField] private Vector3 attackOffset;
    
    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position + attackOffset, transform.forward * attackRange, Color.red);
        if (Input.GetMouseButtonDown(0) && CanAttack())
        {
            Attack();
        }
    }

    void Attack()
    {
        isAttacking = true; 
        animator.SetTrigger("Attack");
        RaycastHit hit;
        if (Physics.Raycast(transform.position + attackOffset, transform.forward, out hit, attackRange, layers ))
        {
            if (hit.transform.CompareTag("AI"))
            {
                Debug.Log("hit");
                EnemyAI enemyAI = hit.transform.GetComponent<EnemyAI>();
                enemyAI.TakeDamage(equipment.weaponItem.damage);
            }
        }

    }

    private bool CanAttack()
    {
        return !isAttacking && equipment.weaponItem != null && !uiManager.panelOpen && !interactBehavior.isBusy;
        Debug.Log(isAttacking);
    }

    public void EndAttack()
    {
        isAttacking = false;
    }
}
