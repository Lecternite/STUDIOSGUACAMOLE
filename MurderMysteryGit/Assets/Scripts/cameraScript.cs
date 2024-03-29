using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class cameraScript : MonoBehaviour
{
    [HideInInspector]
    public GameObject player;

    procedure player_procedure = () => { };

    public float rotX = 0;
    public float rotY = 0;

    Vector3 target = Vector3.zero;

    [SerializeField]
    float mouseSensivity;

    public event Action cameraUpdated;

    public void SetPlayer(GameObject _player)
    {
        player = _player;
        player_procedure = () =>
        {
            target = player.transform.position;
            transform.position = target + player.transform.up * 0.6f;
        };
    }

    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    void LateUpdate()
    {
        float multiplier;
        if (Inputter.Instance.playerInput.actions["Scope"].IsPressed())
        {
            multiplier = 0.55f;
        }
        else
        {
            multiplier = 1f;
        }

        rotY += Inputter.Instance.playerInput.actions["Look"].ReadValue<Vector2>().x * mouseSensivity * multiplier;
        rotX -= Inputter.Instance.playerInput.actions["Look"].ReadValue<Vector2>().y * mouseSensivity * multiplier;
        rotX = Mathf.Clamp(rotX, -90f, 90f);

        transform.rotation = Quaternion.Euler(rotX, rotY, 0);

        player_procedure();

        if (Inputter.Instance.playerInput.actions["MouseLock"].WasPressedThisFrame())
        {
            if (Cursor.visible)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }

        cameraUpdated?.Invoke();
    }
}
