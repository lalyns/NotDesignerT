using System.Collections;
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
        MoveDir(new Vector3(-1, 0, 0));
        InputMove();
        StartMove();
    }

    void InputMove()
    {
        if (moveCheck)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                move = this.transform.position + new Vector3(0, 0, -1);
                moveCheck = false;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                move = this.transform.position + new Vector3(0, 0, 1);
                moveCheck = false;
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                move = this.transform.position + new Vector3(1, 0, 0);
                moveCheck = false;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                move = this.transform.position + new Vector3(-1,0,0);
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
        float maxDistance = 5;
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
        // ray발사 자기 바로 앞블럭이 그림자블럭이라면
        // 몇x만큼간다.
        // 몇y만큼간다.
        Vector3 returnVector = Vector3.zero;

        RaycastHit hit;

        Vector3 thisPos = this.transform.position;
        thisPos.y -= 0.4f;

        Debug.DrawRay(thisPos, dir);
        if (Physics.Raycast(thisPos, dir, out hit, this.transform.localScale.x, (1 << 9))) // 그림자블럭이있는지판단
        {
            if (Physics.Raycast(thisPos, dir, out hit, 1000, (1 << 10))) // 다음블럭이 몇번째 블럭인지 판단
            {
                //x 넣기
                returnVector.x = (int)Vector3.Distance(thisPos, hit.point);
                Debug.Log(returnVector);

                if (Physics.Raycast(hit.point, Vector3.up, out hit, 1000, (1 << 11)))
                {
                    //y 넣기
                }
            }
        }

        return Vector3.zero;
    }
}
