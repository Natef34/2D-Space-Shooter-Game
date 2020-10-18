using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{

    [SerializeField]
    private float _speed = 3.0f;
    [SerializeField] //0=tripleshot, 1=speed, 2=shield
    private int powerupID;
    private AudioSource _powerupSound;

    private void Start() 
    {
        _powerupSound = GameObject.Find("Powerup_Sound").GetComponent<AudioSource>();
        if (_powerupSound == null)
        {
            Debug.LogError("Power up audio source is null!");
        }   
    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -6)
        {
            Destroy(this.gameObject);
        }
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                switch(powerupID)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        break;
                    case 2:
                        player.ShieldsActive();
                        break;
                    default:
                        Debug.Log("Default value");
                        break;
                }
            }
            _powerupSound.Play();
            Destroy(this.gameObject);
        }
    }
}
