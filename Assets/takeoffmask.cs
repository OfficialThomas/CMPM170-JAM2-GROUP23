using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class takeoffmask : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "Player2")
        {
            GameObject originalGameObject = GameObject.Find("Player2");
            GameObject originalGameObject1 = GameObject.Find("first encounter");
            GameObject child = originalGameObject.transform.GetChild(1).gameObject;
            GameObject childer = child.transform.GetChild(0).gameObject;
            GameObject movingwall = originalGameObject1.transform.GetChild(7).gameObject;
            Debug.Log(childer.transform.position.y);
            childer.transform.localPosition = new Vector3(-0.11f, 10, 0.55f);
            movingwall.transform.localPosition = new Vector3(9.6f, 0.42f, -4.39f);
        }


    }
}