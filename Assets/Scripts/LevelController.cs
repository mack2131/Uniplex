using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    //TODO - поедание блоков с места
    //     - закладка взрывчатки
    //массив состояния уровня
    public int[,] level;
    //юлок взрыва
    public GameObject expl;
    public GameObject infotron;
    public bool spwnInfo;
    public Vector2 exitPos;
    private bool[] spwnInfoPos = new bool[9];

    // Start is called before the first frame update
    void Start()
    {
        //берем состояние уровня
        level = GetComponent<LevelManager>().levelState;
    }

    void Update()
    {
        if (!GetComponent<LevelManager>().gameOver)
        {
            //смотрим управление за Мёрфи
            GetComponent<MurphyBehaviour>().MurphyUpdate();
            //смотрим за зонками
            GetComponent<ZonkBehaviour>().ZorksUpdate();
            //смотрим за инфотронами
            GetComponent<InfotronBehavior>().InfotronUpdate();
            //смотрим за сник снеками
            GetComponent<SnikSnakBehaviour>().SnikSnakUpdate();
            //смотрим за электроном
            GetComponent<ElectronBehaviour>().ElectronUpdate();

            if (GetComponent<LevelManager>().infotrons == 0)
                level[(int)exitPos.x, (int)exitPos.y] = 13;
        }
    }


    //Функция, которая вызывает взрыв 3х3 в центре с (centerX, centerY)
    public void Explosion(float centerX, float centerY, bool spawnInfotron)
    {
        //создаются 9 блоков взрыва в токой очередности
        // ***
        // *0*
        // ***
        // где * - блок взрыва, а 0 - зонк
        /* up */
        GameObject[] explosion = new GameObject[9];
        explosion[0] = Instantiate(expl);
        explosion[0].transform.position = new Vector3(centerX, centerY + 1, 0);
        /* down */
        explosion[1] = Instantiate(expl);
        explosion[1].transform.position = new Vector3(centerX, centerY - 1, 0);
        /* left */
        explosion[2] = Instantiate(expl);
        explosion[2].transform.position = new Vector3(centerX - 1, centerY, 0);
        /* right */
        explosion[3] = Instantiate(expl);
        explosion[3].transform.position = new Vector3(centerX + 1, centerY, 0);
        /* up-left */
        explosion[4] = Instantiate(expl);
        explosion[4].transform.position = new Vector3(centerX - 1, centerY + 1, 0);
        /* up-right */
        explosion[5] = Instantiate(expl);
        explosion[5].transform.position = new Vector3(centerX + 1, centerY + 1, 0);
        /* down-left */
        explosion[6] = Instantiate(expl);
        explosion[6].transform.position = new Vector3(centerX - 1, centerY - 1, 0);
        /* down-right */
        explosion[7] = Instantiate(expl);
        explosion[7].transform.position = new Vector3(centerX + 1, centerY - 1, 0);
        /* myself */
        explosion[8] = Instantiate(expl);
        explosion[8].transform.position = new Vector3(centerX, centerY, 0);
        for (int i = explosion.Length - 1; i > 0; i--)
            CheckExplosion(explosion[i], i);
        //Debug.Log(spwnInfo);
        if (spwnInfo)
            SpawnInfotrons(centerX, centerY);
    }

    void SpawnInfotrons(float centerX, float centerY)
    {
        //создаются 9 блоков взрыва в токой очередности
        // ***
        // *0*
        // ***
        // где * - блок взрыва, а 0 - зонк
        /* up */
        GameObject[] infotrons = new GameObject[9];
        if (spwnInfoPos[0])
        {
            infotrons[0] = Instantiate(infotron);
            infotrons[0].transform.position = new Vector3(centerX, centerY + 1, 0);
            GetComponent<InfotronBehavior>().allInfo.Add(new InfotronInfo(infotrons[0], centerX, centerY + 1));
            level[(int)centerX, (int)centerY + 1] = 9;
        }
        /* down */
        if (spwnInfoPos[1])
        {
            infotrons[1] = Instantiate(infotron);
            infotrons[1].transform.position = new Vector3(centerX, centerY - 1, 0);
            GetComponent<InfotronBehavior>().allInfo.Add(new InfotronInfo(infotrons[1], centerX, centerY - 1));
            level[(int)centerX, (int)centerY - 1] = 9;
        }
        /* left */
        if (spwnInfoPos[2])
        {
            infotrons[2] = Instantiate(infotron);
            infotrons[2].transform.position = new Vector3(centerX - 1, centerY, 0);
            GetComponent<InfotronBehavior>().allInfo.Add(new InfotronInfo(infotrons[2], centerX - 1, centerY));
            level[(int)centerX - 1, (int)centerY] = 9;
        }
        /* right */
        if (spwnInfoPos[3])
        {
            infotrons[3] = Instantiate(infotron);
            infotrons[3].transform.position = new Vector3(centerX + 1, centerY, 0);
            GetComponent<InfotronBehavior>().allInfo.Add(new InfotronInfo(infotrons[3], centerX + 1, centerY));
            level[(int)centerX + 1, (int)centerY] = 9;
        }
        /* up-left */
        if (spwnInfoPos[4])
        {
            infotrons[4] = Instantiate(infotron);
            infotrons[4].transform.position = new Vector3(centerX - 1, centerY + 1, 0);
            GetComponent<InfotronBehavior>().allInfo.Add(new InfotronInfo(infotrons[4], centerX - 1, centerY + 1));
            level[(int)centerX - 1, (int)centerY + 1] = 9;
        }
        /* up-right */
        if (spwnInfoPos[5])
        {
            infotrons[5] = Instantiate(infotron);
            infotrons[5].transform.position = new Vector3(centerX + 1, centerY + 1, 0);
            GetComponent<InfotronBehavior>().allInfo.Add(new InfotronInfo(infotrons[5], centerX + 1, centerY + 1));
            level[(int)centerX + 1, (int)centerY + 1] = 9;
        }
        /* down-left */
        if (spwnInfoPos[6])
        {
            infotrons[6] = Instantiate(infotron);
            infotrons[6].transform.position = new Vector3(centerX - 1, centerY - 1, 0);
            GetComponent<InfotronBehavior>().allInfo.Add(new InfotronInfo(infotrons[6], centerX - 1, centerY - 1));
            level[(int)centerX - 1, (int)centerY - 1] = 9;
        }
        /* down-right */
        if (spwnInfoPos[7])
        {
            infotrons[7] = Instantiate(infotron);
            infotrons[7].transform.position = new Vector3(centerX + 1, centerY - 1, 0);
            GetComponent<InfotronBehavior>().allInfo.Add(new InfotronInfo(infotrons[7], centerX + 1, centerY - 1));
            level[(int)centerX + 1, (int)centerY - 1] = 9;
        }
        /* myself */
        if (spwnInfoPos[8])
        {
            infotrons[8] = Instantiate(infotron);
            infotrons[8].transform.position = new Vector3(centerX, centerY, 0);
            GetComponent<InfotronBehavior>().allInfo.Add(new InfotronInfo(infotrons[8], centerX, centerY));
            level[(int)centerX, (int)centerY] = 9;
        }

        //for (int i = 0; i < spwnInfoPos.Length; i++)
        //Debug.Log(i + " : " + spwnInfoPos[i]);

        for (int i = 0; i < spwnInfoPos.Length; i++)
            spwnInfoPos[i] = false;

        spwnInfo = false;
    }

    void CheckExplosion(GameObject explosionBlock, int index)
    {
        //Debug.Log("index : " + index);
        //level[(int)explosionBlock.transform.position.x, (int)explosionBlock.transform.position.y] = -1;
        Collider[] colliders;
        
        if ((colliders = Physics.OverlapSphere(new Vector3((int)explosionBlock.transform.position.x, (int)explosionBlock.transform.position.y, -0.2f), 0.4f)).Length >= 1)
        {
            if (index == 1 && colliders.Length != 0)
                Debug.Log("work");
            //Debug.Log(colliders[0].name);
            if (colliders[0].GetComponent<Explosable>() != null)
            {
                colliders[0].GetComponent<Explosable>().canBeExploded = true;
                level[(int)explosionBlock.transform.position.x, (int)explosionBlock.transform.position.y] = -1;
                if (spwnInfo)
                    spwnInfoPos[index] = true;
                else spwnInfoPos[index] = false;
            }

            if (colliders[0].GetComponent<Base>() != null)
            {
                colliders[0].GetComponent<Base>().canDestory = true;
                //colliders[0].GetComponent<Explosable>().DestroyThisFCKNOBJ();//Destroy(colliders[0]);
                level[(int)explosionBlock.transform.position.x, (int)explosionBlock.transform.position.y] = -1;
                if (spwnInfo)
                    spwnInfoPos[index] = true;
                else spwnInfoPos[index] = false;
            }

            if (colliders[0].GetComponent<RAM>() != null)
            {
                colliders[0].GetComponent<Explosable>().DestroyThisFCKNOBJ();//Destroy(colliders[0]);
                level[(int)explosionBlock.transform.position.x, (int)explosionBlock.transform.position.y] = -1;
                if (spwnInfo)
                    spwnInfoPos[index] = true;
                else spwnInfoPos[index] = false;
            }

            if (colliders[0].GetComponent<SnikSnak>() != null)
            {
                Destroy(colliders[0]);
                level[(int)explosionBlock.transform.position.x, (int)explosionBlock.transform.position.y] = -1;
                if (spwnInfo)
                    spwnInfoPos[index] = true;
                else spwnInfoPos[index] = false;
            }

            if (colliders[0].GetComponent<Electron>() != null)
            {
                level[(int)explosionBlock.transform.position.x, (int)explosionBlock.transform.position.y] = -1;
                if (spwnInfo)
                    spwnInfoPos[index] = true;
                else spwnInfoPos[index] = false;
            }
            //Debug.Log("index :" + index + " " + spwnInfoPos[index]);
        }
    }
}
