using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Moving Points")]
    [SerializeField] private bool horizontal;
    [SerializeField] private Transform topRightEdge;
    [SerializeField] private Transform bottomLeftEdge;

    [Header("Platform")]
    [SerializeField] private Transform platform;

    [Header("Movement Parameters")]
    [SerializeField] private float speed;

    [Header("Idle Behavior")]
    [SerializeField] private float idleDuration;
    private float idleTimer;
    private bool movingUp = true;
    private bool movingRight = true;

    private void Update()
    {
        if (horizontal)
        {
            if (movingRight)
            {
                if (platform.position.x <= topRightEdge.position.x)
                    MoveInDirectionHorizontal(1);
                else
                    DirectionChange();
            }
            else
            {
                if (platform.position.x >= bottomLeftEdge.position.x)
                    MoveInDirectionHorizontal(-1);
                else
                    DirectionChange();
            }
        }
        else
        {
            if (movingUp)
            {
                if (platform.position.y <= topRightEdge.position.y)
                    MoveInDirectionVertical(1);
                else
                    DirectionChange();
            }
            else
            {
                if (platform.position.y >= bottomLeftEdge.position.y)
                    MoveInDirectionVertical(-1);
                else
                    DirectionChange();
            }
        }
        
    }

    private void MoveInDirectionVertical(int direction)
    {
        idleTimer = 0;

        platform.position = new Vector3(platform.position.x, platform.position.y + Time.deltaTime * direction * speed, platform.position.z);
    }
    
    private void MoveInDirectionHorizontal(int direction)
    {
        idleTimer = 0;

        platform.position = new Vector3(platform.position.x + Time.deltaTime * direction * speed, platform.position.y, platform.position.z);
    }

    private void DirectionChange()
    {
        idleTimer += Time.deltaTime;

        if (idleTimer > idleDuration)
        {
            movingUp = !movingUp;
            movingRight = !movingRight;
        }
    }
}
