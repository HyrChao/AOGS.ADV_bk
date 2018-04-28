//2018-04-28 14:10:51
//By Chao

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotate : MonoBehaviour {

    public bool EnableRotate = true;
    public bool AxisRandom = false;
    public float rotateSpeed = 1f;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (EnableRotate)
        {            
            Vector3 randomVector = Vector3.zero;
            if (AxisRandom)
            {
                float random = Random.Range(0f, 5f);
                float randomX = Random.Range(0f, random);
                float randomY = Random.Range(0f, random);
                float randomZ = Random.Range(0f, random);
                randomVector = new Vector3(randomX, randomY, randomZ);
            }
            else
            {
                float random = Random.Range(1f, 1.5f);
                randomVector = new Vector3(random, random, random);
            }

            //Vector3 rotation = transform.rotation.eulerAngles;
            //rotation += (randomVector * Time.deltaTime * rotateSpeed);
            transform.Rotate(randomVector*Time.deltaTime* rotateSpeed*20f);
        }



	}
}
