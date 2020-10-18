using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player : MonoBehaviour
{
    //public or private reference
    //data type(int, float, bool, string)
    //every variable has a name
    //optional value assigned
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;
    [SerializeField]
    private float _tripleShotTime = 5.0f;
    [SerializeField]
    private float _speedBoostTime = 5.0f;
    [SerializeField]
    private float _speed = 5.0f;
    [SerializeField]
    private float _speedBoostSpeed = 8.5f;
    
    private bool _speedBoostActive = false;
    private bool _shieldsActive;
    private bool _tripleShotActive = false;
    
    
    [SerializeField]
    private GameObject _playerShield;
    [SerializeField]
    private GameObject _leftEngine, _rightEngine;
    [SerializeField]    
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _playerExplosion;
    [SerializeField]
    private AudioSource _laserAudioSource;
    private AudioSource _explosionAudioSource;
    private Animator _animator;


    [SerializeField]
    private int _score;
    [SerializeField]
    private int _lives = 3;

    [SerializeField]
    private UIManager _uiManager;
    private SpawnManager _spawnManager;
    
    
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0,0,0);
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is null!");
        }

        if (_uiManager == null)
        {
            Debug.Log("UI Manager is null!");
        }
        
        _leftEngine.SetActive(false); //left engine explosion
        _rightEngine.SetActive(false); //right engine explosion

        _laserAudioSource = GameObject.Find("Laser_Sound").GetComponent<AudioSource>();
        if (_laserAudioSource == null)
        {
            Debug.LogError("Laser audio source is null!");
        }
        
        _explosionAudioSource = GameObject.Find("Explosion_Sound").GetComponent<AudioSource>();
        if (_explosionAudioSource == null)
        {
            Debug.LogError("Explosion audio source is null!");
        }
        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("Player animator is null!");
        }

    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        if (Input.GetKeyDown(KeyCode.Space) && (Time.time > _canFire))
        {
            ShootLaser();
        }
    }


    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.D) == true)
        {
            _animator.SetBool("PlayerTurnRightBool", true); 
        }
        else if (Input.GetKey(KeyCode.D) == false)
        {
            _animator.SetBool("PlayerTurnRightBool", false);
        }
        if (Input.GetKey(KeyCode.A) == true)
        {
            _animator.SetBool("PlayerTurnLeftBool", true);
        }
        else if (Input.GetKey(KeyCode.A) == false)
        {
            _animator.SetBool("PlayerTurnLeftBool", false);
        }

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
      
        if (_speedBoostActive)
        {
            transform.Translate(direction * _speedBoostSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(direction * _speed * Time.deltaTime);
        }

        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y <= -3.8f)
        {
            transform.position = new Vector3(transform.position.x, -3.8f, 0);
        }

        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }

    }

    void ShootLaser()
    {
        _canFire = Time.time + _fireRate;
        
        if (_tripleShotActive)
        {
            Instantiate(_tripleShotPrefab, transform.position + new Vector3(-.967f, .4730f, -3.446f), Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, .75f, 0), Quaternion.identity);
        }

        _laserAudioSource.Play();
    }

    public void Damage()
    {
        if (_shieldsActive)
        {
            _shieldsActive = false;
            _playerShield.SetActive(false);
            return;
        }

        _lives--;
        _uiManager.UpdateLives(_lives);

        if (_lives == 2)
        {
            _leftEngine.SetActive(true);
        }
        if (_lives == 1)
        {
            _rightEngine.SetActive(true);
        }   

        if(_lives < 1)
        {
            _spawnManager.OnPlayerDeath(); //stops spawning enemies and powerups
            _uiManager.GameOver();
            Instantiate(_playerExplosion, transform.position, Quaternion.identity);
            _explosionAudioSource.Play();
            Destroy(this.gameObject);
        }
    }

    public void TripleShotActive()
    {
        _tripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());

    }

    public void SpeedBoostActive()
    {
        _speedBoostActive = true;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    public void ShieldsActive()
    {
        _shieldsActive = true;
        _playerShield.SetActive(true);
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(_tripleShotTime);
        _tripleShotActive = false;
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(_speedBoostTime);
        _speedBoostActive = false;
    }

    public void IncreaseScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "EnemyLaser")
        {
            Damage();
            _explosionAudioSource.Play();
            Destroy(other.gameObject);
        }    
    }

}
