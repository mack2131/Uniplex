/**********************************************/
/* Скрипт, движение частиц инфотрона     
 * 
 * Создан Белым Койотом
/**********************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfotronParticlesMove : MonoBehaviour {

    public int spereIndex;

	// Use this for initialization
	void Start () 
    {
		
	}
	
	// Update is called once per frame
	void Update () 
    {
        switch (spereIndex)//двигаем каждую из трех сфер инфотрона по-разному, в разном направлении
        {
            case 0:
                {
                    transform.RotateAround(transform.parent.transform.position, Vector3.up, Time.deltaTime * 10.0f);
                    transform.RotateAround(transform.parent.transform.position, Vector3.right, Time.deltaTime * 10.0f);
                    break;
                }
            case 1:
                {
                    transform.RotateAround(transform.parent.transform.position, -Vector3.up, Time.deltaTime * 10.0f);
                    transform.RotateAround(transform.parent.transform.position, Vector3.right, Time.deltaTime * 10.0f);
                    break;
                }
            case 2:
                {
                    transform.RotateAround(transform.parent.transform.position, Vector3.up, Time.deltaTime * 10.0f);
                    transform.RotateAround(transform.parent.transform.position, -Vector3.right, Time.deltaTime * 10.0f);
                    break;
                }
        }
	}
}
