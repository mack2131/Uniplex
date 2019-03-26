/*************************************************/
/* Скрипт отвечает за создание уровня по картинке 
 *              
 * Создан Белым Койотом           
/************************************************/

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

public class LevelBuilder : MonoBehaviour {

    public GameObject basePrefab;//префаб блока печатной платы
    //public GameObject bugPrefab;
    public GameObject exitPrefab;//префаб блока выходы
    public GameObject hardwarePrefab;//префаб блока hardware
    public GameObject hardwareRedPrefab;//префаб блока hardware другого типа
    public GameObject hardwareGreenPrefab;//префаб блока hardware другого типа
    public GameObject hardwareBluePrefab;//префаб блока hardware другого типа
    public GameObject hardwareColorPrefab;//префаб блока hardware другого типа
    public GameObject hardwareRedTransPrefab;//префаб блока hardware другого типа
    public GameObject hardwareYTransPrefab;//префаб блока hardware другого типа
    public GameObject hardwareMixedTransPrefab;//префаб блока hardware другого типа
    public GameObject hardwareFlatTransPrefab;//префаб блока hardware другого типа
    public GameObject hardwareBigRedTransPrefab;//префаб блока hardware другого типа
    public GameObject hardwareSTransPrefab;//префаб блока hardware другого типа
    public GameObject infotronPrefab;//префаб блока инфотрона
    public GameObject murphyPrefab;//префаб блока мёрфи
    public GameObject ramChipsPrefab;//префаб блока ram
    public GameObject terminalPrefab;//префаб блока терминал
    public GameObject zonkPrefab;//префаб блока зонка
    public GameObject discRedPrefab;//префаб блока красной дискеты
    public GameObject discYellowPrefab;//префаб блока желтой дискеты
    public GameObject discOrangePrefab;//префаб блока оранжевой дискеты
    public GameObject levelBorder;//префаб блока границы уровня
    public GameObject portRLPrefab;//префаб блока порт влево-вправо
    public GameObject portUDPrefab;//префаб блока порт вверх-вниз
    public GameObject portLPrefab;//префаб блока порт только влево
    public GameObject portRPrefab;//префаб блока порт только вправо
    public GameObject portUPrefab;//префаб блока порт только вверх
    public GameObject portDPrefab;//префаб блока порт только виз
    public GameObject portRLUDPrefab;//префаб блока порт во все направления
    public GameObject snikSnakPrefab;//префаб блока сник снэк
    public GameObject electronPrefab;//префаб блока электрон

    public int[,] levelArray;//префаб блока состояние уровня в массиве

    /* хранилища для объектов уровня: */
    private GameObject levelStorage;//глобальное хранилище для всего лабиринта
    private GameObject wallStorage;//для стен
    private GameObject baseStorage;//для печатной платы
    private GameObject zonkStorage;//для зонков
    private GameObject infotronStorage;//для инфотронов
    private GameObject ramStorage;//для ram
    private GameObject hardwareStorage;//для hardware
    private GameObject exitStorage;//для выходов
    private GameObject discRedStorage;//для красных дискет
    private GameObject discYellowStorage;//для желтых дискет
    private GameObject discOrangeStorage;//для оранжевых дискет
    private GameObject portStorage;//для портов
    private GameObject ssStorage;//для сник снэков
    private GameObject terminalStorage;//для терминалов
    private GameObject electronStorage;//для электронов
    private GameObject bugStorage;//для багов

    private Vector2 exitPos;

    private int width;//ширина уровня
    private int height;//высота блоков
    private GameObject levelManager;//объект менеджера уровня

    public Texture2D LEVELMAP;

    public GameObject levelIsBuidPanel;//для того чтобы обойти загрузку уровня в 20 секунд и более, мы будем строить уровень после загрузке сцены по нажатию любой кнопки, для этого говорим игроку, что уровень загружен, хотя это не так, но так во много много раз быстрее. после нажатия любой кнопки в загруженной сцене уровень собирается меньше чем за секунду
        
	// Use this for initialization
	void Start () 
    {
            levelManager = GameObject.Find("Level Manager");//ищем объект менеджера уровня
            LEVELMAP = GameObject.FindObjectOfType<MainInformation>().gameObject.GetComponent<MainInformation>().map;
            LevelBuid();//функция сторительства уровня
    }

    void LevelBuid()//функция сторительства уровня
    {
        CreateStorages();//функция создания хранилищ для объектов
        ImageToBlocks();//функция, которая переводит пиксельную карту уровня в блоки
        SendLevelState();//передаем необходимые данные в объекты
        //DebugPrintArray();//печать массива уровня для откладки

        CombineMesh(wallStorage);
        //CombineMesh(baseStorage);

        GameObject.FindObjectOfType<ZonkBehaviour>().zorkStorage = zonkStorage;
        GameObject.FindObjectOfType<InfotronBehavior>().infoStorage = infotronStorage;
        GameObject.FindObjectOfType<SnikSnakBehaviour>().snikStorage = ssStorage;
        GameObject.FindObjectOfType<ElectronBehaviour>().snikStorage = electronStorage;
        GetComponent<LevelController>().exitPos.x = exitPos.x;
        GetComponent<LevelController>().exitPos.y = exitPos.y;

        //Destroy(this.gameObject);//ломаем объект с этим скриптом

        Destroy(GetComponent<LevelBuilder>());
    }

    void ImageToBlocks()//функция создания хранилищ для объектов
    {
        //загружаем нужную карту уровня
        //Texture2D levelImage = Resources.Load("Levels/001") as Texture2D;
        //Texture2D levelImage = Resources.Load("Levels/map_" + (GameObject.FindObjectOfType<MainInformation>().GetComponent<MainInformation>().levelNumber + 1).ToString() + "ofc") as Texture2D;
        
        Texture2D levelImage = LEVELMAP as Texture2D;
        width = levelImage.width + 2;//задаем ширины из карты
        height = levelImage.height + 2;//задаем высоту из карты
        levelArray = new int[width, height];//создаем массив уровня
        BuildBorders();
        for (int i = 1; i < width - 1; i++)//по всем пикселям
        {
            for (int j = 1; j < height - 1; j++)
            {
                Color32 pixel = levelImage.GetPixel(i - 1,j - 1);//берем пиксель
                VisualizePixel(pixel, i, j);//превращаем пиксель в объект по координатам i(x), j(y)
            }
        }
    }

    void BuildBorders()
    {
        /* граница снизу */
        for (int i = 0; i < width; i++)
        {
            GameObject wall = Instantiate(levelBorder);
            wall.transform.position = new Vector3(i, 0, 0);
            wall.transform.SetParent(wallStorage.transform, false);
            levelArray[i, 0] = 0;
        }
        /* граница сверху */
        for (int i = 0; i < width; i++)
        {
            GameObject wall = Instantiate(levelBorder);
            wall.transform.position = new Vector3(i, height - 1, 0);
            wall.transform.SetParent(wallStorage.transform, false);
            levelArray[i, height - 1] = 0;
        }
        /* граница слева */
        for (int i = 0; i < height; i++)
        {
            GameObject wall = Instantiate(levelBorder);
            wall.transform.position = new Vector3(0, i, 0);
            wall.transform.SetParent(wallStorage.transform, false);
            levelArray[0, i] = 0;
        }
        /* граница справа */
        for (int i = 0; i < height; i++)
        {
            GameObject wall = Instantiate(levelBorder);
            wall.transform.position = new Vector3(width - 1, i, 0);
            wall.transform.SetParent(wallStorage.transform, false);
            levelArray[width - 1, i] = 0;
        }
    }

    void NewVisualization(Color32 color, int i, int j)
    {

        
    }

    void VisualizePixel(Color32 color, int i, int j)//функция, которая превращает пиксель в блок по координатам
    {
        /* идет сравнение цветов пикселей с цветом, который определяет кокнретный блок
         * если цвет совпал, ставится нужный блок и записывается значение блока в массив уровня*/

        /* Соотношение значений массива уровня с блоками:
         * -99 - игрок
         * -1  - пусто
         *  0  - граница уровня
         *  2  - печатная плата
         *  3  - зорк
         *  4  - блоки рам
         *  5  - блоки hardware
         *  6  - красная дискета
         *  7  - желтая дискета
         *  8  - оранжевая лискета
         *  9  - инфотрон
         *  10 - ножницы
         *  11 - электрон
         *  12 - выход закрыт
         *  13 - выход открыт
         *  20 - взрыв
         */
        
        /* level board */
        if (color.r == 128 &&
            color.g == 128 &&
            color.b == 128)
        {
            GameObject wall = Instantiate(levelBorder);
            wall.transform.position = new Vector3(i, j, 0);
            wall.transform.SetParent(wallStorage.transform, false);
            levelArray[i, j] = 0;
            return;
        }

        /* exit */
        if (color.r == 255 &&
            color.g == 106 &&
            color.b == 0)
        {
            GameObject exitBlock = Instantiate(exitPrefab);
            exitBlock.transform.position = new Vector3(i, j, 0);
            exitBlock.transform.SetParent(exitStorage.transform, false);
            levelArray[i, j] = 12;
            exitPos.x = i;
            exitPos.y = j;
            return;
        }
        /* murphy */
        else if (color.r == 255 &&
            color.g == 0 &&
            color.b == 0)
        {
            GameObject player = Instantiate(murphyPrefab);
            player.transform.position = new Vector3(i, j, 0);
            player.transform.SetParent(levelStorage.transform, false);
            levelArray[i, j] = -99;
            return;
        }
        /* base blocks */
        else if (color.r == 0 &&
            color.g == 255 &&
            color.b == 0)
        {
            GameObject baseBlock = Instantiate(basePrefab);
            baseBlock.transform.position = new Vector3(i, j, 0);
            baseBlock.transform.SetParent(baseStorage.transform, false);
            levelArray[i, j] = 2;
            return;
        }
        /* bug block */
        else if (color.r == 0 &&
                 color.g == 254 &&
                 color.b == 0)
        {
            GameObject baseBlock = Instantiate(basePrefab);
            baseBlock.transform.position = new Vector3(i, j, 0);
            baseBlock.transform.SetParent(baseStorage.transform, false);
            //baseBlock.GetComponent<Base>().isBug = true;
            levelArray[i, j] = 2;
            return;
        }
        /* zonk */
        else if (color.r == 127 &&
            color.g == 0 &&
            color.b == 55)
        {
            GameObject zonkBlock = Instantiate(zonkPrefab);
            zonkBlock.transform.position = new Vector3(i, j, 0);
            zonkBlock.transform.SetParent(zonkStorage.transform, false);
            zonkBlock.name = "Zonk " + i + " " + j;
            levelArray[i, j] = 3;
            return;
        }
        /* ram blocks */
        else if (color.r == 0 &&
            color.g == 0 &&
            color.b == 255)
        {
            GameObject ramBlock = Instantiate(ramChipsPrefab);
            ramBlock.transform.position = new Vector3(i, j, 0);
            ramBlock.transform.SetParent(ramStorage.transform, false);
            levelArray[i, j] = 4;
            return;
        }
        /* hardware всго 11 типов блоков*/
        else if (color.r == 0 &&
            color.g == 0 &&
            color.b == 0)
        {
            GameObject hardBlock = Instantiate(hardwarePrefab);
            hardBlock.transform.position = new Vector3(i, j, 0);
            hardBlock.transform.SetParent(hardwareStorage.transform, false);
            levelArray[i, j] = 5;
            return;
        }
        else if (color.r == 96 &&
                 color.g == 252 &&
                 color.b == 255)
        {
            GameObject hardBlock = Instantiate(hardwareBluePrefab);
            hardBlock.transform.position = new Vector3(i, j, 0);
            hardBlock.transform.SetParent(hardwareStorage.transform, false);
            levelArray[i, j] = 5;
            return;
        }
        else if (color.r == 255 &&
                 color.g == 38 &&
                 color.b == 59)
        {
            GameObject hardBlock = Instantiate(hardwareRedPrefab);
            hardBlock.transform.position = new Vector3(i, j, 0);
            hardBlock.transform.SetParent(hardwareStorage.transform, false);
            levelArray[i, j] = 5;
            return;
        }
        else if (color.r == 173 &&
                 color.g == 255 &&
                 color.b == 189)
        {
            GameObject hardBlock = Instantiate(hardwareGreenPrefab);
            hardBlock.transform.position = new Vector3(i, j, 0);
            hardBlock.transform.SetParent(hardwareStorage.transform, false);
            levelArray[i, j] = 5;
            return;
        }
        else if (color.r == 255 &&
                 color.g == 252 &&
                 color.b == 183)
        {
            GameObject hardBlock = Instantiate(hardwareColorPrefab);
            hardBlock.transform.position = new Vector3(i, j, 0);
            hardBlock.transform.SetParent(hardwareStorage.transform, false);
            levelArray[i, j] = 5;
            return;
        }
        else if (color.r == 178 &&
                 color.g == 55 &&
                 color.b == 86)
        {
            GameObject hardBlock = Instantiate(hardwareRedTransPrefab);
            hardBlock.transform.position = new Vector3(i, j, 0);
            hardBlock.transform.SetParent(hardwareStorage.transform, false);
            levelArray[i, j] = 5;
            return;
        }
        else if (color.r == 124 &&
                 color.g == 124 &&
                 color.b == 26)
        {
            GameObject hardBlock = Instantiate(hardwareYTransPrefab);
            hardBlock.transform.position = new Vector3(i, j, 0);
            hardBlock.transform.SetParent(hardwareStorage.transform, false);
            levelArray[i, j] = 5;
            return;
        }
        else if (color.r == 122 &&
                 color.g == 33 &&
                 color.b == 97)
        {
            GameObject hardBlock = Instantiate(hardwareMixedTransPrefab);
            hardBlock.transform.position = new Vector3(i, j, 0);
            hardBlock.transform.SetParent(hardwareStorage.transform, false);
            levelArray[i, j] = 5;
            return;
        }
        else if (color.r == 77 &&
                 color.g == 85 &&
                 color.b == 122)
        {
            GameObject hardBlock = Instantiate(hardwareFlatTransPrefab);
            hardBlock.transform.position = new Vector3(i, j, 0);
            hardBlock.transform.SetParent(hardwareStorage.transform, false);
            levelArray[i, j] = 5;
            return;
        }
        else if (color.r == 124 &&
                 color.g == 39 &&
                 color.b == 41)
        {
            GameObject hardBlock = Instantiate(hardwareBigRedTransPrefab);
            hardBlock.transform.position = new Vector3(i, j, 0);
            hardBlock.transform.SetParent(hardwareStorage.transform, false);
            levelArray[i, j] = 5;
            return;
        }
        else if (color.r == 158 &&
                 color.g == 117 &&
                 color.b == 255)
        {
            GameObject hardBlock = Instantiate(hardwareSTransPrefab);
            hardBlock.transform.position = new Vector3(i, j, 0);
            hardBlock.transform.SetParent(hardwareStorage.transform, false);
            levelArray[i, j] = 5;
            return;
        }
        /* infotron */
        else if (color.r == 255 &&
                 color.g == 255 &&
                 color.b == 255)
        {
            GameObject infBlock = Instantiate(infotronPrefab);
            infBlock.transform.position = new Vector3(i, j, 0);
            infBlock.transform.SetParent(infotronStorage.transform, false);
            levelArray[i, j] = 9;
            return;
        }
        /* red disc */
        else if (color.r == 255 &&
                 color.g == 107 &&
                 color.b == 104)
        {
            GameObject disk = Instantiate(discRedPrefab);
            disk.transform.position = new Vector3(i, j, 0);
            disk.transform.SetParent(discRedStorage.transform, false);
            levelArray[i, j] = 6;
            return;
        }
        /* yellow disc */
        else if (color.r == 252 &&
                 color.g == 255 &&
                 color.b == 104)
        {
            GameObject disk = Instantiate(discYellowPrefab);
            disk.transform.position = new Vector3(i, j, 0);
            disk.transform.SetParent(discYellowStorage.transform, false);
            levelArray[i, j] = 7;
            return;
        }
        /* orange disc */
        else if (color.r == 255 &&
                 color.g == 178 &&
                 color.b == 112)
        {
            GameObject disk = Instantiate(discOrangePrefab);
            disk.transform.position = new Vector3(i, j, 0);
            disk.transform.SetParent(discOrangeStorage.transform, false);
            levelArray[i, j] = 8;
            return;
        }
        /* RL Port */
        else if (color.r == 255 &&
                 color.g == 56 &&
                 color.b == 53)
        {
            GameObject port = Instantiate(portRLPrefab);
            port.transform.position = new Vector3(i, j, 0);
            port.transform.SetParent(portStorage.transform, false);
            levelArray[i, j] = 2;
            return;
        }
        /* UD Port */
        else if (color.r == 255 &&
                 color.g == 76 &&
                 color.b == 79)
        {
            GameObject port = Instantiate(portUDPrefab);
            port.transform.position = new Vector3(i, j, 0);
            port.transform.SetParent(portStorage.transform, false);
            levelArray[i, j] = 2;
            return;
        }
        /* RLUD Port */
        else if (color.r == 255 &&
                 color.g == 40 &&
                 color.b == 44)
        {
            GameObject port = Instantiate(portRLUDPrefab);
            port.transform.position = new Vector3(i, j, 0);
            port.transform.SetParent(portStorage.transform, false);
            levelArray[i, j] = 2;
            return;
        }
        /* D Port */
        else if (color.r == 255 &&
                 color.g == 150 &&
                 color.b == 160)
        {
            GameObject port = Instantiate(portDPrefab);
            port.transform.position = new Vector3(i, j, 0);
            port.transform.SetParent(portStorage.transform, false);
            levelArray[i, j] = 2;
            return;
        }
        /* U Port */
        else if (color.r == 255 &&
                 color.g == 109 &&
                 color.b == 109)
        {
            GameObject port = Instantiate(portUPrefab);
            port.transform.position = new Vector3(i, j, 0);
            port.transform.SetParent(portStorage.transform, false);
            levelArray[i, j] = 2;
            return;
        }
        /* R Port */
        else if (color.r == 255 &&
                 color.g == 221 &&
                 color.b == 235)
        {
            GameObject port = Instantiate(portRPrefab);
            port.transform.position = new Vector3(i, j, 0);
            port.transform.SetParent(portStorage.transform, false);
            levelArray[i, j] = 2;
            return;
        }
        /* L Port */
        else if (color.r == 255 &&
                 color.g == 178 &&
                 color.b == 191)
        {
            GameObject port = Instantiate(portLPrefab);
            port.transform.position = new Vector3(i, j, 0);
            port.transform.SetParent(portStorage.transform, false);
            levelArray[i, j] = 2;
            return;
        }
        /* Snik Snak */
        else if (color.r == 63 &&
                 color.g == 15 &&
                 color.b == 7)
        {
            GameObject ss = Instantiate(snikSnakPrefab);
            ss.transform.position = new Vector3(i, j, 0);
            ss.transform.SetParent(ssStorage.transform, false);
            levelArray[i, j] = 10;
            return;
        }
        /* Terminal */
        else if (color.r == 39 &&
                 color.g == 82 &&
                 color.b == 142)
        {
            GameObject terminal = Instantiate(terminalPrefab);
            terminal.transform.position = new Vector3(i, j, 0);
            terminal.transform.SetParent(terminalStorage.transform, false);
            levelArray[i, j] = 2;
            return;
        }
        /* Electron */
        else if (color.r == 20 &&
                 color.g == 50 &&
                 color.b == 50)
        {
            GameObject el = Instantiate(electronPrefab);
            el.transform.position = new Vector3(i, j, 0);
            el.transform.SetParent(electronStorage.transform, false);
            levelArray[i, j] = 10;
            return;
        }
        else if (color.r == 80 &&
                color.g == 123 &&
                color.b == 127)
            levelArray[i, j] = -1;
    }

    void CreateStorages()//функция, которая отвечает за создание объектов-хранилищ, к которым в дети запишутся блоки
    {
        levelStorage = new GameObject("Level Storage");
        wallStorage = new GameObject("Wall Storage");
        wallStorage.transform.SetParent(levelStorage.transform, false);

        baseStorage = new GameObject("Base Storage");
        baseStorage.transform.SetParent(levelStorage.transform, false);

        bugStorage = new GameObject("Bug Storage");
        bugStorage.transform.SetParent(levelStorage.transform, false);

        zonkStorage = new GameObject("Zonk Storage");
        zonkStorage.transform.SetParent(levelStorage.transform, false);

        infotronStorage = new GameObject("Infotron Storage");
        infotronStorage.transform.SetParent(levelStorage.transform, false);

        ramStorage = new GameObject("Ram Storage");
        ramStorage.transform.SetParent(levelStorage.transform, false);

        hardwareStorage = new GameObject("Hardware Storage");
        hardwareStorage.transform.SetParent(levelStorage.transform, false);

        infotronStorage = new GameObject("Infotron Storage");
        infotronStorage.transform.SetParent(levelStorage.transform, false);

        exitStorage = new GameObject("Exit Storage");
        exitStorage.transform.SetParent(levelStorage.transform, false);

        discRedStorage = new GameObject("Disc Red Storage");
        discRedStorage.transform.SetParent(levelStorage.transform, false);

        discYellowStorage = new GameObject("Disc Yellow Storage");
        discYellowStorage.transform.SetParent(levelStorage.transform, false);

        discOrangeStorage = new GameObject("Disc Orange Storage");
        discOrangeStorage.transform.SetParent(levelStorage.transform, false);

        portStorage = new GameObject("Port Storage");
        portStorage.transform.SetParent(levelStorage.transform, false);

        ssStorage = new GameObject("Snik Snak Storage");
        ssStorage.transform.SetParent(levelStorage.transform, false);

        terminalStorage = new GameObject("Terminal Storage");
        terminalStorage.transform.SetParent(levelStorage.transform, false);

        electronStorage = new GameObject("Electron Storage");
        electronStorage.transform.SetParent(levelStorage.transform, false);
    }

    void SendLevelState()//фнукция, которая передает массив уровня объектам, которым это нужно
    {
        levelManager.GetComponent<LevelManager>().levelState = levelArray;
        for (int i = 0; i < zonkStorage.transform.childCount; i++)//передаем массив уровня зонкам
            zonkStorage.transform.GetChild(i).GetComponent<Zonk>().levelManager = levelManager;
        //for (int i = 0; i < baseStorage.transform.childCount; i++)//передаем массив уровня печатной плате
          //  baseStorage.transform.GetChild(i).GetComponent<Base>().levelManager = levelManager;
        //for (int i = 0; i < infotronStorage.transform.childCount; i++)//передаем массив уровня инфотронам
            //infotronStorage.transform.GetChild(i).GetComponent<Infotron>().levelManager = levelManager;
        for (int i = 0; i < exitStorage.transform.childCount; i++)//передаем массив уровня выходам
            exitStorage.transform.GetChild(i).GetComponent<Exit>().levelManager = levelManager;
        for (int i = 0; i < discRedStorage.transform.childCount; i++)//передаем массив уровня красным дискетам
            discRedStorage.transform.GetChild(i).GetComponent<RedDisc>().levelManager = levelManager;
        for (int i = 0; i < discOrangeStorage.transform.childCount; i++)//передаем массив уровня красным дискетам
            discOrangeStorage.transform.GetChild(i).GetComponent<OrangeDisc>().levelManager = levelManager;
        for (int i = 0; i < terminalStorage.transform.childCount; i++)//передаем хранилище желтых дискет в терминал
            terminalStorage.transform.GetChild(i).GetComponent<Terminal>().yellowDiscStorage = discYellowStorage;
        //for (int i = 0; i < ssStorage.transform.childCount; i++)//передаем массив уровня сник-снекам
          //  ssStorage.transform.GetChild(i).GetComponent<SnikSnak>().levelManager = levelManager;
        for (int i = 0; i < electronStorage.transform.childCount; i++)//передаем массив уровня сник-снекам
            electronStorage.transform.GetChild(i).GetComponent<Electron>().levelManager = levelManager;
        levelManager.GetComponent<LevelManager>().murphy = GameObject.Find("Murphy New(Clone)");
        //GameObject.Find("Murphy New(Clone)").GetComponent<Murphy>().levelManager = levelManager;//передаем массив уровня самому Мёрфи
    }

    void DebugPrintArray()//печать состояния массива, используется для отладки
    {
        string debugOutPut = "";
        for (int i = 0; i < width; i++)//по всем элементам массива по строкам
        {
            for (int j = 0; j < height; j++)//по всем элементам массива по столбцам
            {
                debugOutPut += levelArray[i, j];//записываем ячейки массива в строку
                if(j == height-1)
                    debugOutPut += "\n";//разделяем каждую строку переходом на новую
            }
        }
        Debug.Log(debugOutPut);//выводим в печать
    }

    void CombineMesh(GameObject mainMesh)
    {
        mainMesh.AddComponent<MeshRenderer>();
        mainMesh.GetComponent<MeshRenderer>().sharedMaterial = mainMesh.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material;

        MeshFilter[] meshFilters = mainMesh.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].mesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);
            i++;
        }

        mainMesh.AddComponent<MeshFilter>();

        mainMesh.transform.GetComponent<MeshFilter>().mesh = new Mesh();
        mainMesh.transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
        mainMesh.gameObject.SetActive(true);
        mainMesh.transform.GetComponent<MeshFilter>().sharedMesh.Optimize();

        for (i = 0; i < mainMesh.transform.childCount; i++)
            Destroy(mainMesh.transform.GetChild(i).gameObject);

        mainMesh.gameObject.isStatic = true;
    }
}
