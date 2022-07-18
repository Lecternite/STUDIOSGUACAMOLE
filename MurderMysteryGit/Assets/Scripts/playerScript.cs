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
    List<PlayerState> stateList = new List<PlayerState>();

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

            if (isServer)
            {
                Clocky.instance.SendTime += sendPlayerStateToClient;
            }

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
        else
        {
            if (isServer)
            {
                Clocky.instance.SendTime -= sendPlayerStateToClient;
            }
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



    void update(float deltaTime)
    {
        if(hasAuthority && isServer)
        {
            Vector2 input = Inputter.Instance.playerInput.actions["Move"].ReadValue<Vector2>();
            movement(deltaTime, new InputSnap(input));
        }

        if (hasAuthority)//client or host local player
        {
            if (!isServer)
            {
                Vector2 input = Inputter.Instance.playerInput.actions["Move"].ReadValue<Vector2>();
                inputList.Insert(0, new InputSnap(input, Clocky.instance.tick));
            }
        }
        else
        {
            if (isServer)
            {
                InputSnap inputS;
                if(inputList.Count > 0)
                {
                    inputS = inputList[0];
                    inputList.RemoveAt(0);
                }
                else
                {
                    inputS = new InputSnap(Vector2.zero);
                }

                movement(deltaTime, inputS);
            }
        }
    }




    void sendPlayerInputToServer()
    {
        //Debug.Log("Sending input buffer: " + inputList.Count.ToString());
        CMD_PlayerInput(inputList);
        inputList.Clear();
    }

    [Command(requiresAuthority = false)]
    void CMD_PlayerInput(List<InputSnap> _inputList)
    {
        if(inputList.Count > 0)
        {
            Debug.Log(inputList[0].moveVec.ToString());
        }
        inputList = _inputList;
    }

    void sendPlayerStateToClient()
    {
        TRPC_PlayerState(GetComponent<NetworkIdentity>().connectionToServer, new PlayerState(transform.position, velocity));
    }

    
    [TargetRpc]
    void TRPC_PlayerState(NetworkConnection conn, PlayerState serverState)
    {
        
    }
    

    public void movement(float deltaTime, InputSnap input)
    {
        Vector2 inputVec = input.moveVec;

        transform.position += velocity * deltaTime;

        grounded = false;
        collision(ref grounded);


        inputVec = Vector3.ClampMagnitude(inputVec, 1f);

        Vector3 flatRight = Vector3.ProjectOnPlane(Camera.main.transform.right, gNormal).normalized;
        Vector3 flatForward = Vector3.Cross(flatRight, gNormal).normalized;

        Vector3 delta = flatRight * inputVec.x + flatForward * inputVec.y;

        velocity -= Vector3.up * 13f * deltaTime;

        velocity += Vector3.ProjectOnPlane(delta * topSpeed - velocity, gNormal);

        //fwdInd.SetPosition(0, transform.position - transform.up * 0.5f);
        //fwdInd.SetPosition(1, fwdInd.GetPosition(0) + flatForward);

        /*
        if (grounded)
        {
            if (Inputter.Instance.playerInput.actions["Jump"].WasPressedThisFrame())
            {
                velocity.y += 5f;
            }
        }
        else
        {
            gNormal = Vector3.up;
        }

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

    public void setState(PlayerState state)
    {
        transform.position = state.position;
        velocity = state.velocity;
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
