using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed;

    public int _CurrentLevel;

    CharacterController cc;
    Vector3 move;
    Vector3 target;
    bool moveCheck;

    public LightSource light;
    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        moveCheck = true;
    }

    // Update is called once per frame
    void Update()
    {
        GroundCheck();
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
    // 타겟지점으로 움직여주는함수
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
                if (Physics.Raycast(this.transform.position, move - this.transform.position, out RaycastHit hit, 0.7f, (1 << 10))) // 벽일경우
                {
                    if (Physics.Raycast(hit.transform.position, move - this.transform.position, 0.7f, (1 << 12) | (1 << 10))) // 벽일경우
                    {
                        moveCheck = true;
                        return;
                    }

                    Vector3 tmove = move;
                    tmove.x *= 2;
                    tmove.z *= 2;
                    hit.transform.position = Vector3.MoveTowards(hit.transform.position, tmove, speed * Time.deltaTime);
                }
                this.transform.position = Vector3.MoveTowards(this.transform.position, move, speed * Time.deltaTime);

            }

            if (Vector3.Distance(this.transform.position, move) == 0)
                moveCheck = true;
        }
    }

    // 맵을올라갈때 대각선을 타고 올라가게 해주는함수
    bool MoveRay()
    {
        float maxDistance = 0.2f;
        RaycastHit hit;

        Vector3 thisPos = this.transform.position;
        thisPos.y -= 0.4f;

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

    // 자기 앞에 무엇이 있는지 체크
    Vector3 MoveDir(Vector3 dir)
    {
        Vector3 returnVector = Vector3.zero;

        RaycastHit hit;

        Vector3 thisPos = this.transform.position;
        thisPos.y -= 0.4f;

        if(Physics.Raycast(thisPos, dir, out hit, this.transform.localScale.x, (1 << 12))) // 벽일경우
        {
            return returnVector; 
        }

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

        thisPos.y -= 0.4f;
        if (Physics.Raycast(thisPos, dir, out hit, this.transform.localScale.x, (1 << 9))) // 그림자블럭이 아래에있다면
        {
            returnVector.x = (light._Direction / 10) +1;
            returnVector.y = _CurrentLevel - hit.transform.parent.transform.GetComponent<ShadowCastObject>()._BlockLevel;

            returnVector.x *= dir.x;
            returnVector.y *= -1;
            returnVector.z *= dir.z;

            return returnVector;
        }

        return dir;
    }
      
    public void GroundCheck()
    {
        Ray ray = new Ray();
        ray.origin = this.transform.position;
        ray.direction = Vector3.down;

        RaycastHit[] hitAll = Physics.RaycastAll(ray.origin, ray.direction * 100f, 100f, 1 << 10, QueryTriggerInteraction.Ignore);
        foreach(RaycastHit hit in hitAll)
        {
            if (hit.transform == this.transform)
                continue;

            if(hit.transform.GetComponent<ShadowCastObject>() == null)
            {
                _CurrentLevel = 1;
            }
            else
            {
                _CurrentLevel = hit.transform.GetComponent<ShadowCastObject>()._BlockLevel + 1;
                break;
            }
        }
    }
}




/// 벽못뚫음
/// 앞에벽없으면 못감