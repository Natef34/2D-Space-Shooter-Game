using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    private float _startX = 0f;
    private float _laserWaitTime;

    private Player _player;
    private Animator _animator;
    private AudioSource _explosionAudioSource;
    private AudioSource _laserAudioSource;
    [SerializeField]
    private EnemyLaser _enemyLaser;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
          Debug.LogError("Player is null!");
        }

        _animator = gameObject.GetComponent<Animator>();
        if (_animator == null)
        {
          Debug.LogError("Animator is null!");
        }

        _explosionAudioSource = GameObject.Find("Explosion_Sound").GetComponent<AudioSource>();
        if (_explosionAudioSource == null)
        {
          Debug.LogError("Explosion audio source is null!");
        }

        _laserAudioSource = GameObject.Find("Laser_Sound").GetComponent<AudioSource>();
        if (_laserAudioSource == null)
        {
          Debug.Log("Laser audio source is null!");
        }

        StartCoroutine (FireLaser());
    }

    // Update is called once per frame
    void Update()
    {
      CalculateMovement();
    }

    void CalculateMovement()
    {
      transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -6)
        {
            _startX = Random.Range(-7, 8);
            transform.position = new Vector3(_startX,7,0);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
          if (_player != null)
          {
              _player.Damage();
          }
          _animator.SetTrigger("OnEnemyDeath");
          _speed = 0;
          Destroy(transform.GetComponent<Collider2D>());
          _explosionAudioSource.Play();
          Destroy(this.gameObject, 2.8f);
        }
        
        if (other.tag == "Laser")
        {
          Destroy(other.gameObject);
          if (_player != null)
          {
            _player.IncreaseScore(10);
          }
          _animator.SetTrigger("OnEnemyDeath");
          _speed = 0;
          Destroy(transform.GetComponent<Collider2D>());
          _explosionAudioSource.Play();
          Destroy(this.gameObject, 2.8f);
        }
    }

    IEnumerator FireLaser()
    {
      _laserWaitTime = Random.Range(.0f,1.5f);
      yield return new WaitForSeconds(_laserWaitTime);
      Instantiate(_enemyLaser, this.transform.position, Quaternion.identity);
      _laserAudioSource.Play();
    }       
}
