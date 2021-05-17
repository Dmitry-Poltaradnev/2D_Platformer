using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public void RestartHandler()
    {
        Scene scene = SceneManager.GetActiveScene();//SceneManager с помощью метода GetActiveScene возвращает инф-цию об активной сцене.
        SceneManager.LoadScene(scene.name);//Загружаем последнюю сцену по имени, не по индексу.
        Time.timeScale = 1f;
    }

    public void ExitHandler()//Метод позволяющий загрузить Main menu.
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }
}
