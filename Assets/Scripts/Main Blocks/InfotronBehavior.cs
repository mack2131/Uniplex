using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfotronBehavior : MonoBehaviour
{
    //хранилище объектов инфотронов
    public GameObject infoStorage;
    //скорость падения
    public float speed = 1.5f;
    //скорость движения влево.вправо
    public float speedLR = 3f;
    //список всех инфотронов, обернутых в свой класс
    public List<InfotronInfo> allInfo = new List<InfotronInfo>();
    //списки объектов кандидатов на движение влево-вправо и вниз
    public List<InfotronInfo> infoToFall = new List<InfotronInfo>();
    public List<InfotronInfo> infoToRight = new List<InfotronInfo>();
    public List<InfotronInfo> infoToLeft = new List<InfotronInfo>();

    // Start is called before the first frame update
    void Start()
    {
        //заполняем лист инфотронов
        FillList();
    }

    public void InfotronUpdate()
    {
        //смотрим, кушается ли блок
        YouEatThis();

        //если есть кандидаты на проверку области движения
        if (allInfo.Count != 0)
            CheckArea();//проверяем, куда можно двигаться

        //если есть кандидаты на движение влево
        if (infoToLeft.Count != 0)
            MoveLeft();//двигаемся

        //если есть кандидаты на движение вправо
        if (infoToRight.Count != 0)
            MoveRight();//двигаемся

        //если есть кандидаты на падение
        if (infoToFall.Count != 0)
            Falling();//падаем
    }

    //ф-ия заполнения листа всех инфотронов в обертке класса
    void FillList()
    {
        //по всем инфоторонам
        //загоняем в список
        for (int i = 0; i < infoStorage.transform.childCount; i++)
            allInfo.Add(new InfotronInfo(infoStorage.transform.GetChild(i).gameObject, infoStorage.transform.GetChild(i).transform.position.x, infoStorage.transform.GetChild(i).transform.position.y));
    }

    //ф-ия проверки, куда может двигаться инфотрон
    void CheckArea()
    {
        //если список пус - прерываемся
        if (allInfo.Count == 0)
            return;

        //идем по всему списку
        for (int i = /*prevIndex*/0; i < allInfo.Count; i++)
        {
            if (CheckDeadElements(allInfo[i].info, allInfo, i))
                break;
            //иначе если под блоком Мёрфи, и мы в падении (allInfo[i].dangerous)
            if (allInfo[i].dangerous && GetComponent<LevelController>().level[(int)allInfo[i].info.transform.position.x, (int)allInfo[i].info.transform.position.y - 1] == -99)
            {
                //вызрываемся
                GetComponent<LevelController>().Explosion(allInfo[i].info.transform.position.x, allInfo[i].info.transform.position.y - 1, false);
            }
            else if(GetComponent<LevelController>().level[(int)allInfo[i].info.transform.position.x, (int)allInfo[i].info.transform.position.y - 1] == 11)
            {
                GetComponent<LevelController>().spwnInfo = true;
                allInfo[i].info.GetComponent<Explosable>().canBeExploded = true;
                GetComponent<LevelController>().Explosion(allInfo[i].info.transform.position.x, allInfo[i].info.transform.position.y - 1, false);
            }
            else if (GetComponent<LevelController>().level[(int)allInfo[i].info.transform.position.x, (int)allInfo[i].info.transform.position.y - 1] == 10)
            {
                allInfo[i].info.GetComponent<Explosable>().canBeExploded = true;
                GetComponent<LevelController>().Explosion(allInfo[i].info.transform.position.x, allInfo[i].info.transform.position.y - 1, false);
            }
            //снизу пусто
            else if (GetComponent<LevelController>().level[(int)allInfo[i].info.transform.position.x, (int)allInfo[i].info.transform.position.y - 1] == -1)
            {
                //говорим, что собираемся двигаться и ставим в ячейку, куда собрались двигаться значение 33
                GetComponent<LevelController>().level[(int)allInfo[i].info.transform.position.x, (int)allInfo[i].info.transform.position.y - 1] = /*33*/9;
                //задаем координату, куда надо двигаться
                allInfo[i].newY = allInfo[i].info.transform.position.y - 1;
                //добавляем в список кандидатов на падение
                infoToFall.Add(allInfo[i]);
                //исключаем из списка проверки области
                allInfo.RemoveAt(i);
            }
            /* если под зорком инфотрон или зорк или рам */
            else if (GetComponent<LevelController>().level[(int)allInfo[i].info.transform.position.x, (int)allInfo[i].info.transform.position.y - 1] == 3 ||
                    GetComponent<LevelController>().level[(int)allInfo[i].info.transform.position.x, (int)allInfo[i].info.transform.position.y - 1] == 9 ||
                    GetComponent<LevelController>().level[(int)allInfo[i].info.transform.position.x, (int)allInfo[i].info.transform.position.y - 1] == 4)
            {
                //Debug.Log("rolling vlock under");
                //и над зорком нету блоков инфотрона или зорка
                if (GetComponent<LevelController>().level[(int)allInfo[i].info.transform.position.x, (int)allInfo[i].info.transform.position.y + 1] != 3 ||
                    GetComponent<LevelController>().level[(int)allInfo[i].info.transform.position.x, (int)allInfo[i].info.transform.position.y + 1] != 9)

                {
                    //если свободно под кандидатом и слево на уровень ниже
                    if (GetComponent<LevelController>().level[(int)allInfo[i].info.transform.position.x - 1, (int)allInfo[i].info.transform.position.y] == -1 &&
                        GetComponent<LevelController>().level[(int)allInfo[i].info.transform.position.x - 1, (int)allInfo[i].info.transform.position.y - 1] == -1)
                    {
                        //забиваем ячейку слева
                        GetComponent<LevelController>().level[(int)allInfo[i].info.transform.position.x - 1, (int)allInfo[i].info.transform.position.y] = /*33*/9;
                        //задаем координаты для движение
                        allInfo[i].newX = allInfo[i].info.transform.position.x - 1;
                        //записываем в список движателей влево
                        infoToLeft.Add(allInfo[i]);
                        //удаляем из свободного плавания
                        allInfo.RemoveAt(i);
                    }
                    //если свободно справа и справа на уровень ниже
                    else if (GetComponent<LevelController>().level[(int)allInfo[i].info.transform.position.x + 1, (int)allInfo[i].info.transform.position.y] == -1 &&
                        GetComponent<LevelController>().level[(int)allInfo[i].info.transform.position.x + 1, (int)allInfo[i].info.transform.position.y - 1] == -1)
                    {
                        //забиваем ячейку справа
                        GetComponent<LevelController>().level[(int)allInfo[i].info.transform.position.x + 1, (int)allInfo[i].info.transform.position.y] = /*33*/9;
                        //задаем координаты для движение
                        allInfo[i].newX = allInfo[i].info.transform.position.x + 1;
                        //записываем в список движателей влево
                        infoToRight.Add(allInfo[i]);
                        //удаляем из свободного плавания
                        allInfo.RemoveAt(i);
                    }
                }
            }
            else if (allInfo[i].dangerous)//если были опасны (в падении)
                allInfo[i].dangerous = false;//теперь не в падении - остановились и некуда падать или двигаться
        }
    }

    void Falling()
    {
        for (int i = 0; i < infoToFall.Count; i++)
        {
            if (CheckDeadElements(infoToFall[i].info, infoToFall, i))
                break;
            if (infoToFall[i].info.transform.position.y > infoToFall[i].newY)
            {
                infoToFall[i].info.transform.Translate(/*-Vector3.up*/-Vector3.right * Time.deltaTime * speed);
                infoToFall[i].dangerous = true;
            }
            else
            {
                infoToFall[i].info.transform.position = new Vector3(infoToFall[i].info.transform.position.x, infoToFall[i].newY, 0);
                GetComponent<LevelController>().level[(int)infoToFall[i].info.transform.position.x, (int)infoToFall[i].info.transform.position.y] = 9;
                GetComponent<LevelController>().level[(int)infoToFall[i].info.transform.position.x, (int)infoToFall[i].info.transform.position.y + 1] = -1;
                allInfo.Add(infoToFall[i]);
                infoToFall.RemoveAt(i);
            }
        }
    }

    void MoveRight()
    {
        for (int i = 0; i < infoToRight.Count; i++)
        {
            if (CheckDeadElements(infoToRight[i].info, infoToRight, i))
                break;
            if (infoToRight[i].info.transform.position.x < infoToRight[i].newX)
            {
                infoToRight[i].info.transform.Translate(/*Vector3.right*/Vector3.up * Time.deltaTime * speedLR);
                infoToRight[i].info.transform.GetChild(0).transform.Rotate(Vector3.forward, Time.deltaTime * 360 * 3, Space.Self);
            }
            else
            {
                infoToRight[i].info.transform.position = new Vector3(infoToRight[i].newX, infoToRight[i].info.transform.position.y, 0);
                GetComponent<LevelController>().level[(int)infoToRight[i].info.transform.position.x, (int)infoToRight[i].info.transform.position.y] = 9;
                infoToRight[i].info.transform.GetChild(0).transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                GetComponent<LevelController>().level[(int)infoToRight[i].info.transform.position.x - 1, (int)infoToRight[i].info.transform.position.y] = -1;
                allInfo.Add(infoToRight[i]);
                infoToRight.RemoveAt(i);
            }
        }
    }

    void MoveLeft()
    {
        for (int i = 0; i < infoToLeft.Count; i++)
        {
            if (CheckDeadElements(infoToLeft[i].info, infoToLeft, i))
                break;
            if (infoToLeft[i].info.transform.position.x > infoToLeft[i].newX)
            {
                infoToLeft[i].info.transform.Translate(/*-Vector3.right*/-Vector3.up * Time.deltaTime * speedLR);
                infoToLeft[i].info.transform.GetChild(0).transform.Rotate(Vector3.forward, Time.deltaTime * -360 * 3, Space.Self);
            }
            else
            {
                infoToLeft[i].info.transform.position = new Vector3((int)infoToLeft[i].newX, (int)infoToLeft[i].info.transform.position.y, 0);
                infoToLeft[i].info.transform.GetChild(0).transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                GetComponent<LevelController>().level[(int)infoToLeft[i].info.transform.position.x, (int)infoToLeft[i].info.transform.position.y] = 9;
                GetComponent<LevelController>().level[(int)infoToLeft[i].info.transform.position.x + 1, (int)infoToLeft[i].info.transform.position.y] = -1;
                allInfo.Add(infoToLeft[i]);
                infoToLeft.RemoveAt(i);
            }
        }
    }


    public void YouEatThis()
    {
        for(int i = 0; i < allInfo.Count; i++)
        {
            if (allInfo[i].info.GetComponent<Infotron>().canBeDestroy)
            {
                allInfo.RemoveAt(i);
                GetComponent<LevelManager>().infotrons--;
            }
        }
    }

    /*
     * Ф-ия проверки, надо ли ломать объект element
     * и удалять его из списка list
     * по индексу index
    */
    bool CheckDeadElements(GameObject element, List<InfotronInfo> list, int index)
    {
        //если на елементе весит компонент Explosable и включен canBeExploded
        if (element.GetComponent<Explosable>().canBeExploded)
        {
            //ломаем объект
            Destroy(element.gameObject);
            //убираем объект из листа
            list.RemoveAt(index);
            //возвращаем парвду
            return true;
        }
        //иначе возвращаем фальш
        else return false;
    }
}

[System.Serializable]
public class InfotronInfo
{
    public GameObject info { set; get; }
    public float newX { set; get; }
    public float newY { set; get; }
    public bool dangerous { set; get; }
    //public bool moving { set; get; }

    public InfotronInfo(GameObject info, float x, float y)
    {
        this.info = info;
    }
}
