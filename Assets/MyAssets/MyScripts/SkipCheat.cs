using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipCheat : MonoBehaviour
{
    public Transform spawnPoint1;
    public Transform spawnPoint2;
    public Transform spawnPoint3;
    public Transform spawnPoint4;
    public Transform spawnPoint5;
    public Transform spawnPoint6;
    public ArrayList spawnArray = new ArrayList();

    public Transform currentSpawn;
    public int index = 0;

    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        spawnArray.Add(spawnPoint1);
        spawnArray.Add(spawnPoint2);
        spawnArray.Add(spawnPoint3);
        spawnArray.Add(spawnPoint4);
        spawnArray.Add(spawnPoint5);
        spawnArray.Add(spawnPoint6);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H)){
            if (index < 5){
                index++;
                currentSpawn = (Transform) spawnArray[index];
                player.transform.position = currentSpawn.position;
            }
        }
    }
}
