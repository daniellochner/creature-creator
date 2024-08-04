using System;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    [Serializable]
    public class Keybindings : Data
    {
        #region Fields
        [Header("Play")]
        [SerializeField] private Keybind walkForwards = new Keybind(KeyCode.W);
        [SerializeField] private Keybind walkBackwards = new Keybind(KeyCode.S);
        [SerializeField] private Keybind walkLeft = new Keybind(KeyCode.A);
        [SerializeField] private Keybind walkRight = new Keybind(KeyCode.D);
        [SerializeField] private Keybind jump = new Keybind(KeyCode.Space);
        [SerializeField] private Keybind flap = new Keybind(KeyCode.Space);
        [SerializeField] private Keybind sprint = new Keybind(KeyCode.LeftShift);
        [SerializeField] private Keybind interact = new Keybind(KeyCode.E);
        [SerializeField] private Keybind drop = new Keybind(KeyCode.Q);
        [SerializeField] private Keybind hold = new Keybind(KeyCode.E);
        [SerializeField] private Keybind talk = new Keybind(KeyCode.T);
        [SerializeField] private Keybind viewPlayers = new Keybind(KeyCode.Tab);
        [SerializeField] private Keybind freeLook = new Keybind(KeyCode.LeftAlt);
        [SerializeField] private Keybind stopMove = new Keybind(KeyCode.LeftControl);
        [SerializeField] private Keybind respawn = new Keybind(KeyCode.R, KeyCode.LeftControl);
        [SerializeField] private Keybind dismount = new Keybind(KeyCode.Space, KeyCode.LeftControl);
        [SerializeField] private Keybind toggleUI = new Keybind(KeyCode.U, KeyCode.LeftControl);
        [SerializeField] private Keybind bite = new Keybind(KeyCode.F);
        [SerializeField] private Keybind dig = new Keybind(KeyCode.F);
        [SerializeField] private Keybind distract = new Keybind(KeyCode.F);
        [SerializeField] private Keybind shoot = new Keybind(KeyCode.F);
        [SerializeField] private Keybind spit = new Keybind(KeyCode.F);
        [SerializeField] private Keybind breatheFire = new Keybind(KeyCode.F);
        [SerializeField] private Keybind growl = new Keybind(KeyCode.G);
        [SerializeField] private Keybind strike = new Keybind(KeyCode.F);
        [SerializeField] private Keybind nightVision = new Keybind(KeyCode.N);
        [SerializeField] private Keybind dance = new Keybind(KeyCode.E);
        [SerializeField] private Keybind spin = new Keybind(KeyCode.F);

        [Header("Build")]
        [SerializeField] private Keybind copy = new Keybind(KeyCode.LeftAlt);
        [SerializeField] private Keybind undo = new Keybind(KeyCode.Z, KeyCode.LeftControl);
        [SerializeField] private Keybind redo = new Keybind(KeyCode.V, KeyCode.LeftControl);
        [SerializeField] private Keybind flip = new Keybind(KeyCode.F, KeyCode.LeftControl);
        [SerializeField] private Keybind toggleAsymmetry = new Keybind(KeyCode.A);

        [Header("General")]
        [SerializeField] private Keybind save = new Keybind(KeyCode.S, KeyCode.LeftControl);
        [SerializeField] private Keybind load = new Keybind(KeyCode.L, KeyCode.LeftControl);
        [SerializeField] private Keybind clear = new Keybind(KeyCode.C, KeyCode.LeftControl);
        [SerializeField] private Keybind import = new Keybind(KeyCode.I, KeyCode.LeftControl);
        [SerializeField] private Keybind export = new Keybind(KeyCode.E, KeyCode.LeftControl);
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
        public Keybind Hold
        {
            get => hold;
            set => hold = value;
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
        public Keybind Dismount
        {
            get => dismount;
            set => dismount = value;
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
            get => shoot;
            set => shoot = value;
        }
        public Keybind Spit
        {
            get => spit;
            set => spit = value;
        }
        public Keybind BreatheFire
        {
            get => breatheFire;
            set => breatheFire = value;
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
        public Keybind Dance
        {
            get => dance;
            set => dance = value;
        }
        public Keybind Spin
        {
            get => spin;
            set => spin = value;
        }

        public Keybind Copy
        {
            get => copy;
            set => copy = value;
        }
        public Keybind Undo
        {
            get => undo;
            set => undo = value;
        }
        public Keybind Redo
        {
            get => redo;
            set => redo = value;
        }
        public Keybind Flip
        {
            get => flip;
            set => flip = value;
        }
        public Keybind ToggleAsymmetry
        {
            get => toggleAsymmetry;
            set => toggleAsymmetry = value;
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

        public bool AnyNone
        {
            get
            {
                return WalkForwards.Key == KeyCode.None || WalkBackwards.Key == KeyCode.None || WalkLeft.Key == KeyCode.None || WalkRight.Key == KeyCode.None || Jump.Key == KeyCode.None || Flap.Key == KeyCode.None || Sprint.Key == KeyCode.None || Interact.Key == KeyCode.None || Drop.Key == KeyCode.None || Talk.Key == KeyCode.None || ViewPlayers.Key == KeyCode.None || FreeLook.Key == KeyCode.None || StopMove.Key == KeyCode.None || Respawn.Key == KeyCode.None || Dismount.Key == KeyCode.None || ToggleUI.Key == KeyCode.None || Bite.Key == KeyCode.None || Dig.Key == KeyCode.None || Distract.Key == KeyCode.None || Shoot.Key == KeyCode.None || Spit.Key == KeyCode.None || Growl.Key == KeyCode.None || Strike.Key == KeyCode.None || NightVision.Key == KeyCode.None || Dance.Key == KeyCode.None || Spin.Key == KeyCode.None || Copy.Key == KeyCode.None || Undo.Key == KeyCode.None || Redo.Key == KeyCode.None || ToggleAsymmetry.Key == KeyCode.None || Flip.Key == KeyCode.None || Save.Key == KeyCode.None || Load.Key == KeyCode.None || Clear.Key == KeyCode.None || Import.Key == KeyCode.None || Export.Key == KeyCode.None;
            }
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
            Hold = new Keybind(KeyCode.E);
            Talk = new Keybind(KeyCode.T);
            ViewPlayers = new Keybind(KeyCode.Tab);
            FreeLook = new Keybind(KeyCode.LeftAlt);
            StopMove = new Keybind(KeyCode.LeftControl);
            Respawn = new Keybind(KeyCode.R, KeyCode.LeftControl);
            Dismount = new Keybind(KeyCode.Space, KeyCode.LeftControl);
            ToggleUI = new Keybind(KeyCode.U, KeyCode.LeftControl);
            Bite = new Keybind(KeyCode.F);
            Dig = new Keybind(KeyCode.F);
            Distract = new Keybind(KeyCode.F);
            Shoot = new Keybind(KeyCode.F);
            Spit = new Keybind(KeyCode.F);
            BreatheFire = new Keybind(KeyCode.F);
            Growl = new Keybind(KeyCode.G);
            Strike = new Keybind(KeyCode.F);
            NightVision = new Keybind(KeyCode.N);
            Dance = new Keybind(KeyCode.E);
            Spin = new Keybind(KeyCode.F);

            Copy = new Keybind(KeyCode.LeftAlt);
            Undo = new Keybind(KeyCode.Z, KeyCode.LeftControl);
            Redo = new Keybind(KeyCode.V, KeyCode.LeftControl);
            Flip = new Keybind(KeyCode.F, KeyCode.LeftControl);
            ToggleAsymmetry = new Keybind(KeyCode.A);

            Save = new Keybind(KeyCode.S, KeyCode.LeftControl);
            Load = new Keybind(KeyCode.L, KeyCode.LeftControl);
            Clear = new Keybind(KeyCode.C, KeyCode.LeftControl);
            Import = new Keybind(KeyCode.I, KeyCode.LeftControl);
            Export = new Keybind(KeyCode.E, KeyCode.LeftControl);
        }
        #endregion
    }
}