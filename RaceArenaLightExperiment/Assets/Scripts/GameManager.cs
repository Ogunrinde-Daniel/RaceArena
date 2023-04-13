using Cinemachine;

using System.Collections.Generic;

using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool gameOver = true;
    public Color hexagonRespawnColor;
    //variables
    private int noOfNpcs = 3;
    private List<GameObject> npcList;
    private List<string> npcNameList;
    private List<string> winpositions;
    private List<Vector3> spawnPositions;
    private List<Vector3> spawnRotation;
    //managers
    private SoundManager soundManager;
    //cars
    //public GameObject player;
    public List<GameObject> playerCars;
    public List<GameObject> npcPrefablist;
    //cameras
    public CinemachineVirtualCamera camera1;
    public CinemachineVirtualCamera camera2;
    public CinemachineVirtualCamera playerCamera;
    //menu screen
    public Toggle lightToggle;
    public Toggle soundTogggle;
    public Button restartButton;
    public GameObject MenuScreen;
    public TextMeshProUGUI xpText;
    public TextMeshProUGUI positionsText;
    //Light
    public GameObject directionalLight;
    public GameObject pointLight;
    //menu slider
    //public Button left;
    //public Button right;
    public GameObject winConfetti;
    public List<GameObject> carImages;
    private int currentcarImage = 0;


    public void swipeLeft()
    {
        currentcarImage  = Mathf.Max(currentcarImage - 1, 0);
        Debug.Log(currentcarImage);
        for (int i = 0; i < carImages.Count; i++)
        {
            carImages[i].SetActive(false);
            if (i == currentcarImage)
            {
                carImages[i].SetActive(true);
            }
        }
    }

    
    public void swipeRight()
    {
        currentcarImage = Mathf.Min(currentcarImage + 1, carImages.Count - 1);
        for (int i = 0; i < carImages.Count; i++)
        {
            carImages[i].SetActive(false);
            if (i == currentcarImage)
            {
                carImages[i].SetActive(true);
            }
        }
    }

    void setPlayer()
    {
        for (int i = 0; i < playerCars.Count; i++)
        {
            if (i == PlayerInfo.currentCar)
            {
                playerCars[i].SetActive(true);
                playerCamera.Follow = playerCars[i].transform;
                playerCamera.LookAt = playerCars[i].transform;
            }
            else
            {
                playerCars[i].SetActive(false);

            }

        }
    }

    private void Awake()
    {
        var randomNo = Random.Range(0,2);
        if (randomNo == 0)
        {
            hexagonRespawnColor = new Color(0, 0, 0);
        }
        else
        {
            hexagonRespawnColor = new Color(1, 1, 1);   //white
        }

        setPlayer();
        FindObjectOfType<MapGenerator>().ResetMap();
    }


    void Start()
    {
        directionalLight.SetActive(PlayerInfo.lightOn);
        pointLight.SetActive(!PlayerInfo.lightOn);

        winpositions = new List<string>();
        populatePositions();
        populateNameList();

        //player.transform.position = spawnPositions[0];
        //player.transform.eulerAngles = spawnRotation[0];

        playerCars[PlayerInfo.currentCar].transform.position = spawnPositions[0];
        playerCars[PlayerInfo.currentCar].transform.eulerAngles = spawnRotation[0];

        spawnPositions.RemoveAt(0);
        spawnRotation.RemoveAt(0);

        for (int i = 0; i < noOfNpcs; i++)
        {
            var randomPos = Random.Range(0, spawnPositions.Count);
            npcList.Add(Instantiate(npcPrefablist[i], spawnPositions[randomPos], Quaternion.identity));
            npcList[i].transform.eulerAngles = spawnRotation[randomPos];
            npcList[i].GetComponent<CharacterController>().characterName = npcNameList[randomPos];

            spawnPositions.RemoveAt(randomPos);
            spawnRotation.RemoveAt(randomPos);
            npcNameList.RemoveAt(randomPos);
        }


        Invoke("switchToCamera2", 0.2f);

        //startEngine
        soundManager = FindObjectOfType<SoundManager>();
        soundManager.revEngine();
        soundManager.startEngine();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (gameOver)
            return;
        bool allNpcDead = true;
        for (int i = 0; i < noOfNpcs; i++)
        {
            if (npcList[i] != null)
            {
                allNpcDead = false;
            }
            
        }

        if (allNpcDead && playerCars[PlayerInfo.currentCar] != null)
        {
            gameOver = true;
            playerWin();
        }
        if (playerCars[PlayerInfo.currentCar] == null)
        {
            //gameOver = true;
            playerLost();
        }

    }

    void playerWin()
    {
        soundManager.revEngine();
        registerPosition(PlayerInfo.playerName);
        PlayerInfo.xp = Mathf.Min(PlayerInfo.xp + PlayerInfo.xpIncreament, PlayerInfo.maxXp);
        winConfetti.transform.position = playerCars[PlayerInfo.currentCar].transform.position;
        if(!winConfetti.activeSelf)winConfetti.SetActive(true);
        Invoke(nameof(showMenuScreen), 3);
        //showMenuScreen();
    }

    void playerLost()
    {
        for (int i = 0; i < noOfNpcs; i++)
        {
            if (npcList[i] != null)
            {
                registerPosition(".....");
            }

        }
        camera1.Priority = playerCamera.Priority + 1;
        showMenuScreen();
    }

    public void registerPosition(string name)
    {
        winpositions.Add(name);
    }

    void showMenuScreen()
    {
        if (MenuScreen.activeSelf)
        {
            return;
        }
        winConfetti.SetActive(false);
        lightToggle.isOn = PlayerInfo.lightOn;
        soundTogggle.isOn = PlayerInfo.soundOn;
        string positionInfo = "";
        for (int i = winpositions.Count - 1; i >= 0; i--)
        {
            positionInfo += (winpositions.Count - i) + ": " + winpositions[i] + "\n";
        }
        /*
        for (int i = 0; i < winpositions.Count; i++)
        {
            positionInfo += (winpositions.Count - i) + ": " + winpositions[i] + "\n";
        }*/

        xpText.text = "XP: " + PlayerInfo.xp.ToString();
        positionsText.text = positionInfo;
        MenuScreen.SetActive(true);
        
    }

    void startGame()
    {
        gameOver = false;
    }

    public void toggleLight()
    {
        PlayerInfo.lightOn = lightToggle.isOn;
        directionalLight.SetActive(PlayerInfo.lightOn);
        pointLight.SetActive(!PlayerInfo.lightOn);


    }

    public void toggleSound()
    {
        soundManager.toggleAllSound(soundTogggle.isOn);
    }

    public void restartLevel()
    {
        switch (currentcarImage)
        {
            case 0:
                PlayerInfo.currentCar = 0;
                break;
            case 1:
                if (PlayerInfo.xp >= 500)
                {
                    PlayerInfo.currentCar = 1;
                }
                else
                {
                    PlayerInfo.currentCar = 0;
                }
                break;
            case 2:
                if (PlayerInfo.xp >= 1000)
                {
                    PlayerInfo.currentCar = 2;
                }
                else
                {
                    PlayerInfo.currentCar = 0;
                }
                break;
            case 3:
                if (PlayerInfo.xp >= 1500)
                {
                    PlayerInfo.currentCar = 3;
                }
                else
                {
                    PlayerInfo.currentCar = 0;
                }
                break;
            default:
                PlayerInfo.currentCar = 3;
                break;


        }
        
        restartButton.interactable = false;
        SceneManager.LoadScene(0);
    }

    void switchToCamera2()
    {
        camera2.Priority = camera1.Priority + 1;
        Invoke("switchToPlayerCamera", 2f);

    }
    
    void switchToPlayerCamera()
    {
        playerCamera.Priority = camera2.Priority + 1;
        Invoke("startGame", 3.5f);
    }

    void populateNameList()
    {
        npcNameList = new List<string>();
        npcNameList.Add("Amarra");
        npcNameList.Add("Aryn");
        npcNameList.Add("Jagod");
        npcNameList.Add("Milsa");
        npcNameList.Add("Breck");
        npcNameList.Add("Bob");
        npcNameList.Add("Jaq");
        npcNameList.Add("InkPot");

    }

    void populatePositions()
    {
        npcList = new List<GameObject>();
        spawnPositions = new List<Vector3>();
        spawnRotation = new List<Vector3>();

        spawnPositions.Add(new Vector3(35f, 2f, 10f));
        spawnPositions.Add(new Vector3(35f, 2f, 80f));
        spawnPositions.Add(new Vector3(5f, 2f, 45f));
        spawnPositions.Add(new Vector3(70f, 2f, 45f));

        spawnRotation.Add(new Vector3(0, 0, 0));
        spawnRotation.Add(new Vector3(0, 180, 0));
        spawnRotation.Add(new Vector3(0, 90, 0));
        spawnRotation.Add(new Vector3(0, -90, 0));
    }
}


/*
 spawnPositions.Add(new Vector3(50f, 2f, 20f));
        spawnPositions.Add(new Vector3(50f, 2f, 70f));
        spawnPositions.Add(new Vector3(20f, 2f, 50f));
        spawnPositions.Add(new Vector3(70f, 2f, 50f));

        spawnRotation.Add(new Vector3(0, 0, 0));
        spawnRotation.Add(new Vector3(0, 180, 0));
        spawnRotation.Add(new Vector3(0, 90, 0));
        spawnRotation.Add(new Vector3(0, -90, 0));
 */