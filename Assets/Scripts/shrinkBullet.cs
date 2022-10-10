using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shrinkBullet : MonoBehaviour
{
    bool hasHit = false;
    bool startLerp = false;
    float lerp = 0;
    GameObject lerpObject;
    Vector3 scale1, scale2;
    float Timer = 0;

    public Material transparentMat;
    Vector3 referencePosition;
   // public growShrinkEffect GSE;
    // Start is called before the first frame update
    void Start()
    {
        if(this.gameObject.name == "shotPlayer")
        {
            referencePosition = GameObject.Find("Player").transform.forward;
        }
        if(this.gameObject.name == "shotPlayer2")
        {
            referencePosition = GameObject.Find("Player2").transform.forward;
        }

        this.GetComponent<Rigidbody>().AddForce(referencePosition * 5);
    }

    // Update is called once per frame
    void Update()
    {
        if(hasHit == false)
        {
            this.GetComponent<Rigidbody>().AddForce(referencePosition * 5);
        }

        if (startLerp)
        {
            if (lerp == 0)
            {
                lerp = 0.01f;
                Timer = 0.07f;
                scale1 = lerpObject.transform.localScale;
                scale2 = new Vector3(lerpObject.transform.localScale.x * 0.25f, lerpObject.transform.localScale.y * 0.25f, lerpObject.transform.localScale.z * 0.25f);
            }
            if (lerp < 1)
            {
                lerpObject.transform.localScale = Vector3.Lerp(scale1, scale2, lerp);

                if (lerp > Timer)
                {
                    GameObject duplicate = Instantiate(lerpObject);
                    duplicate.transform.parent = null;
                    growShrinkEffect GSEScript = duplicate.AddComponent<growShrinkEffect>();
                    if (duplicate.GetComponent<Renderer>() != null)
                    {
                        duplicate.GetComponent<MeshRenderer>().material = transparentMat;
                    }

                    if(duplicate.transform.gameObject.name == "penis(Clone)")
                    {
                        duplicate.transform.GetChild(0).GetComponent<MeshRenderer>().material = transparentMat;
                        duplicate.transform.GetChild(1).GetComponent<MeshRenderer>().material = transparentMat;
                        duplicate.transform.GetChild(2).GetComponent<MeshRenderer>().material = transparentMat;
                    }
                    // duplicate.transform.GetChild(0).GetComponent<MeshRenderer>().material = transparentMat;
                  //  duplicate.transform.GetChild(0).transform.GetChild(1).GetComponent<Renderer>().material = transparentMat;
                    Destroy(duplicate, 0.2f);

                    Timer = Timer + 0.07f;
                }

                Timer = Timer + (Time.deltaTime);
                lerp = lerp + (3 * Time.deltaTime);
            }
            if (lerp > 1)
            {
                lerpObject.transform.localScale = scale2;

                Destroy(this.gameObject);
            }
        }

    }

    void OnCollisionEnter(Collision col)
    {
        if (col.transform.gameObject.name == "Player" || col.transform.gameObject.name == "Player2")
        {
            Destroy(this.gameObject);
        }
        else if(hasHit == false)
        {
            hasHit = true;
            startLerp = true;
            this.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            lerpObject = col.transform.gameObject;
            this.GetComponent<TrailRenderer>().enabled = false;
            
        }

    }
}
