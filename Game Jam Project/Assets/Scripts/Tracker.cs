using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker : MonoBehaviour
{
    public BuildingSO type;
    public GameObject coin;
    public bool placed;

    public float taxTimeLeft;

    public void Start()
    {
        taxTimeLeft = type.taxFrequency;
    }

    public void Update()
    {
        if (type.isTaxable && placed && transform.childCount == 1)
        {
            taxTimeLeft -= Time.deltaTime;
        }
        if(taxTimeLeft < 0)
        {
            taxTimeLeft = type.taxFrequency;
            TaxComplete().GetComponent<Coin>().value = type.population * 2;
        }
    }
    public GameObject TaxComplete()
    {
        return Instantiate(coin, transform.position + new Vector3(0f, 1.5f, 0f), Quaternion.Euler(0f, 0f, 0f), transform);
    }
}
