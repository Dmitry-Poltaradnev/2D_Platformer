using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Controller : MonoBehaviour
{
    [SerializeField] private Transform enemyModelTransform;
    [SerializeField] private float WalkDistance = 6f;// Данная переменная отвечает за дистанцию которую будет проходить враг при патрулировании. 
    [SerializeField] private float PatrolSpeed = 1f;// Скорость передвижения при патрулировании.
    [SerializeField] private float TimeToWait = 5f;// Переменная отвечающая за ожидание между точками патрулирования.
    [SerializeField] private float timeToChase = 3f;
    [SerializeField] private float ChasingSpeed = 3f;//Скорость преследования игрока.
    [SerializeField] private float MinDistanceToPlayer = 1.5f;//Переменная отвечающая за дистанцию преследования врага(если она меньше данного значения то враг должен стоять на месте).


    private Rigidbody2D _rb;
    private Transform _playerTransform;//Переменная отвечает за позицию игрока.
    private Vector2 _LeftWalkPosition;// Правая крайняя точка патрулирования. 
    private Vector2 _RightWalkPosition;// Левая крайняя точка патрулирования.
    private Vector2 _nextPoint;

    private bool _isFacingRight = true;// Переменная для отслеживания направления. Изначально повёрнут направо.
    private bool _isWait = false;
    private bool _isChasingPlayer;//Переменная отвечающая за изменение режима из патруля в режим преследования.


    private float _chaseTime;
    private float _waitTime; //Переменная ожидания после достижения крайней точки.
    private float _walkSpeed;//Данная переменная будет отвечать за изменение скорости врага, Принимая значения PatrolSpeed либо ChasingSpeed. 

    public bool IsfacingRight//С помощью данной ф-ции мы передаём значение в другие скрипты для чтения. 
    {
        get => _isFacingRight;
    }

    public void StartChasingPlayer()//Ф-ция для преследования игрока после попадания в радиус агро. 
    {
        _isChasingPlayer = true;//Переводим врага в режим преследования.
        _chaseTime = timeToChase;//Каждый раз когда враг видит игрока, таймер обнуляется.
        _walkSpeed = ChasingSpeed;
    }

    private void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();//Ищем transform c тэгом Player.
        _rb = GetComponent<Rigidbody2D>();
        _LeftWalkPosition = transform.position;// Будет равен изначальной точке врага.
        _RightWalkPosition = _LeftWalkPosition + Vector2.right * WalkDistance;//Высчитываем крайнюю точку правую, создавая новый вектор умножая его на дистанцию движения. 
        _waitTime = TimeToWait;// Приравниваем к переменной из редактора.
        _chaseTime = timeToChase;
        _walkSpeed = PatrolSpeed;//Изначально враг будет ходить со скоростью, PatrolSpeed.
    }

    private void Update()
    {
        if (_isChasingPlayer)//При включении режима преследования, запускается таймер.
        {
            StartChasingTimer();
        }

        if (_isWait && !_isChasingPlayer)//Логика таймера для последующего разворота врага, этом он не в режиме преследования.
        {
            StartWaitTimer();
        }
        if (ShouldWait())// Враг должен ждать.
        {
            _isWait = true;
        }
    }

    private void FixedUpdate()// Вся физика прописывается в FixedUpdate 
    {
        _nextPoint = Vector2.right * _walkSpeed * Time.fixedDeltaTime;//Он идёт в правую сторону с положительной скоростью.

        if (_isChasingPlayer && Mathf.Abs(DistanceToPlayer()) < MinDistanceToPlayer)/*Если положение игрока - положение по x врага меньше минимальной дистанции, то выходим из данной функции.
                                                                   но так как по x значение может быть меньше 0 то нужно использовать метод Math.abs (возвращает абсолютное 
                                                                   значение числа то есть если  значение -3 то вернёт 3*/
        {
            return;
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
        if (!_isFacingRight)// Если он не смотрит вправо то умножаем его scale на -1. разворачиваем.
        {
            _nextPoint.x *= -1;
        }

        _rb.MovePosition((Vector2)transform.position + _nextPoint);/*Так как transform.position это Vector3 но мы приводим к Vector2 указывая Unity убрать значение z.
                                                                                                        Также для оптимизации умножаем на Time.fixedDeltaTime (fixedDeltaTime т.к мы в FixedUpdate) чтобы          
                                                                                                        на всех платформах игра вела себя одинаково */
    }

    private void ChasePlayer()
    {
        float distance = DistanceToPlayer();/*Вычисление дистанции от нашего врага до нашего игрока, если Player находится справа от врага
                                                                             то distance будет положительным, если слева то distance будет отрицательным.*/


        if (distance < 0)//Если distance > 0 то присваиваем 1 , если меньше то -1.
        {
            _nextPoint.x *= -1;
        }
        if (distance > 0.2 && !_isFacingRight)//Если Player находиться справа от врага, но он смотрит влево, то пусть он смотрит вправо.
        {
            Flip();
        }
        else if (distance < 0.2 && _isFacingRight)//Обратное условие верхнему.
        {
            Flip();
        }

        _rb.MovePosition((Vector2)transform.position + _nextPoint);
    }

    private float DistanceToPlayer()//Ф-ция будет возвращать разница(дистанцию между transform.position)
    {
        return _playerTransform.position.x - transform.position.x;
    }

    private void StartWaitTimer()//Также выводим в отдельную ф-цию.
    {
        _waitTime -= Time.deltaTime;
        if (_waitTime < 0f)
        {
            _waitTime = TimeToWait;
            _isWait = false;
            Flip();
        }
    }

    private void StartChasingTimer()//Метод таймер преследования. Все таймеры вызываются в Update.
    {
        _chaseTime -= Time.deltaTime;

        if (_chaseTime < 0)//Как только таймер истекает, преследование прекращается, то обратно переходим в режим патруля, и обнуляем таймер.
        {
            _isChasingPlayer = false;
            _chaseTime = timeToChase;
            _walkSpeed = PatrolSpeed;//После окончания таймера, скорость врага переходит обратно в PatrolSpeed.
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
        Vector3 playerScale = enemyModelTransform.localScale;
        playerScale.x *= -1;
        enemyModelTransform.localScale = playerScale;
    }
}
