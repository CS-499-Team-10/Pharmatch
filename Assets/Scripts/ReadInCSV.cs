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
        string fp = "Assets/DrugInfo/DrugNames.csv";
        Dictionary<string, List<string>> sortedDrugs = AddToDic(fp);
        Dictionary<string, List<string>> relatedDrugs = RelatedDrugsDic(sortedDrugs);

        foreach(KeyValuePair<string, List<string>> x in relatedDrugs)
        {
            Console.WriteLine("Drug Class: {0}, Drugs: {1}", x.Key, x.Value.ToString());
            string output = "Drug Name: " + x.Key;
            Debug.Log(output);
            Debug.Log("Related Drugs");
            foreach (string s in x.Value)
                Debug.Log(s);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    string[] FormatLine(string line)
    {
        int numAddSubStrings = 0;
        if (line.Contains("\""))
        {

        }
    }

    Dictionary<string, List<string>> AddToDic(string filePath)
    {
        //Read drugs in from CSV, sort by drug class
        Dictionary<string, List<string>> drugs = new Dictionary<string, List<string>>();
        string line = "";
        using (StreamReader sr = new StreamReader(@filePath))
        {
            line = sr.ReadLine();
            while (sr.Peek() != -1)
            {
                line = sr.ReadLine();
                string[] splitted = line.Split(',');
                if (!drugs.ContainsKey(splitted[2]))
                {
                    List<string> drugList = new List<string>();
                    drugList.Add(splitted[0]);
                    drugs.Add(splitted[2], drugList);
                }
                else
                    drugs[splitted[2]].Add(splitted[0]);     
            }
        }

        
        return drugs;

    }

    Dictionary<string, List<string>> RelatedDrugsDic(Dictionary<string, List<string>> sortedDrugs)
    {
        //Using sorted drugs, make new dictionary with drug names as keys, related drugs as values
        Dictionary<string, List<String>> relatedDrugs = new Dictionary<string, List<string>>();

        foreach (KeyValuePair<string, List<string>> x in sortedDrugs)
        {
            //Check if only one drug in category
            if (x.Value.Count == 1)
            {
                List<string> relDrugs = new List<string>();
                relDrugs.Add(x.Value[0]);
                relatedDrugs.Add(x.Value[0], relDrugs);
            }    
            else
            {
                //for every drug in the related category, relate it to every other drug
                foreach (string s in x.Value)
                {
                    if (!relatedDrugs.ContainsKey(s))
                    {
                        string relDrugTempKey = s;
                        List<string> drugList = new List<string>();
                        foreach (string st in x.Value)
                        {
                            drugList.Add(st);
                        }
      
                        relatedDrugs.Add(relDrugTempKey, drugList);
                    }
                }
            }
            
        }

        return relatedDrugs;


    }

}
