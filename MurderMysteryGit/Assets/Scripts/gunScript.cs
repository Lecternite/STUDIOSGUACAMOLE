using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Mirror;

public class gunScript : NetworkBehaviour
{
    public GameObject cubePrefab;

    Vector3 defaultPos = new Vector3(0.4f, -0.25f, 0.3f);
    Vector3 scopedPos = new Vector3(0f, -0.175f, 0.4f);

    float value = 0;
    float v = 0;

    [HideInInspector]
    public Image pointer;

    [SerializeField]
    ParticleSystem spark;

    float recoil = 0;

    Vector3 startPos;
    Quaternion startRot;
    float startTime;

    float cooldown = 0;

    SphereCollider col;

    cameraScript camScript;

    public event Action GunDropped;

    float amount = 0f;

    Vector3 offset;

    bool playerShotThisFrame = false;

    GameEvents gameEvents;

    public struct RayCommand
    {
        public Ray ray;
        public int tick;
        public float lagState;
        public RayCommand(Ray _ray, int _tick, float _lagState)
        {
            ray = _ray;
            tick = _tick;
            lagState = _lagState;
        }
    }

    public enum GunState
    {
        Idle,
        TransitioningToHeld,
        Held,
    }

    GunState mode = GunState.Idle;

    public static List<RayCommand> rayCmdList = new List<RayCommand>();

    private void Awake()
    {
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
            mode = GunState.TransitioningToHeld;
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
        if(mode == GunState.Idle)
        {
            return;
        }
        else if(mode == GunState.TransitioningToHeld)
        {
            float t = (Time.time - startTime) * 5f;
            transform.position = Vector3.Lerp(startPos, Camera.main.transform.TransformPoint(Vector3.Lerp(defaultPos, scopedPos, v)), t);
            transform.rotation = Quaternion.Slerp(startRot, Camera.main.transform.localToWorldMatrix.rotation, t);
            if (t > 1f)
            {
                mode = GunState.Held;
            }
        }
        else if(mode == GunState.Held)
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

    [Command(requiresAuthority = false)]
    void CMD_Shoot(Ray ray, int tick, float lagState)
    {
        rayCmdList.Add(new RayCommand(ray, tick, lagState));
    }

    private void Start()
    {
        Clocky.instance.GameTick += update;
        gameEvents = FindObjectOfType<GameEvents>();
    }

    private void Update()
    {
        cooldown = Mathf.Clamp01(cooldown - Time.deltaTime);
        playerShotThisFrame = Inputter.Instance.playerInput.actions["Fire"].WasPressedThisFrame() || playerShotThisFrame;
    }

    void processRay(RayCommand rayCommand)
    {
        int tick = (int)rayCommand.lagState;

        RaycastHit hit;
        if (EntityHistory.Instance.RayPast(tick, rayCommand.ray, 100f, out hit))
        {
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForceAtPosition(Camera.main.transform.forward * 5f, hit.point, ForceMode.Impulse);
            }
            if (hit.collider.gameObject.tag == "Player")
            {
                hit.collider.gameObject.GetComponent<playerScript>().Respawn();
            }
            if (hit.collider.gameObject.tag == "Lag Tester")
            {
                hit.collider.gameObject.GetComponent<EntityLagTesterScript>().RPC_Indicate();
            }
            Instantiate(spark, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
        }
    }

    void update(float deltaTime)
    {

        if (!isServer)
        {
            gameEvents.updateLaggyState();
        }

        if (playerShotThisFrame && mode == GunState.Held)
        {
            recoil = 70;
            if (isServer)
            {
                CMD_Shoot(new Ray(Camera.main.transform.position, Camera.main.transform.forward), Clocky.instance.tick, Clocky.instance.tick * 1f);

            }
            else
            {
                CMD_Shoot(new Ray(Camera.main.transform.position, Camera.main.transform.forward), Clocky.instance.tick, gameEvents.lagTesterState);
                if (isClientOnly)
                {
                    RaycastHit hit0;
                    if (Physics.Raycast(new Ray(Camera.main.transform.position, Camera.main.transform.forward), out hit0, 100f))
                    {
                        Instantiate(spark, hit0.point, Quaternion.FromToRotation(Vector3.forward, hit0.normal));
                    }
                }

            }
        }


        if (isServer)
        {
            while (rayCmdList.Count > 0)//drop every late tick
            {
                if (rayCmdList[0].tick < Clocky.instance.tick)
                {
                    Debug.Log("Shoot command dropped, tick offset: " + (Clocky.instance.tick - rayCmdList[0].tick).ToString());
                    rayCmdList.RemoveAt(0);
                }
                else
                {
                    break;
                }
            }
            if (rayCmdList.Count > 0)//process the oldest tick if it matches the current tick
            {
                if (rayCmdList[0].tick == Clocky.instance.tick)
                {
                    processRay(rayCmdList[0]);
                    rayCmdList.RemoveAt(0);
                }
            }
        }

        playerShotThisFrame = false;
    }
}