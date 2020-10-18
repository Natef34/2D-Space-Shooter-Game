using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotateSpeed = 25.0f;
    private Animator _animator;
    private SpawnManager _spawnManager;
    private AudioSource _explosionAudioSource;

    // Update is called once per frame
    private void Start() 
    {
        _animator = gameObject.GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("Animator is null!");
        } 

        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();

        _explosionAudioSource = GameObject.Find("Explosion_Sound").GetComponent<AudioSource>();
        if (_explosionAudioSource == null)
        {
            Debug.LogError("Explosion audio source is null!");
        }
    }
    void Update()
    {
        transform.Rotate((new Vector3(0, 0, _rotateSpeed) * Time.deltaTime));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.tag == "Player") || (other.tag == "Laser"))
        {
            _animator.SetTrigger("OnAsteroidDestroyed");
            _spawnManager.StartSpawning();
            Destroy(other.gameObject);
            Destroy(transform.GetComponent<CircleCollider2D>());
            _explosionAudioSource.Play();
            Destroy(this.gameObject, 2.8f);
        }    
    }
}
