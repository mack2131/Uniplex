/**********************************************/
/* Скрипт, отвечающий за возвожность сохранять
 *  прогресс игрока
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
using System.Runtime.Serialization.Formatters.Binary;
using LitJson;

public class SaveGame : MonoBehaviour
{

    public static void Save()//функция сохранения игры
    {
        string json = JsonUtility.ToJson(SaveData.savedGame);//создаем строку json из объекта savedGame класса SaveData
        File.WriteAllText(Application.dataPath + "//StreamingAssets" + "/Player.plex", json);//сохраняем строку в файл
    }

    public static void Load()//фукнция загрузки
    {
        if (File.Exists(Application.dataPath + "//StreamingAssets" + "/Player.plex"))//если файл сохранения существует
        {
            string json = File.ReadAllText(Application.dataPath + "//StreamingAssets" + "/Player.plex");//загружаем строку из файла
            SaveData.savedGame = JsonUtility.FromJson<SaveData>(json);//преобразуем строку в объект savedGame класса SaveData
        }
    }
}

[Serializable]
public class SaveData//класс, который хранит инфу о данных для сохранения
{
    public static SaveData savedGame = new SaveData();//объект сохраненной игры

    /* время игры на уровне */
    public int seconds;
    public int minutes;
    public int hours;

    /* глобальное, общее время игры */
    public int globSeconds;
    public int globMinutes;
    public int globHours;

    /* уровень звука */
    public float soundVolume;
}

