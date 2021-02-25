using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Text;

public class ReadInCSV : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string fp = "Assets/DrugInfo/60DrugNames.csv";
        const int brandKey = 0;
        const int genericKey = 1;
        Dictionary<string, List<string>> brandDrugsKey = PopulateDictionary(fp, brandKey);  //dictionary with the brand name of the drug as the key
        Dictionary<string, List<string>> genericDrugsKey = PopulateDictionary(fp, genericKey);  //dictionary with the generic name of the drug as the key

        TestBrandAsKey(brandDrugsKey);
        TestGenericAsKey(genericDrugsKey);
    }

    private void TestBrandAsKey(Dictionary<string, List<string>> testBrandDic)
    {
        string output = "";
        foreach(KeyValuePair<string, List<string>> x in testBrandDic)
        {
            //output = "Brand Name: " + x.Key + "\tGeneric Name: " + x.Value.ToString();
            output = "Brand Name: " + x.Key;
            Debug.Log(output);
            Debug.Log("Generic Name(s): ");
            foreach (string y in x.Value)
                Debug.Log(y);
        }
    }

    private void TestGenericAsKey(Dictionary<string, List<string>> testGenericDic)
    {
        string output = "";
        foreach (KeyValuePair<string, List<string>> x in testGenericDic)
        {
            // output = "Generic Name: " + x.Key/* + "\tBrand Name: " + x.Value*/;
            output = "Generic Name: " + x.Key;
            Debug.Log(output);
            Debug.Log("Brand Name(s): ");
            foreach (string y in x.Value)
                Debug.Log(y);

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //function takes as input filePath - the name of the file to read in, keyVal - whether the brand name or generic name is the key of the dictionary
    //keyVal - 0: brand name is the key
    //          1: generic name is the key
    public Dictionary<string, List<string>> PopulateDictionary(string filePath, int keyVal)
    {
        //close file just in case
        //Read drugs in from CSV, key is brand name of the drugs
        Dictionary<string, List<string>> drugs = new Dictionary<string, List<string>>();
        string line = "";
        using (StreamReader sr = new StreamReader(@filePath))
        {
            line = sr.ReadLine();
            while (sr.Peek() != -1)
            {
                line = sr.ReadLine();
                string[] splitted = line.Split(',');
                //codeine is the only drug without a brand name, and contains parentheses, so this can be refined later, but to get it to work, added this to check
                if (!splitted[0].Contains("("))
                {
                    string generic = splitted[0];
                    string brand = splitted[1];

                    //if the brand name is the key
                    if (keyVal == 0)
                    {
                        //check if key is already in dictionary
                        if (drugs.ContainsKey(brand))
                        {
                            drugs[brand].Add(generic);
                        }
                        //key is not in the dictionary, create new value list
                        else
                        {
                            List<string> valList = new List<string>();
                            valList.Add(generic);
                            drugs.Add(brand, valList);
                        }
                           
                    }
                    //if the generic name is the key
                    else
                    {
                        //check if key is already in dictionary
                        if (drugs.ContainsKey(generic))
                        {
                            drugs[generic].Add(brand);
                        }
                        //key is not in the dictionary, create new value list
                        else
                        {
                            List<string> valList = new List<string>();
                            valList.Add(brand);
                            drugs.Add(generic, valList);
                        }

                    }
                        

                }
                else
                {
                    List<string> special = new List<string>();
                    special.Add(splitted[0]);
                    drugs.Add(splitted[0], special); //this should only happen for codeine, and it is added as both the key and the value   
                }
                      
            }
        }

        return drugs;
    }

}
