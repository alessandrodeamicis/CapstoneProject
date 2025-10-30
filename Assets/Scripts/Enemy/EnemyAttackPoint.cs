using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackPoint : MonoBehaviour
{
    [SerializeField] private int _damageAttack = 2;

    private void Start()
    {
        EnemyLifeController enemyLifeController = GetComponentInParent<EnemyLifeController>();
        enemyLifeController.OnEnemyDeath.AddListener(DisableObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerLifeController playerLifeController = other.gameObject.GetComponent<PlayerLifeController>();
            playerLifeController.AddHp(-_damageAttack);

            Animator playerAnimator = other.gameObject.GetComponent<Animator>();
            playerAnimator.SetTrigger("hurt");
        }
    }

    void DisableObject()
    {
        gameObject.SetActive(false);
    }
}
