using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    [SerializeField] private float SpeedX = -1f;//Публичная переменная для последующего изменения скорости.
                                                //При использовании [SerializeField] и private переменной - это значит, что другие скрипты не будут иметь доступа к данной переменной, кроме вынесенного поля в редакторе.

    [SerializeField] private Animator animator;// Получаем доступ к переменной аниматора.
    private Rigidbody2D rb;
    private Finish finish;
    private Level_Arm level_Arm;
    private bool isFinish = false;

    private float horizontal = 0f;
    private bool isGround = false; // Данная переменная говорит нам находится ли игрок на земле. 
    private bool isJump = false; // Изначально мы говорим, что наш игрок не в прыжке.
    private bool isFacingRight = true; // Переменная для последующего изменения flipa персонажа.
    private bool isLevelArm = false;// Переменная для проверки является ли это рычагом или нет.


    const float speedXMultiplyer = 150f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        finish = GameObject.FindGameObjectWithTag("Finish").GetComponent<Finish>();// Передаем в finish объект с тэгом Finish.
        level_Arm = FindObjectOfType<Level_Arm>();  //Поиск объекта на сцене с типом Level_Arm. При этом поиск происходит по всей иерархии на сцене, а не по определённым объектам.
    }

    void Update()// Вызывается каждый фрейм.
    {
        horizontal = Input.GetAxis("Horizontal");//edit->project setting->input  -1 : 1
        animator.SetFloat("speedX", Mathf.Abs(horizontal));//Вызываем переменную аниматор, SetFloat так как переменная float, далее указываем значение данного параметра horizontal.
        if (Input.GetKey(KeyCode.W) && isGround) //Как только мы нажали на w isGround становиться false и не позволяет дать силу второй раз до того как опять не будет коллизий.
        {
            isJump = true;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (isFinish)
            {
                finish.FinishLevel();
            }
            if (isLevelArm)
            {
                level_Arm.ActivateLeverArm();//Вызывем ф-цию финиш lvl из class Level_Arm
            }
        }
    }
    void FixedUpdate()/*Все манипуляции с velocity(а также с физикой) происходят в FixedUpdate(обновляется не каждый frame, а по истечинию определённого интервала)
                        для просмотра чему равен данный интервал edit->project setting->time->fix time step */
    {
        rb.velocity = new Vector2(horizontal * SpeedX * speedXMultiplyer * Time.fixedDeltaTime, rb.velocity.y);/*Для передачи скорости объекту каждый frame. rb.velocity.y(указываем сохранять нынешнюю скорость по вертикали)
                                                                                                               Также необходимо умножить Time.fixedDeltaTime (это время между вызовами данной функции) 
                                                                                                               т.к. FixedUpdate не константа, а иногда она зависит от мощности устройства.*/
        if (isJump)
        {
            rb.AddForce(new Vector2(0f, 500f));
            isGround = false;
            isJump = false;
        }
        if (horizontal > 0f && !isFacingRight)// Если horizontal > 0 поворачиваем персонажа вправо.
        {
            Flip();
        }
        else if (horizontal < 0f && isFacingRight)
        {
            Flip();
        }

        void Flip()// Чтобы не плодить код добавляем отдельный метод для разворота.
        {
            isFacingRight = !isFacingRight;
            Vector3 playerScale = transform.localScale;// В данной переменной будет храниться Scale transform player. В данной строке нам доступно только чтение.
            playerScale.x *= -1;//Меняем значение с позитивного на негативное.
            transform.localScale = playerScale;
        }
    }
    void OnCollisionEnter2D(Collision2D other) //Указываем, что при коллизии двух коллайдеров. 
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGround = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)// Ф-ция служащая для возможности прохождения в Collider.
    {
        Level_Arm level_ArmTemp = other.GetComponent<Level_Arm>();//Проверяем является ли данный Collider Level_Arm если он не null, то идём в Update и можем вызвать ф-цию level_Arm.ActivateLeverArm();
        if (other.CompareTag("Finish"))
        {
            Debug.Log("Worked");
            isFinish = true;
        }
        if (level_ArmTemp != null)
        {
            isLevelArm = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        Level_Arm level_ArmTemp = other.GetComponent<Level_Arm>();
        if (other.CompareTag("Finish") && isFinish)//Если мы ушли от Collider с тэгом Finish.
        {
            Debug.Log("Not Worked");
            isFinish = false;
        }
        if (level_ArmTemp != null)
        {
            isLevelArm = false;
        }
    }

}
