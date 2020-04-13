using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : Singleton<MenuManager>
{
	public MenuClassifier loadingMenuClassifier;

	private Dictionary<string, Menu> menuList = new Dictionary<string, Menu>();

	public void addMenu(Menu menu)
	{
		if (menuList.ContainsKey(menu.menuClassifier.menuName) == false)
		{
			menuList.Add(menu.menuClassifier.menuName, menu);
		}
		else
		{
			// Assert?
			Debug.LogError("Multiple menus being added: " + menu.menuClassifier.menuName);
		}
	}

	public T getMenu<T>(MenuClassifier classifier) where T : Menu
	{
		Menu menu;
		if (menuList.TryGetValue(classifier.menuName, out menu))
		{
			return menu as T;
		}
		Debug.LogError("Menu does not exist: " + classifier.menuName);
		return null;
	}

	public void showMenu(MenuClassifier classifier, string options = "")
	{
		Menu menu;
		if (menuList.TryGetValue(classifier.menuName, out menu))
		{
			menu.onShowMenu(options);
		}
	}

	public void hideMenu(MenuClassifier classifier, string options = "")
	{
		Menu menu;
		if (menuList.TryGetValue(classifier.menuName, out menu))
		{
			menu.onHideMenu(options);
		}
	}
}
