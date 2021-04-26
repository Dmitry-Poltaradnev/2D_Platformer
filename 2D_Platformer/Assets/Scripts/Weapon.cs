using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private Attack_Controller _attackController;//Добавляем объект со скрипта Attack_Controller


    private void Start()
    {
        _attackController = transform.root.GetComponent<Attack_Controller>();/*Один из способов передачи ссылки на компонент, при использовании root он пробегается по родительским
                                                                              * объектам отдавая корневой объект на сцене находящийся выше всех в иерархии */
    }

    private void OnTriggerEnter2D(Collider2D other)//Данная ф-ция будет вызываться как только враг войдет в коллайдер оружия, проверяем является ли объект врагом либо нет.
    {
        Enemy_Controller enemy_Controller = other.GetComponent<Enemy_Controller>();
        if (enemy_Controller != null && _attackController.IsAttack)//Если мы попали в enemy_Controller и при этом мы нажали кнопку атаки, то 
        {
            Debug.Log("attack");
        }
    }
}
