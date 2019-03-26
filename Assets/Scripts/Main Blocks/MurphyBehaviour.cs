using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MurphyBehaviour : MonoBehaviour
{

    public GameObject player;//game object of player
    private int pMoveDir;//direction of player movement
    private bool moving;//new important
    private bool up, down, left, right;

    public int newX;
    public int newY;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindObjectOfType<Murphy>().gameObject;
        pMoveDir = -1;//new important
    }

    public void MurphyUpdate()
    {
        ListenNewInput();
        if (!moving)
            WhatIsOnMyWay();
        Move();
        WinLevel();
    }

    void ListenNewInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (Input.GetKeyUp(KeyCode.UpArrow))
                Eat(true, false, false, false);
            else if (Input.GetKeyUp(KeyCode.DownArrow))
                Eat(false, true, false, false);
            else if (Input.GetKeyUp(KeyCode.RightArrow))
                Eat(false, false, true, false);
            else if (Input.GetKeyUp(KeyCode.LeftArrow))
                Eat(false, false, false, true);
            return;
        }

        if(Input.GetKeyUp(KeyCode.Escape))
            GetComponent<LevelController>().Explosion((int)player.transform.position.x, (int)player.transform.position.y, false);

        if (Input.GetKey(KeyCode.UpArrow) && !moving)
        {
            pMoveDir = 0;
        }
        else if (Input.GetKey(KeyCode.DownArrow) && !moving)
        {
            pMoveDir = 1;
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && !moving)
        {
            pMoveDir = 2;
        }
        else if (Input.GetKey(KeyCode.RightArrow) && !moving)
        {
            pMoveDir = 3;
        }
    }

    void WhatIsOnMyWay()
    {
        player.transform.GetChild(0).transform.rotation = Quaternion.Euler(0, -180, 0);
        switch (pMoveDir)
        {
            case 0: //up
                {
                    if (GetComponent<LevelController>().level[(int)player.transform.position.x, (int)player.transform.position.y + 1] != 0 &&
                        GetComponent<LevelController>().level[(int)player.transform.position.x, (int)player.transform.position.y + 1] != 3 &&
                        GetComponent<LevelController>().level[(int)player.transform.position.x, (int)player.transform.position.y + 1] != 4 &&
                        GetComponent<LevelController>().level[(int)player.transform.position.x, (int)player.transform.position.y + 1] != 5)
                    {
                        if (GetComponent<LevelController>().level[(int)player.transform.position.x, (int)player.transform.position.y + 1] == 33)
                        {
                            Debug.Log(33);
                            GetComponent<LevelController>().Explosion(player.transform.position.x, player.transform.position.y, false);
                        }
                        player.transform.GetChild(0).transform.rotation = Quaternion.Euler(-90, 0, 0);
                        up = true;
                        moving = true;
                        newX = (int)player.transform.position.x;
                        newY = (int)player.transform.position.y + 1;
                        DetectBlock();
                        pMoveDir = -1;
                    }
                    break;
                }
            case 1: //down
                {
                    if (GetComponent<LevelController>().level[(int)player.transform.position.x, (int)player.transform.position.y - 1] != 0 &&
                        GetComponent<LevelController>().level[(int)player.transform.position.x, (int)player.transform.position.y - 1] != 3 &&
                        GetComponent<LevelController>().level[(int)player.transform.position.x, (int)player.transform.position.y - 1] != 4 &&
                        GetComponent<LevelController>().level[(int)player.transform.position.x, (int)player.transform.position.y - 1] != 5)
                    {
                        if (GetComponent<LevelController>().level[(int)player.transform.position.x, (int)player.transform.position.y - 1] == 33)
                        {
                            //Debug.Log(33);
                            GetComponent<LevelController>().Explosion(player.transform.position.x, player.transform.position.y, false);
                        }
                        player.transform.GetChild(0).transform.rotation = Quaternion.Euler(90, -180, 0);
                        down = true;
                        moving = true;
                        newX = (int)player.transform.position.x;
                        newY = (int)player.transform.position.y - 1;
                        DetectBlock();
                        pMoveDir = -1;
                    }
                    break;
                }
            case 2: //left
                {
                    if (GetComponent<LevelController>().level[(int)player.transform.position.x - 1, (int)player.transform.position.y] != 0 &&
                        GetComponent<LevelController>().level[(int)player.transform.position.x - 1, (int)player.transform.position.y] != 3 &&
                        GetComponent<LevelController>().level[(int)player.transform.position.x - 1, (int)player.transform.position.y] != 4 &&
                        GetComponent<LevelController>().level[(int)player.transform.position.x - 1, (int)player.transform.position.y] != 5)
                    {
                        if (GetComponent<LevelController>().level[(int)player.transform.position.x - 1, (int)player.transform.position.y] == 33)
                            GetComponent<LevelController>().Explosion(player.transform.position.x, player.transform.position.y, false);
                        player.transform.GetChild(0).transform.rotation = Quaternion.Euler(0, -90, 0);
                        left = true;
                        moving = true;
                        newX = (int)player.transform.position.x - 1;
                        newY = (int)player.transform.position.y;
                        DetectBlock();
                        pMoveDir = -1;
                    }
                    //push
                    else if(GetComponent<LevelController>().level[(int)player.transform.position.x - 1, (int)player.transform.position.y] == 3 && GetComponent<LevelController>().level[(int)player.transform.position.x - 2, (int)player.transform.position.y] == -1)
                    {
                        GetObjectOnPositionAsObj((int)player.transform.position.x - 1, (int)player.transform.position.y).GetComponent<Zonk>().pushL = true;
                    }
                    break;
                }
            case 3: //right
                {
                    if (GetComponent<LevelController>().level[(int)player.transform.position.x + 1, (int)player.transform.position.y] != 0 &&
                        GetComponent<LevelController>().level[(int)player.transform.position.x + 1, (int)player.transform.position.y] != 3 &&
                        GetComponent<LevelController>().level[(int)player.transform.position.x + 1, (int)player.transform.position.y] != 4 &&
                        GetComponent<LevelController>().level[(int)player.transform.position.x + 1, (int)player.transform.position.y] != 5)
                    {
                        if (GetComponent<LevelController>().level[(int)player.transform.position.x + 1, (int)player.transform.position.y] == 33)
                            GetComponent<LevelController>().Explosion(player.transform.position.x, player.transform.position.y, false);
                        else if (GetComponent<LevelController>().level[(int)player.transform.position.x + 1, (int)player.transform.position.y] == 3 && GetComponent<LevelController>().level[(int)player.transform.position.x + 2, (int)player.transform.position.y] == -1)
                            GetObjectOnPositionAsObj((int)player.transform.position.x + 1, (int)player.transform.position.y).GetComponent<Zonk>().pushR = true;
                        player.transform.GetChild(0).transform.rotation = Quaternion.Euler(0, -270, 0);
                        right = true;
                        moving = true;
                        newX = (int)player.transform.position.x + 1;
                        newY = (int)player.transform.position.y;
                        DetectBlock();
                        pMoveDir = -1;
                    }
                    //push
                    else if (GetComponent<LevelController>().level[(int)player.transform.position.x + 1, (int)player.transform.position.y] == 3 && GetComponent<LevelController>().level[(int)player.transform.position.x + 2, (int)player.transform.position.y] == -1)
                    {
                        GetObjectOnPositionAsObj((int)player.transform.position.x + 1, (int)player.transform.position.y).GetComponent<Zonk>().pushR = true;
                    }
                    break;
                }
        }
    }

    void Move()
    {
        if(up || down || right || left)
            player.GetComponent<Murphy>().AnimationEat();
        else player.GetComponent<Murphy>().AnimationIdle();
        if (up)
        {
            if (player.transform.position.y < newY)
                player.transform.Translate(Vector3.up * Time.deltaTime * player.GetComponent<Murphy>().speed);
            else
            {
                player.transform.position = new Vector3(newX, newY, 0);
                GetComponent<LevelController>().level[newX, newY] = -99; // from prev position
                GetComponent<LevelController>().level[newX, newY - 1] = -1; // from prev position
                up = false;
                moving = false;
            }
        }
        if (down)
        {
            if (player.transform.position.y > newY)
                player.transform.Translate(-Vector3.up * Time.deltaTime * player.GetComponent<Murphy>().speed);
            else
            {
                player.transform.position = new Vector3(newX, newY, 0);
                GetComponent<LevelController>().level[newX, newY] = -99; // from prev position
                GetComponent<LevelController>().level[newX, newY + 1] = -1; // from prev position
                down = false;
                moving = false;
            }
        }
        if (left)
        {
            if (player.transform.position.x > newX)
                player.transform.Translate(-Vector3.right * Time.deltaTime * player.GetComponent<Murphy>().speed);
            else
            {
                player.transform.position = new Vector3(newX, newY, 0);
                GetComponent<LevelController>().level[newX, newY] = -99; // from prev position
                GetComponent<LevelController>().level[newX + 1, newY] = -1; // from prev position
                left = false;
                moving = false;
            }
        }
        if (right)
        {
            if (player.transform.position.x < newX)
                player.transform.Translate(Vector3.right * Time.deltaTime * player.GetComponent<Murphy>().speed);
            else
            {
                player.transform.position = new Vector3(newX, newY, 0);
                GetComponent<LevelController>().level[newX, newY] = -99; // from prev position
                GetComponent<LevelController>().level[newX - 1, newY] = -1; // from prev position
                right = false;
                moving = false;
            }
        }
    }

    void Eat(bool up, bool down, bool right, bool left)
    {
        if(up)
        {
            if(GetComponent<LevelController>().level[(int)player.transform.position.x, (int)player.transform.position.y + 1] == 2 ||
               GetComponent<LevelController>().level[(int)player.transform.position.x, (int)player.transform.position.y + 1] == 9)
            {
                newX = (int)player.transform.position.x;
                newY = (int)player.transform.position.y + 1;
                DetectBlock();
                GetComponent<LevelController>().level[newX, newY] = -1;
            }
        }
        else if (down)
        {
            if (GetComponent<LevelController>().level[(int)player.transform.position.x, (int)player.transform.position.y - 1] == 2 ||
                GetComponent<LevelController>().level[(int)player.transform.position.x, (int)player.transform.position.y - 1] == 9)
            {
                newX = (int)player.transform.position.x;
                newY = (int)player.transform.position.y - 1;
                DetectBlock();
                GetComponent<LevelController>().level[newX, newY] = -1;
            }
        }
        else if (right)
        {
            //Debug.Log(GetComponent<LevelController>().level[(int)player.transform.position.x + 1, (int)player.transform.position.y]);
            if (GetComponent<LevelController>().level[(int)player.transform.position.x + 1, (int)player.transform.position.y] == 2 ||
                GetComponent<LevelController>().level[(int)player.transform.position.x + 1, (int)player.transform.position.y] == 9)
            {
                newX = (int)player.transform.position.x + 1;
                newY = (int)player.transform.position.y;
                DetectBlock();
                GetComponent<LevelController>().level[newX, newY] = -1;
            }
        }
        else if (left)
        {
            if (GetComponent<LevelController>().level[(int)player.transform.position.x - 1, (int)player.transform.position.y] == 2 ||
                GetComponent<LevelController>().level[(int)player.transform.position.x - 1, (int)player.transform.position.y] == 9)
            {
                newX = (int)player.transform.position.x - 1;
                newY = (int)player.transform.position.y;
                DetectBlock();
                GetComponent<LevelController>().level[newX, newY] = -1;
            }
        }
        newX = (int)player.transform.position.x;
        newY = (int)player.transform.position.y;
    }

    void DetectBlock()
    {
        //TODO  - при поедании печатной платы нету звукуа!!!
        //                    и у инфотрона тоже
        GameObject block = GetObjectOnPositionAsObj(newX, newY);
        if (GetComponent<LevelController>().level[newX, newY] != -1)
        {
            if (block.GetComponent<Base>() != null)
                block.GetComponent<Base>().canDestory = true;
            else if (block.GetComponent<Infotron>() != null)
                block.GetComponent<Infotron>().canBeDestroy = true;
        }
    }

    void GetObjectOnPosition(/*int x, int y*/float x, float y)
    {
        Collider[] colliders;
        if ((colliders = Physics.OverlapSphere(new Vector3(x, y, 0), 0.4f)).Length >= 1)
        {
            Debug.Log(colliders[0].gameObject.name + " of " + x + " " + y);
            //Debug.Log(colliders.Length);
            if (colliders[0].GetComponent<Base>() != null)
                colliders[0].GetComponent<Base>().canDestory = true;
            //return colliders[0].gameObject;
        }
        else Debug.Log(colliders.Length);
        //else return null;
    }

    GameObject GetObjectOnPositionAsObj(int x, int y)
    {
        Collider[] colliders;
        if ((colliders = Physics.OverlapSphere(new Vector3(x, y, -0.2f), 0.4f)).Length >= 1)
        {
            //Debug.Log("As obj: " + colliders[0].gameObject.name + " of " + x + " " + y);
            return colliders[0].gameObject;
        }
        else return null;
    }

    void WinLevel()
    {
        if(GetComponent<LevelController>().level[(int)player.transform.position.x, (int)player.transform.position.y] == 13)
        {
            SceneManager.LoadScene("Main Menu");
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        /*Gizmos.DrawWireSphere(new Vector3(toX, toY, -0.2f), 0.5f);*/
        Gizmos.DrawWireSphere(new Vector3(newX, newY, -0.2f), 0.4f);
    }
}
