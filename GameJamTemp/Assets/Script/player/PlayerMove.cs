using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed;

    public int _CurrentLevel = 0;

    Vector3 tDir;
    Vector3 move;
    Vector3 oldmove;
    Vector3 target;

    bool moveCheck;

    bool isBlock = false;
    GameObject Block;

    GameObject Ground;

    float moveTimer;

    public LightSource light;
    // Start is called before the first frame update
    void Start()
    {
        moveCheck = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.anyKeyDown && moveCheck)
        {
            isBlock = ForwardCheck();
            Ground = GroundCheck();

            InputMove();
        }



        Debug.Log(isBlock);
        if (!moveCheck)
        {
            if (!isBlock) // 앞에 아무것도없다
            {
                // 앞으로갈지 아래로갈지 못움직일지
                RaycastHit hit;

                if (Physics.Raycast(Ground.transform.position, tDir, out hit, 1.0f)) // 못움직인다.
                {
                    Debug.Log(hit.transform.gameObject.name);
                    if (hit.transform.gameObject.layer == GameLibrary.GameManager.LAYER_BLOCK)
                    {
                        StartMove();
                    }

                    if (hit.transform.gameObject.layer == GameLibrary.GameManager.LAYER_SHADOW)
                    {
                        MoveDownStair();
                    }
                }


            }
            else // 앞에 무언가있다.
            {
                if (Block.layer == GameLibrary.GameManager.LAYER_SHADOW)
                {
                    MoveUpStair();
                }
                if (Block.layer == GameLibrary.GameManager.LAYER_BOX)
                {
                    PushBox();
                }
            }
        }
    }

    void InputMove()
    {
        if (moveCheck)
        {

            oldmove = this.transform.position;

            if (Input.GetKeyDown(KeyCode.W))
            {
                tDir = new Vector3(0, 0, -1);
                move = this.transform.position + new Vector3(0, 0, -1);
                moveCheck = false;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                tDir = new Vector3(0, 0, 1);
                move = this.transform.position + new Vector3(0, 0, 1);
                moveCheck = false;
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                tDir = new Vector3(1, 0, 0);
                move = this.transform.position + new Vector3(1, 0, 0);
                moveCheck = false;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                tDir = new Vector3(-1, 0, 0);
                move = this.transform.position + new Vector3(-1, 0, 0);
                moveCheck = false;
            }
        }
    }

    void StartMove()
    {
        if (!moveCheck && !isBlock)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, move, speed * Time.deltaTime);

            if (Vector3.Distance(this.transform.position, move) <= 0)
            {
                moveCheck = true;
            }
        }
    }

    public GameObject GroundCheck()
    {
        GameObject temp = null;

        Ray ray;
        RaycastHit hit;
        Vector3 tmove = this.transform.position;
        tmove.y -= 0.49f;

        if (Physics.Raycast(tmove, Vector3.down, out hit, 1.0f))
        {
            temp = hit.transform.gameObject;
        }

        return temp;

    }

    public bool ForwardCheck()
    {
        bool isBlock = false;
        moveTimer = 0;

        Vector3 direction = Vector3.zero;
        if (Input.GetKeyDown(KeyCode.W))
        {
            direction = Vector3.back;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            direction = Vector3.forward;
        }

        if(Input.GetKeyDown(KeyCode.A))
        {
            direction = Vector3.right;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            direction = Vector3.left;
        }

        RaycastHit hit;
        Vector3 tmove = this.transform.position;
        tmove.y -= 0.49f;

        if (Physics.Raycast(tmove, direction, out hit, 1f)) // 물체가있다.
        {
            isBlock = true;
            Block = hit.transform.gameObject;
        }

        return isBlock;
    }

    public void PushBox()
    {
        // 자기앞에 박스가있을대
        // 박스는 자기의속도만큼 이동한다.
        // 그러나 박스앞에 벽 or 박스 or 
    }

    
    public void MoveUpStair()
    {
        moveTimer += Time.deltaTime;
        //위로 한칸 앞으로두칸
        if(moveTimer < 0.2f)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, oldmove + tDir + new Vector3(0,1,0), speed * Time.deltaTime);
        }
        if(moveTimer >= 0.2f && moveTimer< 0.4f)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, oldmove + tDir + tDir + new Vector3(0, 1, 0), speed * Time.deltaTime);
        }
        if(moveTimer >= 0.4f)
        {
            moveCheck = true;
            isBlock = false;
            moveTimer = 0;
        }
    }

    public void MoveDownStair()
    {
        moveTimer += Time.deltaTime;
        //위로 한칸 앞으로두칸
        if (moveTimer < 0.4f)
        { // 앞으로
            this.transform.position = Vector3.MoveTowards(this.transform.position, oldmove + tDir + tDir - new Vector3(0, 1, 0), speed * Time.deltaTime);
        }
        
        if (moveTimer >= 0.4f)
        {
            moveCheck = true;
            isBlock = false;
            moveTimer = 0;
        }
    }
}