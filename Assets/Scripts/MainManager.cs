using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text GameOverText;

    public bool m_Started = false;
    private int m_Points;
    public int highScore;
    public string highScoreHolder;

    public bool m_GameOver = false;

    public static MainManager Instance;

    public string playerName;
    public GameObject inputField;
    public TextMeshProUGUI welcome;
    public Text bestScore;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadName();
        LoadHighScore();
    }
    // Start is called before the first frame update
    void Start()
    {
        
        inputField = GameObject.Find("Name");
              
        welcome.text = "Best Score " + highScoreHolder + ": " + highScore;
        inputField.GetComponent<TMP_InputField>().text = playerName;





    }

   void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log(highScore);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log(playerName);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log(highScoreHolder);
        }
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
               
                RestartGame();
                
            }
        }
    }
    public void SpawnBricks()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }
    public void StoreName()
    {
        
        playerName = inputField.GetComponent<TMP_InputField>().text;
       

    }
    void AddPoint(int point)
    {
        ScoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        GameOverText = GameObject.Find("Gameover").GetComponent<Text>();
        m_GameOver = true;
        GameOverText.enabled = true;
          if (m_Points > highScore)
        {
            
            SaveHighScore();
            LoadHighScore();

        }
        


    }
       public void RestartGame()
    {
       
        SceneManager.LoadScene(1, LoadSceneMode.Single);
        StartCoroutine(SpawnCd());
        m_GameOver = false;
        m_Started = false;
        m_Points = 0;
        
    }

    IEnumerator SpawnCd()
    {
        yield return new WaitForSeconds(0.2f);
        SpawnBricks();   
        
        Ball = GameObject.Find("Ball").GetComponent<Rigidbody>();
        bestScore = GameObject.Find("BestScore").GetComponent<Text>();
        bestScore.text = "Best Score : " + highScoreHolder + ": " + highScore;
    }
   
    [System.Serializable]
    class SaveData
    {
        public int m_Points;
        public string playerName;
        public int highScore;
        public string highScoreHolder;
    }
    public void SaveName()
    {
        SaveData data = new SaveData();
        data.playerName = playerName;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/namesavefile.json", json);
    }
    public void LoadName()
    {
        string path = Application.persistentDataPath + "/namesavefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            playerName = data.playerName;
        }
    }
    public void SaveHighScore()
    {
        SaveData data = new SaveData();
        data.highScore = m_Points;
        data.highScoreHolder = playerName;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadHighScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            highScore = data.highScore;
            highScoreHolder = data.highScoreHolder;
        }
    }

    //public void SaveHighScoreHolder()
    //{
    //    SaveData data = new SaveData();
     //   
     //
//
       // string json = JsonUtility.ToJson(data);
//
       // File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
        
  //  }
   // public void LoadHighScoreHolder()
   // {
    //    string path = Application.persistentDataPath + "/savefile.json";
     //   if (File.Exists(path))
//{
      //      string json = File.ReadAllText(path);
      //      SaveData data = JsonUtility.FromJson<SaveData>(json);
      //
      //      
       // }
   // }
    
}

