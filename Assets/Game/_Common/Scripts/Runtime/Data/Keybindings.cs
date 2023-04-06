using System;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [Serializable]
    public class Keybindings : Data
    {
        #region Fields
        [Header("Play")]
        [SerializeField] private Keybind walkForwards;
        [SerializeField] private Keybind walkBackwards;
        [SerializeField] private Keybind walkLeft;
        [SerializeField] private Keybind walkRight;
        [SerializeField] private Keybind jump;
        [SerializeField] private Keybind flap;
        [SerializeField] private Keybind sprint;
        [SerializeField] private Keybind interact;
        [SerializeField] private Keybind drop;
        [SerializeField] private Keybind talk;
        [SerializeField] private Keybind viewPlayers;
        [SerializeField] private Keybind freeLook;
        [SerializeField] private Keybind stopMove;
        [SerializeField] private Keybind respawn;
        [SerializeField] private Keybind toggleUI;
        [SerializeField] private Keybind bite;
        [SerializeField] private Keybind dig;
        [SerializeField] private Keybind distract;
        [SerializeField] private Keybind eat;
        [SerializeField] private Keybind ping;
        [SerializeField] private Keybind growl;
        [SerializeField] private Keybind strike;
        [SerializeField] private Keybind nightVision;

        [Header("Build")]
        [SerializeField] private Keybind copy;

        [Header("General")]
        [SerializeField] private Keybind save;
        [SerializeField] private Keybind load;
        [SerializeField] private Keybind clear;
        [SerializeField] private Keybind import;
        [SerializeField] private Keybind export;
        #endregion

        #region Properties
        public Keybind WalkForwards
        {
            get => walkForwards;
            set => walkForwards = value;
        }
        public Keybind WalkBackwards
        {
            get => walkBackwards;
            set => walkBackwards = value;
        }
        public Keybind WalkLeft
        {
            get => walkLeft;
            set => walkLeft = value;
        }
        public Keybind WalkRight
        {
            get => walkRight;
            set => walkRight = value;
        }
        public Keybind Jump
        {
            get => jump;
            set => jump = value;
        }
        public Keybind Flap
        {
            get => flap;
            set => flap = value;
        }
        public Keybind Sprint
        {
            get => sprint;
            set => sprint = value;
        }
        public Keybind Interact
        {
            get => interact;
            set => interact = value;
        }
        public Keybind Drop
        {
            get => drop;
            set => drop = value;
        }
        public Keybind Talk
        {
            get => talk;
            set => talk = value;
        }
        public Keybind ViewPlayers
        {
            get => viewPlayers;
            set => viewPlayers = value;
        }
        public Keybind FreeLook
        {
            get => freeLook;
            set => freeLook = value;
        }
        public Keybind StopMove
        {
            get => stopMove;
            set => stopMove = value;
        }
        public Keybind Respawn
        {
            get => respawn;
            set => respawn = value;
        }
        public Keybind ToggleUI
        {
            get => toggleUI;
            set => toggleUI = value;
        }
        public Keybind Bite
        {
            get => bite;
            set => bite = value;
        }
        public Keybind Dig
        {
            get => dig;
            set => dig = value;
        }
        public Keybind Distract
        {
            get => distract;
            set => distract = value;
        }
        public Keybind Shoot
        {
            get => eat;
            set => eat = value;
        }
        public Keybind Spit
        {
            get => ping;
            set => ping = value;
        }
        public Keybind Growl
        {
            get => growl;
            set => growl = value;
        }
        public Keybind Strike
        {
            get => strike;
            set => strike = value;
        }
        public Keybind NightVision
        {
            get => nightVision;
            set => nightVision = value;
        }

        public Keybind Copy
        {
            get => copy;
            set => copy = value;
        }

        public Keybind Save
        {
            get => save;
            set => save = value;
        }
        public Keybind Load
        {
            get => load;
            set => load = value;
        }
        public Keybind Clear
        {
            get => clear;
            set => clear = value;
        }
        public Keybind Import
        {
            get => import;
            set => import = value;
        }
        public Keybind Export
        {
            get => export;
            set => export = value;
        }
        #endregion

        #region Methods
        public override void Revert()
        {
            WalkForwards = new Keybind(KeyCode.W);
            WalkBackwards = new Keybind(KeyCode.S);
            WalkLeft = new Keybind(KeyCode.A);
            WalkRight = new Keybind(KeyCode.D);
            Jump = new Keybind(KeyCode.Space);
            Flap = new Keybind(KeyCode.Space);
            Sprint = new Keybind(KeyCode.LeftShift);
            Interact = new Keybind(KeyCode.E);
            Drop = new Keybind(KeyCode.Q);
            Talk = new Keybind(KeyCode.T);
            ViewPlayers = new Keybind(KeyCode.Tab);
            FreeLook = new Keybind(KeyCode.LeftAlt);
            StopMove = new Keybind(KeyCode.LeftControl);
            Respawn = new Keybind(KeyCode.R, KeyCode.LeftControl);
            ToggleUI = new Keybind(KeyCode.U, KeyCode.LeftControl);
            Bite = new Keybind(KeyCode.F);
            Dig = new Keybind(KeyCode.F);
            Distract = new Keybind(KeyCode.F);
            Shoot = new Keybind(KeyCode.F);
            Spit = new Keybind(KeyCode.F);
            Growl = new Keybind(KeyCode.G);
            Strike = new Keybind(KeyCode.F);
            NightVision = new Keybind(KeyCode.N);

            Copy = new Keybind(KeyCode.LeftAlt);

            Save = new Keybind(KeyCode.S, KeyCode.LeftControl);
            Load = new Keybind(KeyCode.L, KeyCode.LeftControl);
            Clear = new Keybind(KeyCode.C, KeyCode.LeftControl);
            Import = new Keybind(KeyCode.I, KeyCode.LeftControl);
            Export = new Keybind(KeyCode.E, KeyCode.LeftControl);
        }
        #endregion
    }
}