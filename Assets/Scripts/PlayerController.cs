using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    Rigidbody _rigidbody;
    Vector3 _start_pos;
    float TimeAccu = 0.0f;

    bool bReplaying = false;

    // Commands:
    Command cmd_W = new MoveForwardCommand();
    Command cmd_A = new MoveLeftCommand();
    Command cmd_S = new MoveBackwardCommand();
    Command cmd_D = new MoveRightCommand();

    Command cmdNothing = new DoNothingCommand();
    Command cmdForward = new MoveForwardCommand();
    Command cmdBackward = new MoveBackwardCommand();
    Command cmdLeft = new MoveLeftCommand();
    Command cmdRight = new MoveRightCommand();

    //ref Command rcmd = ref cmdNothing;

    Command _last_command = null;

    // Stacks to store the commands
    Stack<Command> _undo_commands = new Stack<Command>();
    Stack<Command> _redo_commands = new Stack<Command>();
    Stack<Command> _replay_commands = new Stack<Command>();

    public enum PlayerState
    {
        WALK,
        JUMP,
        CROUCH,
        REPLAY
    }

    public PlayerState _currentState;


    // Set a keybinding
    void SetCommand(ref Command cmd, ref Command new_cmd)
    {
        cmd = new_cmd;
    }

    void SwapCommands(ref Command A, ref Command B)
    {
        Command tmp = A;
        A = B;
        B = tmp;

    //    _undo_commands.Push();
    //    Command cmd = _undo_commands.Pop();
    }

    void ClearCommands()
    {
        SetCommand(ref cmd_W, ref cmdNothing);
        SetCommand(ref cmd_A, ref cmdNothing);
        SetCommand(ref cmd_S, ref cmdNothing);
        SetCommand(ref cmd_D, ref cmdNothing);
    }

    private void OnTriggerEnter(Collider other) {

        if (other.CompareTag("Coin"))
        {
            Destroy(other.gameObject); // Destroy the coin!
        }

        else if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground") { _currentState = PlayerState.WALK; }
    }
    private void OnCollisionExit(Collision collision) 
    {
        if (collision.gameObject.tag == "Ground") { _currentState = PlayerState.JUMP; }
    }

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _start_pos = transform.position;
    }

    IEnumerator Replay()
    {
        // Go through all the replay commands
        while (_replay_commands.Count > 0)
        {
            Command cmd = _replay_commands.Pop();
            _undo_commands.Push(cmd);
            cmd.Execute(_rigidbody);
            yield return new WaitForSeconds(.5f);
        }

        bReplaying = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch (_currentState)
        {
            case PlayerState.WALK:
                Console.WriteLine("WALK state");

                #region commands
                if (Input.GetKeyDown(KeyCode.W))
                {
                    _currentState = PlayerState.WALK;
                    cmd_W.Execute(_rigidbody);
                    _undo_commands.Push(cmd_W);
                    _redo_commands.Clear();
                    //_last_command = cmd_W;
                }
                if (Input.GetKeyDown(KeyCode.A))
                {
                    _currentState = PlayerState.WALK;
                    cmd_A.Execute(_rigidbody);
                    _undo_commands.Push(cmd_A);
                    _redo_commands.Clear();
                    //_last_command = cmd_A;
                }
                if (Input.GetKeyDown(KeyCode.S))
                {
                    _currentState = PlayerState.WALK;
                    cmd_S.Execute(_rigidbody);
                    _undo_commands.Push(cmd_S);
                    _redo_commands.Clear();
                    //_last_command = cmd_S;
                }
                if (Input.GetKeyDown(KeyCode.D))
                {
                    _currentState = PlayerState.WALK;
                    cmd_D.Execute(_rigidbody);
                    _undo_commands.Push(cmd_D);
                    _redo_commands.Clear();
                    //_last_command = cmd_D;
                }
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    _rigidbody.AddForce(10.0f * transform.up, ForceMode.Impulse);
                    _currentState = PlayerState.JUMP;
                }
                if (Input.GetKeyDown(KeyCode.LeftControl))
                {
                    //gameObject.transform.localScale -= Vector3.up * 0.5f;
                    _currentState = PlayerState.CROUCH;
                }
                #endregion commands

                break;

            case PlayerState.JUMP:
                Console.WriteLine("JUMP state");

                #region commands
                // no options?
                #endregion commands

                break;

            case PlayerState.CROUCH:
                Console.WriteLine("CROUCH state");

                #region commands
                if (Input.GetKeyDown(KeyCode.LeftControl))
                {
                    //gameObject.transform.localScale += Vector3.up * 0.5f; // uncrouch
                    _currentState = PlayerState.WALK;
                }
                #endregion commands

                break;

            case PlayerState.REPLAY:
                Console.WriteLine("REPLAY state");

                #region commands
                if (Input.GetKeyDown(KeyCode.R))
                {
                    _currentState = PlayerState.REPLAY;
                    bReplaying = true;
                    TimeAccu = 0.0f;

                    // Get the Undo-stack and "reverse" it
                    while (_undo_commands.Count > 0)
                    {
                        _replay_commands.Push(_undo_commands.Pop());
                    }
                    // Move the player to the start position
                    transform.position = _start_pos;

                    // Start the replay
                    StartCoroutine(Replay());
                }
                #endregion commands

                break;

            default:
                break;
        }


        if (bReplaying)
        {
            TimeAccu += Time.deltaTime;
            //...
        }

        else
        {

            //if (Input.GetKeyDown(KeyCode.R))
            //{
            //    _currentState = PlayerState.REPLAY;
            //    //bReplaying = true;
            //    TimeAccu = 0.0f;

            //    // Get the Undo-stack and "reverse" it
            //    while( _undo_commands.Count > 0)
            //    {
            //        _replay_commands.Push(_undo_commands.Pop());
            //    }
            //    // Move the player to the start position
            //    transform.position = _start_pos;

            //    // Start the replay
            //    StartCoroutine( Replay());

            //}

            //if (Input.GetKeyDown(KeyCode.W))
            //{
            //    cmd_W.Execute(_rigidbody);
            //    _undo_commands.Push(cmd_W);
            //    _redo_commands.Clear();
            //    //_last_command = cmd_W;
            //}
            //if (Input.GetKeyDown(KeyCode.A))
            //{
            //    cmd_A.Execute(_rigidbody);
            //    _undo_commands.Push(cmd_A);
            //    _redo_commands.Clear();
            //    //_last_command = cmd_A;
            //}
            //if (Input.GetKeyDown(KeyCode.S))
            //{
            //    cmd_S.Execute(_rigidbody);
            //    _undo_commands.Push(cmd_S);
            //    _redo_commands.Clear();
            //    //_last_command = cmd_S;
            //}
            //if (Input.GetKeyDown(KeyCode.D))
            //{
            //    cmd_D.Execute(_rigidbody);
            //    _undo_commands.Push(cmd_D);
            //    _redo_commands.Clear();
            //    //_last_command = cmd_D;
            //}
            //if (Input.GetKeyDown(KeyCode.Space))
            //{
            //    _currentState = PlayerState.JUMP;
            //    _rigidbody.AddForce(10.0f * transform.up, ForceMode.Impulse);
            //}

            if (Input.GetKeyDown(KeyCode.Z))
            {
                // If there are commands in the stack...
                if (_undo_commands.Count > 0)
                {
                    // ... pop one command out and execute it.
                    Command cmd = _undo_commands.Pop();
                    _redo_commands.Push(cmd);
                    cmd.Undo(_rigidbody);
                }
            }
            if (Input.GetKeyDown(KeyCode.X))
            {

                if (_redo_commands.Count > 0)
                {
                    Command cmd = _redo_commands.Pop();
                    _undo_commands.Push(cmd);
                    cmd.Execute(_rigidbody);
                }

            }

            // We can swap commands if we want to
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                //ClearCommands();
                //SwapCommands(ref cmd_A, ref cmd_D);
            }

        }

    }
}
