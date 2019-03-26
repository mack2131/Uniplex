/**********************************************/
/* Скрипт, отвечающий за хранение главной инфы
 * для главного меню и настройки уровня 
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

public class MainInformation : MonoBehaviour
{
    public int levelNumber;//номер уровня
    public int infotron;//количество инфортронов
    public string levelName;//имя уровня
    public Dropdown levelList;//список с уровнями
    public Text hoursText;//текст для вывода глобального времени игры
    public Text minutesText;//текст для вывода глобального времени игры
    public Text secondsText;//текст для вывода глобального времени игры
    private int sec;//секунды
    private int min;//минуты
    private int hr;//часы игры
    public Slider volumeSlider;//слайдер для уровня звука

    private List<String> options = new List<string>();//список с название уровня
    private int[] infotrons = new int[111];//массив с количеством инфортронов для каждого уровня

    public Texture2D map;
    private Texture2D[] maps =  new Texture2D[111];

    // Use this for initialization
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Main Menu")//если объект находится в главном меню
        {
            DontDestroyOnLoad(this.gameObject);//говорим, что его нельзя ломать при переходе в другую сцену
            /* WEBGL DISABLING
             * LoadGame();//загружаем игру
             * 
             */
            FillLevelList();//заполняем список с уровнями
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Main Menu")//если объект в главном меню
        {
            AudioListener.volume = volumeSlider.value;//запоминаем уровень звука от слайдера
            PrepareDataToBuildLevel();//готовим данные к сохранению
        }
    }

    public void PrepareDataToBuildLevel()//готовим данные к настройке уровня
    {
        levelNumber = levelList.value;//запоминаем номер уровня
        levelName = levelList.options[levelList.value].text;//запоминаем название уровня
        infotron = infotrons[levelList.value];//запоминаем количество инфортронов
        map = maps[levelList.value];

        /* выводим инфу о времени игры */
        secondsText.text = sec.ToString();
        minutesText.text = min.ToString();
        hoursText.text = hr.ToString();
    }

    void FillLevelList()//заполняем список с уровнями
    {
        for (int i = 0; i < 111; i++)//идем по всем
            options.Add((i + 1) + " " + GetLevelName(i + 1));//добавляем в список с названиями название
        levelList.ClearOptions();//очищаем список
        levelList.AddOptions(options);//добавляем названия уровней 
    }

    void LoadGame()//загружаем игры
    {
        SaveGame.Load();//загружаем
        sec = SaveData.savedGame.seconds + SaveData.savedGame.globSeconds;//секунды общей игры складываются из предыдущих общих секунд и секунд, которые игрок наиграл только что
        if (sec >= 60)//если секунд больше 60
        {
            min++;//прибавляем минуты
            sec -= 60;//пишем разницу
        }
        min = SaveData.savedGame.minutes + SaveData.savedGame.globMinutes;//с минутами аналогично, как и с секундами, см выше
        if (min >= 60)//если больше 60
        {
            hr++;//прибавляем часы
            min -= 60;//записываем разницу
        }
        hr  = SaveData.savedGame.hours + SaveData.savedGame.globHours;//с часами аналогично см выше
        
        volumeSlider.value = SaveData.savedGame.soundVolume;//загружаем уровень звука в слайдер
        AudioListener.volume = SaveData.savedGame.soundVolume;//загружаем уровень звука
    }

    public void PrepareDataToSave()//готовим данные к сохранению
    {
        /* обнуляем время игры на уровне */
        SaveData.savedGame.seconds = 0;
        SaveData.savedGame.minutes = 0;
        SaveData.savedGame.hours = 0;

        /* запоминаем уровень звука */
        SaveData.savedGame.soundVolume = AudioListener.volume;

        /* запоминаем глобальное время */
        SaveData.savedGame.globSeconds = sec;
        SaveData.savedGame.globMinutes = min;
        SaveData.savedGame.globHours = hr;
    }

    string GetLevelName(int number)//берем название уровня по его номеру
    {
        switch (number)
        {
            case 1:
                { 
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 19;//записываем, сколько инфотронов нужно собрать для прохождения
                    return "WARM UP";//возвращаем имя
                }
            case 2:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 0;
                    return "EXIT!";
                }
            case 3:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 59;
                    return "STONE AGE";
                }
            case 4:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 55;
                    return "COLLECTOR";
                }
            case 5:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 51;
                    return "EASY DEAL";
                }
            case 6:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 15;
                    return "DOWN THE FALL";
                }
            case 7:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 43;
                    return "GO THROUGH!";
                }
            case 8:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 140;
                    return "GET THOSE TOO!!";
                }
            case 9:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 64;
                    return "BUG FUNNY";
                }
            case 10:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 170;
                    return "EASY COME, EASY GO!";
                }
            case 11:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 25;
                    return "RUN FOR LIFE!";
                }
            case 12:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 45;
                    return "IGNORE THEM";
                }
            case 13:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 155;
                    return "ANONYMOUS";
                }
            case 14:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 60;
                    return "FALL OUT";
                }
            case 15:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 95;
                    return "ONE THING...";
                }
            case 16:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 45;
                    return "JUST DO IT!";
                }
            case 17:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 9;
                    return "BANG!";
                }
            case 18:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 1;
                    return "QUICKY!";
                }
            case 19:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 64;
                    return "CRAZY HARRY";
                }
            case 20:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 46;
                    return "MINOTAURUS!";
                }
            case 21:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 54;
                    return "GRAVITY";
                }
            case 22:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 53;
                    return "UP AND DOWN";
                }
            case 23:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 69;
                    return "BOMB TIME";
                }
            case 24:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 14;
                    return "200 MOVING OBJECTS!";
                }
            case 25:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 123;
                    return "YAWN!";
                }
            case 26:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 1;
                    return "ONE";
                }
            case 27:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 133;
                    return "ONCE AROUND";
                }
            case 28:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 124;
                    return "BABA`s WORK";
                }
            case 29:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 74;
                    return "TNT";
                }
            case 30:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 99;
                    return "NO PROBLEM!";
                }
            case 31:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 105;
                    return "TIME TO OPEN!";
                }
            case 32:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 58;
                    return "EASY BUSINESS";
                }
            case 33:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 56;
                    return "INSIDE-OUTSIDE-INSIDE";
                }
            case 34:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 90;
                    return "EASY RIDER!";
                }
            case 35:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 9;
                    return "NO MIRACLE!";
                }
            case 36:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 1;
                    return "PUSH!";
                }
            case 37:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 87;
                    return "NEARLY BORING";
                }
            case 38:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 40;
                    return "BOMBASTIC";
                }
            case 39:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 76;
                    return "SIMPLE";
                }
            case 40:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 75;
                    return "MIND THE STONES";
                }
            case 41:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 5;
                    return "TIME-RUNNER";
                }
            case 42:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 11;
                    return "LITTLE PLEASURE";
                }
            case 43:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 18;
                    return "ANTI-CLOCKWISE";
                }
            case 44:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 44;
                    return "TELEPATHY";
                }
            case 45:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 144;
                    return "EASY WORK";
                }
            case 46:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 34;
                    return "CHAMBER MUSIC";
                }
            case 47:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 35;
                    return "LOOKS DIFFICULT";
                }
            case 48:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 103;
                    return "READY-STEADY-GO";
                }
            case 49:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 28;
                    return "FALL-FESTIVAL";
                }
            case 50:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 42;
                    return "DON`T WORRY!!";
                }
            case 51:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 40;
                    return "STATE OF MIND";
                }
            case 52:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 110;
                    return "DO IT YOURSELF";
                }
            case 53:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 15;
                    return "FILL IN";
                }
            case 54:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 4;
                    return "CORNER CONFUSION!";
                }
            case 55:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 75;
                    return "THE ELDRITCH!";
                }
            case 56:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 50;
                    return "KEEP COOL";
                }
            case 57:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 45;
                    return "THOSE WE LIKE!!";
                }
            case 58:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 130;
                    return "ALL TOGETHER!";
                }
            case 59:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 53;
                    return "VICE-";
                }
            case 60:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 70;
                    return "VERSA";
                }
            case 61:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 75;
                    return "PRE-IGNITION!";
                }
            case 62:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 1;
                    return "GUMBO";
                }
            case 63:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 130;
                    return "PHIL`S FIRST!";
                }
            case 64:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 70;
                    return "THERE YOU GO";
                }
            case 65:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 13;
                    return "PATHFINDER";
                }
            case 66:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 50;
                    return "ISLAND HOPPER";
                }
            case 67:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 125;
                    return "GET THEM OUT!";
                }
            case 68:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 12;
                    return "THOUGH GOIND";
                }
            case 69:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 100;
                    return "hURRY UP!";
                }
            case 70:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 110;
                    return "TOUGH BUT POSSIBLE!";
                }
            case 71:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 131;
                    return "HEAD-HUNTER";
                }
            case 72:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 110;
                    return "COMMUNITY WORK!";
                }
            case 73:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 75;
                    return "ONE WAY";
                }
            case 74:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 1;
                    return "INSPIN-OUTSPIN";
                }
            case 75:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 75;
                    return "NICE";
                }
            case 76:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 120;
                    return "ALL THE FAMILY!";
                }
            case 77:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 100;
                    return "LITTLE DETAIL";
                }
            case 78:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 93;
                    return "TRANSPORT IT";
                }
            case 79:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 45;
                    return "DON`T WASTE THEM!";
                }
            case 80:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 55;
                    return "CLASSIC ENTERTAINMENT";
                }
            case 81:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 100;
                    return "DIAGONAL";
                }
            case 82:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 130;
                    return "TIME IF INFO";
                }
            case 83:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 100;
                    return "FUN-TASTIC";
                }
            case 84:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 31;
                    return "ALL THAT FUSS";
                }
            case 85:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 78;
                    return "A PRO`S WORK";
                }
            case 86:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 79;
                    return "CRASH";
                }
            case 87:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 46;
                    return "SEE YOU TOMORROW!";
                }
            case 88:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 40;
                    return "FLIP AND FLOP";
                }
            case 89:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 26;
                    return "TRIPLE FORK";
                }
            case 90:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 50;
                    return "KNOW THAT ?";
                }
            case 91:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 39;
                    return "VOYAGER III";
                }
            case 92:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 70;
                    return "HAM AND CHEESE SANDWICH";
                }
            case 93:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 9;
                    return "TIMING!";
                }
            case 94:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 45;
                    return "EINSTEIN`S FAVOURITE!";
                }
            case 95:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 100;
                    return "GOOD LUCK";
                }
            case 96:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 40;
                    return "CRACKER";
                }
            case 97:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 56;
                    return "GOOD JOB";
                }
            case 98:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 80;
                    return "MIXED SALAD";
                }
            case 99:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 0;
                    return "JOYSTICK-HANDLING";
                }
            case 100:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 152;
                    return "IQ!";
                }
            case 101:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 80;
                    return "ON-OFF";
                }
            case 102:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 170;
                    return "SUB-EFFECTS";
                }
            case 103:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 40;
                    return "KNOW-HOW!";
                }
            case 104:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 170;
                    return "CHAOS";
                }
            case 105:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 68;
                    return "ONE`S MISSING";
                }
            case 106:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 88;
                    return "WATCH OUT";
                }
            case 107:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 78;
                    return "FIND THE WAY";
                }
            case 108:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 90;
                    return "SADISM";
                }
            case 109:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 12;
                    return "UPSTAIRS-DOWNSTAIRS";
                }
            case 110:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 190;
                    return "TROUBLE AHEAD";
                }
            case 111:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 91;
                    return "BRAINMAN!";
                }
            case 112:
                {
                    maps[number - 1] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 91;
                    return "TEST!";
                }
            default:
                {
                    maps[number] = Resources.Load("Levels/map_" + number.ToString() + "ofc") as Texture2D;
                    infotrons[number - 1] = 0;
                    return "LEVEL NUMBER ERROR";
                }
        }
    }
}
