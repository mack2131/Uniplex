/**********************************************/
/* Скрипт, отвечающий за блок взрыва      
 * 
 * Создан Белым Койотом
/**********************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    [Tooltip("how many seconds explosion will be live")]
    public float lifetime;//время жизни взрыва
    private float lifetimeCounter;//счетчик времени
    private GameObject levelManager;//менеджер уровня
    private bool onObject;//находится ли объект взрыва на каком-то объекте, true - пустая ячейка, false - не пустая
    private float timer;//через сколько секунд после появения объекта мы проверим, столкнулся ли он с другими объектами
    public bool spawnInfotron;//флаг спавна инфотронов
    public GameObject infotronPrefab;//префаб инфотрона

    public int life;
    public float totalLife = 1.5f;

    void Start()
    {
        levelManager = GameObject.FindObjectOfType<LevelManager>().gameObject;//ищем и берем менджер уровня
        onObject = false;//не на объекте
        StartCoroutine(Life());
    }

    /*void Update()
    {
        Lifetime();//функция подсчета времени жизни взрыва
        timer += Time.deltaTime;//считаем время до инициализации, находимся ли мы в пустой ячейки
        if (!onObject && timer >=0.3)//если мы в пустой ячейке
        {
            //transform.GetComponent<BoxCollider>().enabled = false;//выключаем коллайдер
            //ставим пустую ячейку на место взрыва
            levelManager.GetComponent<LevelManager>().levelState[(int)transform.position.x, (int)transform.position.y] = -1;
        }
        if (spawnInfotron)//если нужно заспавнить инфотрон
            lifetime = lifetime / 3;//уменьшаем время жизни взрыва в 2 раза
    }*/
    
    void OnCollisionEnterOld(Collision coll)//если столкнулись с чем-то
    {
        if (coll.collider.tag != "Wall" && coll.collider.tag != "Hardware")//и объект не стена, которую нельзя сломать
        {
            if (coll.collider.GetComponent<Murphy>() != null)//если с мёрфи
                onObject = true;//говорим, что мы на объекте
            else if (coll.collider.GetComponent<RedDisc>() != null)//если красная дискета
            {
                coll.collider.GetComponent<RedDisc>().chained = true;//запускаем цепную реакцию
                onObject = true;//говорим, что мы на объекте
                //ставим пустую ячейку на место взрыва
                levelManager.GetComponent<LevelManager>().levelState[(int)transform.position.x, (int)transform.position.y] = -1;
                //выключаем коллайдер
                transform.GetComponent<BoxCollider>().enabled = false;
                return;
            }
            else if (coll.collider.GetComponent<OrangeDisc>() != null)//если оранжевая
            {
                coll.collider.GetComponent<OrangeDisc>().chained = true;//запускаем цепную реакцию
                onObject = true;//говорим, что мы на объекте
                //ставим пустую ячейку на место взрыва
                levelManager.GetComponent<LevelManager>().levelState[(int)transform.position.x, (int)transform.position.y] = -1;
                //выключаем коллайдер
                transform.GetComponent<BoxCollider>().enabled = false;
                return;
            }

            onObject = true;//говорим, что мы на объекте
            //ставим пустую ячейку на место взрыва
            levelManager.GetComponent<LevelManager>().levelState[(int)transform.position.x, (int)transform.position.y] = -1;
            //выключаем коллайдер
            transform.GetComponent<BoxCollider>().enabled = false;
            Destroy(coll.collider.gameObject);//то ломаем этот объект
        }
        else if (coll.collider.GetComponent<Murphy>() != null)
        {
            onObject = true;//говорим, что мы на объекте
            levelManager.GetComponent<LevelManager>().levelState[(int)transform.position.x, (int)transform.position.y] = -1;//ставим пустую ячейку на место взрыва
            Destroy(coll.gameObject);
        }
        else spawnInfotron = false;
    }

    void Lifetime()//функция подсчета жизни
    {
        lifetimeCounter += Time.deltaTime;//увеличиваем счетчик
        if (lifetimeCounter >= lifetime)//если счетчик больше или равен времени жизни взрыва
        {
            SpawnInfotron();
            Destroy(gameObject);//уничтожаем взрыв
        }
    }

    void SpawnInfotron()//если спавним инфотрон
    {
        if(spawnInfotron)//когда есть флаг спавна инфотронов
        {
            GameObject.FindObjectOfType<LevelManager>().gameObject.GetComponent<LevelManager>().levelState[(int)transform.position.x, (int)transform.position.y] = 3;//берем у менеджера уровней состояние уровня
            GameObject info = Instantiate(infotronPrefab);//создаем инфотрон из префаба
            info.transform.position = transform.position;//ставим в нужную позицию
            //info.GetComponent<Infotron>().levelManager = levelManager;//передаем менеджер уровня
            transform.GetComponent<BoxCollider>().enabled = false;//отключаем коллайдер взрыва
            Destroy(gameObject);//уничтожаем взрыв
        }
    }

    void Destructione()
    {
        levelManager.GetComponent<LevelManager>().levelState[(int)transform.position.x, (int)transform.position.y] = -1;//ставим пустую ячейку на место взрыва
        Destroy(this.gameObject);
    }

    IEnumerator Life()
    {
        while(true)
        {
            life++;
            if (life >= totalLife)
                Destroy(this.gameObject);
            yield return new WaitForSeconds(1);
        }
    }
}
