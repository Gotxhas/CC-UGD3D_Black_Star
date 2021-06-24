﻿#define DEBUG_RocketShipController

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class RocketShipController : MonoBehaviour
{
    #region Variables Declaration
    [Header("Movement and Rotation Speeds")]
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private AudioClip _audioClip;

    private AudioSource _audioSource;
    //private float _startVolume;
    private Rigidbody _rigidbody;
    private float _horizontalInput;
    private float _verticalInput;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
        //_startVolume = _audioSource.volume;
    }

    // Update is called once per frame
    void Update()
    {
        RocketControllerMovement();
        RockectControllerRotation();
        PlayAudio(_audioClip);
    }

    #region RocketShip Movement

    private void RocketControllerMovement()
    {
        _verticalInput = Input.GetAxisRaw("Vertical");

        //moves rocketship according to input and coordinate system
        _rigidbody.AddRelativeForce(Vector3.up * _verticalInput * _movementSpeed, ForceMode.Force);

    }
    #endregion

    #region RocketShip Rotation
    private void RockectControllerRotation()
    {
        _horizontalInput = Input.GetAxis("Horizontal");

        _rigidbody.freezeRotation = true;

        //rotates rocketship according to input
        transform.Rotate(Vector3.back, _horizontalInput * Time.deltaTime * _rotationSpeed);

        _rigidbody.freezeRotation = false;
    }

    #endregion

    #region Collision Detection

    private void OnCollisionEnter(Collision collision)
    {

        switch (collision.gameObject.tag)
        {
            case "LaunchingPad":
#if DEBUG_RocketShipController
                Debug.Log("Ship is on LaunchingPad");
#endif
                break;
            case "LandingPad":
#if DEBUG_RocketShipController
                Debug.Log("Ship is on LandingPad");
#endif
                break;

            default:
#if DEBUG_RocketShipController
                Debug.Log("Death");
#endif
                Destroy(gameObject);
                break;

        }
    }


    private void OnCollisionExit(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "LaunchingPad":
#if DEBUG_RocketShipController
                Debug.Log("Ship is airborne");
#endif
                break;
        }
    }

    #endregion


    #region Audio

    private void PlayAudio(AudioClip index)
    {
        if (_verticalInput > 0 && !_audioSource.isPlaying)
        {
            _audioSource.clip = index;
            _audioSource.Play();
        }

        else if (_verticalInput <= 0 && _audioSource.isPlaying)
        {
            _audioSource.Stop();
            //StartCoroutine(FadeSoundOUt(_audioSource, 0.5f));
        }
    }

    //Fades out the sound of the rocket while vertical input is not pressed, but it does not work right. Needs more work 
    //private IEnumerator FadeSoundOUt(AudioSource audioSource, float fadeTime)
    //{
    //    float startVolume = _startVolume;

    //    while (audioSource.volume > 0)
    //    {
    //        audioSource.volume -= startVolume * Time.deltaTime / fadeTime;

    //        yield return null;
    //    }

    //    audioSource.Stop();
    //    audioSource.volume = startVolume;
    //}

    #endregion

}
