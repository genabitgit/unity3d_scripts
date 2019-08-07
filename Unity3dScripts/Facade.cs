using System;
using UnityEngine;
using AstralRobotics.Simudoll.Core;

/// <summary>
/// Methods of this class will be called from the browser javascript code
/// </summary>
public class Facade : MonoBehaviour
{
	StartScript _startScript;

	void Start ()
	{
		_startScript = GetComponent<StartScript> ();
	}

	public void SimulationStart()
	{
		_startScript.Simulation.Start ();
	}

	public void SimulationStop()
	{
		_startScript.Simulation.Stop ();
	}

	public void SimulationPause()
	{
		_startScript.Simulation.Pause ();
	}

	public void SimulationResume()
	{
		_startScript.Simulation.Resume ();
	}
}