using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ReSharper disable All

public class DayLightingColliderTransform 
{
	public bool updateNeeded = false;

	public Vector2 position = Vector2.zero;
	public Vector2 scale = Vector3.zero;
	public float rotation = 0;
	
	private bool flipX = false;
	private bool flipY = false;

	private float height = 0;

	private float sunDirection = 0;
	private float sunSoftness = 1;
	private float sunHeight = 1;

	private DayLightColliderShape shape;

	private Transform _transform = null;
	private Vector2 scale2D = Vector2.zero;
	private Vector2 position2D = Vector2.zero;
	private float rotation2D = 0;

	private SpriteRenderer spriteRenderer = null;
	
	private float timeSinceLastCalled;

	private float delay = 0.1f;
	
	public void Reset() 
	{
		position = Vector2.zero;
		rotation = 0;
		scale = Vector3.zero;
	}

	public void SetShape(DayLightColliderShape shape) 
	{
		this.shape = shape;
	}

	public void Update() 
	{
		_transform = shape.transform;

		scale2D = _transform.lossyScale;
		position2D = _transform.position;
		RotationController();

		spriteRenderer = shape.spriteShape.GetSpriteRenderer();
		
		if (shape.transform == null) {
			return;
		}
		
		updateNeeded = false;
		
		if (position != position2D) 
		{
			position = position2D;
		}
		
		if (sunDirection != Lighting2D.DayLightingSettings.direction) 
		{
			sunDirection = Lighting2D.DayLightingSettings.direction;
			updateNeeded = true;
		}

		if (sunHeight != Lighting2D.DayLightingSettings.height) 
		{
			sunHeight = Lighting2D.DayLightingSettings.height;
			updateNeeded = true;
		}

		if (sunSoftness != Lighting2D.DayLightingSettings.softness.intensity) 
		{
			sunSoftness = Lighting2D.DayLightingSettings.softness.intensity;
			updateNeeded = true;
		}
				
		if (scale != scale2D) 
		{
			scale = scale2D;
			updateNeeded = true;
		}

		if (rotation != rotation2D) 
		{
			rotation = rotation2D;
			updateNeeded = true;
		}

		if (height != shape.height) {
			height = shape.height;

			updateNeeded = true;
		}

		if (shape.shadowType == DayLightCollider2D.ShadowType.SpritePhysicsShape) {
			if (spriteRenderer != null) {
				if (spriteRenderer.flipX != flipX || spriteRenderer.flipY != flipY) {
					flipX = spriteRenderer.flipX;
					flipY = spriteRenderer.flipY;

					updateNeeded = true;
					
					shape.ResetLocal(); // World
				}
			}
		}
		
	}

	private void RotationController()
	{
		timeSinceLastCalled += Time.deltaTime;
		if (timeSinceLastCalled > delay)
		{
			rotation2D = _transform.rotation.eulerAngles.z;
			timeSinceLastCalled = 0f;
		}
	}
	
}

public class DayLightTilemapColliderTransform {
	public bool moved = false;

	//public Vector2 position = Vector2.zero;
	//public Vector2 scale = Vector3.zero;
	//public float rotation = 0;
	
	private float height = 0;

	private float sunDirection = 0;
	private float sunSoftness = 1;
	private float sunHeight = 1;

	public void Update(DayLightTilemapCollider2D id) {		
		Transform transform = id.transform;

		moved = false;

		if (sunDirection != Lighting2D.DayLightingSettings.direction) {
			sunDirection = Lighting2D.DayLightingSettings.direction;

			moved = true;
		}

		if (sunHeight != Lighting2D.DayLightingSettings.height) {
			sunHeight = Lighting2D.DayLightingSettings.height;

			moved = true;
		}

		if (sunSoftness != Lighting2D.DayLightingSettings.softness.intensity) {
			sunSoftness = Lighting2D.DayLightingSettings.softness.intensity;

			moved = true;
		}

		if (height != id.height) {
			height = id.height;

			moved = true;
		}

		// Unnecesary check
		if (height < 0.01f) {
			height = 0.01f;
		}
	}
}
