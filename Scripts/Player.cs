using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField] BulletFactory bullet_mgr;
	Transform t;

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		t = transform;
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}

	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update()
	{
		if ( Input.GetMouseButton(0))
		{
			bullet_mgr.ShotBullet(t.position, t.forward);
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Cursor.visible = !Cursor.visible;
		}
	}
}
