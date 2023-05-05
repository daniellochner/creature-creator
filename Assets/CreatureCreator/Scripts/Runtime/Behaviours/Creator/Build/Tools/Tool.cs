// Creature Creator - https://github.com/daniellochner/Creature-Creator
// Copyright (c) Daniel Lochner

using System;
using System.Collections.Generic;
using UnityEngine;

namespace DanielLochner.Assets.CreatureCreator
{
    public abstract class Tool : MonoBehaviour
    {
        #region Fields
        [SerializeField] protected Transform model;
        [SerializeField] protected ScaledAxes scaledAxes;

        protected TransformationTools transformationTools;
        protected Drag drag;
        protected Hover hover;

        private Vector3 initialScale;
        #endregion

        #region Properties
        public BodyPartEditor BodyPartEditor { get; private set; }

        public abstract bool CanShow { get; }

        public virtual float ScaleFactor => BodyPartEditor.BodyPartConstructor.BodyPart.ToolsScaleFactor;

        protected abstract Change Type { get; }
        #endregion

        #region Methods
        private void LateUpdate()
        {
            if (transform.parent == transformationTools.transform)
            {
                Vector3 scale = BodyPartEditor.transform.localScale.Inverse();

                scale.x = scaledAxes.x ? 1f : scale.x;
                scale.y = scaledAxes.y ? 1f : scale.y;
                scale.z = scaledAxes.z ? 1f : scale.z;

                transform.localScale = scale;
            }
        }

        public virtual void Initialize()
        {
            drag = GetComponent<Drag>();
            hover = GetComponent<Hover>();
            transformationTools = GetComponentInParent<TransformationTools>();

            initialScale = model.localScale;
        }

        public void Setup(BodyPartEditor bpe)
        {
            BodyPartEditor = bpe;

            model.localScale = initialScale * ScaleFactor;

            Setup();

            hover.OnEnter.AddListener(delegate
            {
                if (!Input.GetMouseButton(0))
                {
                    BodyPartEditor.CreatureEditor.Camera.CameraOrbit.Freeze();
                }
            });
            hover.OnExit.AddListener(delegate
            {
                if (!Input.GetMouseButton(0))
                {
                    BodyPartEditor.CreatureEditor.Camera.CameraOrbit.Unfreeze();
                }
            });

            drag.OnDrag.AddListener(delegate
            {
                BodyPartEditor.BodyPartConstructor.Flip();
            });
        }
        protected virtual void Setup()
        {
            drag.ResetEvents();
            drag.OnPress.AddListener(delegate
            {
                BodyPartEditor.CreatureEditor.Camera.CameraOrbit.Freeze();

                transform.SetParent(Dynamic.Transform);

                foreach (Tool tool in transformationTools.TransformationToolsDict.Values)
                {
                    if (tool != this)
                    {
                        tool.gameObject.SetActive(false);
                    }
                }
            });
            drag.OnRelease.AddListener(delegate
            {
                BodyPartEditor.CreatureEditor.Camera.CameraOrbit.Unfreeze();

                transform.parent = transformationTools.transform;
                transform.localRotation = Quaternion.identity;

                foreach (KeyValuePair<Transformation, Tool> transformationTool in transformationTools.TransformationToolsDict)
                {
                    if (BodyPartEditor.BodyPartConstructor.BodyPart.Transformations.HasFlag(transformationTool.Key) && transformationTool.Value.CanShow)
                    {
                        transformationTool.Value.gameObject.SetActive(true);
                    }
                }

                EditorManager.Instance.TakeSnapshot(Type);
            });
        }
        #endregion

        #region Nested
        [Serializable]
        public class ScaledAxes
        {
            public bool x, y, z;
        }
        #endregion
    }
}