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

    Vector3 velocity;

    CapsuleCollider myCollider;

    int layermask;

    Vector3 gNormal = Vector3.up;

    float topSpeed = 5f;

    bool canPickUp = true;

    public GameEvents gameEvents;

    public static event Action playerCreated;

    gunScript gun;


    public override void OnStartClient()
    {
        base.OnStartClient();
        if (base.hasAuthority)
        {
            gameEvents = FindObjectOfType<GameEvents>();

            myCollider = GetComponent<CapsuleCollider>();
            layermask = ~(1 << 6);
            Camera.main.GetComponent<cameraScript>().player = gameObject;
            fwdInd = Instantiate(ForwardIndicatorPrefab);
            tag = "Player";
            gameObject.layer = 6;

            gameEvents.toserver_sendName(netIdentity, gameEvents.playerUserName);
            gameEvents.toserver_requestNames(netIdentity);

            playerCreated?.Invoke();
        }
        else
        {
            tag = "EnemyPlayer";
            gameObject.layer = 0;
            nameTag = Instantiate(NameTagPrefab);
            nameTag.transform.SetParent(gameObject.transform);
            nameTag.transform.localPosition = Vector3.up * 1.2f;
            nameTag.GetComponent<TMPro.TMP_Text>().text = netId.ToString();
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


    void Update()
    {
        if (!base.hasAuthority)
        {
            return;
        }
        
        transform.position += velocity * Time.deltaTime;

        if (Inputter.Instance.playerInput.actions["Respawn"].WasPressedThisFrame())
        {
            local_Respawn();
        }

        gNormal = Vector3.up;
        bool grounded = false;
        collision(ref grounded);

        //if (grounded)
        //{
        //    RaycastHit hit;
        //    if (Physics.Raycast(transform.position - transform.up * myCollider.height/2f, -transform.up, out hit, myCollider.radius * 3f, layermask))
        //    {
        //        transform.position = hit.point + transform.up * (myCollider.height / 2f + myCollider.radius);
        //    }
        //
        //}

        if (Inputter.Instance.playerInput.actions["Jump"].WasPressedThisFrame() && grounded)
        {
            velocity.y += 5f;
        }

        Vector2 input = Inputter.Instance.playerInput.actions["Move"].ReadValue<Vector2>();

        input = Vector3.ClampMagnitude(input, 1f);

        Vector3 flatRight = Vector3.ProjectOnPlane(Camera.main.transform.right, gNormal).normalized;
        Vector3 flatForward = Vector3.Cross(flatRight, gNormal).normalized;

        Vector3 delta = flatRight * input.x + flatForward * input.y;

        velocity -= Vector3.up * 13f * Time.deltaTime;

        velocity += Vector3.ProjectOnPlane(delta * topSpeed - velocity, gNormal);

        fwdInd.SetPosition(0, transform.position - transform.up * 0.5f);
        fwdInd.SetPosition(1, fwdInd.GetPosition(0) + flatForward);

        if (Inputter.Instance.playerInput.actions["Crouch"].IsPressed())
        {
            myCollider.height = 1f;
        }
        else
        {
            myCollider.height = 2f;
        }
    }


    void gunIsDropped()
    {
        canPickUp = true;
        gun.GunDropped -= gunIsDropped;
    }


    #region Respawn()
    public void local_Respawn()
    {
        transform.position = new Vector3(0, 20, 0);
        velocity = Vector3.zero;
    }

    [ClientRpc]
    public void toclient_Respawn()
    {
        local_Respawn();
    }

    [Command(requiresAuthority = false)]
    public void toserver_Respawn()
    {
        toclient_Respawn();
    }
    #endregion

}
