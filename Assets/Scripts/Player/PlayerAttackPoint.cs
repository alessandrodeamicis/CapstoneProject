using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackPoint : MonoBehaviour
{
    [SerializeField] private int _damageAttack = 3;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 7)
        {
            Animator enemyAnimator = other.gameObject.GetComponent<Animator>();
            enemyAnimator.SetTrigger("hurt");

            EnemyLifeController enemyLifeController = other.gameObject.GetComponent<EnemyLifeController>();
            enemyLifeController.AddHp(-_damageAttack);
        }
    }
}
