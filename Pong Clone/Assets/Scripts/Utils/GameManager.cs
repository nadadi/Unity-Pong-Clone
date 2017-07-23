using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Pong;

namespace Utils
{
	[RequireComponent(typeof(AudioSource))]
	public class GameManager : MonoBehaviour
	{
		public static GameManager instance;
		[SerializeField] int scoreToWinGame = 10;
		[SerializeField] AudioClip pointSfx;
		[SerializeField] AudioClip paddleSfx;
		[SerializeField] AudioClip borderSfx;

		GameManager _instance;
		Paddle[] _players;
		Ball _theBall;
		GameObject[] _borders;
		int _player1Score;
		Text _player1ScoreTxt;
		int _player2Score;
		Text _player2ScoreTxt;
		float _screenHeight;
		float _realWorldHeight;
		float _screenWidth;
		float _realdWorldWidth;
		int _difficulty;
        bool _gameOver;
		Text _gameOverTxt;
		GameObject[] _buttons;
		AudioSource _audio;

		void Awake()
		{
			// Singleton pattern...
			if (instance != null)
			{
				instance.Start();
				DestroyImmediate(gameObject);
			}
			else
			{
				_instance = this;
				instance = _instance;
				DontDestroyOnLoad(gameObject);
			}
		}

		void Start()
		{
			LoadData();
			SetPositionAndScale();
			LoadDifficulty();
		}

		void LoadData()
		{
			_players = FindObjectsOfType<Paddle>();
			_theBall = FindObjectOfType<Ball>();
			_borders = GameObject.FindGameObjectsWithTag("Border");
			GameObject score1 = GameObject.FindGameObjectWithTag("Score1");
			if (score1 != null)
				_player1ScoreTxt = score1.GetComponent<Text>();
			GameObject score2 = GameObject.FindGameObjectWithTag("Score2");
			if (score2 != null)
				_player2ScoreTxt = score2.GetComponent<Text>();
			GameObject gameOver = GameObject.FindGameObjectWithTag("GameOver");
			if (gameOver != null)
				_gameOverTxt = gameOver.GetComponent<Text>();
			_buttons = GameObject.FindGameObjectsWithTag("Button");
			_audio = GetComponent<AudioSource>();

			for (int i = 0; i < _buttons.Length; i++)
			{
				_buttons[i].SetActive(false);
			}
            _player1Score = 0;
            _player2Score = 0;
            _gameOver = false;
		}

		void SetPositionAndScale()
		{
			_screenWidth = Screen.width;
			_screenHeight = Screen.height;
			float orthographicSize = Camera.main.orthographicSize;

			_realdWorldWidth = orthographicSize * 2.0f * _screenWidth / _screenHeight;
			_realWorldHeight = orthographicSize * 2.0f;

			if (_players.Length > 0)
			{
				// 1º player
				if (_players[0] != null)
				{
					_players[0].transform.localScale = new Vector2(_realdWorldWidth / 80, _realWorldHeight / 8);
					_players[0].transform.position = Camera.main.ScreenToWorldPoint(new Vector3(_screenWidth / 10, _screenHeight / 2, 10f));
				}

				// 2º player
				if (_players[1] != null)
				{
					_players[1].transform.localScale = new Vector2(_realdWorldWidth / 80, _realWorldHeight / 8);
					_players[1].transform.position = Camera.main.ScreenToWorldPoint(new Vector3(_screenWidth * 9 / 10, _screenHeight / 2, 10f));
				}
			}

			// The ball
			if (_theBall != null)
			{
				_theBall.transform.localScale = new Vector2(_realdWorldWidth / 60, _realdWorldWidth / 60);
				_theBall.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(_screenWidth / 2, _screenHeight / 2, 10f));
			}

			if (_borders.Length > 0)
			{
				// Upper border
				if (_borders[0] != null)
				{
					_borders[0].transform.localScale = new Vector2(_realdWorldWidth * 6 / 7, _realWorldHeight / 10);
					_borders[0].transform.position = Camera.main.ScreenToWorldPoint(new Vector3(_screenWidth / 2, _screenHeight, 10f));
				}

				// Lower border
				if (_borders[1] != null)
				{
					_borders[1].transform.localScale = new Vector2(_realdWorldWidth * 6 / 7, _realWorldHeight / 10);
					_borders[1].transform.position = Camera.main.ScreenToWorldPoint(new Vector3(_screenWidth / 2, 0, 10f));
				}
			}
		}

		void SetNewPoint()
		{
			// The ball
			if (_theBall != null)
			{
				_theBall.transform.localScale = new Vector2(_realdWorldWidth / 60, _realdWorldWidth / 60);
				_theBall.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(_screenWidth / 2, _screenHeight / 2, 10f));
			}

			if (_players.Length > 0)
			{
				// 1º player
				if (_players[0] != null)
				{
					_players[0].transform.localScale = new Vector2(_realdWorldWidth / 80, _realWorldHeight / 8);
					_players[0].transform.position = Camera.main.ScreenToWorldPoint(new Vector3(_screenWidth / 10, _screenHeight / 2, 10f));
				}

				// 2º player
				if (_players[1] != null)
				{
					_players[1].transform.localScale = new Vector2(_realdWorldWidth / 80, _realWorldHeight / 8);
					_players[1].transform.position = Camera.main.ScreenToWorldPoint(new Vector3(_screenWidth * 9 / 10, _screenHeight / 2, 10f));
				}
			}
		}

		public void GameOver()
		{
            if (_gameOver)
                return;
            
			PlayPointSFX();
			// Calculate distance from the ball to players.
			float distancePlayer1 = Vector2.Distance(_theBall.transform.position, _players[0].transform.position);
			float distancePlayer2 = Vector2.Distance(_theBall.transform.position, _players[1].transform.position);

			if (distancePlayer1 > distancePlayer2)
			{
				_player1Score++;
				if (_player1ScoreTxt != null)
					_player1ScoreTxt.text = _player1Score.ToString();
			}
			else
			{
				_player2Score++;
				if (_player2ScoreTxt != null)
					_player2ScoreTxt.text = _player2Score.ToString();
			}

			//Debug.Log("Player1: " + _player1Score + ", Player2: " + _player2Score);

			if (_player1Score >= scoreToWinGame || _player2Score >= scoreToWinGame)
			{
				if (_player1Score > _player2Score)
				{
					if (_gameOverTxt != null)
					{
                        if (BothPlayersAreHumans())
                        {
                            _gameOverTxt.text = "Player 1 wins!";
                        }
                        else
                        {
							_gameOverTxt.text = "You win!";
                        }
					}
				}
				else
				{
					if (BothPlayersAreHumans())
					{
						_gameOverTxt.text = "Player 2 wins!";
					}
					else
					{
						_gameOverTxt.text = "You lose!";
					}
				}

				for (int i = 0; i < _buttons.Length; i++)
				{
					_buttons[i].SetActive(true);
				}

				Time.timeScale = 0;
				Debug.Log("Gano un jugador");
                _gameOver = true;
			}
			else
			{
				SetNewPoint();
				//SetPositionAndScale();    
			}
		}

		public bool BothPlayersAreHumans()
		{
			if (_players.Length > 0)
			{
				if (_players[0].Player() == "Human" && _players[1].Player() == "Human")
					return true;
			}
			return false;
		}

		public void PlayAgain()
		{
			Time.timeScale = 1;
			int lvlIndex = SceneManager.GetActiveScene().buildIndex;
			SceneManager.LoadScene(lvlIndex);
		}

		public void LoadMainMenu()
		{
			Time.timeScale = 1;
			SceneManager.LoadScene(0);
		}

		public void LoadHumanVsCPU()
		{
			Time.timeScale = 1;
			SceneManager.LoadScene("HumanCPU");
		}

		public void LoadTwoHumans()
		{
			Time.timeScale = 1;
			SceneManager.LoadScene("TwoHumans");
		}

		public void OpenCodeSourceLink()
		{
			Application.OpenURL("https://github.com/nadadi/Unity-Pong-Clone");
		}

		public void ExitGame()
		{
			Application.Quit();
		}

		void PlayPointSFX()
		{
			_audio.PlayOneShot(pointSfx);
		}

		public void PlayPaddleSFX()
		{
			_audio.PlayOneShot(paddleSfx);
		}

		public void PlayBorderSFX()
		{
			_audio.PlayOneShot(borderSfx);
		}

		public void SetDifficulty(int difficulty)
		{
			_difficulty = difficulty;
			LoadHumanVsCPU();
		}

		void LoadDifficulty()
		{
			if (BothPlayersAreHumans() || _players.Length < 2)
				return;

			switch (_difficulty)
			{
				case 0:
					_players[1].EasyDifficulty();
					break;
				case 1:
					_players[1].NormalDifficulty();
					break;
				case 2:
					_players[1].HardDifficulty();
					break;
				default:
					_players[1].NormalDifficulty();
					break;
			}

			_players[1].SetDifficulty();
		}
	}   
}