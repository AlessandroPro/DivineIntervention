using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
	public MenuClassifier menuClassifier;

	public enum StartMenuState
	{
		Ingnore,
		Active,
		Disabled
	}
	public StartMenuState startMenuState = StartMenuState.Active;
	public bool resetPosition = true;

	private Animator animator;
	private bool isOpen;

	public bool IsOpen
	{
		get
		{
			if (animator != null)
			{
				return animator.GetBool("isOpen");
			}
			return isOpen;
		}
		set
		{
			if (animator != null)
			{
				animator.SetBool("isOpen", value);
			}
			else
			{
				gameObject.SetActive(value);
			}
			isOpen = value;
		}
	}

	public virtual void Awake()
	{
		// Need to add this to the Menu Manager
		MenuManager.Instance.addMenu(this);

		animator = GetComponent<Animator>();
		if (resetPosition == true)
		{
			var rect = GetComponent<RectTransform>();
			rect.localPosition = Vector3.zero;
		}
	}

	public virtual void Start()
	{
		switch(startMenuState)
		{
			case StartMenuState.Active:
				gameObject.SetActive(true);
				IsOpen = true;
				break;

			case StartMenuState.Disabled:
				// Remove this if your animation will disable the gameObject
				gameObject.SetActive(false);
				IsOpen = false;
				break;
		}
	}

	public virtual void onShowMenu(string options)
	{
		IsOpen = true;
	}

	public virtual void onHideMenu(string options)
	{
		IsOpen = false;
	}

}
