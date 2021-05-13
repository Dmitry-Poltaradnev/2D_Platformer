using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private AudioSource enemyHitSound;
    [SerializeField] private float damage = 20f;//Переменная определяющая кол-во damage.
    private Attack_Controller _attackController;//Добавляем объект со скрипта Attack_Controller


    private void Start()
    {
        _attackController = transform.root.GetComponent<Attack_Controller>();/*Один из способов передачи ссылки на компонент, при использовании root он пробегается по родительским
                                                                              * объектам отдавая корневой объект на сцене находящийся выше всех в иерархии */
    }

    private void OnTriggerEnter2D(Collider2D other)//Данная ф-ция будет вызываться как только враг войдет в коллайдер оружия, проверяем является ли объект врагом либо нет.
    {
        Enemy_Health enemy_Health = other.GetComponent<Enemy_Health>();
        if (enemy_Health != null && _attackController.IsAttack)//Если мы попали в enemy_Controller и при этом мы нажали кнопку атаки, то 
        {
            enemy_Health.ReduceHealth(damage);
            Debug.Log("attack");
            enemyHitSound.Play();
        }
    }
}
