/*==============================================================================
Copyright (c) 2017 PTC Inc. All Rights Reserved.

Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/

using UnityEngine;
using Vuforia;

/// <summary>
///     A custom handler that implements the ITrackableEventHandler interface.
/// </summary>
public class GraphEventHandler : MonoBehaviour, ITrackableEventHandler
{
	#region PRIVATE_MEMBER_VARIABLES

	protected TrackableBehaviour mTrackableBehaviour;
	public bool tracked = false;
	public GameObject panel;
	public GameObject w_button;


	#endregion // PRIVATE_MEMBER_VARIABLES

	#region UNTIY_MONOBEHAVIOUR_METHODS

	protected virtual void Start()
	{
		//panel = GameObject.Find ("Panel");
		panel.SetActive (false);
		mTrackableBehaviour = GetComponent<TrackableBehaviour>();
		if (mTrackableBehaviour)
			mTrackableBehaviour.RegisterTrackableEventHandler(this);
	}

	#endregion // UNTIY_MONOBEHAVIOUR_METHODS

	#region PUBLIC_METHODS

	/// <summary>
	///     Implementation of the ITrackableEventHandler function called when the
	///     tracking state changes.
	/// </summary>
	public void OnTrackableStateChanged(
		TrackableBehaviour.Status previousStatus,
		TrackableBehaviour.Status newStatus)
	{
		if (newStatus == TrackableBehaviour.Status.DETECTED ||
			newStatus == TrackableBehaviour.Status.TRACKED ||
			newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
		{
			Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
			tracked = true;
			OnTrackingFound();
		}
		else if (previousStatus == TrackableBehaviour.Status.TRACKED &&
			newStatus == TrackableBehaviour.Status.NOT_FOUND)
		{
			Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
			tracked = false;
			OnTrackingLost();
		}
		else
		{
			// For combo of previousStatus=UNKNOWN + newStatus=UNKNOWN|NOT_FOUND
			// Vuforia is starting, but tracking has not been lost or found yet
			// Call OnTrackingLost() to hide the augmentations
			OnTrackingLost();
		}
	}

	#endregion // PUBLIC_METHODS

	#region PRIVATE_METHODS

	protected virtual void OnTrackingFound()
	{
		var rendererComponents = GetComponentsInChildren<Renderer>(true);
		var colliderComponents = GetComponentsInChildren<Collider>(true);
		var canvasComponents = GetComponentsInChildren<Canvas>(true);

		// Enable rendering:
		foreach (var component in rendererComponents)
			component.enabled = true;

		// Enable colliders:
		foreach (var component in colliderComponents)
			component.enabled = true;

		// Enable canvas':
		foreach (var component in canvasComponents)
			component.enabled = true;

		panel.SetActive (true);
		if (GameObject.Find ("Hand").GetComponent<Hand> ().selectedIngredient != "")
			w_button.SetActive (true);
	}


	protected virtual void OnTrackingLost()
	{
		var rendererComponents = GetComponentsInChildren<Renderer>(true);
		var colliderComponents = GetComponentsInChildren<Collider>(true);
		var canvasComponents = GetComponentsInChildren<Canvas>(true);

		// Disable rendering:
		foreach (var component in rendererComponents)
			component.enabled = false;

		// Disable colliders:
		foreach (var component in colliderComponents)
			component.enabled = false;

		// Disable canvas':
		foreach (var component in canvasComponents)
			component.enabled = false;

		panel.SetActive (false);
		if (!GameObject.Find ("Hand").GetComponent<Hand> ().wayfinding)
			w_button.SetActive (false);
	}

	public void newGraph() {
		if (!tracked)
			OnTrackingLost ();
	}

	#endregion // PRIVATE_METHODS
}

