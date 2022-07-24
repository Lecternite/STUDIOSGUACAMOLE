using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class gunScript : MonoBehaviour
{
    Vector3 defaultPos = new Vector3(0.4f, -0.25f, 0.3f);
    Vector3 scopedPos = new Vector3(0f, -0.175f, 0.4f);

    float value = 0;
    float v = 0;

    [HideInInspector]
    public Image pointer;

    [SerializeField]
    ParticleSystem spark;

    float recoil = 0;

    int layerMask;

    int mode = 0;

    Vector3 startPos;
    Quaternion startRot;
    float startTime;

    float cooldown = 0;

    SphereCollider col;

    cameraScript camScript;

    public event Action GunDropped;

    float amount = 0f;

    Vector3 offset;


    private void Awake()
    {
        layerMask = ~(1 << 6);
        col = GetComponent<SphereCollider>();
        camScript = FindObjectOfType<cameraScript>();
        camScript.cameraUpdated += OnCamUpdate;
        pointer = GameObject.FindGameObjectWithTag("CrossHair").GetComponent<Image>();
    }
    private void OnDestroy()
    {
        camScript.cameraUpdated -= OnCamUpdate;
    }

    public void collect(ref bool canpickup)
    {
        if (cooldown < 0.001f)
        {
            mode = 1;
            startPos = transform.position;
            startRot = transform.rotation;
            startTime = Time.time;

            col.enabled = false;

            canpickup = false;
        }
    }

    public void drop()
    {
        mode = 0;
        col.enabled = true;
        cooldown = 1f;
        GunDropped?.Invoke();
    }

    private void OnCamUpdate()
    {
        if(mode == 0)
        {
            return;
        }
        else if(mode == 1)
        {
            float t = (Time.time - startTime) * 5f;
            transform.position = Vector3.Lerp(startPos, Camera.main.transform.TransformPoint(Vector3.Lerp(defaultPos, scopedPos, v)), t);
            transform.rotation = Quaternion.Slerp(startRot, Camera.main.transform.localToWorldMatrix.rotation, t);
            if (t > 1f)
            {
                mode = 2;
            }
        }
        else
        {
            if (Inputter.Instance.playerInput.actions["Drop"].WasPressedThisFrame())
            {
                drop();
                return;
            }
            if (Inputter.Instance.playerInput.actions["Scope"].IsPressed())
            {
                value += 10 * Time.deltaTime;
                value = Mathf.Clamp01(value);
            }
            else
            {
                value -= 10 * Time.deltaTime;
                value = Mathf.Clamp01(value);
            }
            v = Mathf.Sin(Mathf.PI * value / 2f);


            if (Inputter.Instance.playerInput.actions["Fire"].WasPressedThisFrame())
            {
                recoil = 70;
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 100f, layerMask))
                {
                    if (hit.rigidbody != null)
                    {
                        hit.rigidbody.AddForceAtPosition(Camera.main.transform.forward * 5f, hit.point, ForceMode.Impulse);
                    }
                    if (hit.collider.gameObject.tag == "EnemyPlayer")
                    {
                        hit.collider.gameObject.GetComponent<playerScript>().toserver_Respawn();
                    }
                    Instantiate(spark, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                }
            }
            recoil -= 300 * Time.deltaTime;
            recoil = Mathf.Max(0f, recoil);


            Camera.main.fieldOfView = Mathf.Lerp(80f, 50f, v);
            pointer.color = new Color(pointer.color.r, pointer.color.g, pointer.color.b, 1f - v);

            if (Camera.main.GetComponent<cameraScript>().player.GetComponent<playerScript>().grounded)
            {
                amount = Mathf.Lerp(amount, Camera.main.GetComponent<cameraScript>().player.GetComponent<playerScript>().velocity.magnitude / 7f, 5f * Time.deltaTime);
                offset = Vector3.Lerp(offset, new Vector3(MathF.Sin(6f * Time.timeSinceLevelLoad) / 10f, (MathF.Abs(MathF.Cos(6f * Time.timeSinceLevelLoad)) - 0.5f) / 15f, 0) * amount, Time.deltaTime * 10f);
            }
            else
            {
                offset = Vector3.Lerp(offset, -Vector3.up * Camera.main.GetComponent<cameraScript>().player.GetComponent<playerScript>().velocity.y / 50f, Time.deltaTime * 10f);
            }

            transform.position = Camera.main.transform.TransformPoint(Vector3.Lerp(defaultPos + offset, scopedPos, v));
            Quaternion tilt = Quaternion.AngleAxis(recoil, Vector3.left);
            transform.rotation = Camera.main.transform.localToWorldMatrix.rotation * tilt;
        }
    }

    private void Update()
    {
        cooldown = Mathf.Clamp01(cooldown - Time.deltaTime);
    }
}
