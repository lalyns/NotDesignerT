﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed;

    public int _CurrentLevel = 0;

    AudioSource audioSource;

    Vector3 tDir;
    Vector3 move;
    Vector3 oldmove;
    Vector3 target;
    Vector3 oldBoxPos;
    bool moveCheck;

    bool isBlock = false;
    GameObject Block;

    GameObject Ground;

    float moveTimer;

    public LightSource light;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
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

        if (!moveCheck)
        {
            if (!isBlock) // 앞에 아무것도없다
            {
                // 앞으로갈지 아래로갈지 못움직일지
                RaycastHit hit;

                if (Physics.Raycast(Ground.transform.position, tDir, out hit, 1.0f)) // 못움직인다.
                {
                    if (hit.transform.gameObject.layer == GameLibrary.GameManager.LAYER_BLOCK)
                    {
                        StartMove();
                    }

                    if (hit.transform.gameObject.layer == GameLibrary.GameManager.LAYER_BOX)
                    {
                        StartMove();
                    }

                    if (hit.transform.gameObject.layer == GameLibrary.GameManager.LAYER_SHADOW)
                    {
                        MoveDownStair();
                    }

                }
                else
                {
                    moveCheck = true;
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
                
                if(Block.layer == GameLibrary.GameManager.LAYER_BLOCK)
                {
                    moveCheck = true;
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
                this.transform.rotation = Quaternion.LookRotation(new Vector3(-1, 0, 0));
                moveCheck = false;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                tDir = new Vector3(0, 0, 1);
                move = this.transform.position + new Vector3(0, 0, 1);
                this.transform.rotation = Quaternion.LookRotation(new Vector3(1, 0, 0));
                moveCheck = false;
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                tDir = new Vector3(1, 0, 0);
                move = this.transform.position + new Vector3(1, 0, 0);
                this.transform.rotation = Quaternion.LookRotation(new Vector3(0, 0, -1));
                moveCheck = false;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                tDir = new Vector3(-1, 0, 0);
                move = this.transform.position + new Vector3(-1, 0, 0);
                this.transform.rotation = Quaternion.LookRotation(new Vector3(0, 0, 1));
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
                audioSource.Play();
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
            oldBoxPos = Block.transform.position;
        }

        return isBlock;
    }

    public void PushBox()
    {
        Block.transform.position = Vector3.MoveTowards(Block.transform.position, oldBoxPos + tDir, speed * Time.deltaTime);

        Debug.Log(Vector3.Distance(Block.transform.position, oldBoxPos + tDir));

        if (Vector3.Distance(Block.transform.position, oldBoxPos + tDir) <= 0)
        {
            audioSource.Play();
            isBlock = true;
            moveCheck = true;
        }
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
            audioSource.Play();
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
            audioSource.Play();
            moveCheck = true;
            isBlock = false;
            moveTimer = 0;
        }
    }
}