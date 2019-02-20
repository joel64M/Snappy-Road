using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarScript : MonoBehaviour {
    public float carSpeed=5f;
    public LayerMask roadMask;
 public   Rigidbody rb;
    public Vector3 rayOffset;
    public float sideForce = 5f;
    void Start () {
        rb = GetComponent<Rigidbody>();
     //   SetConstraints();
    
    }

   public float boost=1;
    public float boostSmooth=3f;
    public float accelerationSmooth = 3f;
    public float brakingSmooth = 3f;

    public float timer1=0;

    bool push;
    int forceSign;
    public bool wrongTile;
    void FixedUpdate () {

      //  Debug.DrawRay(transform.position + new Vector3(rayOffset.x, 0, -0.85f), Vector3.down, Color.red, 1);
        //Debug.DrawRay(transform.position + new Vector3(-rayOffset.x, 0, -0.85f), Vector3.down, Color.red, 1);

        if (GameManagerScript.instance.isGameStart && !GameManagerScript.instance.isGameOver)
        {   if(wrongTile)
            if (!Physics.Raycast(transform.position + new Vector3(rayOffset.x, 0, -0.85f), Vector3.down, 10, roadMask))
            {

                ReleaseConstraints();
                StartCoroutine(AddForce(1));
                StartCoroutine(WaitForGameOver());
                GameManagerScript.instance.GameOver();

            }
            if (wrongTile)
            if (!Physics.Raycast(transform.position+ new Vector3(-rayOffset.x , 0,-0.85f), Vector3.down, 10, roadMask))
            {

                ReleaseConstraints();
                StartCoroutine(AddForce(-1));
                StartCoroutine(WaitForGameOver());
                GameManagerScript.instance.GameOver();

            }
            if(!wrongTile)
            if (!Physics.Raycast(transform.position, Vector3.down, 10, roadMask))
            {
                ReleaseConstraints();
                if (!Physics.Raycast(transform.position + new Vector3(rayOffset.x, 0, -0.85f), Vector3.down, 10, roadMask))
                {

                    ReleaseConstraints();
                    StartCoroutine(AddForce(1));
                    StartCoroutine(WaitForGameOver());
                    GameManagerScript.instance.GameOver();

                }
                if (!Physics.Raycast(transform.position + new Vector3(-rayOffset.x, 0, -0.85f), Vector3.down, 10, roadMask))
                {

                    ReleaseConstraints();
                    StartCoroutine(AddForce(-1));
                    StartCoroutine(WaitForGameOver());
                    GameManagerScript.instance.GameOver();

                }



            }
            if (Vector3.Distance(transform.position, GameManagerScript.instance.currentPos) > 15 || wrongTile)
            {
                // if (timer1>=0 && timer1 <= 1f)
                {
                    timer1 += Time.deltaTime *accelerationSmooth;
                }
            }
            else 
            {
                //  if (timer1 >= 0 && timer1 <= 1f)
                {
                    timer1 -= Time.deltaTime * brakingSmooth;
                    //   boost = Mathf.Lerp(1, 6, timer1);
                }
            }
            timer1 = Mathf.Clamp01(timer1);
            boost = Mathf.Lerp(1, 4, timer1);

            rb.velocity = Vector3.forward * carSpeed * boost;
        }

       

    
    
    }
    IEnumerator WaitForGameOver()
    {
        yield return new WaitForSeconds(1f);
        UIManagerScript.instance.GameOver();
    }
    Vector3 dir = Vector3.zero;
    IEnumerator AddForce(int i)
    {
        float t=0;
        boost = 1f;
      
        dir.x = 1.75f *i;
        dir.y = -1;
        while (t <=2f )
        {
            // print("adding force"); 
            rb.velocity = Vector3.forward * carSpeed * boost;
            t += Time.deltaTime;
            rb.AddForce(dir* sideForce *t);
        }
        boost = 0;
       yield return null;
    }
     void SetConstraints()
    {
     
        rb.constraints = RigidbodyConstraints.FreezePositionX |RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    }

    void ReleaseConstraints()
    {
        rb.constraints = RigidbodyConstraints.None;
     //   rb.velocity = Vector3.zero;
    }
}
