using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid
{
    public class PlayerSide : MonoBehaviour
    {
        [Tooltip("Player side"), SerializeField]
        private PlayerSidePick playerNumber;

        public PlayerSidePick GetNumber => playerNumber;

        public enum PlayerSidePick
        {
            FirstPlayer,
            SecondPlayer
        }
    }
}