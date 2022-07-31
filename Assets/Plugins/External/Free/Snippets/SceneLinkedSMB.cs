// Credit: https://forum.unity.com/threads/extending-statemachinebehaviours.488314/

using UnityEngine;
using UnityEngine.Animations;

public class SceneLinkedSMB<TMonoBehaviour> : SealedSMB where TMonoBehaviour : MonoBehaviour
{
    protected Animator m_Animator;
    protected TMonoBehaviour m_MonoBehaviour;

    bool m_FirstFrameHappened;
    bool m_LastFrameHappened;

    public AnimatorStateInfo StateInfo { get; private set; }

    public static void Initialize(Animator animator, TMonoBehaviour monoBehaviour)
    {
        SceneLinkedSMB<TMonoBehaviour>[] sceneLinkedSMBs = animator.GetBehaviours<SceneLinkedSMB<TMonoBehaviour>>();

        for (int i = 0; i < sceneLinkedSMBs.Length; i++)
        {
            sceneLinkedSMBs[i].InternalInitialize(animator, monoBehaviour);
        }
    }

    protected void InternalInitialize(Animator animator, TMonoBehaviour monoBehaviour)
    {
        m_Animator = animator;
        m_MonoBehaviour = monoBehaviour;
        OnStart(animator);
    }

    public sealed override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {
        StateInfo = stateInfo;

        m_FirstFrameHappened = false;

        //if (animator.GetLayerWeight(layerIndex) != 0f)
        {
            OnSLStateEnter(animator, stateInfo, layerIndex);
            OnSLStateEnter(animator, stateInfo, layerIndex, controller);
        }
    }

    public sealed override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {
        if (animator.IsInTransition(layerIndex) && animator.GetNextAnimatorStateInfo(layerIndex).fullPathHash == stateInfo.fullPathHash)
        {
            //if (animator.GetLayerWeight(layerIndex) != 0f)
            {
                OnSLTransitionToStateUpdate(animator, stateInfo, layerIndex);
                OnSLTransitionToStateUpdate(animator, stateInfo, layerIndex, controller);
            } 
        }

        if (!animator.IsInTransition(layerIndex) && !m_FirstFrameHappened)
        {
            m_FirstFrameHappened = true;

            //if (animator.GetLayerWeight(layerIndex) != 0f)
            {
                OnSLStatePostEnter(animator, stateInfo, layerIndex);
                OnSLStatePostEnter(animator, stateInfo, layerIndex, controller);
            }
        }

        if (!animator.IsInTransition(layerIndex))
        {
            //if (animator.GetLayerWeight(layerIndex) != 0f)
            {
                OnSLStateNoTransitionUpdate(animator, stateInfo, layerIndex);
                OnSLStateNoTransitionUpdate(animator, stateInfo, layerIndex, controller);
            }
        }

        if (animator.IsInTransition(layerIndex) && !m_LastFrameHappened && m_FirstFrameHappened)
        {
            m_LastFrameHappened = true;

            //if (animator.GetLayerWeight(layerIndex) != 0f)
            {
                OnSLStatePreExit(animator, stateInfo, layerIndex);
                OnSLStatePreExit(animator, stateInfo, layerIndex, controller);
            }
        }

        if (animator.IsInTransition(layerIndex) && animator.GetCurrentAnimatorStateInfo(layerIndex).fullPathHash == stateInfo.fullPathHash)
        {
            //if (animator.GetLayerWeight(layerIndex) != 0f)
            {
                OnSLTransitionFromStateUpdate(animator, stateInfo, layerIndex);
                OnSLTransitionFromStateUpdate(animator, stateInfo, layerIndex, controller);
            }
        }
    }

    public sealed override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {
        m_LastFrameHappened = false;

        //if (animator.GetLayerWeight(layerIndex) != 0f)
        {
            OnSLStateExit(animator, stateInfo, layerIndex);
            OnSLStateExit(animator, stateInfo, layerIndex, controller);
        }
    }

    /// <summary>
    /// Called by a MonoBehaviour in the scene during its Start function.
    /// </summary>
    public virtual void OnStart(Animator animator) { }

    /// <summary>
    /// Called before Updates when execution of the state first starts (on transition to the state).
    /// </summary>
    public virtual void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }

    /// <summary>
    /// Called after OnSLStateEnter every frame during transition to the state.
    /// </summary>
    public virtual void OnSLTransitionToStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }

    /// <summary>
    /// Called on the first frame after the transition to the state has finished.
    /// </summary>
    public virtual void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }

    /// <summary>
    /// Called every frame when the state is not being transitioned to or from.
    /// </summary>
    public virtual void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }

    /// <summary>
    /// Called on the first frame after the transition from the state has started.  Note that if the transition has a duration of less than a frame, this will not be called.
    /// </summary>
    public virtual void OnSLStatePreExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }

    /// <summary>
    /// Called after OnSLStatePreExit every frame during transition to the state.
    /// </summary>
    public virtual void OnSLTransitionFromStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }

    /// <summary>
    /// Called after Updates when execution of the state first finshes (after transition from the state).
    /// </summary>
    public virtual void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }

    /// <summary>
    /// Called before Updates when execution of the state first starts (on transition to the state).
    /// </summary>
    public virtual void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller) { }

    /// <summary>
    /// Called after OnSLStateEnter every frame during transition to the state.
    /// </summary>
    public virtual void OnSLTransitionToStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller) { }

    /// <summary>
    /// Called on the first frame after the transition to the state has finished.
    /// </summary>
    public virtual void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller) { }

    /// <summary>
    /// Called every frame when the state is not being transitioned to or from.
    /// </summary>
    public virtual void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller) { }

    /// <summary>
    /// Called on the first frame after the transition from the state has started.  Note that if the transition has a duration of less than a frame, this will not be called.
    /// </summary>
    public virtual void OnSLStatePreExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller) { }

    /// <summary>
    /// Called after OnSLStatePreExit every frame during transition to the state.
    /// </summary>
    public virtual void OnSLTransitionFromStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller) { }

    /// <summary>
    /// Called after Updates when execution of the state first finshes (after transition from the state).
    /// </summary>
    public virtual void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller) { }
}

public abstract class SealedSMB : StateMachineBehaviour
{
    public sealed override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }

    public sealed override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }

    public sealed override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }
}