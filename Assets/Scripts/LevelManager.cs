/**********************************************/
/* Скрипт, отвечающий за менджер уровня
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

public class LevelManager : MonoBehaviour {

    /* переменные для отслеживания состояния уровня */
    public int[,] levelState;//массив состояния уровня
    public int levelNumber;//номер уровня
    public string levelName;//название уровня
    public int infotrons;//текущее количество инфотронов
    private int infotronsNeeded;//сколько всего нужно собрать инфотронов
    public GameObject murphy;//объект Мёрфи

    /* переменные для вывода информации в интерфейс */
    public Text infotronCounterText;//объект текст для вывода количества оставшихся инфотронов
    public Text levelLabelName;//объект текст для вывода назавания уровня
    public Text maxInfotrons;//объект текст для вывода номера уровня (название неправльное)
    public Text secondsText;//объект текст для вывода количества секунд
    public Text minutesText;//объект текст для вывода количества минут
    public Text hoursText;//объект текст для вывода количества часов
    public Text bombCounterText;//объект текст для вывода количества бомб
    public GameObject gameOverPanel;//панель, которая появляется при смерте игрока
    public Text statisticsText;////объект текст для вывода текста после смерти игрока

    /* переменные для сохранения времени игры и звука */
    private float sec;//текущие секунды
    private int minut;//текущие минуты
    private int hr;//текущие часы
    private int gsec;//глобальные секунды
    private int gminut;//глобальные минуты
    private int ghr;//глобальные часы
    private float delay;//задержка, которая будет после смерти игрока до появления экрана о смерти
    private bool canWaitForPressingAnyKey;//символизирует, что можно начинать ожидать от игрока нажатия любой кнопки для продолжения

    public bool gameOver;

	// Use this for initialization
	void Start ()
    {
        /* WEBGL
         * 
         * LoadGame();//загружаем игру
         * 
         */
        LevelSettings();//настраиваем уровень
	}

    // Update is called once per frame
    void Update()
    {
        if (IsPlayerDead())//если игрок умер
            GameOver();//вызываем функцию конца игры
        else//если жив
        {
            levelLabelName.text = "--" + levelName + "--";//выводим имя уровня
            infotronCounterText.text = infotrons.ToString();//выводим количество оставшихся инфотронов
            maxInfotrons.text = levelNumber.ToString();//выводим номер уровня
            bombCounterText.text = murphy.GetComponent<Murphy>().ammo.ToString();//выводим колчистево бомб
            ClockCount();//запускаем функцию подсчета времени
        }
    }

    void LevelSettings()//ф-ия настройки времени
    {
        GameObject information = GameObject.FindObjectOfType<MainInformation>().gameObject;//ищем объект с главной иноформацией из главного меню
        levelName = information.GetComponent<MainInformation>().levelName;//берем имя
        Debug.Log(levelName);
        levelNumber = information.GetComponent<MainInformation>().levelNumber + 1;//берем номер уровня
        infotrons = infotronsNeeded = information.GetComponent<MainInformation>().infotron;//берем количество инфортронов для прохождения
        //GameObject.FindObjectOfType<LevelBuilder>().LevelBuid();
        //Destroy(information.gameObject);//уничтожаем объект с информацией
    }

    void ClockCount()// функция подсчета времени
    {
        sec += Time.deltaTime;//считаем секунды
        if (sec >= 60)//если их больше 60
        {
            sec = 0;//обнуляем
            minut++;//увеличиваем минуты
            if (minut >= 60)//если минут больше 60
            {
                minut = 0;//обнуляем
                hr++;//прибавляем часы
            }
        }

        secondsText.text = ((int)sec).ToString();//выводим инфу о секундах
        minutesText.text = minut.ToString();//выводим инфу о минутах
        hoursText.text = hr.ToString();//выводим инфу о чаасах
    }

    bool IsPlayerDead()//жив ли игрок?
    {
        if (GameObject.FindObjectOfType<Murphy>() == null)//если есть в сцене объект игрока
            return true;//то игрок жив
        else return false;//иначе взорвался
    }

    public void GameOver()//ф-ия конца игры
    {
        gameOver = true;
        delay += Time.deltaTime;//считаем задержку
        if (delay >= 2)//если она больше двух секунд
        {
            gameOverPanel.SetActive(true);//активируем панель с текстом о конце света(игры) 
            statisticsText.text = "You have collected " + (infotronsNeeded - infotrons) + " out of the " + infotronsNeeded;//выводим текст статистики
            canWaitForPressingAnyKey = true;//говорим, что можно ждать нажатия кнопки любой для продолжения
        }
        if (canWaitForPressingAnyKey)//если ждем нажатие
        {
            if (Input.anyKey)//если жем что-то
            {
                PrepareDataToSave();//готовим данные к сохранению
                SaveGame.Save();//сохраняем
                Destroy(GameObject.FindObjectOfType<MainInformation>().gameObject);
                SceneManager.LoadScene("Main Menu");//грузим главное меню
            }
        }
    }

    public void PrepareDataToSave()//готовим данные к сохранению
    {
        /* записываем текущее время */
        SaveData.savedGame.seconds     = (int)sec;
        SaveData.savedGame.minutes     = minut;
        SaveData.savedGame.hours       = hr;

        /* записываем текущий уровень звука */
        SaveData.savedGame.soundVolume = AudioListener.volume;

        /* записываем глобальное время игры */
        SaveData.savedGame.globSeconds = gsec;
        SaveData.savedGame.globMinutes = gminut;
        SaveData.savedGame.globHours   = ghr;
    }

    void LoadGame()//загружаем игру
    {
        SaveGame.Load();//загружаем игру
        /* запоминаем глобальное время */
        gsec = SaveData.savedGame.globSeconds;
        gminut = SaveData.savedGame.globMinutes;
        ghr = SaveData.savedGame.globHours;
        /* запоминаем уровень звука */
        AudioListener.volume = SaveData.savedGame.soundVolume;
    }
}
