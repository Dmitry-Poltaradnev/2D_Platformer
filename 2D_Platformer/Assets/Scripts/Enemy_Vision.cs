using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Vision : MonoBehaviour
{
    [SerializeField] private GameObject currentHitObject;//Данная переменная хранит в себе объект которого коснулась наша окружность.
    [SerializeField] private float circleRadius;//Радиус данной окружности.
    [SerializeField] private float maxDistance;//Предел видимости противника.
    [SerializeField] private LayerMask layerMask;//Слой который будет виден противнику.

    private Enemy_Controller _enemy_Controller;//Создаем переменную в которую передаём из Enemy_Controller через get. 
    private Vector2 _origin;//Точка где будет находиться противник.
    private Vector2 _direction;//Направления от точки origin до создания окружности.

    private float _currentHitDistance;//Расстояние от нашего противника до объекта который попал в радиус нашей окружности.

    private void Start()
    {
        _enemy_Controller = GetComponent<Enemy_Controller>();//Передаём компонент Enemy_Controller для дальнейшей работы.
    }

    private void Update()
    {
        _origin = transform.position;//Точка нахождения врага.

        if (_enemy_Controller.IsfacingRight)//Проверяем для последующего изменения направление прицела при изменении направления врага 
        {
            _direction = Vector2.right;//Создаём из origin круг который будет направлен вправо на расстояние maxDistance.
        }
        else
        {
            _direction = Vector2.left;
        }


        RaycastHit2D hit = Physics2D.CircleCast(_origin, circleRadius, _direction, maxDistance, layerMask);/*Создаем невидимый объект в форме окружности, с радиусом, на определённой
                                                                                                          * дистанции, и на определённом слое*/

        if (hit)//hit хранит информацию о collider которую он сейчас ударил. 
        {
            currentHitObject = hit.transform.gameObject;//Если какой-либо collider попал в нашу окружность, то мы  сохраняем данный игровой объект в переменную currentHitObject.
            _currentHitDistance = hit.distance;// На каком расстоянии мы ударили определённый объект.

            if (currentHitObject.CompareTag("Player"))//Если это collider с тэгом Player.
            {
                _enemy_Controller.startChasingPlayer();
            }
            else
            {
                currentHitObject = null;//Мы не видим никакого объекта.
                _currentHitDistance = maxDistance;//Дистанция по прежнему равна max.                
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;//Добавляем цвет Gizmos.
        Gizmos.DrawLine(_origin, _origin + _direction * _currentHitDistance);//Рисуем от нашего противника до нашего объекта который находится в окружности сейчас.
        Gizmos.DrawWireSphere(_origin + _direction * _currentHitDistance, circleRadius);//Прорисовываем её там где окружность ударилась об объект.
    }

}
