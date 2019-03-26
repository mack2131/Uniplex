/**********************************************/
/* Скрипт, отвечающий за кнопку играть главного
 * меню 
 * Создан Белым Койотом
/**********************************************/

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

public class PlayButton : MonoBehaviour {
	
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(Play);//вызываем обработчик кнопки играть
    }

    void Play()//функция запуска игры при нажатии кнопки играт
    {
        //ищем объект с главной информацие в главном меню и вызываем функцию подготовки данных к сохранению
        GameObject.FindObjectOfType<MainInformation>().gameObject.GetComponent<MainInformation>().PrepareDataToSave();
        //сохраняем игру
        /* WEBGL
         * SaveGame.Save();
         * 
         */
        //загружаем сцену с уровнем
        SceneManager.LoadScene("Level 0");
    }
}
