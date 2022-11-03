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
            float new_y = origPos.y - Mathf.PingPong(Time.time * 0.3f, 1f) * 7.5f;
            myObject.transform.position = new Vector3(origPos.x, new_y, origPos.z);
        }
        else if (myObject.CompareTag("BedHoriz")){
            float new_x = origPos.x + Mathf.PingPong(Time.time * 0.2f, 1f) * 6f;
            myObject.transform.position = new Vector3(new_x, origPos.y, origPos.z);
        }
        else if (myObject.CompareTag("ChairSpin")){
            myObject.transform.Rotate(0, 25 * Time.deltaTime, 0);
        }
        else if (myObject.CompareTag("FridgeSpin")){
            myObject.transform.Rotate(0, 10 * Time.deltaTime, 0);
        }
        else if (myObject.CompareTag("EndBed")){
            float new_y = origPos.y - Mathf.PingPong(Time.time * 0.4f, 1f) * 5f;
            myObject.transform.position = new Vector3(origPos.x, new_y, origPos.z);
        }
    }
}
