using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Buttons : MonoBehaviour
{
    public BuildingSO buildingTypeHolder;
    public GameObject panel;

    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI populationText;
    public TextMeshProUGUI energyText;
    public TextMeshProUGUI foodText;


    public void appear()
    {
        panel.SetActive(true);
        moneyText.text = "Money: " + buildingTypeHolder.moneyOutput.ToString();
        if(buildingTypeHolder.moneyOutput > 0)
        {
            moneyText.color = Color.green;
        }
        else if (buildingTypeHolder.moneyOutput < 0)
        {
            moneyText.color = Color.red;
        }
        else
        {
            moneyText.color = Color.grey;
        }

        populationText.text = "Population: " + buildingTypeHolder.population.ToString();
        if (buildingTypeHolder.population > 0)
        {
            populationText.color = Color.green;
        }
        else
        {
            populationText.color = Color.grey;
        }

        energyText.text = "Energy: " + buildingTypeHolder.energyOutput.ToString();
        if (buildingTypeHolder.energyOutput > 0)
        {
            energyText.color = Color.green;
        }
        else if (buildingTypeHolder.energyOutput < 0)
        {
            energyText.color = Color.red;
        }
        else
        {
            energyText.color = Color.grey;
        }

        foodText.text = "Food: " + buildingTypeHolder.foodOutput.ToString();
        if (buildingTypeHolder.foodOutput > 0)
        {
            foodText.color = Color.green;
        }
        else if (buildingTypeHolder.foodOutput < 0)
        {
            foodText.color = Color.red;
        }
        else
        {
            foodText.color = Color.grey;
        }

    }

    public void disappear()
    {
        panel.SetActive(false);
    }
}
