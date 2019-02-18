using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour {
    public static GameManagerScript instance;

    public bool isGameStart, isGameOver;
    public float score;
    
    public GameObject platformPrefab;
    public GameObject cloudPrefab;
    public GameObject perfectPrefab;

    public  GameObject[] platformArray;
    public GameObject[] cloudArray;
    public GameObject[] perfectArray;

    public float correctRange=1;
    public float swingSpeed = 2f;
 public   Vector3 currentPos = Vector3.zero;
    public Vector3 currentCloudPos = new Vector3(0,0,-8);
    public  int platformIndex = -1;
    public int cloudIndex = -1;
    public int perfectIndex = -1;

    float movement = 0;

    Vector3 startPos = new Vector3(0, 0, -6.25f);

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

        }
        for (int i = 0; i < 4; i++)
        {
            PlaceCloud();
        }
    }

    private void Update()
    {
        if(isGameStart && !isGameOver)
        {
            if (Input.GetMouseButtonDown(0))
            {

                int ind = platformIndex;
                if (ind == -1)
                {
                    ind = platformArray.Length - 1;
                }
                if (!player.GetComponent<CarScript>().wrongTile  && platformArray[ind].transform.position.x >  - correctRange && platformArray[ind].transform.position.x < correctRange)
                {
                    NextPlatform();
                    PlaceCloud();
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
      
    }
    void CalculateDistance()
    {
        score = Vector3.Distance(startPos, player.transform.position);
        UIManagerScript.instance.UpdateScore(score);
    }
    void PlaceCloud()
    {
        currentCloudPos.x =Random.Range(0f,1f)> 0.5 ? Random.Range(-5f, -2f) : Random.Range(2, 5f);
        currentCloudPos.z += 8;
        currentCloudPos.y= -1.4f;
        cloudIndex++;

        if (cloudIndex > cloudArray.Length - 1)
        {
            cloudIndex = 0;
        }
        cloudArray[cloudIndex].transform.position = currentCloudPos;
        cloudArray[cloudIndex].SetActive(true);
    }
    void MovePlatform()
    {
      swingSpeed += Time.deltaTime*0.04f;
      movement += Time.deltaTime *swingSpeed;
        platformArray[platformIndex].transform.position = new Vector3(Mathf.Sin(movement) * 5f, 0, currentPos.z);
    }
    public void StartGame()
    {
        isGameStart = true;
        isGameOver = false;
        score = 0;
        InvokeRepeating("CalculateDistance", 0, 0.3f);
       
    }

    public void GameOver()
    {
        isGameStart = false;
        isGameOver = true;
        CancelInvoke("CalculateDistance");
       
        if(score > PlayerPrefs.GetFloat("SCORE", 0f))
        {
            PlayerPrefs.SetFloat("SCORE", score);
        }
      
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
        perfectArray[perfectIndex].SetActive(true);

    }
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
        if (platformArray[lastPlatformIndex].transform.position.x <= 0.2f && platformArray[lastPlatformIndex].transform.position.x >= -0.2f)
        {
            platformArray[lastPlatformIndex].transform.position = new Vector3(0, 0, platformArray[lastPlatformIndex].transform.position.z);
            Perfect();
          //  print(platformArray[lastPlatformIndex].transform.name);
        }
        platformArray[platformIndex].SetActive(true);

      

    }

}
