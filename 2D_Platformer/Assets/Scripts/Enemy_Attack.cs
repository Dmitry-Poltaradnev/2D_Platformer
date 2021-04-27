using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Attack : MonoBehaviour
{
    [SerializeField] private float damage = 20f;
    [SerializeField] private float timeToDamage = 1f;//Переменная для введения таймера на срабатывание коллизий.

    private float _damageTime;
    private bool _isDamage = true;

    private void Start()
    {
        _damageTime = timeToDamage;
    }

    private void Update()
    {
        if (!_isDamage)//Если мы можем наносить урон, то запускаем таймер.
        {
            _damageTime -= Time.deltaTime;
            if (_damageTime <= 0f)//Если таймер = 0, то позволяем нанести урон повторно, и запускаем таймер опять.
            {
                _isDamage = true;
                _damageTime = timeToDamage;
            }
        }
    }
    private void OnCollisionStay2D(Collision2D other)//При коллизии коллайдеров.
    {
        Player_Health player_Health = other.gameObject.GetComponent<Player_Health>();

        if (player_Health != null && _isDamage)
        {
            player_Health.ReduceHealth(damage);
            _isDamage = false;
        }
    }    
}
