using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class in charge of keeping track of the user's input.
/// </summary>
public class UserInput : MonoBehaviour
{
    /* In a calm area, Controls restricted to Move, Action
     * In streets, can attack
     */
    private byte attacksPressed;
    private PlayerMove playerMove;
    private PlayerAttack playerAttack;
    private PlayerAction playerAction;
    private UnitAnimationLayers unitAnimationLayers;
    private Vector2 directionalInput;

    private void Awake()
    {
        playerMove = GetComponent<PlayerMove>();
        playerAttack = GetComponent<PlayerAttack>();
        playerAction = GetComponent<PlayerAction>();
        unitAnimationLayers = GetComponent<UnitAnimationLayers>();
    }
    private void Update()
    {
        //Inputs independent on if the game is paused or not.
        if (Input.GetKeyDown(KeyCode.P))
        {
            GameManager.Instance.PauseGame();
        }

        //Ignore any normal gameplay if game is paused.
        if (GameManager.Instance.GamePaused())
        {
            return;
        }
        //Movement
        directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        //Action/Jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (playerAction.HasInteractable())
            {
                //interact
                playerAction.Interact();
            }
            else
            {
                if (playerMove.Grounded())
                {
                    playerMove.Jump(directionalInput);
                }
            }
        }
        if (playerMove.Grounded())
        {
            playerMove.Move(directionalInput);
        }
        //Attacking
        //TO GRAB: Punch + Kick
        if (Input.GetKeyDown(KeyCode.J))
        {
            //Punch
            attacksPressed |= 0x1;
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            //Kick
            attacksPressed |= 0x2;
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            //TODO: Make Special
            attacksPressed |= 0x4;
        }
        if (attacksPressed > 0)
        {
            playerAttack.MakeAttack(GetFlippedByte(DirectionByte()), attacksPressed);
            attacksPressed = 0;
        }

        //Debugging Commands
        if (Input.GetKeyDown(KeyCode.O))
        {
            unitAnimationLayers.SetAttackLayer();
        }
    }
    /// <summary>
    /// Return the correct byte regardless where the player is facing (e.g. if facing Left, holding left will result in 6)
    /// </summary>
    /// <returns></returns>
    private byte GetFlippedByte(byte directionByte)
    {
        //BASE CASE 1: If byte greater than 9, return neutral
        if (directionByte > 9)
        {
            return 5;
        }
        //BASE CASE 1: If already facing right, return the original
        if (Mathf.Sign(transform.localScale.x) >= 0)
        {
            return directionByte;
        }
        else
        {
            //If up, down, or neutral; return the original
            if ((directionByte == 2) || (directionByte == 5) || (directionByte == 8))
            {
                return directionByte;
            }
            else
            {
                //If divisible by 3, subtract 2; otherwise, add 2
                if (directionByte % 3 == 0)
                {
                    return (directionByte -= 2);
                }
                else
                {
                    return directionByte += 2;
                }
            }
        }
    }
    /// <summary>
    /// Return a single numbe based on the directional input.
    /// </summary>
    /// <returns></returns>
    private byte DirectionByte()
    {
        if (directionalInput.x < 0)
        {
            if (directionalInput.y > 0)
            {
                return 7;
            }
            else if (directionalInput.y == 0)
            {
                return 4;
            }
            else
            {
                return 1;
            }
        }
        else if (directionalInput.x == 0)
        {
            if (directionalInput.y > 0)
            {
                return 8;
            }
            else if (directionalInput.y == 0)
            {
                return 5;
            }
            else
            {
                return 2;
            }
        }
        else
        {
            if (directionalInput.y > 0)
            {
                return 9;
            }
            else if (directionalInput.y == 0)
            {
                return 6;
            }
            else
            {
                return 3;
            }
        }
    }
}
