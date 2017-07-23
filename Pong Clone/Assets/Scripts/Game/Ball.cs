using UnityEngine;
using Utils;

namespace Pong
{
	[RequireComponent(typeof(Rigidbody2D))]
	public class Ball : MonoBehaviour
	{
		[SerializeField] float speed = 5f;

		Rigidbody2D _rigidbody;
		int _direction;

		void Awake()
		{
			_rigidbody = GetComponent<Rigidbody2D>();
		}

		void Start()
		{
			_rigidbody.bodyType = RigidbodyType2D.Dynamic;
			_rigidbody.interpolation = RigidbodyInterpolation2D.Interpolate;
			_rigidbody.gravityScale = 0;
			GiveImpulse();
		}

		void GiveImpulse()
		{
			_direction = (Random.Range(0, 2) == 0) ? -1 : 1;
			_rigidbody.velocity = new Vector2(_direction * speed, 0f);
		}

		void OnBecameInvisible()
		{
			GameManager.instance.GameOver();
			GiveImpulse();
		}

		void OnCollisionEnter2D(Collision2D collision)
		{
			if (collision.gameObject.CompareTag("Border"))
				GameManager.instance.PlayBorderSFX();
		}
	}
}   