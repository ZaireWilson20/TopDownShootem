using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int c = gameObject.transform.childCount;
        Debug.Log("Child count = " + c);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
