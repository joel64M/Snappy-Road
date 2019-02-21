using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameAnalyticsSDK;
public class GameManagerScript : MonoBehaviour {
    public static GameManagerScript instance;

    public bool isGameStart, isGameOver;
    public int score;
    public int multiplier=1;
    
    public GameObject platformPrefab;
    public GameObject cloudPrefab;
    public GameObject perfectPrefab;

    public Material glow;
    public  GameObject[] platformArray;
    public GameObject[] cloudArray;
    public GameObject[] perfectArray;
    public TextMesh[] textMeshArray;

    public AudioSource ads;
    public AudioClip fallClip;
    public float correctRange=1;
    public float swingSpeed = 2f;
 public   Vector3 currentPos = Vector3.zero;
    public Vector3 currentCloudPos = new Vector3(0,0,-8);
    Vector3 tempCloudPos = Vector3.zero;
    Vector3 randRot = Vector3.zero;
    public  int platformIndex = -1;
    public int cloudIndex = -1;
    public int perfectIndex = -1;

    float movement = 0;

    float glowStrength = 0;
    float glowTimer;
    float s;

    public GameObject player;
    private void Awake()
    {
        instance = this;

    }
    private void Start()
    {

        for (int i = 0; i < platformArray.Length; i++)
        {
            platformArray[i] = Instantiate(platformPrefab ,new Vector3(1,0,0),Quaternion.identity);
            platformArray[i].SetActive(false);
            platformArray[i].name ="Platform " +(i).ToString();
        }
        for (int i = 0; i < cloudArray.Length; i++)
        {
            cloudArray[i] = Instantiate(cloudPrefab);
            cloudArray[i].SetActive(false);

        }
        for (int i = 0; i < perfectArray.Length; i++)
        {
            perfectArray[i] = Instantiate(perfectPrefab);
            perfectArray[i].SetActive(false);
            textMeshArray[i] =  perfectArray[i].gameObject.GetComponentInChildren<TextMesh>();

        }
        for (int i = 0; i < 6; i++)
        {
            PlaceCloud();
        }

    }
    int ind;
    private void Update()
    {
        if(isGameStart && !isGameOver)
        {
            if (Input.GetMouseButtonDown(0))
            {
                PlayTapSound();
                ind = platformIndex;
                if (ind == -1)
                {
                    ind = platformArray.Length - 1;
                }
                if (!player.GetComponent<CarScript>().wrongTile  && platformArray[ind].transform.position.x >  - correctRange && platformArray[ind].transform.position.x < correctRange)
                {

                    NextPlatform();
                    placeCloud++;
                    if(placeCloud>=2)
                    PlaceCloud();
                    score = score+ 1+ multiplier;
                    UIManagerScript.instance.UpdateScore(score);
                }
              else
                {
                    // isGameOver = true;
                  player.GetComponent<CarScript>().wrongTile = true;
                   
                }
            }
            if(!player.GetComponent<CarScript>().wrongTile)
            MovePlatform();

       
        }
        s -= Time.deltaTime *0.7f;

        glowTimer = Mathf.Clamp01(s);
      //  if (s >= 1)
           // glowTimer = -1;
        glowStrength = Mathf.Lerp(0, 1, glowTimer);
        glow.SetFloat("_MKGlowTexStrength", glowStrength);
    }
    void CalculateDistance()
    {
      //  score = Vector3.Distance(startPos, player.transform.position);
        UIManagerScript.instance.UpdateScore(score);
    }

    void PlayTapSound()
    {
        ads.Play();
    }
    Vector3 vec = Vector3.zero;
    void MovePlatform()
    {
      swingSpeed += Time.deltaTime*0.04f;
      movement += Time.deltaTime *swingSpeed;
        vec.x = Mathf.Sin(movement) * 5f;
        vec.z = currentPos.z;
        platformArray[platformIndex].transform.position = vec;// new Vector3(Mathf.Sin(movement) * 5f, 0, currentPos.z);
    }
    public void StartGame()
    {
        isGameStart = true;
        isGameOver = false;
        score = 0;
        // InvokeRepeating("CalculateDistance", 0, 0.3f);
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "Game Started");

    }

    public void GameOver()
    {
        isGameStart = false;
        isGameOver = true;
        //   CancelInvoke("CalculateDistance");
        ads.clip = fallClip;
        Invoke("PlayTapSound", 0.25f);
        if(score > PlayerPrefs.GetInt("SCORE", 0))
        {
            PlayerPrefs.SetInt("SCORE", score);
        }
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "Score " + score.ToString());

    }


    void Perfect()
    {
        perfectIndex++;

        if (perfectIndex > perfectArray.Length - 1)
        {
            perfectIndex = 0;
        }
        perfectArray[perfectIndex].SetActive(false);
        perfectArray[perfectIndex].transform.position = currentPos;
        textMeshArray[perfectIndex].text = "+" + multiplier.ToString();
        perfectArray[perfectIndex].SetActive(true);

    }
    Vector3 perfectVec = Vector3.zero;

    public void  NextPlatform()
    {

  
        currentPos.z += 4;
        platformIndex++;
        int lastPlatformIndex = platformIndex-1;
        if (lastPlatformIndex < 0)
        {
            lastPlatformIndex =  platformArray.Length-1;
        }
        if (platformIndex > platformArray.Length - 1)
        {
            platformIndex = 0;
        }
        platformArray[platformIndex].transform.position = currentPos;
        if (platformArray[lastPlatformIndex].transform.position.x <= 0.25f && platformArray[lastPlatformIndex].transform.position.x >= -0.25f)
        {
            perfectVec.z = platformArray[lastPlatformIndex].transform.position.z;
            platformArray[lastPlatformIndex].transform.position = perfectVec;// new Vector3(0, 0, );
           // glowTimer = 1;
            s = 1;
              //   print(platformArray[lastPlatformIndex].transform.name);
              multiplier++;
            Perfect();
        }
        else
        {
          //  glowTimer = -1;
            multiplier = 0;
        }
   
        platformArray[platformIndex].SetActive(true);

      

    }
    int placeCloud;
    void PlaceCloud()
    {
        placeCloud = 0;
        float xx = Random.Range(0f, 1f); ;
        currentCloudPos.x = xx>0.5f? Random.Range(-6f, -2.5f) : Random.Range(2.5f, 6f);
        currentCloudPos.y = -1.4f;
        currentCloudPos.z += 8;
        NextCloudIndex(0);
        currentCloudPos.x = xx < 0.5f ? Random.Range(-6f, -2.5f) : Random.Range(2.5f, 6f);
        currentCloudPos.y = -4.4f;
        NextCloudIndex(0);

    }

    void NextCloudIndex(int i)
    {
        cloudIndex++;

        if (cloudIndex > cloudArray.Length - 1)
        {
            cloudIndex = 0;
        }
        tempCloudPos  = currentCloudPos;
        tempCloudPos.z += i;
        randRot.y = Random.Range(1, 4) * 90;
        randRot.x = Random.Range(1,3) * 180;
       // randRot.z = Random.Range(1, 4) * 90;
        cloudArray[cloudIndex].transform.position = tempCloudPos;
        cloudArray[cloudIndex].transform.rotation = Quaternion.Euler(randRot);
        cloudArray[cloudIndex].SetActive(true);
    }
}
