//TODO ++ ИНФОТРОНЫ
//придумать, как сделать так, чтобы при уничтожении 
//не бегать по всем зонкам, а 
//создать еще один лист, в который складыать на уничтожение

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZonkBehaviour : MonoBehaviour
{
    public GameObject zorkStorage;
    public float speed = 1.5f;
    public float speedLR = 3f;
    public List<ZorksInfo> allZorks = new List<ZorksInfo>();
    public List<ZorksInfo> zorksToFall = new List<ZorksInfo>();
    public List<ZorksInfo> zorksToRight = new List<ZorksInfo>();
    public List<ZorksInfo> zorksToLeft = new List<ZorksInfo>();
    public List<ZorksInfo> zonksPushLeft = new List<ZorksInfo>();
    public List<ZorksInfo> zonksPushRight = new List<ZorksInfo>();

    // Start is called before the first frame update
    void Start()
    {
        FillZorksList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ZorksUpdate()
    {
        ExplosionDestroy();

        if (allZorks.Count != 0)
            CheckArea();

        if (zorksToRight.Count != 0)
            MoveRight();

        if (zorksToLeft.Count != 0)
            MoveLeft();

        if (zorksToFall.Count != 0)
            Falling();
    }
    
    public void CheckArea()
    {
        if (allZorks.Count == 0)
            return;

        for(int i = /*prevIndex*/0; i < allZorks.Count; i++)
        {
            if (CheckDeadElements(allZorks[i].zork, allZorks, i))
                break;

            if (allZorks[i].zork.GetComponent<Zonk>().pushL)
            {
                GetComponent<LevelController>().level[(int)allZorks[i].zork.transform.position.x - 1, (int)allZorks[i].zork.transform.position.y] = 33;
                GetComponent<LevelController>().level[(int)allZorks[i].zork.transform.position.x, (int)allZorks[i].zork.transform.position.y] = -1;
                allZorks[i].newX = allZorks[i].zork.transform.position.x - 1;
                zorksToLeft.Add(allZorks[i]);
                allZorks.RemoveAt(i);
            }
            else if (allZorks[i].zork.GetComponent<Zonk>().pushR)
            {
                GetComponent<LevelController>().level[(int)allZorks[i].zork.transform.position.x + 1, (int)allZorks[i].zork.transform.position.y] = /*33*/3;
                GetComponent<LevelController>().level[(int)allZorks[i].zork.transform.position.x, (int)allZorks[i].zork.transform.position.y] = -1;
                allZorks[i].newX = allZorks[i].zork.transform.position.x + 1;
                zorksToRight.Add(allZorks[i]);
                allZorks.RemoveAt(i);
            }
            else if (allZorks[i].dangerous && GetComponent<LevelController>().level[(int)allZorks[i].zork.transform.position.x, (int)allZorks[i].zork.transform.position.y - 1] == -99)
            {
                GetComponent<LevelController>().Explosion(allZorks[i].zork.transform.position.x, allZorks[i].zork.transform.position.y - 1, false);
                allZorks[i].zork.GetComponent<Zonk>().canBeDestroy = true;
            }
            else if (GetComponent<LevelController>().level[(int)allZorks[i].zork.transform.position.x, (int)allZorks[i].zork.transform.position.y - 1] == 11)
            {
                GetComponent<LevelController>().spwnInfo = true;
                GetComponent<LevelController>().Explosion(allZorks[i].zork.transform.position.x, allZorks[i].zork.transform.position.y - 1, false);
                
            }
            else if (GetComponent<LevelController>().level[(int)allZorks[i].zork.transform.position.x, (int)allZorks[i].zork.transform.position.y - 1] == 10)
            {
                GetComponent<LevelController>().Explosion(allZorks[i].zork.transform.position.x, allZorks[i].zork.transform.position.y - 1, false);
                allZorks[i].zork.GetComponent<Explosable>().canBeExploded = true;
            }
            //снизу пусто
            else if (GetComponent<LevelController>().level[(int)allZorks[i].zork.transform.position.x, (int)allZorks[i].zork.transform.position.y - 1] == -1)
            {
                GetComponent<LevelController>().level[(int)allZorks[i].zork.transform.position.x, (int)allZorks[i].zork.transform.position.y - 1] = /*33*/3;
                allZorks[i].newY = allZorks[i].zork.transform.position.y - 1;
                zorksToFall.Add(allZorks[i]);
                allZorks.RemoveAt(i);
            }
            /* если под зорком инфотрон или зорк или рам */
            else if (GetComponent<LevelController>().level[(int)allZorks[i].zork.transform.position.x, (int)allZorks[i].zork.transform.position.y - 1] == 3 ||
                    GetComponent<LevelController>().level[(int)allZorks[i].zork.transform.position.x, (int)allZorks[i].zork.transform.position.y - 1] == 9 ||
                    GetComponent<LevelController>().level[(int)allZorks[i].zork.transform.position.x, (int)allZorks[i].zork.transform.position.y - 1] == 4)
            {
                //Debug.Log("rolling vlock under");
                //и над зорком нету блоков инфотрона или зорка
                if (GetComponent<LevelController>().level[(int)allZorks[i].zork.transform.position.x, (int)allZorks[i].zork.transform.position.y + 1] != 3 ||
                    GetComponent<LevelController>().level[(int)allZorks[i].zork.transform.position.x, (int)allZorks[i].zork.transform.position.y + 1] != 9)

                {
                    if (GetComponent<LevelController>().level[(int)allZorks[i].zork.transform.position.x - 1, (int)allZorks[i].zork.transform.position.y] == -1 &&
                             GetComponent<LevelController>().level[(int)allZorks[i].zork.transform.position.x - 1, (int)allZorks[i].zork.transform.position.y - 1] == -1)
                    {
                        GetComponent<LevelController>().level[(int)allZorks[i].zork.transform.position.x - 1, (int)allZorks[i].zork.transform.position.y] = /*33*/3;
                        allZorks[i].newX = allZorks[i].zork.transform.position.x - 1;
                        zorksToLeft.Add(allZorks[i]);
                        allZorks.RemoveAt(i);
                    }
                    //если свободно справа и справа на уровенб ниже
                    else if (GetComponent<LevelController>().level[(int)allZorks[i].zork.transform.position.x + 1, (int)allZorks[i].zork.transform.position.y] == -1 &&
                             GetComponent<LevelController>().level[(int)allZorks[i].zork.transform.position.x + 1, (int)allZorks[i].zork.transform.position.y - 1] == -1)
                    {
                        GetComponent<LevelController>().level[(int)allZorks[i].zork.transform.position.x + 1, (int)allZorks[i].zork.transform.position.y] = /*33*/3;
                        allZorks[i].newX = allZorks[i].zork.transform.position.x + 1;
                        zorksToRight.Add(allZorks[i]);
                        allZorks.RemoveAt(i);
                    }
                }
            }
            else if (allZorks[i].dangerous)
                allZorks[i].dangerous = false;
        }
    }

    void FillZorksList()
    {
        for (int i = 0; i < zorkStorage.transform.childCount; i++)
            allZorks.Add(new ZorksInfo(zorkStorage.transform.GetChild(i).gameObject, zorkStorage.transform.GetChild(i).transform.position.x, zorkStorage.transform.GetChild(i).transform.position.y));
    }

    void Falling()
    {
        for(int i = 0; i < zorksToFall.Count; i++)
        {
            if (CheckDeadElements(zorksToFall[i].zork, zorksToFall, i))
                break;
            if (zorksToFall[i].zork.transform.position.y > zorksToFall[i].newY)
            {
                zorksToFall[i].zork.transform.Translate(-Vector3.up * Time.deltaTime * speed);
                zorksToFall[i].dangerous = true;
            }
            else
            {
                zorksToFall[i].zork.transform.position = new Vector3(zorksToFall[i].zork.transform.position.x, zorksToFall[i].newY, 0);
                GetComponent<LevelController>().level[(int)zorksToFall[i].zork.transform.position.x, (int)zorksToFall[i].zork.transform.position.y] = 3;
                GetComponent<LevelController>().level[(int)zorksToFall[i].zork.transform.position.x, (int)zorksToFall[i].zork.transform.position.y + 1] = -1;
                allZorks.Add(zorksToFall[i]);
                zorksToFall.RemoveAt(i);
            }
        }
    }

    void MoveRight()
    {
        for (int i = 0; i < zorksToRight.Count; i++)
        {
            if (CheckDeadElements(zorksToRight[i].zork, zorksToRight, i))
                break;
            if (zorksToRight[i].zork.transform.position.x < zorksToRight[i].newX)
            {
                zorksToRight[i].zork.transform.Translate(Vector3.right * Time.deltaTime * speedLR );
                zorksToRight[i].zork.transform.GetChild(0).transform.Rotate(Vector3.forward, Time.deltaTime * -360 * 3, Space.Self);
            }
            else
            {
                zorksToRight[i].zork.transform.position = new Vector3(zorksToRight[i].newX, zorksToRight[i].zork.transform.position.y, 0);
                zorksToRight[i].zork.transform.GetChild(0).transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                GetComponent<LevelController>().level[(int)zorksToRight[i].zork.transform.position.x, (int)zorksToRight[i].zork.transform.position.y] = 3;
                GetComponent<LevelController>().level[(int)zorksToRight[i].zork.transform.position.x - 1, (int)zorksToRight[i].zork.transform.position.y] = -1;

                if (zorksToRight[i].zork.GetComponent<Zonk>().pushR)
                    zorksToRight[i].zork.GetComponent<Zonk>().pushR = false;

                allZorks.Add(zorksToRight[i]);
                zorksToRight.RemoveAt(i);
            }
        }
    }

    void MoveLeft()
    {
        for (int i = 0; i < zorksToLeft.Count; i++)
        {
            if (CheckDeadElements(zorksToLeft[i].zork, zorksToLeft, i))
                break;
            if (zorksToLeft[i].zork.transform.position.x > zorksToLeft[i].newX)
            {
                zorksToLeft[i].zork.transform.Translate(-Vector3.right * Time.deltaTime * speedLR);
                zorksToLeft[i].zork.transform.GetChild(0).transform.Rotate(Vector3.forward, Time.deltaTime * 360 * 3, Space.Self);
            }
            else
            {
                zorksToLeft[i].zork.transform.position = new Vector3((int)zorksToLeft[i].newX, (int)zorksToLeft[i].zork.transform.position.y, 0);
                zorksToLeft[i].zork.transform.GetChild(0).transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                GetComponent<LevelController>().level[(int)zorksToLeft[i].zork.transform.position.x, (int)zorksToLeft[i].zork.transform.position.y] = 3;
                GetComponent<LevelController>().level[(int)zorksToLeft[i].zork.transform.position.x + 1, (int)zorksToLeft[i].zork.transform.position.y] = -1;

                if (zorksToLeft[i].zork.GetComponent<Zonk>().pushL)
                    zorksToLeft[i].zork.GetComponent<Zonk>().pushL = false;

                allZorks.Add(zorksToLeft[i]);
                zorksToLeft.RemoveAt(i);
            }
        }
    }

    void ExplosionDestroy()
    {
        /*for(int i = 0; i < allZorks.Count; i++)
        {
            if (allZorks[i].zork.GetComponent<Zonk>().canBeDestroy)
                allZorks.RemoveAt(i);
        }*/
    }

    /*
     * Ф-ия проверки, надо ли ломать объект element
     * и удалять его из списка list
     * по индексу index
    */
    bool CheckDeadElements(GameObject element, List<ZorksInfo> list, int index)
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
public class ZorksInfo
{
    public GameObject zork { set; get; }
    public float newX { set; get; }
    public float newY { set; get; }
    public bool dangerous { set; get; }
    //public bool moving { set; get; }

    public ZorksInfo(GameObject zork, float x, float y)
    {
        this.zork = zork;
    }
}
