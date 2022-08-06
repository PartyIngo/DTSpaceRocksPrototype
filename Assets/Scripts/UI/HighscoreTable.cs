using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreTable : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;
    private List<Transform> highscoreEntryTransformList;


    void Awake()
    {
        showHighScores();

    }


    void Update()
    {
        //Check if a new High Score candidate is there. If so, save if to the High Scores and reset the container
        int newScore = PlayerPrefs.GetInt("NewScore");
        string newName = PlayerPrefs.GetString("NewName");
        //Debug.Log(newName + "     " + newScore);
        if ((newScore != 0) && (newName != ""))
        {
            AddHighscoreEntry(newScore, newName);
            PlayerPrefs.SetInt("NewScore", 0);
            PlayerPrefs.SetString("NewName", "");
            PlayerPrefs.Save();
        }
    }

    //Shows the High Scores on the screen
    private void showHighScores()
    {
        entryContainer = transform.Find("HighscoreTable");
        entryTemplate = entryContainer.Find("HighscoreRow");

        entryTemplate.gameObject.SetActive(false);

        //AddHighscoreEntry(999999999, "BLA");


        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Debug.Log(jsonString);
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        if (highscores == null)
        {
            // There's no stored table, initialize
            Debug.Log("Initializing table with default values...");
            AddHighscoreEntry(3, "DES");
            AddHighscoreEntry(2, "CRI");
            AddHighscoreEntry(1, "PGO");
            // Reload
            jsonString = PlayerPrefs.GetString("highscoreTable");
            highscores = JsonUtility.FromJson<Highscores>(jsonString);
        }


        //Sorting of the List
        for (int i = 0; i < highscores.highscoreEntryList.Count; i++)
        {
            for (int j = i + 1; j < highscores.highscoreEntryList.Count; j++)
            {
                if (highscores.highscoreEntryList[j].score > highscores.highscoreEntryList[i].score)
                {
                    //Swap
                    HighscoreEntry tmp = highscores.highscoreEntryList[i];
                    highscores.highscoreEntryList[i] = highscores.highscoreEntryList[j];
                    highscores.highscoreEntryList[j] = tmp;
                }
            }
        }

        highscoreEntryTransformList = new List<Transform>();
        foreach (HighscoreEntry highscoreEntry in highscores.highscoreEntryList)
        {
            CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
        }
    }




    /**
     * Creates an Instance of an UI Element, that represents the text for rank, name and score
     */
    private void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, List<Transform> transformList)
    {
        float templateHeight = 75f;
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, ((-templateHeight * transformList.Count) + 350));
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;
        
        string rankString;
        rankString = rank + ".";
        entryTransform.Find("posText").GetComponent<Text>().text = rankString;

        string name = highscoreEntry.name;
        entryTransform.Find("nameText").GetComponent<Text>().text = name;

        int score = highscoreEntry.score;
        entryTransform.Find("scoreText").GetComponent<Text>().text = score.ToString();

        transformList.Add(entryTransform);
    }

    private void AddHighscoreEntry(int score, string name)
    {
        //Create HighscoreEntry
        HighscoreEntry highscoreEntry = new HighscoreEntry { score = score, name = name };
        
        //Load saved Highscores
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        if (highscores == null)
        {
            // There's no stored table, initialize
            highscores = new Highscores()
            {
                highscoreEntryList = new List<HighscoreEntry>()
            };
        }



        //Add new Entry to Highscores
        highscores.highscoreEntryList.Add(highscoreEntry);


        //Sort Entries
        for (int i = 0; i < highscores.highscoreEntryList.Count; i++)
        {
            for (int j = i + 1; j < highscores.highscoreEntryList.Count; j++)
            {
                if (highscores.highscoreEntryList[j].score > highscores.highscoreEntryList[i].score)
                {
                    //Swap
                    HighscoreEntry tmp = highscores.highscoreEntryList[i];
                    highscores.highscoreEntryList[i] = highscores.highscoreEntryList[j];
                    highscores.highscoreEntryList[j] = tmp;
                }
            }
        }

        //Keep the list by 10 items max
        if (highscores.highscoreEntryList.Count > 10)
        {
            for (int h = highscores.highscoreEntryList.Count; h > 10; h--)
            {
                highscores.highscoreEntryList.RemoveAt(10);
            }
        }


        //set the last candidate, to check for a new highscore
        PlayerPrefs.SetInt("lastPlacement", highscores.highscoreEntryList[9].score);
        PlayerPrefs.Save();
        Debug.Log("10TH PLACEMENT" + PlayerPrefs.GetInt("lastPlacement"));


        //Save updated Highscores
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();

        showHighScores();
    }

    //public void addItem(int score, string name)
    //{
    //    AddHighscoreEntry(score, name);
    //}


    //public void resetPlayerPrefs()
    //{
    //    PlayerPrefs.DeleteAll();
    //}


    private class Highscores
    {
        public List<HighscoreEntry> highscoreEntryList;

    }

    /*
     * Single High Score entry
     */
    [System.Serializable]
    private class HighscoreEntry
    {
        public int score;
        public string name;
    }


}
