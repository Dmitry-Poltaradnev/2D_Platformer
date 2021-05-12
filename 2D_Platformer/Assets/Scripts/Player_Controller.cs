using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    [SerializeField] private float SpeedX = -1f;//Публичная переменная для последующего изменения скорости.
                                                //При использовании [SerializeField] и private переменной - это значит, что другие скрипты не будут иметь доступа к данной переменной, кроме вынесенного поля в редакторе.

    [SerializeField] private Animator animator;// Получаем доступ к переменной аниматора.
    [SerializeField] Transform playerModelTransform;// Переменная для последующего Flipa самой модели персонажа.
    private Rigidbody2D _rb;
    private Finish _finish;
    private Level_Arm _level_Arm;
    private AudioSource _jumpSound;//Получаем доступ к переменной AudioSource.

    private bool _isFinish = false;

    private float _horizontal = 0f;
    private bool _isGround = false; // Данная переменная говорит нам находится ли игрок на земле. 
    private bool _isJump = false; // Изначально мы говорим, что наш игрок не в прыжке.
    private bool _isFacingRight = true; // Переменная для последующего изменения flipa персонажа.
    private bool _isLevelArm = false;// Переменная для проверки является ли это рычагом или нет.


    const float speedXMultiplyer = 150f;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _finish = GameObject.FindGameObjectWithTag("Finish").GetComponent<Finish>();// Передаем в finish объект с тэгом Finish.
        _level_Arm = FindObjectOfType<Level_Arm>();  //Поиск объекта на сцене с типом Level_Arm. При этом поиск происходит по всей иерархии на сцене, а не по определённым объектам.
        _jumpSound = GetComponent<AudioSource>();//Передаём компонент AudioSource в переменную.
    }

    void Update()// Вызывается каждый фрейм.
    {
        _horizontal = Input.GetAxis("Horizontal");//edit->project setting->input  -1 : 1
        animator.SetFloat("speedX", Mathf.Abs(_horizontal));//Вызываем переменную аниматор, SetFloat так как переменная float, далее указываем значение данного параметра horizontal.
        if (Input.GetKey(KeyCode.W) && _isGround) //Как только мы нажали на w isGround становиться false и не позволяет дать силу второй раз до того как опять не будет коллизий.
        {
            _isJump = true;
            _jumpSound.Play();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (_isFinish)
            {
                _finish.FinishLevel();
            }
            if (_isLevelArm)
            {
                _level_Arm.ActivateLeverArm();//Вызывем ф-цию финиш lvl из class Level_Arm
            }
        }
    }
    void FixedUpdate()/*Все манипуляции с velocity(а также с физикой) происходят в FixedUpdate(обновляется не каждый frame, а по истечинию определённого интервала)
                        для просмотра чему равен данный интервал edit->project setting->time->fix time step */
    {
        _rb.velocity = new Vector2(_horizontal * SpeedX * speedXMultiplyer * Time.fixedDeltaTime, _rb.velocity.y);/*Для передачи скорости объекту каждый frame. rb.velocity.y(указываем сохранять нынешнюю скорость по вертикали)
                                                                                                               Также необходимо умножить Time.fixedDeltaTime (это время между вызовами данной функции) 
                                                                                                               т.к. FixedUpdate не константа, а иногда она зависит от мощности устройства.*/
        if (_isJump)
        {
            _rb.AddForce(new Vector2(0f, 500f));
            _isGround = false;
            _isJump = false;
        }
        if (_horizontal > 0f && !_isFacingRight)// Если horizontal > 0 поворачиваем персонажа вправо.
        {
            Flip();
        }
        else if (_horizontal < 0f && _isFacingRight)
        {
            Flip();
        }

        void Flip()// Чтобы не плодить код добавляем отдельный метод для разворота.
        {
            _isFacingRight = !_isFacingRight;
            Vector3 playerScale = playerModelTransform.localScale;// В данной переменной будет храниться Scale transform model. В данной строке нам доступно только чтение.
            playerScale.x *= -1;//Меняем значение с позитивного на негативное.
            playerModelTransform.localScale = playerScale;
        }
    }
    void OnCollisionEnter2D(Collision2D other) //Указываем, что при коллизии двух коллайдеров. 
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            _isGround = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)// Ф-ция служащая для возможности прохождения в Collider.
    {
        Level_Arm level_ArmTemp = other.GetComponent<Level_Arm>();//Проверяем является ли данный Collider Level_Arm если он не null, то идём в Update и можем вызвать ф-цию level_Arm.ActivateLeverArm();
        if (other.CompareTag("Finish"))
        {
            Debug.Log("Worked");
            _isFinish = true;
        }
        if (level_ArmTemp != null)
        {
            _isLevelArm = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        Level_Arm level_ArmTemp = other.GetComponent<Level_Arm>();
        if (other.CompareTag("Finish") && _isFinish)//Если мы ушли от Collider с тэгом Finish.
        {
            Debug.Log("Not Worked");
            _isFinish = false;
        }
        if (level_ArmTemp != null)
        {
            _isLevelArm = false;
        }
    }

}
