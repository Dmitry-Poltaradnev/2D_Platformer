using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileControls : MonoBehaviour
{
    private Attack_Controller _attack_Controller;
    private Player_Controller _player_Controller;

    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        _attack_Controller = playerObject.GetComponent<Attack_Controller>();
        _player_Controller = playerObject.GetComponent<Player_Controller>();
    }

    public void Attack()
    {
        _attack_Controller.Attack();
    }

    public void Jump()
    {
        _player_Controller.Jump();
    }

    public void Interact()
    {
        _player_Controller.Interact();
    }
}
