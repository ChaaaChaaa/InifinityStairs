using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static int LEFT_POSITION = 0;
    private static int START_INDEX = 0;
    private static int RIGHT_POSITION = 1;

    public GameObject character;
    public Transform platformParents;
    public GameObject platform;
    private List<GameObject> platformList = new List<GameObject>();
    private List<int> platformPositionCheckList = new List<int>();

    public bool gameStart = false;

    private int posIndex = 0;
    private int characterPositionIndex = 0;

    public float currentTime = 0.0f;
    public float destinationTime = 10.0f; //전체시간
    public Slider slider;
    public float addTimeFlow = 0.001f; //시간이 지날수록 점점 빠르게 줄어듦

    public Text scoreResultText;
    public int score;

    public PlayerController playerController;

    void Start()
    {
        DataLoad();
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStart)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                CheckPlatform(characterPositionIndex, RIGHT_POSITION);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                CheckPlatform(characterPositionIndex, LEFT_POSITION);
            }

            destinationTime = destinationTime - addTimeFlow; //매프레임마다 실행되므로 destination 전체 시간이 줄어들게 된다.
            currentTime = currentTime - Time.deltaTime; //프레임-프레임 사이의 시간만큼 currentTime을 계속 빼줘서 줄어든다.
            slider.value = currentTime / destinationTime; //흘러간 시간을 전체 시간에서 나누면 비율을 나타냄

            if (currentTime < 0f)
            {
                result();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Init();
            }

            else
            {
                playerController.Die();
            }
        }
    }

    public void DataLoad()
    {
        for (int i = 0; i < 20; i++)
        {
            GameObject newStair = Instantiate(platform, Vector3.zero, Quaternion.identity);
            //                             복제할 object, 복제된 objcet 위치, 복제된 project의 방향
            newStair.transform.parent = platformParents; //newStair의 위치가 PlatormParent의 자식으로 계속 생성이 된다.
            platformList.Add(newStair); //Platform의 요소로 붙게된다.
            platformPositionCheckList.Add(LEFT_POSITION); // 왼쪽이면 0, 오른쪽이면 1
        }

        platform.SetActive(false);
    }

    public void Init() //캐릭터, 발판 위치 초기화
    {
        character.transform.position = new Vector3(0f, -0.275f, 0);

        for (posIndex = 0; posIndex < 20;)
        {
            //Next_Platform(posIndex);
            NextPlatform(posIndex);
        }

        destinationTime = 10.0f;
        currentTime = destinationTime;

        characterPositionIndex = 0;

        score = 0;
        scoreResultText.text = score.ToString();
        //playerController.animator.Play("Walk",0,1000);
        playerController.InitPlayer();
        gameStart = true;
    }

    public void NextPlatform(int idx)
    {
        int pos = Random.Range(0, 2);

        if (idx == 0)
        {
            // 첫번째 발판의 경우 
            platformList[idx].transform.position = new Vector3(0, -0.5f, 0);
        }
        else
        {
            if (posIndex < 20)
            {
                if (pos == 0)
                {
                    // 왼쪽 발판일 경우 
                    platformPositionCheckList[idx - 1] = pos; //else일때 최소 1부터 이므로 첫번째 원소부터 넣기 위해 -1을 함
                    platformList[idx].transform.position =
                        platformList[idx - 1].transform.position + new Vector3(-1f, 0.5f, 0);
                }
                else
                {
                    // 오른쪽 발판일 경우 
                    platformPositionCheckList[idx - 1] = pos;
                    platformList[idx].transform.position =
                        platformList[idx - 1].transform.position + new Vector3(1f, 0.5f, 0);
                }
            }
            else
            {
                if (pos == LEFT_POSITION)
                {
                    if (posIndex % 20 == START_INDEX) // object pulling
                    {
                        platformPositionCheckList[20 - 1] = pos; //else일때 최소 1부터 이므로 첫번째 원소부터 넣기 위해 -1을 함
                        platformList[idx % 20].transform.position =
                            platformList[20 - 1].transform.position + new Vector3(-1f, 0.5f, 0);
                    }

                    else
                    {
                        platformPositionCheckList[idx % 20 - 1] = pos;
                        platformList[idx % 20].transform.position =
                            platformList[idx % 20 - 1].transform.position + new Vector3(-1f, 0.5f, 0);
                    }
                }
                else
                {
                    // 오른쪽 발판일 경우 
                    if (posIndex % 20 == 0)
                    {
                        platformPositionCheckList[20 - 1] = pos; //else일때 최소 1부터 이므로 첫번째 원소부터 넣기 위해 -1을 함
                        platformList[idx % 20].transform.position =
                            platformList[20 - 1].transform.position + new Vector3(1f, 0.5f, 0);
                    }

                    else
                    {
                        platformPositionCheckList[idx % 20 - 1] = pos; //else일때 최소 1부터 이므로 첫번째 원소부터 넣기 위해 -1을 함
                        platformList[idx % 20].transform.position =
                            platformList[idx % 20 - 1].transform.position + new Vector3(1f, 0.5f, 0);
                    }
                }
            }
        }

        score++;
        scoreResultText.text = score.ToString();
        posIndex++;
    }

    void CheckPlatform(int currentIndex, int direction)
    {
        //Debug.Log("currentIndex :"+currentIndex%20+" /Platform : "+platformPositionCheckList[currentIndex%20]+" /Direction : "+direction);
        if (platformPositionCheckList[currentIndex % 20] == direction) //캐릭터 이동 방향에 발판이 있음
        {
            characterPositionIndex++;
            //캐릭터의 위치를 성공한 발판 위에 올린다.
            character.transform.position =
                platformList[characterPositionIndex % 20].transform.position + new Vector3(0f, 0.2f, 0);

            NextPlatform(posIndex);
        }
        else
        {
            result();
        }
    }

    public void result()
    {
        playerController.Die();
        gameStart = false;
    }
}