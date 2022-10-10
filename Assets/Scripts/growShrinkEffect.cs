using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class growShrinkEffect : MonoBehaviour
{
    Color color;
    public Material material, material2;
    float dissolvePos;
    public static int counter = 0;

    public GameObject playerGameObject;
    // Start is called before the first frame update
    void Start()
    {
        
        if (this.transform.gameObject.name == "Player(Clone)")
        {
            this.transform.GetChild(0).transform.GetChild(1).GetComponent<Renderer>().material = material;
            dissolvePos = this.transform.GetChild(0).transform.GetChild(1).GetComponent<Renderer>().material.GetFloat("Vector1_76ead27140c74e10a3d759c635215783");
        }
        else if(this.transform.gameObject.name == "Player2(Clone)")
        {
            this.transform.GetChild(0).transform.GetChild(1).GetComponent<Renderer>().material = material2;
            dissolvePos = this.transform.GetChild(0).transform.GetChild(1).GetComponent<Renderer>().material.GetFloat("Vector1_76ead27140c74e10a3d759c635215783");
        }
        else if(this.transform.gameObject.name == "penis(Clone)")
        {
            dissolvePos = this.transform.GetChild(0).GetComponent<Renderer>().material.GetFloat("Vector1_76ead27140c74e10a3d759c635215783");
        }
        else
        {
            dissolvePos = this.GetComponent<Renderer>().material.GetFloat("Vector1_76ead27140c74e10a3d759c635215783");
        }
        

        //  Debug.Log(transform.hierarchyCount);
        //   Debug.Log(transform.GetChild(15).gameObject.name);

        playerGameObject = GameObject.Find("Player");
     //   copyChildren(this.transform.gameObject, playerGameObject);
    }

    // Update is called once per frame
    void Update()
    {
        dissolvePos = dissolvePos - 0.006f;

        if(this.transform.gameObject.name == "Player(Clone)" || this.transform.gameObject.name == "Player2(Clone)")
        {
            this.transform.GetChild(0).transform.GetChild(1).GetComponent<Renderer>().material.SetFloat("Vector1_76ead27140c74e10a3d759c635215783", dissolvePos);
        }
        else
        {
            if (this.transform.gameObject.name == "penis(Clone)")
            {
                this.transform.GetChild(0).GetComponent<MeshRenderer>().material.SetFloat("Vector1_76ead27140c74e10a3d759c635215783", dissolvePos);
                this.transform.GetChild(1).GetComponent<MeshRenderer>().material.SetFloat("Vector1_76ead27140c74e10a3d759c635215783", dissolvePos);
                this.transform.GetChild(2).GetComponent<MeshRenderer>().material.SetFloat("Vector1_76ead27140c74e10a3d759c635215783", dissolvePos);
            }

            else if (this.GetComponent<Renderer>() != null)
            {
                this.GetComponent<Renderer>().material.SetFloat("Vector1_76ead27140c74e10a3d759c635215783", dissolvePos);
            }


            
        }

    }

    private static Transform findObject(string name, Transform player)
    {
        foreach (Transform child in player)
        {
            if(child.name == name)
            {
                return child;
            }
            else
            {
                Transform found = findObject(name, child);
                if(found != null)
                {
                    return found;
                }
            }
        }

        return null;
    }
    
    private static void copyChildren(GameObject obj, GameObject player)
    {
        for (int i = 0; i < obj.transform.childCount; i++)
        {
        //    Debug.Log("name " + obj.transform.gameObject.name + " I " + i + " count " + obj.transform.childCount);
           // Debug.Log("name: " + obj.transform.GetChild(i).transform.gameObject.name);
            if(obj.transform.GetChild(i).childCount > 0)
            {
             //   Debug.Log("total " + obj.transform.GetChild(i).childCount);
                for(int x = 0; x < obj.transform.GetChild(i).childCount; x++)
                {
                   // Debug.Log(x);
                    copyChildren(obj.transform.GetChild(i).transform.GetChild(x).transform.gameObject, player);
                }
                // obj.transform.GetChild(i).transform.position = player.transform.Find(obj.transform.GetChild(i).transform.gameObject.name).transform.position;
                //  obj.transform.GetChild(i).transform.rotation = player.transform.Find(obj.transform.GetChild(i).transform.gameObject.name).transform.rotation;

                 obj.transform.GetChild(i).transform.position = findObject(obj.transform.GetChild(i).transform.gameObject.name, player.gameObject.transform).position;
                 obj.transform.GetChild(i).transform.rotation = findObject(obj.transform.GetChild(i).transform.gameObject.name, player.gameObject.transform).rotation;


            }
            else
            {
                counter++;
                //     Debug.Log(counter);

                //  Debug.Log(obj.transform.GetChild(i).transform.gameObject.name);
                //    Debug.Log(player.transform.Find(obj.transform.GetChild(i).transform.gameObject.name).transform.gameObject.name);

                //   obj.transform.GetChild(i).transform.position = player.transform.Find(obj.transform.GetChild(i).transform.gameObject.name).transform.position;
                //   obj.transform.GetChild(i).transform.rotation = player.transform.Find(obj.transform.GetChild(i).transform.gameObject.name).transform.rotation;

                obj.transform.GetChild(i).transform.position = findObject(obj.transform.GetChild(i).transform.gameObject.name, player.gameObject.transform).position;
                obj.transform.GetChild(i).transform.rotation = findObject(obj.transform.GetChild(i).transform.gameObject.name, player.gameObject.transform).rotation;

            }
        }
    }
}
