using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosable : MonoBehaviour
{
    public bool canBeExploded;

    public void DestroyThisFCKNOBJ()
    {
        Destroy(this.gameObject);
    }
}
