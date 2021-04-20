﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Controller : MonoBehaviour
{
    [SerializeField] private float WalkDistance = 6f;// Данная переменная отвечает за дистанцию которую будет проходить враг при патрулировании. 
    [SerializeField] private float WalkSpeed = 1f;// Скорость передвижения.
    [SerializeField] private float TimeToWait = 5f;// Переменная отвечающая за ожидание между точками патрулирования.

    private Rigidbody2D _rb;
    private Transform _playerTransform;//Переменная отвечает за позицию игрока.
    private Vector2 _LeftWalkPosition;// Правая крайняя точка патрулирования. 
    private Vector2 _RightWalkPosition;// Левая крайняя точка патрулирования.
    private Vector2 _nextPoint;

    private bool _isFacingRight = true;// Переменная для отслеживания направления. Изначально повёрнут направо.
    private bool _isWait = false;
    private bool _isChasingPlayer;//Переменная отвечающая за изменение режима из патруля в режим преследования.
    private float _waitTime; //Переменная ожидания после достижения крайней точки.

    public bool IsfacingRight//С помощью данной ф-ции мы передаём значение в другие скрипты для чтения. 
    {
        get => _isFacingRight;
    }

    public void startChasingPlayer()//Ф-ция для преследования игрока после попадания в радиус агро. 
    {
        _isChasingPlayer = true;//Переводим врага в режим преследования.
    }

    private void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();//Ищем transform c тэгом Player.
        _rb = GetComponent<Rigidbody2D>();
        _LeftWalkPosition = transform.position;// Будет равен изначальной точке врага.
        _RightWalkPosition = _LeftWalkPosition + Vector2.right * WalkDistance;//Высчитываем крайнюю точку правую, создавая новый вектор умножая его на дистанцию движения. 
        _waitTime = TimeToWait;// Приравниваем к переменной из редактора.
    }

    private void Update()
    {
        if (_isWait)//Логика таймера для последующего разворота врага.
        {
            Wait();
        }
        if (ShouldWait())// Враг должен ждать.
        {
            _isWait = true;
        }
    }

    private void FixedUpdate()// Вся физика прописывается в FixedUpdate 
    {
        _nextPoint = Vector2.right * WalkSpeed * Time.fixedDeltaTime;//Он идёт в правую сторону с положительной скоростью.
        if (!_isFacingRight)// Если он не смотрит вправо то умножаем его scale на -1. разворачиваем.
        {
            _nextPoint.x *= -1;
        }

        if (_isChasingPlayer)
        {
            ChasePlayer();
        }


        if (!_isWait && !_isChasingPlayer)
        {
            Patrol();
        }
    }
    private void Patrol()
    {
        _rb.MovePosition((Vector2)transform.position + _nextPoint);/*Так как transform.position это Vector3 но мы приводим к Vector2 указывая Unity убрать значение z.
                                                                                                        Также для оптимизации умножаем на Time.fixedDeltaTime (fixedDeltaTime т.к мы в FixedUpdate) чтобы          
                                                                                                        на всех платформах игра вела себя одинаково */
    }

    private void ChasePlayer()
    {
        float distance = _playerTransform.position.x - transform.position.x;/*Вычисление дистанции от нашего врага до нашего игрока, если Player находится справа от врага
                                                                                 то distance будет положительным, если слева то distance будет отрицательным.*/


        float multiplier = distance > 0 ? 1 : -1;//Если distance > 0 то присваиваем 1 , если меньше то -1.
        _nextPoint *= multiplier;

        _rb.MovePosition((Vector2)transform.position + _nextPoint);
    }

    private void Wait()//Также выводим в отдельную ф-цию.
    {
        _waitTime -= Time.deltaTime;
        if (_waitTime < 0f)
        {
            _waitTime = TimeToWait;
            _isWait = false;
            Flip();
        }
    }

    private bool ShouldWait()//Выводим в функцию для того чтоб убрать из Update.
    {
        bool isOutOfRightBoundary = _isFacingRight && transform.position.x >= _RightWalkPosition.x; //Если он направлен вправо и его текущая позиция >= крайней правой точки патруля.
        bool isOutOfLeftBoundary = !_isFacingRight && transform.position.x <= _LeftWalkPosition.x;

        return isOutOfRightBoundary || isOutOfLeftBoundary;
    }

    private void OnDrawGizmos()//Метод для отрисовки линии движения .
    {
        Gizmos.color = Color.red;//Ф-ция для изменения цвета линии отрисовки.
        Gizmos.DrawLine(_LeftWalkPosition, _RightWalkPosition);//Отрисовка от левой точки до правой точки.
    }

    void Flip()//Метод для разворота врага, аналог разворота персонажа.
    {
        _isFacingRight = !_isFacingRight;
        Vector3 playerScale = transform.localScale;
        playerScale.x *= -1;
        transform.localScale = playerScale;
    }
}