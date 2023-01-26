using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class GridBuildSystem : MonoBehaviour
{
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
    

    private void Start()
    {
        timeLeft = timePerTurn;
        instance = this;
        Selected(0);
        roundTimer.maxValue = timePerTurn;
    }

    void Update()
    {
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
            moneyChangeText.transform.parent.gameObject.GetComponent<Image>().color = Color.red;
        }
        else
        {
            moneyChangeText.transform.parent.gameObject.GetComponent<Image>().color = Color.white;
        }

        if (foodSupply + foodChange <= 0)
        {
            foodChangeText.transform.parent.gameObject.GetComponent<Image>().color = Color.red;
        }
        else
        {
            foodChangeText.transform.parent.gameObject.GetComponent<Image>().color = Color.white;
        }

        if (energySupply + energyChange <= 0)
        {
            energyChangeText.transform.parent.gameObject.GetComponent<Image>().color = Color.red;
        }
        else
        {
            energyChangeText.transform.parent.gameObject.GetComponent<Image>().color = Color.white;
        }

        if (population + populationChange > populationCapacity)
        {
            populationChangeText.transform.parent.gameObject.GetComponent<Image>().color = Color.red;
        }
        else
        {
            populationChangeText.transform.parent.gameObject.GetComponent<Image>().color = Color.white;
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
                img.color = new Color(0, 100, 255, 255);
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

    public void GameIteration()
    {
        population += populationChange;
        print(population);

        energySupply += energyChange;
        foodSupply += foodChange;
        money += moneyChange;
        if (money < 0 || foodSupply < 0 || energySupply < 0 || population > populationCapacity)
        {
            gameRunning = false;
            gameOverScreen.SetActive(true);
        }
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
            img.color = new Color(0, 100, 255, 255);
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
            typeButtons[typeButtons.Count - 1].color = Color.red;
            return;
        }
        buildMode = true;
        typeButtons[selected].color = Color.red;
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