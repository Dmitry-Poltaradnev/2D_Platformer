using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Health : MonoBehaviour
{
    private Animator _animator;

    [SerializeField] private float health = 100f;//Изначальное кол-во хелс-поинтов.

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void ReduceHealth(float damage)//Ф-ция для понижения хп принимает значение damage из скрипта Weapon.
    {
        health -= damage;//При вызове ReduceHealth уменьшаем health.
        _animator.SetTrigger("takeDamage");
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }
}
