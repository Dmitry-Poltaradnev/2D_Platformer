using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Health : MonoBehaviour
{

    [SerializeField] private Slider healthSlider;//Добавляем поле слайдера hp.
    [SerializeField] private Animator _animator;
    [SerializeField] private float _totalHealth = 200f;//Изначальное кол-во hp.

    private float _health;//Это значение будет менять при получении damage, _health нынешнее значение hp игрока.

    private void Start()
    {
        _health = _totalHealth;//Передаём значение кол-ва hp в слайдер, Величина нынешнего hp не может превышать величину макс-го здоровья.
        InitHealth();
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

    private void InitHealth()
    {
        healthSlider.value = _health / _totalHealth;
    }
}
