/**********************************************/
/* Скрипт, отвечающий за блок выхода
 *  
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

public class Exit : MonoBehaviour {

    public GameObject levelManager;//менеджер уровня

    private bool canExit;//активирован выход

    public AudioSource sounds;//звуки
    private bool startExit;//зашли ли мы на активированный выход

	// Use this for initialization
	void Start () 
    {
        canExit = false;//выйти нельзя
        sounds = GetComponent<AudioSource>();//собираем звуки с объекта
	}
	
	// Update is called once per frame
	void Update () 
    {
        FinishLevel();//фукция, которая определяет, мжно ли выйти с уровня
	}

    void FinishLevel()//фукция, которая определяет, мжно ли выйти с уровня
    {
        if (levelManager.GetComponent<LevelManager>().infotrons == 0)//если собрано необходимое количество инфотронов
        {
            levelManager.GetComponent<LevelManager>().infotronCounterText.text = "Exit is allow";//пишем,ч то можно выйти
            gameObject.tag = "Exit";//меняем тэг объекта на "Exit"
            canExit = true;//активируем выход
        }
        if (startExit)//если мы зашли на активированный выход
        {
            if (!sounds.isPlaying)//пока не закончит играть музыка завершения уровня
            {
                GameObject.FindObjectOfType<LevelManager>().gameObject.GetComponent<LevelManager>().PrepareDataToSave();//вызываем функцию подготовки данных для сохранения
                SaveGame.Save();//сохраняем игру
                SceneManager.LoadScene("Main Menu");//как музыка проиграла, выходим в главное меню
            }
        }
    }

    void OnCollisionEnter(Collision col)//если с кем-то столкнулись
    {
        if (canExit)//если активирован выход
        {
            if (col.gameObject.GetComponent<Murphy>() != null)//если объект столкновения - мёрфи
            {
                col.gameObject.GetComponent<SphereCollider>().enabled = false;//отключаем коллайдер мёрфи
                startExit = true;//говорим, что мёрфи зашел на выход
                sounds.Play();//проигрываем музыку завершения уровня
            }
        }
    }
}
