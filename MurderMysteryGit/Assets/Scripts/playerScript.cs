using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class playerScript : NetworkBehaviour
{
    [SerializeField]
    LineRenderer ForwardIndicatorPrefab;
    LineRenderer fwdInd;
    [SerializeField]
    GameObject NameTagPrefab;
    [HideInInspector]
    public GameObject nameTag;

    public Vector3 velocity;

    CapsuleCollider myCollider;

    int layermask;

    Vector3 gNormal = Vector3.up;

    float topSpeed = 5f;

    bool canPickUp = true;

    public GameEvents gameEvents;

    public static event Action<playerScript> playerCreated;

    gunScript gun;

    public bool grounded;

    [SyncVar]
    public bool imposter = false;

    List<InputSnap> inputList = new List<InputSnap>();

    bool jumped = false;


    InputSnap[] clntPrdctnBffr = new InputSnap[100];


    public void Awake()
    {
        gameEvents = FindObjectOfType<GameEvents>();
        myCollider = GetComponent<CapsuleCollider>();
        layermask = ~(1 << 6);
        Clocky.instance.GameTick += update;
    }

    public void OnDestroy()
    {
        Clocky.instance.GameTick -= update;
    }

    public override void OnStartClient()
    {
        velocity = Vector3.zero;
        base.OnStartClient();
        if (hasAuthority)
        {
            Camera.main.GetComponent<cameraScript>().player = gameObject;
            fwdInd = Instantiate(ForwardIndicatorPrefab);
            tag = "Player";
            gameObject.layer = 6;

            gameEvents.toserver_sendName(netIdentity, gameEvents.playerUserName);
            gameEvents.server_requestNames(netIdentity);

            playerCreated?.Invoke(this);

            gameEvents.GameStateEntered.AddListener(handleStateChange);

            if (!isServer)
            {
                Clocky.instance.SendTime += sendPlayerInputToServer;
            }
        }
        else
        {
            tag = "EnemyPlayer";
            if (isServer)
            {
                gameObject.layer = 6;
            }
            else
            {
                gameObject.layer = 0;
            }
            nameTag = Instantiate(NameTagPrefab);
            nameTag.transform.SetParent(gameObject.transform);
            nameTag.transform.localPosition = Vector3.up * 1.2f;
            nameTag.GetComponent<TMPro.TMP_Text>().text = netId.ToString();
        }
        if (isServer)
        {
            Clocky.instance.SendTime += handleServerSendTime;
            Clocky.instance.SendTime += sendPlayerStateToClient;
        }
    }

    public override void OnStopClient()
    {
        base.OnStopClient();
        if (hasAuthority)
        {
            gameEvents.GameStateEntered.RemoveListener(handleStateChange);
            if (!isServer)
            {
                Clocky.instance.SendTime += sendPlayerInputToServer;
            }
        }
        if (isServer)
        {
            Clocky.instance.SendTime -= handleServerSendTime;
            Clocky.instance.SendTime -= sendPlayerStateToClient;
        }
    }

    void collision(ref bool _grounded)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 3f, layermask);
        for (int i = 0; i < colliders.Length; i++)
        {
            Transform t = colliders[i].transform;
            Vector3 direction;
            float distance;
            if (Physics.ComputePenetration(myCollider, transform.position, transform.rotation, colliders[i], t.position, t.rotation, out direction, out distance))
            {
                if (colliders[i].gameObject.tag == "GunCollectible")
                {
                    if (canPickUp)
                    {
                        gun = colliders[i].gameObject.GetComponent<gunScript>();
                        gun.collect(ref canPickUp);
                        gun.GunDropped += gunIsDropped;
                    }
                }
                else
                {
                    transform.position += direction * distance;
                    velocity = Vector3.ProjectOnPlane(velocity, direction);
                    if (Vector3.Angle(direction, Vector3.up) <= 60f)
                    {
                        gNormal = direction;
                        _grounded = true;
                    }
                }
            }
        }

    }

    public void setImposter(bool _imposter)
    {
        imposter = _imposter;
    }

    public void handleStateChange(GameEvents.GameState state)
    {
        Debug.Log(state.ToString());
    }

    private void Update()
    {
        jumped = Inputter.Instance.playerInput.actions["Jump"].WasPressedThisFrame() || jumped;
    }

    void update(float deltaTime)
    {
        if(hasAuthority && isServer)//HOST LOCAL PLAYER
        {
            Vector2 input = Inputter.Instance.playerInput.actions["Move"].ReadValue<Vector2>();
            movement(deltaTime, new InputSnap(input, 0, Camera.main.transform.right, jumped));
        }

        if(hasAuthority && !isServer)//CLIENT LOCAL PLAYER
        {
            Vector2 input = Inputter.Instance.playerInput.actions["Move"].ReadValue<Vector2>();
            InputSnap currentSnap = new InputSnap(input, Clocky.instance.tick, Camera.main.transform.right, jumped);
            inputList.Add(currentSnap);
            clntPrdctnBffr[Clocky.instance.tick % 100] = currentSnap;
            movement(deltaTime, currentSnap);
        }

        if(!hasAuthority && isServer)//SERVER PLAYER
        {
            InputSnap inputS = new InputSnap(Vector3.zero);

            //Parse through the input buffer to get the one corresponding to this tick if there is one
            while (inputList.Count > 0)//drop every late tick
            {
                if (inputList[0].tick < Clocky.instance.tick)
                {
                    Debug.Log("Input dropped, tick offset: " + (Clocky.instance.tick - inputList[0].tick).ToString());
                    inputList.RemoveAt(0);
                }
                else
                {
                    break;
                }
            }
            if (inputList.Count > 0)
            {
                if (inputList[0].tick == Clocky.instance.tick)
                {
                    inputS = inputList[0];
                    inputList.RemoveAt(0);
                }
            }

            movement(deltaTime, inputS);

        }

        jumped = false;
    }

    void handleServerSendTime()
    {
        RPC_SyncPositionToClients(netIdentity, transform.position);
    }

    [ClientRpc]
    void RPC_SyncPositionToClients(NetworkIdentity myId, Vector3 _position)
    {
        if (!myId.hasAuthority)
        {
            transform.position = _position;
        }
    }

    void sendPlayerInputToServer()
    {
        CMD_PlayerInput(inputList);
        inputList.Clear();
    }

    [Command(requiresAuthority = false)]
    void CMD_PlayerInput(List<InputSnap> _inputList)
    {
        for(int i = 0; i < _inputList.Count; i++)
        {
            inputList.Add(_inputList[i]);
        }
    }

    void sendPlayerStateToClient()
    {
        TRPC_PlayerState(GetComponent<NetworkIdentity>().connectionToClient, new PlayerState(transform.position, velocity, Clocky.instance.tick, gNormal, grounded));
    }
    
    [TargetRpc]
    void TRPC_PlayerState(NetworkConnection conn, PlayerState serverState)
    {
        
        transform.position = serverState.position;
        velocity = serverState.velocity;
        gNormal = serverState.gNormal;
        grounded = serverState.grounded;

        for(int i = serverState.tick + 1; i <= Clocky.instance.tick; i++)
        {
            movement(Clocky.instance.minTimeBetweenTicks, clntPrdctnBffr[i % 100]);
        }
        
    }

    public void movement(float deltaTime, InputSnap input)
    {
        Vector2 inputVec = input.moveVec;

        transform.position += velocity * deltaTime;

        grounded = false;
        collision(ref grounded);


        inputVec = Vector3.ClampMagnitude(inputVec, 1f);

        Vector3 flatRight = Vector3.ProjectOnPlane(input.camTransform, gNormal).normalized;
        Vector3 flatForward = Vector3.Cross(flatRight, gNormal).normalized;

        Vector3 delta = flatRight * inputVec.x + flatForward * inputVec.y;

        velocity -= Vector3.up * 13f * deltaTime;

        velocity += Vector3.ProjectOnPlane(delta * topSpeed - velocity, gNormal);

        //fwdInd.SetPosition(0, transform.position - transform.up * 0.5f);
        //fwdInd.SetPosition(1, fwdInd.GetPosition(0) + flatForward);

        
        if (grounded)
        {
            if (input.jumped)
            {
                velocity.y += 5f;
            }
        }
        else
        {
            gNormal = Vector3.up;
        }
        /*
        if (Inputter.Instance.playerInput.actions["Respawn"].WasPressedThisFrame())
        {
            local_Respawn();
        }

        if (Inputter.Instance.playerInput.actions["Crouch"].IsPressed())
        {
            myCollider.height = 1f;
        }
        else
        {
            myCollider.height = 2f;
        }
        */
    }

    void gunIsDropped()
    {
        canPickUp = true;
        gun.GunDropped -= gunIsDropped;
    }

    public void local_Respawn()
    {
        transform.position = new Vector3(0, 20, 0);
        velocity = Vector3.zero;
    }

    #region RPC

    [ClientRpc]
    public void toclient_Respawn()
    {
        if(hasAuthority)
        {
            if(gameEvents.gameState == GameEvents.GameState.murderMystery)
            {
                if (imposter)
                {
                    gameEvents.server_EnterGameState(GameEvents.GameState.gameEnding);
                }
                server_GhostMe();
            }
            local_Respawn();
        }
    }

    [Command(requiresAuthority = false)]
    public void toserver_Respawn()
    {
        toclient_Respawn();
    }

    [Command(requiresAuthority = false)]
    public void server_GhostMe()
    {
        client_GhostMe();
    }

    [ClientRpc]
    public void client_GhostMe()
    {
        if (!hasAuthority)
        {
            GetComponent<MeshRenderer>().enabled = false;
            nameTag.GetComponent<TMPro.TMP_Text>().alpha = 0;
        }
    }


    #endregion

}
