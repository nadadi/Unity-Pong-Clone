using UnityEngine;
using Utils;

namespace Pong
{
	[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
	public class Paddle : MonoBehaviour
	{
		[SerializeField] TypeOfPlayer player;
		[SerializeField] Mode difficulty;
		[SerializeField] float speed = 1f;
		[SerializeField] LayerMask borderLayer;

		Ball _theBall;
		Rigidbody2D _rigidbody;
		Collider2D _collider;
		Vector2 velocity;
		int _direction;
		float _minDistanceToMove;
		bool _touchingBorder;
		enum TypeOfPlayer { Player1, Player2, AI };
		enum Mode { Easy, Normal, Hard }

		void Awake()
		{
			_rigidbody = GetComponent<Rigidbody2D>();
			_collider = GetComponent<Collider2D>();
			_theBall = FindObjectOfType<Ball>();
		}

		void Start()
		{
			_rigidbody.bodyType = RigidbodyType2D.Kinematic;
			_rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
			_rigidbody.interpolation = RigidbodyInterpolation2D.Interpolate;
			_rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
			_collider.isTrigger = true;
			velocity = Vector2.zero;
		}

		void Update()
		{
			DetermineInput();
		}

		void FixedUpdate()
		{
			_rigidbody.MovePosition(_rigidbody.position + velocity * speed * Time.fixedDeltaTime);
		}

		void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.CompareTag("Ball"))
			{
				Rigidbody2D rb2d = collision.GetComponent<Rigidbody2D>();
				GameManager.instance.PlayPaddleSFX();
				// Speed up ball...
				float newXVel = rb2d.velocity.x + rb2d.velocity.x / 100;

				// If paddle is moving...
				if (velocity.y != 0f)
				{
					// Modify the ball's trayectory.
					rb2d.velocity = new Vector2(-newXVel, -velocity.y * Random.Range(3.0f, 5.5f));
				}
				else // If paddle isn't moving...
				{
					// Case 1: The ball has a Y considerable speed so we don't have to change it...
					if (rb2d.velocity.y > 1.0f)
						rb2d.velocity = new Vector2(-newXVel, rb2d.velocity.y);
					// Case 2: The ball hasn't a Y considerable speed so we have to change it...
					else
						rb2d.velocity = new Vector2(-newXVel, -Random.Range(-1.5f, 1.5f));
				}
			}
		}

		void DetermineInput()
		{
			// The player is a human.
			if (player == TypeOfPlayer.Player1 || player == TypeOfPlayer.Player2)
			{
				float vertical;
				if (player == TypeOfPlayer.Player1)
				{
					vertical = Input.GetAxisRaw("VerticalPlayer1");
				}
				else
				{
					vertical = Input.GetAxisRaw("VerticalPlayer2");
				}

				// If it's possible to move up or down, then assign new velocity...
				if ((PossibleToMoveUp() && vertical > 0) || (PossibleToMoveDown() && vertical < 0))
					velocity = new Vector2(0, vertical);
				// if not, then set velocity to (0,0).
				else
					velocity = Vector2.zero;
			}
			else // If player is AI.
			{
				if (_theBall != null)
				{
					// Calculate a Y distance from ball to paddle.
					float yDistance = Vector2.Distance(new Vector2(0, _theBall.transform.position.y), new Vector2(0, transform.position.y));

					// According to difficulty this condition can be true...
					if (yDistance > _minDistanceToMove)
					{
						// If it's possible to move and ball is above paddle, the move to up, else move to down.
						if (_theBall.transform.position.y > transform.position.y && PossibleToMoveUp())
							velocity = Vector2.up;
						else if (_theBall.transform.position.y < transform.position.y && PossibleToMoveDown())
							velocity = Vector2.down;
					}
					else
					{
						velocity = Vector2.zero;
					}
				}
			}
		}

		bool PossibleToMoveUp()
		{
			RaycastHit2D upRaycast = Physics2D.Raycast(transform.position, Vector2.up, transform.localScale.y * 0.75f, borderLayer.value);
			if (upRaycast.collider != null)
				return false;

			return true;
		}

		bool PossibleToMoveDown()
		{
			RaycastHit2D downRaycast = Physics2D.Raycast(transform.position, Vector2.down, transform.localScale.y * 0.75f, borderLayer.value);

			if (downRaycast.collider != null)
				return false;

			return true;
		}

		public void EasyDifficulty()
		{
			difficulty = Mode.Easy;
		}

		public void NormalDifficulty()
		{
			difficulty = Mode.Normal;
		}

		public void HardDifficulty()
		{
			difficulty = Mode.Hard;
		}

		public void SetDifficulty()
		{
			if (difficulty == Mode.Easy)
			{
				_minDistanceToMove = 0.93f;
			}
			else if (difficulty == Mode.Normal)
			{
				_minDistanceToMove = 0.81f;
			}
			else
			{
				_minDistanceToMove = 0.75f;
			}
		}

		public string Player()
		{
			if (player == TypeOfPlayer.AI)
				return "AI";
			return "Human";
		}
	}   
}