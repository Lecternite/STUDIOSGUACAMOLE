using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class forwardIndicatorScript : MonoBehaviour
{

    GameObject player;
    LineRenderer lr;
    Vector3 direction;

    public void setPlayer(GameObject _player)
    {
        player = _player;
    }

    public void setDirection(Vector3 _direction)
    {
        direction = _direction;
    }

    // Start is called before the first frame update


}
