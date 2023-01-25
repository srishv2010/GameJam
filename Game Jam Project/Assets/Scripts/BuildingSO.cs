using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create Building", fileName = " Building")]
public class BuildingSO : ScriptableObject
{
    public GameObject buildingPrefab;
    public ServiceSO buildingService;
    public float cost;

    public float population;
    public float energyOutput;
    public float moneyOutput;
    public float foodOutput;
    public float happinessOutput;

    public bool isTaxable;
    public float taxFrequency;

}
