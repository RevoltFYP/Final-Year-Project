using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrapManager : MonoBehaviour {

    public Text scrapText;
    public int pooledAmount;
    public GameObject scrapPrefab;

    public int scrap { get; set; }

    public List<GameObject> scrapList { get; set; }

	// Use this for initialization
	void Awake () {
        scrapText.text = scrap.ToString();

        scrapList = new List<GameObject>();
        
        // Testing //
        //scrap = 100;

        for(int i = 0; i < pooledAmount; i++)
        {
            GameObject obj = (GameObject)Instantiate(scrapPrefab);
            scrapList.Add(obj);
            obj.SetActive(false);
            GameObject.DontDestroyOnLoad(obj);
        }
	}
	
    public void AddScrap(int amount)
    {
        scrap += amount;

        // Update UI 
        scrapText.text = "Scrap: " + scrap.ToString();
    }

    public void RemoveScrap(int amount)
    {
        scrap -= amount;

        // Update UI 
        scrapText.text = "Scrap: " + scrap.ToString();
    }
}
