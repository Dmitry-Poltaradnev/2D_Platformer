using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Enemy_Health : MonoBehaviour
{


    [SerializeField] private Animator _animator;
    [SerializeField] private Slider healthSlider;//Поле для hp enemy.
    [SerializeField] private float totalHealth = 100f;//Изначальное кол-во hp.
    private float _health;
    private void Start()
    {
        _health = totalHealth;
        InitHealth();
    }

    private void InitHealth()//Ф-ция для инициализации кол-ва hp.
    {
        healthSlider.value = _health / totalHealth;// При вызове ReduceHealth приравниваем к Slider.
    }

    public void ReduceHealth(float damage)//Ф-ция для понижения хп принимает значение damage из скрипта Weapon.
    {
        _health -= damage;//При вызове ReduceHealth уменьшаем health.
        InitHealth();
        _animator.SetTrigger("takeDamage");
        if (_health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }
}
