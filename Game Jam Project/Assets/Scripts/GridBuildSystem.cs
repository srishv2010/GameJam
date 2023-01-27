using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class GridBuildSystem : MonoBehaviour
{
    public Material grassMAT;
    public Transform ground;

    public static GridBuildSystem instance;

    public List<Image> typeButtons = new List<Image>();
    public List<GameObject> buildings = new List<GameObject>();

    public List<BuildingSO> buildingTypes = new List<BuildingSO>();

    public int selectedBuilding = 200;
    public Transform buildingsParent;
    public Vector2 cellSize;
    public GameObject active;

    public bool buildMode;
    public bool moveMode;
    public bool deleteMode;

    public LayerMask groundMask;
    public LayerMask buildingMask;
    public Quaternion buildingRotation;


    public float factories;
    public float farms;
    public float houses;

    public float population = 0f;

    public float foodSupply = 10f;
    public float money = 5000f;
    public float populationCapacity = 0f;
    public float energySupply = 10f;

    public float foodChange = 0f;
    public float moneyChange = 0f;
    public float energyChange = 0f;
    public float populationChange = 5f;

    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI foodText;
    public TextMeshProUGUI energyText;
    public TextMeshProUGUI populationText;

    public TextMeshProUGUI foodChangeText;
    public TextMeshProUGUI moneyChangeText;
    public TextMeshProUGUI energyChangeText;
    public TextMeshProUGUI populationChangeText;

    public float timePerTurn = 10f;
    public float timeLeft;
    public Slider roundTimer;
    public TextMeshProUGUI roundTimeLeftText;

    public bool tutorialComplete = true;
    public bool gameRunning = true;

    public GameObject gameOverScreen;

    public Quests questSystem;

    public float roundsComplete;
    public float levelsComplete = 1;

    public Quests.Quest currentQuest;
    public float built;
    public TextMeshProUGUI questText;
    public TextMeshProUGUI dmsgText;

    public Color changeColor;
    public Color changeColorH;
    public Color selectedColor;
    public Color buttonColor;

    public TextMeshProUGUI roundsText;
    private void Start()
    {
        chooseQuest();
        grassMAT.mainTextureScale = Vector2.one * 10f;
        timeLeft = timePerTurn;
        instance = this;
        Selected(0);
        roundTimer.maxValue = timePerTurn;
    }

    void Update()
    {
        if (gameRunning)
        {
            roundsText.text = $"Round {roundsComplete} / {levelsComplete * 20}";
            if (tutorialComplete)
            {
                timeLeft -= Time.deltaTime;
                if(timeLeft < 0)
                {
                    timeLeft = timePerTurn;
                    GameIteration();
                }
            }
            roundTimer.value = timeLeft;
            roundTimeLeftText.text = Mathf.Ceil(timeLeft).ToString();

            moneyText.text = ("Gold: £" + money.ToString());
            foodText.text = ("Food: " + foodSupply.ToString());
            energyText.text = ("Energy: " + energySupply.ToString());
            populationText.text = ("Population: " + population.ToString() + " / " + populationCapacity.ToString());

            moneyChangeText.text = (moneyChange.ToString());
            foodChangeText.text = (foodChange.ToString());
            energyChangeText.text = (energyChange.ToString());
            populationChangeText.text = (populationChange.ToString());

            if (money + moneyChange <= 0)
            {
                moneyChangeText.transform.parent.gameObject.GetComponent<Image>().color = changeColorH;
            }
            else
            {
                moneyChangeText.transform.parent.gameObject.GetComponent<Image>().color = changeColor;
            }

            if (foodSupply + foodChange <= 0)
            {
                foodChangeText.transform.parent.gameObject.GetComponent<Image>().color = changeColorH;
            }
            else
            {
                foodChangeText.transform.parent.gameObject.GetComponent<Image>().color = changeColor;
            }

            if (energySupply + energyChange <= 0)
            {
                energyChangeText.transform.parent.gameObject.GetComponent<Image>().color = changeColorH;
            }
            else
            {
                energyChangeText.transform.parent.gameObject.GetComponent<Image>().color = changeColor;
            }

            if (population + populationChange > populationCapacity)
            {
                populationChangeText.transform.parent.gameObject.GetComponent<Image>().color = changeColorH;
            }
            else
            {
                populationChangeText.transform.parent.gameObject.GetComponent<Image>().color = changeColor;
            }


            if (Input.GetKeyDown(KeyCode.X))
            {
                Destroy(active);
                active = null;
                deleteMode = false;
                moveMode = false;
                buildMode = false;

                foreach (Image img in typeButtons)
                {
                    img.color = buttonColor;
                }
            }

            if (active != null && Input.GetMouseButtonDown(2) && moveMode == true)
            {
                Place();
            }

            if (moveMode)
            {
                Move();
            }

            if (Input.GetMouseButtonDown(2))
            {
                if (buildMode == true && !moveMode)
                {
                    Build();
                }
                if (deleteMode)
                {
                    Delete();
                }
            }
        }
    }

    public void chooseQuest()
    {
        if(roundsComplete < 5)
        {
            List<Quests.Quest> x = questSystem.quests[(int)roundsComplete - 1].questsList;
            currentQuest =  x[(int)Mathf.Round(Random.Range(0, x.Count))];
        }
        else
        {
            List<Quests.Quest> x = questSystem.quests[4].questsList;
            currentQuest = x[(int)Mathf.Round(Random.Range(0, 4))];
        }
        built = 0;
        string s = "";
        if(currentQuest.numberOfBuildings > 1)
        {
            s = "s";
        }
        questText.text = $"Build {currentQuest.numberOfBuildings} {currentQuest.buildingType.BuildingName + s} ({built}/{currentQuest.numberOfBuildings})";
    }

    public void GameIteration()
    {
        roundsComplete += 1;
        population += populationChange;
        print(population);

        energySupply += energyChange;
        foodSupply += foodChange;
        money += moneyChange;
        bool m = money < 0;
        bool f = foodSupply < 0;
        bool e = energySupply < 0;
        bool p = population > populationCapacity;
        bool c = currentQuest.numberOfBuildings > built;
        string dmsg = "";
        if (m)
        {
            dmsg = "You went bankrupt";
            gameRunning = false;
            gameOverScreen.SetActive(true);
        }
        if (f)
        {
            dmsg = "Your citizens starved";
            gameRunning = false;
            gameOverScreen.SetActive(true);
        }
        if (e)
        {
            dmsg = "The lights went out";
            gameRunning = false;
            gameOverScreen.SetActive(true);
        }
        if (p)
        {
            dmsg = "Your citizens ran out of place";
            gameRunning = false;
            gameOverScreen.SetActive(true);
        }
        if (c)
        {
            dmsg = "You failed to meet your citizens demands";
            gameRunning = false;
            gameOverScreen.SetActive(true);
        }
        dmsgText.text = dmsg;


        if (roundsComplete == levelsComplete * 20)
        {
            levelsComplete += 1;
            ground.localScale = 1.2f * ground.localScale;
            grassMAT.mainTextureScale = new Vector2(ground.localScale.x * 10f, ground.localScale.z * 10f); 
        }
        chooseQuest();
    }

    public void resetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void openMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void toggleGameState()
    {
        gameRunning = false;
    }

    public void AddMoney(int amount)
    {
        money += amount;
    }

    public void Place()
    {
        if (active != null)
        {
            Vector3 currPos = active.transform.position;
            foreach (GameObject item in buildings)
            {
                if(item.transform.position == currPos || money - active.GetComponent<Tracker>().type.cost < 0)
                {
                    return;
                }
            }
            buildings.Add(active);
            moveMode = false;
            buildMode = true;
            money -= active.GetComponent<Tracker>().type.cost;
            active.GetComponent<Tracker>().placed = true;

            BuildingSO buildingSO = active.GetComponent<Tracker>().type;

            if(buildingSO == currentQuest.buildingType)
            {
                string s = "";
                if (currentQuest.numberOfBuildings > 1)
                {
                    s = "s";
                }
                built += 1;
                built = Mathf.Clamp(built, Mathf.NegativeInfinity, currentQuest.numberOfBuildings);
                questText.text = $"Build {currentQuest.numberOfBuildings} {currentQuest.buildingType.BuildingName + s} ({built}/{currentQuest.numberOfBuildings})";
            }

            energyChange += buildingSO.energyOutput;
            foodChange += buildingSO.foodOutput;
            moneyChange += buildingSO.moneyOutput;

            populationCapacity += buildingSO.population;

            active = null;
        }
    }

    public void Selected(int selected)
    {
        foreach(Image img in typeButtons)
        {
            img.color = buttonColor;
        }

        deleteMode = false;
        buildMode = false;
        moveMode = false;


        if(active != null)

        {
            Destroy(active);
            active = null;
        }

        if (selected == selectedBuilding)
        {
            selectedBuilding = 22;
            return;
        }

        selectedBuilding = selected;
        if (selected == -1)
        {
            deleteMode = true;
            typeButtons[typeButtons.Count - 1].color = selectedColor;
            return;
        }
        buildMode = true;
        typeButtons[selected].color = selectedColor;
    }

    public void Delete()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition).origin, Camera.main.ScreenPointToRay(Input.mousePosition).direction, out hit, buildingMask))
        {
            int x = (int)(Mathf.Round(hit.point.x / cellSize.x) * cellSize.x);
            int z = (int)(Mathf.Round(hit.point.z / cellSize.y) * cellSize.y);
            Vector3 pos = new Vector3(x, 0f, z);
            foreach(GameObject item in buildings)
            {
                if(item.transform.position == pos)
                {
                    BuildingSO buildingSO = active.GetComponent<Tracker>().type;

                    if (buildingSO == currentQuest.buildingType)
                    {
                        string s = "";
                        if (currentQuest.numberOfBuildings > 1)
                        {
                            s = "s";
                        }
                        built -= 1;
                        questText.text = $"Build {currentQuest.numberOfBuildings} {currentQuest.buildingType.BuildingName + s} ({built}/{currentQuest.numberOfBuildings})";
                    }

                    energyChange -= buildingSO.energyOutput;
                    foodChange -= buildingSO.foodOutput;
                    moneyChange -= buildingSO.moneyOutput;

                    populationCapacity -= buildingSO.population;

                    buildings.Remove(item);
                    Destroy(item);
                    return;
                }
            }
        }
    }

    public void Build()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition).origin, Camera.main.ScreenPointToRay(Input.mousePosition).direction, out hit, groundMask))
        {
            int x = (int)(Mathf.Round(hit.point.x / cellSize.x) * cellSize.x);
            int z = (int)(Mathf.Round(hit.point.z / cellSize.y) * cellSize.y);
            active = Instantiate(buildingTypes[selectedBuilding].buildingPrefab, new Vector3(x, 0f, z), buildingRotation, buildingsParent);
            buildMode = false;
            moveMode = true;
        }
    }
    
    public void Move()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            active.transform.Rotate(new Vector3(0f, 90f, 0f));
            buildingRotation = active.transform.rotation;
        }


        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition).origin, Camera.main.ScreenPointToRay(Input.mousePosition).direction, out hit, groundMask))
        {
            int x = (int)(Mathf.Round(hit.point.x / cellSize.x) * cellSize.x);
            int z = (int)(Mathf.Round(hit.point.z / cellSize.y) * cellSize.y);
            active.transform.position = new Vector3(x, 0f, z);
        }
    }
}