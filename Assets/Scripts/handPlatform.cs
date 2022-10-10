using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class handPlatform : MonoBehaviour
{
    public GameObject hand;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var newPos = hand.transform.position;
        newPos.x += 1;
        newPos.y += 0.4f;
        newPos.z -= 0.4f;
        this.transform.position = newPos;

        var newRot = hand.transform.rotation;
        newRot.x -= 10;
        this.transform.rotation = newRot;
    }
}
