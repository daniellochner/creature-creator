using System;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [Serializable]
    public class Keybindings : Data
    {
        #region Fields
        [SerializeField] private Keybinding walkForwards;
        [SerializeField] private Keybinding walkBackwards;
        [SerializeField] private Keybinding walkLeft;
        [SerializeField] private Keybinding walkRight;
        [SerializeField] private Keybinding jump;
        [SerializeField] private Keybinding sprint;
        [SerializeField] private Keybinding interact;
        [SerializeField] private Keybinding drop;
        [SerializeField] private Keybinding talk;
        [SerializeField] private Keybinding viewPlayers;
        [SerializeField] private Keybinding copy;
        [SerializeField] private Keybinding freeLook;
        [SerializeField] private Keybinding respawn;
        #endregion

        #region Properties
        public Keybinding WalkForwards
        {
            get => walkForwards;
            set => walkForwards = value;
        }
        public Keybinding WalkBackwards
        {
            get => walkBackwards;
            set => walkBackwards = value;
        }
        public Keybinding WalkLeft
        {
            get => walkLeft;
            set => walkLeft = value;
        }
        public Keybinding WalkRight
        {
            get => walkRight;
            set => walkRight = value;
        }
        public Keybinding Jump
        {
            get => jump;
            set => jump = value;
        }
        public Keybinding Sprint
        {
            get => sprint;
            set => sprint = value;
        }
        public Keybinding Interact
        {
            get => interact;
            set => interact = value;
        }
        public Keybinding Drop
        {
            get => drop;
            set => drop = value;
        }
        public Keybinding Talk
        {
            get => talk;
            set => talk = value;
        }
        public Keybinding ViewPlayers
        {
            get => viewPlayers;
            set => viewPlayers = value;
        }
        public Keybinding Copy
        {
            get => copy;
            set => copy = value;
        }
        public Keybinding FreeLook
        {
            get => freeLook;
            set => freeLook = value;
        }
        public Keybinding Respawn
        {
            get => respawn;
            set => respawn = value;
        }
        #endregion

        #region Methods
        public override void Revert()
        {
            WalkForwards = new Keybinding(KeyCode.W);
            WalkBackwards = new Keybinding(KeyCode.S);
            WalkLeft = new Keybinding(KeyCode.A);
            WalkRight = new Keybinding(KeyCode.D);
            Jump = new Keybinding(KeyCode.Space);
            Sprint = new Keybinding(KeyCode.LeftShift);
            Interact = new Keybinding(KeyCode.E);
            Drop = new Keybinding(KeyCode.Q);
            Talk = new Keybinding(KeyCode.T);
            ViewPlayers = new Keybinding(KeyCode.Tab);
            Copy = new Keybinding(KeyCode.LeftAlt);
            FreeLook = new Keybinding(KeyCode.LeftAlt);
            Respawn = new Keybinding(KeyCode.R, KeyCode.LeftControl);
        }
        #endregion
    }
}