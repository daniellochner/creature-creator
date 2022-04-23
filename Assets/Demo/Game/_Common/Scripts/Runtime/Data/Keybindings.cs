using System;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [Serializable]
    public class Keybindings : Data
    {
        #region Fields
        [SerializeField] private KeyCode walkForwards;
        [SerializeField] private KeyCode walkBackwards;
        [SerializeField] private KeyCode walkLeft;
        [SerializeField] private KeyCode walkRight;
        [SerializeField] private KeyCode jump;
        [SerializeField] private KeyCode sprint;
        [SerializeField] private KeyCode interact;
        [SerializeField] private KeyCode drop;
        [SerializeField] private KeyCode talk;
        [SerializeField] private KeyCode viewPlayers;
        [SerializeField] private KeyCode copy;
        [SerializeField] private KeyCode viewMap;
        [SerializeField] private KeyCode freeLook;
        [SerializeField] private KeyCode respawn;
        #endregion

        #region Properties
        public KeyCode WalkForwards
        {
            get => walkForwards;
            set => walkForwards = value;
        }
        public KeyCode WalkBackwards
        {
            get => walkBackwards;
            set => walkBackwards = value;
        }
        public KeyCode WalkLeft
        {
            get => walkLeft;
            set => walkLeft = value;
        }
        public KeyCode WalkRight
        {
            get => walkRight;
            set => walkRight = value;
        }
        public KeyCode Jump
        {
            get => jump;
            set => jump = value;
        }
        public KeyCode Sprint
        {
            get => sprint;
            set => sprint = value;
        }
        public KeyCode Interact
        {
            get => interact;
            set => interact = value;
        }
        public KeyCode Drop
        {
            get => drop;
            set => drop = value;
        }
        public KeyCode Talk
        {
            get => talk;
            set => talk = value;
        }
        public KeyCode ViewPlayers
        {
            get => viewPlayers;
            set => viewPlayers = value;
        }
        public KeyCode Copy
        {
            get => copy;
            set => copy = value;
        }
        public KeyCode ViewMap
        {
            get => viewMap;
            set => viewMap = value;
        }
        public KeyCode FreeLook
        {
            get => freeLook;
            set => freeLook = value;
        }
        public KeyCode Respawn
        {
            get => respawn;
            set => respawn = value;
        }
        #endregion

        #region Methods
        public override void Revert()
        {
            WalkForwards = KeyCode.W;
            WalkBackwards = KeyCode.S;
            WalkLeft = KeyCode.A;
            WalkRight = KeyCode.D;
            Jump = KeyCode.Space;
            Sprint = KeyCode.LeftShift;
            Interact = KeyCode.E;
            Drop = KeyCode.Q;
            Talk = KeyCode.T;
            ViewPlayers = KeyCode.Tab;
            Copy = KeyCode.LeftAlt;
            ViewMap = KeyCode.M;
            FreeLook = KeyCode.LeftAlt;
            Respawn = KeyCode.R;
        }
        #endregion
    }
}