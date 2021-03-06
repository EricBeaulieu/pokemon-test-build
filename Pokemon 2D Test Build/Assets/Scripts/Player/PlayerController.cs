using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed;
    LayerMask solidObjectLayermask;
    LayerMask grassLayer;

    bool _isMoving;
    bool _isRunning;

    public event Action OnEncounter;

    Vector2 _currentInput;

    Animator _anim;

    const float TILE_CENTER_OFFSET = 0.5f;
    const float STANDARD_WALKING_SPEED = 5f;
    const float STANDARD_RUNNING_SPEED = 12.5f;

    #region Getters/Setters

    bool isRunning
    {
        get { return _isRunning; }

        set
        {
            _isRunning = value;
            if (value == true)
            {
                movementSpeed = STANDARD_RUNNING_SPEED;
            }
            else
            {
                movementSpeed = STANDARD_WALKING_SPEED;
            }
        }
    }

    #endregion

    void Awake()
    {
        _anim = GetComponent<Animator>();

        solidObjectLayermask = LayerMask.GetMask("SolidObjects");
        grassLayer = LayerMask.GetMask("Grass");
    }

    public void HandleUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            isRunning = true;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isRunning = false;
        }

        if (_isMoving == false)
        {
            _currentInput.x = Input.GetAxisRaw("Horizontal");
            _currentInput.y = Input.GetAxisRaw("Vertical");

            //Removes Diagnol Input
            if (_currentInput.x != 0) _currentInput.y = 0;

            if (_currentInput != Vector2.zero)
            {
                Vector3 targetPos = transform.position;

                targetPos.x += _currentInput.x;
                targetPos.y += _currentInput.y;

                _anim.SetFloat("moveX", _currentInput.x);
                _anim.SetFloat("moveY", _currentInput.y);

                if(CheckIfWalkable(targetPos))
                {
                    StartCoroutine(MoveToPosition(targetPos));
                }
                else
                {
                    //Play bump into wall sound
                }

                
            }
        }

        _anim.SetBool("isMoving", _isMoving);
        _anim.SetBool("isRunning", isRunning);
    }

    bool CheckIfWalkable(Vector3 targetPos)
    {
        Vector2 targetPositionFixed = new Vector2(Mathf.FloorToInt(targetPos.x) + TILE_CENTER_OFFSET, Mathf.FloorToInt(targetPos.y) + TILE_CENTER_OFFSET);

        if(Physics2D.OverlapCircle(targetPositionFixed,0.25f,solidObjectLayermask) != null)
        {
            return false;
        }

        return true;
    }

    IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        _isMoving = true;

        while((targetPosition - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPosition;
        _isMoving = false;

        CheckForWildEncounter();
    }

    void CheckForWildEncounter()
    {
        if (Physics2D.OverlapCircle(transform.position, 0.25f, grassLayer) != null)
        {
            _anim.SetBool("isMoving", false);
            OnEncounter();
        }
    }
}
