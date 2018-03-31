using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public GameObject originalObj;
    public int score = 0;
    public int bestscore = 20;
    public UnityEngine.UI.Text ScoreText;
    public UnityEngine.UI.Text YourScoreText;
    public UnityEngine.UI.Text BestScoreText;
    public UnityEngine.UI.Text TimerText;
    float time = 15f; // оставшееся время раунда
    float timer = 0f; // отсчитывает каждую секунду
    float roundTime = 100f; // заданное время раунда в секундах
    public float circleTime = 100f; // заданное максимальное время на изменение цвета круга
    public Canvas panelCanvas;
    public bool playingAllowed = true;

    public int[] lastCircleCols = new int[3]; // массив для последних трех цветов кругов
    public int lccIndex = 0; // индекс для lastCircleCols
    public int[] lastSquareCols = new int[3]; // массив для последних трех цветов квадратов
    public int lscIndex = 0; // индекс для lastSquareCols

    public TextAsset colorstxtfile;
    int c1 = 0;
    int c2 = 0;
    public float[,] colorList = new float[255, 3];

    public bool filesRead = false;

    string memory = "";
    int numberOfColors;

    public int[] circleColors = new int [255];
    public int circleNumber = 0;

    public int[] squareColors = new int[255];
    public int squareNumber = 0;

    int bm1 = 0; // номер символа в файле, откуда начинается время раунда
    int bm2 = 0; // номер символа в файле, откуда начинается время жизни круга
    int bm3 = 0; // номер символа в файле, откуда начинается рекордное кол-во очков


    public void ReadingFile()
    {
        if (filesRead == false)
        {
            string colorstxtfilestring = colorstxtfile.text;

            //Reading colors
            for (int i = 0; i < 90000; i++)
            {
                if (colorstxtfilestring[i] != ' ' && colorstxtfilestring[i] != ' ' && colorstxtfilestring[i] != '/' && colorstxtfilestring[i] != 'n')
                {
                    memory += colorstxtfilestring[i];
                }
                if (colorstxtfilestring[i] == ' ')
                {
                    colorList[c1, c2] = System.Convert.ToSingle(memory);
                    memory = "";
                    c2++;
                }
                if (colorstxtfilestring[i] == '.')
                {
                    colorList[c1, c2] = System.Convert.ToSingle(memory);
                    memory = "";
                    c2 = 0;
                    c1++;
                }
                if (colorstxtfilestring[i] == '!')
                {
                    bm1 = i;
                    c1--;
                    numberOfColors = c1;
                    memory = "";
                    break;
                }
            }

            for (int i = bm1 + 1; i < 90000; i++)
            {
                if (colorstxtfilestring[i] == '.')
                {
                    bm2 = i;
                    roundTime = System.Convert.ToSingle(memory);
                    time = roundTime;
                    memory = "";
                    break;
                }
                if (colorstxtfilestring[i] != ' ' && colorstxtfilestring[i] != ',' && colorstxtfilestring[i] != '!' && colorstxtfilestring[i] != '/' && colorstxtfilestring[i] != 'n')
                {
                    memory += colorstxtfilestring[i];
                }

            }

            for (int i = bm2 + 1; i < 90000; i++)
            {
                if (colorstxtfilestring[i] == '.')
                {
                    bm3 = i;
                    circleTime = System.Convert.ToSingle(memory);
                    Debug.Log(circleTime);
                    memory = "";
                    break;
                }
                if (colorstxtfilestring[i] != ' ' && colorstxtfilestring[i] != ',' && colorstxtfilestring[i] != '!' && colorstxtfilestring[i] != '/' && colorstxtfilestring[i] != 'n')
                {
                    memory += colorstxtfilestring[i];
                }

            }
            loadingRecord();
            filesRead = true;
        }
    }

    public void savingRecord()
    {
        System.IO.File.WriteAllText("Assets/Data/record.txt", System.Convert.ToString(bestscore));
    }

    public void loadingRecord()
    {
        bestscore = System.Convert.ToInt32(System.IO.File.ReadAllText("Assets/Data/record.txt"));
    }

    public void newSquare(Vector3 vec, int index)
    {
        score++;
        ScoreText.text = System.Convert.ToString(score);
        if (score <= bestscore)
        {
            YourScoreText.text = "Your score: " + score;
        }
        else
            YourScoreText.text = "Your new record: " + score;
        
        
        squareNumber = index;
        Debug.Log(index);
        GameObject clonedObj = Instantiate<GameObject>(this.originalObj, vec, Quaternion.identity);
        clonedObj.name = "Square " + score;

    }


	// Use this for initialization
	void Start () 
    {
        BestScoreText.text = System.Convert.ToString(bestscore);
	}
	
	// Update is called once per frame
	void Update () 
    {
        timer += Time.deltaTime;
        if (timer >= 1f && time > 0f)
        {
            timer = 0f;
            time = time-1;
           
        }

        float time_minutes = time / 60f;
        float time_seconds = time - (int)time_minutes * 60f;
        if (time_seconds >= 10)
        {
            this.TimerText.text = (int)time_minutes + ":" + (int)time_seconds;
        }
        if (time_seconds < 10)
        {
            this.TimerText.text = (int)time_minutes + ":0" + (int)time_seconds;
        }


        if (time == 0)
        {
            EndRound();
        }
	}

    void EndRound()
    {
        if (score > bestscore)
        {
            bestscore = score;
            BestScoreText.text = System.Convert.ToString(bestscore);
            savingRecord();
        }
        panelCanvas.enabled = true;
        playingAllowed = false;
    }

    void Awake()
    {
        panelCanvas.enabled = false;
        playingAllowed = true;
    }

    public void PlayAgain()
    {
        panelCanvas.enabled = false;
        playingAllowed = true;
        score = 0;
        ScoreText.text = System.Convert.ToString(score);
        timer = 0f;
        time = roundTime;
    }

    public void EndGame()
    {
        Application.Quit();
    }

    public int NumberOfColors
    {
        get { return numberOfColors; }
        set { numberOfColors = value; }
    }
}
