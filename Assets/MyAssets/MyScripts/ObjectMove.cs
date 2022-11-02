using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMove : MonoBehaviour
{
    public GameObject myObject;
    private Vector3 origPos;
    
    // Start is called before the first frame update
    void Start()
    {
        origPos = myObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (myObject.CompareTag("BedVert")){
            // if (myObject.transform.position.y >= origPos.y - 7.5f){
            //     myObject.transform.Translate(Vector3.down * 2f * Time.deltaTime);
            // }
            // else if (myObject.transform.position.y < origPos.y){
            //     myObject.transform.Translate(Vector3.up * 2f * Time.deltaTime);
            // }
            float new_y = origPos.y - Mathf.PingPong(Time.time * 0.3f, 1f) * 7.5f;
            myObject.transform.position = new Vector3(origPos.x, new_y, origPos.z);
        }
    }
}
