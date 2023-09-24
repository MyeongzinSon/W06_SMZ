using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public abstract class BossPattern : MonoBehaviour
{
	#region PublicVariables
	#endregion

	#region PrivateVariables
	[SerializeField]protected Boss main;
	protected Animator anim;
	[SerializeField] protected string animationStateName;
	[SerializeField] protected float preDelaySeconds;
	[SerializeField] protected float postDelaySeconds;
	private WaitForSeconds waitpreDelay;
	private WaitForSeconds waitpostDelay;
	#endregion

	#region PublicMethod

	public void StartAct()
    {
		StartCoroutine(nameof(Act));
    }
	public IEnumerator Act()
	{
		PreProcessing();
		PlayAnimation();
		yield return waitpreDelay;
		main.GetComponent<Collider2D>().enabled = true;
		Debug.Log($"Pattern:Act({GetInstanceID()})");
		ActionContext();
		yield return waitpostDelay;
		PostProcessing();

		CallNextAction();
	}
	public void CallNextAction()
	{
		main.PatternNext();
	}
	public virtual void ShutdownAction()
	{
		StopCoroutine(nameof(Act));
	}
	#endregion

	#region PrivateMethod
	protected virtual void Awake()
	{
		transform.parent.TryGetComponent(out main);
		anim = main.GetAnimator();
		waitpreDelay = new WaitForSeconds(preDelaySeconds);
		waitpostDelay = new WaitForSeconds(postDelaySeconds);

	}
	private void Start()
	{
		
	}
	
	protected virtual void OnDisable()
	{
		ShutdownAction();
	}
	private void PlayAnimation()
	{
		if (animationStateName != "")
		{
			anim = main.GetComponent<Animator>();
			
			anim.Play(animationStateName);
		}
	}
	protected virtual void PreProcessing()
	{

	}
	protected virtual void PostProcessing()
	{

	}
	protected abstract void ActionContext();
	#endregion
}
