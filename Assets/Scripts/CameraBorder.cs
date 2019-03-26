using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBorder : MonoBehaviour {

    public GameObject target;

    public float interpVelocity;
    public Vector3 offset;
    private Vector3 targetPos;

	// Use this for initialization
	void Start () 
    {
        //targetPos = transform.position;
        offset.z = -0.75f;

        target = GameObject.FindObjectOfType<Murphy>().gameObject;
        targetPos = transform.position;
        transform.position = new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z);
    }
	
	// Update is called once per frame
	void Update () 
    {

	}

    void LateUpdate()
    {
        /*if (target != null)
            transform.position = new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z);*/
        /*if (target != null)
        {
            Vector3 posNoZ = transform.position;
            posNoZ.z = transform.position.z;
            Vector3 targetDirection = target.transform.position - posNoZ;
            interpVelocity = targetDirection.magnitude * 5f;
            targetPos = transform.position + (targetDirection.normalized * interpVelocity * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, targetPos + offset, 0.25f);
        }*/
        if (target != null)
            transform.position = new Vector3(target.transform.position.x, target.transform.position.y, -10);
    }


}
