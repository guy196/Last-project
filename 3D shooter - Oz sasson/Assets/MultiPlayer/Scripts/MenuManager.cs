using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
	public static MenuManager Instance;
	[SerializeField] Menu[] menus;

	void Awake()
	{
		Instance = this;
	}
	public void OpenMenu(string menuName)
	{
		for (int i = 0; i < menus.Length; i++) //run throw all the menus
		{
			if(menus[i].menuName == menuName) //check if the menue name is == the Menuname
			{
				OpenMenu(menus[i]); //open the menu that should have open
			}
			else if (menus[i].open) //close the other menu that opens
			{
				CloseMenu(menus[i]); //close the menu that should be closed
			}
		}
	}
	public void OpenMenu(Menu menu)
	{
		for (int i = 0; i < menus.Length; i++)
		{
			if (menus[i].open)
			{
				CloseMenu(menus[i]);
			}
		}
		menu.Open();
	}

	public void CloseMenu(Menu menu)
	{
		menu.Close();
	}
}
