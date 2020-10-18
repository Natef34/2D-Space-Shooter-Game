using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //handle to Text
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Image _LivesImg;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private GameObject _gameOver;
    [SerializeField]
    private GameObject _restart;
    [SerializeField]
    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _gameOver.SetActive(false);
        _restart.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if (_gameManager == null)
        {
            Debug.Log("game manager is null!");
        }

    }

    // Update is called once per frame
    void Update()
    {   
    }

    public void UpdateScore(int _playerScore)
    {
        _scoreText.text = "Score: " + _playerScore.ToString();
    }

    public void UpdateLives(int currentLives)
    {
        _LivesImg.sprite = _liveSprites[currentLives];
    }

    public void GameOver()
    {
        _gameManager.GameOver();
        _restart.SetActive(true);
        StartCoroutine(GameOverFlicker());

    }

    IEnumerator GameOverFlicker()
    {
        for(int i=0; i<3; i++)
        {
            _gameOver.SetActive(true);
            yield return new WaitForSeconds(.5f);
            _gameOver.SetActive(false);
            yield return new WaitForSeconds(.5f);
        }
        _gameOver.SetActive(true);
    }
}
