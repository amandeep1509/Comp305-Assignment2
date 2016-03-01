using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    // PRIVATE INSTANCE VARIABLES
    private int _scoreValue;
    private int _livesValue;
    private int _win;    

    [SerializeField]
   private AudioSource _winSound;

   


    // PUBLIC ACCESS METHODS
    public int ScoreValue
    {
        get
        {
            return this._scoreValue;
        }

        set
        {
            this._scoreValue = value;
            this.ScoreLabel.text = "Score: " + this._scoreValue;
        }
    }

    public int Win
    {
        get
        {
            return this._win;
        }
        set
        {
            this._win = value;
      
            if (this._win == 1)
            {
                this.EndScoreLabel.text = "Scores: " + this._scoreValue;
                this._winGame();
            }
        }
    }

    public int LivesValue
    {
        get
        {
            return this._livesValue;
        }

        set
        {
            this._livesValue = value;
            if (this._livesValue <= 0 )
            {
                this.EndScoreLabel.text = "Scores: " + this._scoreValue;
                this._gameOver();
            }
            else {
                this.LivesLabel.text = "Lives: " + this._livesValue;
            }
        }
    }

    // PUBLIC INSTANCE VARIABLES
    public Text LivesLabel;
    public Text ScoreLabel;
    public Text GameOverLabel;
    public Text EndScoreLabel;
    public Text winLabel;
    public Button RestartButton;
    public int win;


     // Use this for initialization
    void Start()
    {
        this._initialize();
       
    }

    // Update is called once per frame
    void Update()
    {

    }

    //PRIVATE METHODS ++++++++++++++++++

    //Initial Method
    private void _initialize()
    {
        this.ScoreValue = 0;
        this.LivesValue = 5;
        this.GameOverLabel.gameObject.SetActive (false);
        this.winLabel.gameObject.SetActive(false);
        this.Win = 0;
        this.RestartButton.gameObject.SetActive(false);
        this.EndScoreLabel.gameObject.SetActive(false);
      
    }

    // Game over on 0 lives
    private void _gameOver()
    {
        this.LivesLabel.gameObject.SetActive(false);
        this.ScoreLabel.gameObject.SetActive(false);
        this.EndScoreLabel.gameObject.SetActive(true);
        this.GameOverLabel.gameObject.SetActive(true);
        
        this.RestartButton.gameObject.SetActive (true);
    }

    //Win the game on getting end diamond
    private void _winGame()
    {
        this.LivesLabel.gameObject.SetActive(false);
        this.ScoreLabel.gameObject.SetActive(false);
        this.EndScoreLabel.gameObject.SetActive(true);
        this.winLabel.gameObject.SetActive(true);
        this._winSound.Play();
        this.RestartButton.gameObject.SetActive(true);
        
    }

    // PUBLIC METHODS

    //Method to restart the game when restart button clicked
    public void RestartButtonClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
     
    }
}