using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnikSnakBehaviour : MonoBehaviour
{
    //скорость движения
    public float speed = 2f;
    //скорость поврота
    public float rotationSpeed = 10;
    //хранилище объектов
    public GameObject snikStorage;
    //списки, в которых хрантся объекты, одетые в специальный класс
    //списки для поворота, для всех объектов, для движения влеов, вправо
    public List<Snik> snikAll = new List<Snik>();
    public List<Snik> snikWait = new List<Snik>();
    public List<Snik> snikRight = new List<Snik>();
    public List<Snik> snikLeft = new List<Snik>();
    public List<Snik> snikDown = new List<Snik>();
    public List<Snik> snikUp = new List<Snik>();
    public List<Snik> snikNear = new List<Snik>();

    // Start is called before the first frame update
    void Start()
    {
        //заполняем список из хранилища
        FillList();
    }

    //Обработчик ножниц
    public void SnikSnakUpdate()
    {
        //если в списке всех что-то есть
        if (snikAll.Count != 0)
            FindWay();//ищем путь

        //если в списке ожидания что-то есть
        if (snikWait.Count != 0)
            Rotate();//куртимся

        //если в списке движения на лево что-то есть
        if (snikLeft.Count != 0)
            MoveLeft();//двигаеам налево

        //если в списке вверх что-то есть
        if (snikUp.Count != 0)
            MoveUp();//двигаемся вверх

        //если в списке вниз что-то есть
        if (snikDown.Count != 0)
            MoveDown();//двигаемся вниз

        //если в списке вправо что-то есть
        if (snikRight.Count != 0)
            MoveRight();//двигаемся влево
    }

    //Ф-ия заполнения основного списка
    void FillList()
    {
        //идем по всех элементам хранилища
        for (int i = 0; i < snikStorage.transform.childCount; i++)
            snikAll.Add(new Snik(snikStorage.transform.GetChild(i).gameObject));//добавляем в список новые объект нового класса
    }

    //Ф-ия поиска пути
    void FindWay()
    {
        //идем по всем элемента основного списка
        for (int i = 0; i < snikAll.Count; i++)
        {
            //если элемент надо убрать из списка (он должен быть уничтожен)
            if (CheckDeadElements(snikAll[i].snik, snikAll, i))
                break;//прерываемся
            //смотрим, куда повернут объект
            //далее смотрим стороны (лево право вверх и вниз)
            //относительно повернутого объекта
            switch (snikAll[i].faceDirection)
            {
                case 0://направо
                    {
                        //Debug.Log((int)snikAll[i].snik.transform.position.x + " " + ((int)snikAll[i].snik.transform.position.y + 1) + " - " + GetComponent<LevelController>().level[(int)snikAll[i].snik.transform.position.x, (int)snikAll[i].snik.transform.position.y + 1]);
                        //проверяем "лево"  относительно самого объекта
                        if (GetComponent<LevelController>().level[(int)snikAll[i].snik.transform.position.x, (int)snikAll[i].snik.transform.position.y + 1] == -1 ||
                            GetComponent<LevelController>().level[(int)snikAll[i].snik.transform.position.x, (int)snikAll[i].snik.transform.position.y + 1] == -99)
                        {
                            if (GetComponent<LevelController>().level[(int)snikAll[i].snik.transform.position.x, (int)snikAll[i].snik.transform.position.y + 1] != -99)
                            {
                                //говорим что в эту ячейку собираемся двигаться
                                GetComponent<LevelController>().level[(int)snikAll[i].snik.transform.position.x, (int)snikAll[i].snik.transform.position.y + 1] = 33;
                            }
                            //освобождаем текущую
                            GetComponent<LevelController>().level[(int)snikAll[i].snik.transform.position.x, (int)snikAll[i].snik.transform.position.y] = -1;
                            //поворачиваем объект налево (по часовой стрелке)
                            snikAll[i].snik.transform.rotation = Quaternion.Euler(0, 0, snikAll[i].snik.transform.rotation.eulerAngles.z + 90);
                            //обновляем направление после поворота
                            snikAll[i].UpdateFaceDirection();
                            //задаем координату для движения
                            snikAll[i].newY = (int)snikAll[i].snik.transform.position.y + 1;
                            //добавляем текущий эелемент в нужный для движения список
                            snikUp.Add(snikAll[i]);
                            //убираем элемент из текущего списка
                            snikAll.RemoveAt(i);
                        }
                        //если прямо свободно
                        else if (GetComponent<LevelController>().level[(int)snikAll[i].snik.transform.position.x + 1, (int)snikAll[i].snik.transform.position.y] == -1 ||
                                 GetComponent<LevelController>().level[(int)snikAll[i].snik.transform.position.x + 1, (int)snikAll[i].snik.transform.position.y] == -99)
                        {
                            if (GetComponent<LevelController>().level[(int)snikAll[i].snik.transform.position.x + 1, (int)snikAll[i].snik.transform.position.y] != -99)
                            {   //говорим что в эту ячейку собираемся двигаться
                                GetComponent<LevelController>().level[(int)snikAll[i].snik.transform.position.x + 1, (int)snikAll[i].snik.transform.position.y] = 33;
                            }
                            //освобождаем текущую
                            GetComponent<LevelController>().level[(int)snikAll[i].snik.transform.position.x, (int)snikAll[i].snik.transform.position.y] = -1;
                            //задаем координату для движения
                            snikAll[i].newX = (int)snikAll[i].snik.transform.position.x + 1;
                            //добавляем текущий эелемент в нужный для движения список
                            snikRight.Add(snikAll[i]);
                            //убираем элемент из текущего списка
                            snikAll.RemoveAt(i);
                        }
                        else if (GetComponent<LevelController>().level[(int)snikAll[i].snik.transform.position.x, (int)snikAll[i].snik.transform.position.y - 1] == -1 ||
                                 GetComponent<LevelController>().level[(int)snikAll[i].snik.transform.position.x, (int)snikAll[i].snik.transform.position.y - 1] == -99)
                        {
                            if (GetComponent<LevelController>().level[(int)snikAll[i].snik.transform.position.x, (int)snikAll[i].snik.transform.position.y - 1] != -99)
                            {   
                                //говорим что в эту ячейку собираемся двигаться
                                GetComponent<LevelController>().level[(int)snikAll[i].snik.transform.position.x, (int)snikAll[i].snik.transform.position.y - 1] = 33;
                            }
                            //освобождаем текущую
                            GetComponent<LevelController>().level[(int)snikAll[i].snik.transform.position.x, (int)snikAll[i].snik.transform.position.y] = -1;
                            //поворачиваем объект налево (против часовой стрелки)
                            snikAll[i].snik.transform.rotation = Quaternion.Euler(0, 0, snikAll[i].snik.transform.rotation.eulerAngles.z - 90);
                            //обновляем направление после поворота
                            snikAll[i].UpdateFaceDirection();
                            //задаем координату для движения
                            snikAll[i].newY = (int)snikAll[i].snik.transform.position.y - 1;
                            //добавляем текущий эелемент в нужный для движения список
                            snikDown.Add(snikAll[i]);
                            //убираем элемент из текущего списка
                            snikAll.RemoveAt(i);
                        }
                        else//если свободных слеток нет, то крутимся и ждем, пока что-то появится
                        {
                            //запоминаем текущий угол поворота
                            snikAll[i].oldAngle = (int)snikAll[i].snik.transform.localEulerAngles.z;
                            //добавляем объект в список поворота
                            snikWait.Add(snikAll[i]);
                            //убираем элемент из текущего списка
                            snikAll.RemoveAt(i);
                        }
                        break;
                    }
                case 1://left
                    {
                        //проверяем "лево"  относительно самого объекта
                        if (GetComponent<LevelController>().level[(int)snikAll[i].snik.transform.position.x, (int)snikAll[i].snik.transform.position.y - 1] == -1 ||
                            GetComponent<LevelController>().level[(int)snikAll[i].snik.transform.position.x, (int)snikAll[i].snik.transform.position.y - 1] == -99)
                        {
                            if(GetComponent<LevelController>().level[(int)snikAll[i].snik.transform.position.x, (int)snikAll[i].snik.transform.position.y - 1] != -99)
                                GetComponent<LevelController>().level[(int)snikAll[i].snik.transform.position.x, (int)snikAll[i].snik.transform.position.y - 1] = 33;
                            GetComponent<LevelController>().level[(int)snikAll[i].snik.transform.position.x, (int)snikAll[i].snik.transform.position.y] = -1;
                            snikAll[i].snik.transform.rotation = Quaternion.Euler(0, 0, snikAll[i].snik.transform.rotation.eulerAngles.z + 90);
                            snikAll[i].UpdateFaceDirection();
                            snikAll[i].newY = (int)snikAll[i].snik.transform.position.y - 1;
                            snikDown.Add(snikAll[i]);
                            snikAll.RemoveAt(i);
                        }
                        //если прямо свободно
                        else if (GetComponent<LevelController>().level[(int)snikAll[i].snik.transform.position.x - 1, (int)snikAll[i].snik.transform.position.y] == -1 ||
                                 GetComponent<LevelController>().level[(int)snikAll[i].snik.transform.position.x - 1, (int)snikAll[i].snik.transform.position.y] == -99)
                        {
                            if(GetComponent<LevelController>().level[(int)snikAll[i].snik.transform.position.x - 1, (int)snikAll[i].snik.transform.position.y] != -99)
                                GetComponent<LevelController>().level[(int)snikAll[i].snik.transform.position.x - 1, (int)snikAll[i].snik.transform.position.y] = 33;
                            GetComponent<LevelController>().level[(int)snikAll[i].snik.transform.position.x, (int)snikAll[i].snik.transform.position.y] = -1;
                            snikAll[i].newX = (int)snikAll[i].snik.transform.position.x - 1;
                            snikLeft.Add(snikAll[i]);
                            snikAll.RemoveAt(i);
                        }
                        else if (GetComponent<LevelController>().level[(int)snikAll[i].snik.transform.position.x, (int)snikAll[i].snik.transform.position.y + 1] == -1 ||
                                 GetComponent<LevelController>().level[(int)snikAll[i].snik.transform.position.x, (int)snikAll[i].snik.transform.position.y + 1] == -99)
                        {
                            if(GetComponent<LevelController>().level[(int)snikAll[i].snik.transform.position.x, (int)snikAll[i].snik.transform.position.y + 1] != -99)
                                GetComponent<LevelController>().level[(int)snikAll[i].snik.transform.position.x, (int)snikAll[i].snik.transform.position.y + 1] = 33;
                            GetComponent<LevelController>().level[(int)snikAll[i].snik.transform.position.x, (int)snikAll[i].snik.transform.position.y] = -1;
                            snikAll[i].snik.transform.rotation = Quaternion.Euler(0, 0, snikAll[i].snik.transform.rotation.eulerAngles.z - 90);
                            snikAll[i].UpdateFaceDirection();
                            snikAll[i].newY = (int)snikAll[i].snik.transform.position.y + 1;
                            snikUp.Add(snikAll[i]);
                            snikAll.RemoveAt(i);
                        }
                        else//если свободных слеток нет, то крутимся и ждем, пока что-то появится
                        {
                            //запоминаем текущий угол поворота
                            snikAll[i].oldAngle = (int)snikAll[i].snik.transform.localEulerAngles.z;
                            //добавляем объект в список поворота
                            snikWait.Add(snikAll[i]);
                            //убираем элемент из текущего списка
                            snikAll.RemoveAt(i);
                        }
                        break;
                    }
                case 2://down
                    {
                        //проверяем "лево"  относительно самого объекта
                        if (GetComponent<LevelController>().level[(int)snikAll[i].snik.transform.position.x + 1, (int)snikAll[i].snik.transform.position.y] == -1 ||
                            GetComponent<LevelController>().level[(int)snikAll[i].snik.transform.position.x + 1, (int)snikAll[i].snik.transform.position.y] == -99)
                        {
                            if(GetComponent<LevelController>().level[(int)snikAll[i].snik.transform.position.x + 1, (int)snikAll[i].snik.transform.position.y] != -99)
                                GetComponent<LevelController>().level[(int)snikAll[i].snik.transform.position.x + 1, (int)snikAll[i].snik.transform.position.y] = 33;
                            GetComponent<LevelController>().level[(int)snikAll[i].snik.transform.position.x, (int)snikAll[i].snik.transform.position.y] = -1;
                            snikAll[i].snik.transform.rotation = Quaternion.Euler(0, 0, snikAll[i].snik.transform.rotation.eulerAngles.z + 90);
                            snikAll[i].UpdateFaceDirection();
                            snikAll[i].newX = (int)snikAll[i].snik.transform.position.x + 1;
                            snikRight.Add(snikAll[i]);
                            snikAll.RemoveAt(i);
                        }
                        //проверяем прямо по движению, куда направлен объект
                        else if (GetComponent<LevelController>().level[(int)snikAll[i].snik.transform.position.x, (int)snikAll[i].snik.transform.position.y - 1] == -1 ||
                                 GetComponent<LevelController>().level[(int)snikAll[i].snik.transform.position.x, (int)snikAll[i].snik.transform.position.y - 1] == -99)
                        {
                            if(GetComponent<LevelController>().level[(int)snikAll[i].snik.transform.position.x, (int)snikAll[i].snik.transform.position.y - 1] != -99)
                                GetComponent<LevelController>().level[(int)snikAll[i].snik.transform.position.x, (int)snikAll[i].snik.transform.position.y - 1] = 33;
                            GetComponent<LevelController>().level[(int)snikAll[i].snik.transform.position.x, (int)snikAll[i].snik.transform.position.y] = -1;
                            snikAll[i].newY = (int)snikAll[i].snik.transform.position.y - 1;
                            snikDown.Add(snikAll[i]);
                            snikAll.RemoveAt(i);
                        }
                        else if (GetComponent<LevelController>().level[(int)snikAll[i].snik.transform.position.x - 1, (int)snikAll[i].snik.transform.position.y] == -1 ||
                                 GetComponent<LevelController>().level[(int)snikAll[i].snik.transform.position.x - 1, (int)snikAll[i].snik.transform.position.y] == -99)
                        {
                            if(GetComponent<LevelController>().level[(int)snikAll[i].snik.transform.position.x - 1, (int)snikAll[i].snik.transform.position.y] != -99)
                                GetComponent<LevelController>().level[(int)snikAll[i].snik.transform.position.x - 1, (int)snikAll[i].snik.transform.position.y] = 33;
                            GetComponent<LevelController>().level[(int)snikAll[i].snik.transform.position.x, (int)snikAll[i].snik.transform.position.y] = -1;
                            snikAll[i].snik.transform.rotation = Quaternion.Euler(0, 0, snikAll[i].snik.transform.rotation.eulerAngles.z - 90);
                            snikAll[i].UpdateFaceDirection();
                            snikAll[i].newX = (int)snikAll[i].snik.transform.position.x - 1;
                            snikLeft.Add(snikAll[i]);
                            snikAll.RemoveAt(i);
                        }
                        else//если свободных слеток нет, то крутимся и ждем, пока что-то появится
                        {
                            //запоминаем текущий угол поворота
                            snikAll[i].oldAngle = (int)snikAll[i].snik.transform.localEulerAngles.z;
                            //добавляем объект в список поворота
                            snikWait.Add(snikAll[i]);
                            //убираем элемент из текущего списка
                            snikAll.RemoveAt(i);
                        }
                        break;
                    }
                case 3://up
                    {
                        //проверяем "лево"  относительно самого объекта
                        if (GetComponent<LevelController>().level[(int)snikAll[i].snik.transform.position.x - 1, (int)snikAll[i].snik.transform.position.y] == -1 ||
                            GetComponent<LevelController>().level[(int)snikAll[i].snik.transform.position.x - 1, (int)snikAll[i].snik.transform.position.y] == -99)
                        {
                            if(GetComponent<LevelController>().level[(int)snikAll[i].snik.transform.position.x - 1, (int)snikAll[i].snik.transform.position.y] != -99)
                                GetComponent<LevelController>().level[(int)snikAll[i].snik.transform.position.x - 1, (int)snikAll[i].snik.transform.position.y] = 33;
                            GetComponent<LevelController>().level[(int)snikAll[i].snik.transform.position.x, (int)snikAll[i].snik.transform.position.y] = -1;
                            snikAll[i].snik.transform.rotation = Quaternion.Euler(0, 0, snikAll[i].snik.transform.rotation.eulerAngles.z + 90);
                            snikAll[i].UpdateFaceDirection();
                            snikAll[i].newX = (int)snikAll[i].snik.transform.position.x - 1;
                            snikLeft.Add(snikAll[i]);
                            snikAll.RemoveAt(i);
                        }
                        //проверяем прямо по движению, куда направлен объект
                        else if (GetComponent<LevelController>().level[(int)snikAll[i].snik.transform.position.x, (int)snikAll[i].snik.transform.position.y + 1] == -1 ||
                                 GetComponent<LevelController>().level[(int)snikAll[i].snik.transform.position.x, (int)snikAll[i].snik.transform.position.y + 1] == -99)
                        {
                            if(GetComponent<LevelController>().level[(int)snikAll[i].snik.transform.position.x, (int)snikAll[i].snik.transform.position.y + 1] != -99)
                                GetComponent<LevelController>().level[(int)snikAll[i].snik.transform.position.x, (int)snikAll[i].snik.transform.position.y + 1] = 33;
                            GetComponent<LevelController>().level[(int)snikAll[i].snik.transform.position.x, (int)snikAll[i].snik.transform.position.y] = -1;
                            snikAll[i].newY = (int)snikAll[i].snik.transform.position.y + 1;
                            snikUp.Add(snikAll[i]);
                            snikAll.RemoveAt(i);
                        }
                        else if (GetComponent<LevelController>().level[(int)snikAll[i].snik.transform.position.x + 1, (int)snikAll[i].snik.transform.position.y] == -1 ||
                                 GetComponent<LevelController>().level[(int)snikAll[i].snik.transform.position.x + 1, (int)snikAll[i].snik.transform.position.y] == -99)
                        {
                            if(GetComponent<LevelController>().level[(int)snikAll[i].snik.transform.position.x + 1, (int)snikAll[i].snik.transform.position.y] != -99)
                                GetComponent<LevelController>().level[(int)snikAll[i].snik.transform.position.x + 1, (int)snikAll[i].snik.transform.position.y] = 33;
                            GetComponent<LevelController>().level[(int)snikAll[i].snik.transform.position.x, (int)snikAll[i].snik.transform.position.y] = -1;
                            snikAll[i].snik.transform.rotation = Quaternion.Euler(0, 0, snikAll[i].snik.transform.rotation.eulerAngles.z - 90);
                            snikAll[i].UpdateFaceDirection();
                            snikAll[i].newX = (int)snikAll[i].snik.transform.position.x + 1;
                            snikRight.Add(snikAll[i]);
                            snikAll.RemoveAt(i);
                        }
                        else//если свободных слеток нет, то крутимся и ждем, пока что-то появится
                        {
                            //запоминаем текущий угол поворота
                            snikAll[i].oldAngle = (int)snikAll[i].snik.transform.localEulerAngles.z;
                            //добавляем объект в список поворота
                            snikWait.Add(snikAll[i]);
                            //убираем элемент из текущего списка
                            snikAll.RemoveAt(i);
                        }
                        break;
                    }
            }
        }
    }

    //Ф-ия поворота и ожидания
    void Rotate()
    {
        //по всем элементам списка
        for (int i = 0; i < snikWait.Count; i++)
        {
            //проверяем, нужно ли уничтожать элемент
            if (CheckDeadElements(snikWait[i].snik, snikWait, i))
                break;//если да, то прерываемся
            //если нужно повернуться до 360
            if (snikWait[i].oldAngle + 90 == 360)
                snikWait[i].oldAngle = -90;//то необходимо особое условие
            //если текущий угол поворота по оси z меньше чем должен быть новый угол (запомненный + 90)
            if (snikWait[i].snik.transform.localRotation.eulerAngles.z <= snikWait[i].oldAngle + 90)
                snikWait[i].snik.transform.Rotate(Vector3.forward, Time.deltaTime * 180, Space.World);//крутимся против часовой
            else//если достаточно повернули
            {
                //ставим угол поворота, каким должен быть, без приближений
                snikWait[i].snik.transform.localRotation = Quaternion.Euler(0, 0, snikWait[i].oldAngle + 90);
                //обновляем направление
                snikWait[i].UpdateFaceDirection();
                //добавляем в основной список
                snikAll.Add(snikWait[i]);
                //убираем их текущего списка
                snikWait.RemoveAt(i);
            }
        }
    }

    //Ф-ия движения направо
    void MoveRight()
    {
        //по всем элементам списка
        for(int i =  0; i < snikRight.Count; i++)
        {
            //проверяем, нужно ли уничтожать элемент
            if (CheckDeadElements(snikRight[i].snik, snikRight, i))
                break;//если да, то прерываемся
            
            if (GetComponent<LevelController>().level[snikRight[i].newX, (int)snikRight[i].snik.transform.position.y] == -99)
                GetComponent<LevelController>().Explosion(snikRight[i].newX, snikRight[i].snik.transform.position.y, false);

            //если текущая координата отличается от той, которая должна быть
            if (snikRight[i].snik.transform.position.x < snikRight[i].newX)
                snikRight[i].snik.transform.Translate(Vector3.right * Time.deltaTime * speed);//двигаемся в нужную сторону
            else//когда уже достачно близко от нужной позиции
            {
                //ставим нужную позицию
                snikRight[i].snik.transform.position = new Vector3(snikRight[i].newX, snikRight[i].snik.transform.position.y, 0);
                //занимаем ячейку
                GetComponent<LevelController>().level[snikRight[i].newX, (int)snikRight[i].snik.transform.position.y] = 10;
                //добавляем в основной список
                snikAll.Add(snikRight[i]);
                //убираем их текущего списка
                snikRight.RemoveAt(i);
            }
        }
    }

    //Ф-ия движения лево
    void MoveLeft()
    {
        //по всем элементам списка
        for (int i = 0; i < snikLeft.Count; i++)
        {
            //проверяем, нужно ли уничтожать элемент
            if (CheckDeadElements(snikLeft[i].snik, snikLeft, i))
                break;//если да, то прерываемся

            if (GetComponent<LevelController>().level[snikLeft[i].newX, (int)snikLeft[i].snik.transform.position.y] == -99)
                GetComponent<LevelController>().Explosion(snikLeft[i].newX, snikLeft[i].snik.transform.position.y, false);

            //если текущая координата отличается от той, которая должна быть
            if (snikLeft[i].snik.transform.position.x > snikLeft[i].newX)
                snikLeft[i].snik.transform.Translate(Vector3.right * Time.deltaTime * speed);//двигаемся в нужную сторону
            else
            {
                //ставим нужную позицию
                snikLeft[i].snik.transform.position = new Vector3(snikLeft[i].newX, snikLeft[i].snik.transform.position.y, 0);
                //занимаем ячейку
                GetComponent<LevelController>().level[snikLeft[i].newX, (int)snikLeft[i].snik.transform.position.y] = 10;
                //добавляем в основной список
                snikAll.Add(snikLeft[i]);
                //убираем их текущего списка
                snikLeft.RemoveAt(i);
            }
        }
    }

    //Ф-ия движения вниз
    void MoveDown()
    {
        //по всем элементам списка
        for (int i = 0; i < snikDown.Count; i++)
        {
            //проверяем, нужно ли уничтожать элемент
            if (CheckDeadElements(snikDown[i].snik, snikDown, i))
                break;//если да, то прерываемся

            if (GetComponent<LevelController>().level[(int)snikDown[i].snik.transform.position.x, (int)snikDown[i].newY] == -99)
                GetComponent<LevelController>().Explosion((int)snikDown[i].snik.transform.position.x, snikDown[i].newY, false);

            //если текущая координата отличается от той, которая должна быть
            if (snikDown[i].snik.transform.position.y > snikDown[i].newY)
                snikDown[i].snik.transform.Translate(Vector3.right * Time.deltaTime * speed);//двигаемся в нужную сторону
            else
            {
                //ставим нужную позицию
                snikDown[i].snik.transform.position = new Vector3(snikDown[i].snik.transform.position.x, snikDown[i].newY, 0);
                //занимаем ячейку
                GetComponent<LevelController>().level[(int)snikDown[i].snik.transform.position.x, snikDown[i].newY] = 10;
                //добавляем в основной список
                snikAll.Add(snikDown[i]);
                //убираем их текущего списка
                snikDown.RemoveAt(i);
            }
        }
    }

    //Ф-ия движения вверх
    void MoveUp()
    {
        //по всем элементам списка
        for (int i = 0; i < snikUp.Count; i++)
        {
            //проверяем, нужно ли уничтожать элемент
            if (CheckDeadElements(snikUp[i].snik, snikUp, i))
                break;//если да, то прерываемся

            if (GetComponent<LevelController>().level[(int)snikUp[i].snik.transform.position.x, (int)snikUp[i].newY] == -99)
                GetComponent<LevelController>().Explosion((int)snikUp[i].snik.transform.position.x, snikUp[i].newY, false);

            //если текущая координата отличается от той, которая должна быть
            if (snikUp[i].snik.transform.position.y < snikUp[i].newY)
                snikUp[i].snik.transform.Translate(Vector3.right * Time.deltaTime * speed);//двигаемся в нужную сторону
            else
            {
                //ставим нужную позицию
                snikUp[i].snik.transform.position = new Vector3(snikUp[i].snik.transform.position.x, snikUp[i].newY, 0);
                //занимаем ячейку
                GetComponent<LevelController>().level[(int)snikUp[i].snik.transform.position.x, snikUp[i].newY] = 10;
                //добавляем в основной список
                snikAll.Add(snikUp[i]);
                //убираем их текущего списка
                snikUp.RemoveAt(i);
            }
        }
    }

    /*
     * Ф-ия проверки, надо ли ломать объект element
     * и удалять его из списка list
     * по индексу index
    */
    bool CheckDeadElements(GameObject element, List<Snik> list, int index)
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

//Класс - обертка для объекта
[System.Serializable]
public class Snik
{
    public GameObject snik;//объект ножниц
    //faceDIrection explain:
    // 0 - right
    // 1 - left
    // 2 - down
    // 3 - up
    public int faceDirection;//куда смотрии объект, включая его поворот
    public int newX;//координата х для движения
    public int newY;//координата у для движения
    public int oldAngle;//угол до начала поворота
    //Конструктор
    public Snik(GameObject snik)
    {
        this.snik = snik;//запоминаем объект
        this.newX = 0;//обнуляем координаты
        this.newY = 0;
        //расчитываем направления относительно угла поворота z
        switch((int)snik.transform.localRotation.eulerAngles.z)
        {
            case 0://right
                {
                    this.faceDirection = 0;
                    break;
                }
            case 90://up
                {
                    this.faceDirection = 3;
                    break;
                }
            case 180://left
                {
                    this.faceDirection = 1;
                    break;
                }
            case 270://down
                {
                    this.faceDirection = 2;
                    break;
                }
        }
    }

    //пересчитываем направление относительно угла поворота по z
    public void UpdateFaceDirection()
    {
        switch ((int)snik.transform.localRotation.eulerAngles.z)
        {
            case 0://right
                {
                    this.faceDirection = 0;
                    break;
                }
            case 90://up
                {
                    this.faceDirection = 3;
                    break;
                }
            case 180://left
                {
                    this.faceDirection = 1;
                    break;
                }
            case 270://down
                {
                    this.faceDirection = 2;
                    break;
                }
        }
    }
}
