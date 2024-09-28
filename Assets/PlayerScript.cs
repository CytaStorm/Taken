using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
	#region Singleton
	public static PlayerScript Player
	{
		get; private set;
	}
	#endregion

	#region Fields
	[SerializeField] private PlayerController _playerController;

	public int _health;
	[SerializeField] private int _inventorySize;
	public GameObject[] _inventory;
	#endregion

	#region Properties
	public PlayerController PlayerController
	{
		get;
		private set;
	}
	#endregion

	private void Awake()
	{
		//Singleton
		if (Player != null && Player != this)
		{
			Destroy(gameObject);
		}
		else
		{
			Player = this;
		}

		//Initialize player stats
		_inventory = new GameObject[_inventorySize];
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
