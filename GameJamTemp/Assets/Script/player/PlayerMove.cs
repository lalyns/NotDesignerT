﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed;

    CharacterController cc;
    Vector3 move;
    Vector3 target;
    bool moveCheck;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        moveCheck = true;
    }

    // Update is called once per frame
    void Update()
    {
        InputMove();
        StartMove();
    }

    void InputMove()
    {
        if (moveCheck)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                move = this.transform.position + MoveDir(new Vector3(0, 0, -1));
                moveCheck = false;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                move = this.transform.position + MoveDir(new Vector3(0, 0, 1));
                moveCheck = false;
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                move = this.transform.position + MoveDir(new Vector3(1, 0, 0));
                moveCheck = false;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                move = this.transform.position + MoveDir(new Vector3(-1,0,0));
                moveCheck = false;
            }
        }
    }

    void StartMove()
    {
        if (!moveCheck)
        {
            // 벽이라면 ray로 타겟을만들어서?
            if (MoveRay())
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, target, speed * Time.deltaTime);
            }
            else
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, move, speed * Time.deltaTime);
            }

            if (Vector3.Distance(this.transform.position, move) == 0)
                moveCheck = true;
        }
    }

    bool MoveRay()
    {
        float maxDistance = 0.2f;
        RaycastHit hit;

        Vector3 thisPos = this.transform.position;
        thisPos.y -= 0.5f;

        Vector3 tmove = move;
        tmove.y = thisPos.y;
        Vector3 rayDir = tmove - thisPos;

        if (Physics.Raycast(thisPos, rayDir, out hit, maxDistance, (1 << 9)))
        {
            target = hit.point;
            target.y += 0.6f;
            return true;
        }

        return false;
    }

    // 내가지금 가야할거리가 몇x 몇y인가 
    Vector3 MoveDir(Vector3 dir)
    {
        Vector3 returnVector = Vector3.zero;

        RaycastHit hit;

        Vector3 thisPos = this.transform.position;
        thisPos.y -= 0.4f;

        if (Physics.Raycast(thisPos, dir, out hit, this.transform.localScale.x, (1 << 9))) // 그림자블럭이있는지판단
        {
            if (Physics.Raycast(thisPos, dir, out hit, 1000, (1 << 10))) // 다음블럭이 몇번째 블럭인지 판단
            {
                Vector3 hitPos = hit.point;
                returnVector.x = (int)Vector3.Distance(thisPos, hitPos) + 1;

                if (Physics.Raycast(hitPos, Vector3.up, out hit, 1000, (1 << 11)))
                {
                    returnVector.y = (int)Vector3.Distance(hitPos, hit.point);
                }
            }

            returnVector.x *= dir.x;
            returnVector.z *= dir.z;
            return returnVector;
        }

        thisPos.y -= 1.2f;

        if (Physics.Raycast(thisPos, dir, out hit, this.transform.localScale.x, (1 << 9))) // 그림자블럭이 아래에있다면
        {
            if (Physics.Raycast(thisPos, dir, out hit, 1000, (1 << 10))) // 다음블럭이 몇번째 블럭인지 판단
            {
                Vector3 hitPos = hit.point;
                returnVector.x = (int)Vector3.Distance(thisPos, hitPos) + 2;

                if (Physics.Raycast(hitPos, Vector3.up, out hit, 1000, (1 << 11)))
                {
                    returnVector.y = -1 * ((int)Vector3.Distance(hitPos, hit.point));
                }
            }

            returnVector.x *= dir.x;
            returnVector.z *= dir.z;
            return returnVector;
        }

        return dir;
    }

         
}



///내가 큐브위에올라가있을때 아래 그림자가있다면
/// x만큼앞으로
/// y만큼아래로