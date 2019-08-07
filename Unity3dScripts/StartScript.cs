using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AstralRobotics.Simudoll.Core;

public class StartScript : MonoBehaviour
{
    Simulation _simulation;

    public Simulation Simulation { get { return _simulation; } }

    void Start()
    {

        var bus = new Bus();
        var simulator = new Simulator(bus);
        var world = simulator.CreateWorld("World01");

        var cube = world.CreateBody(name: "cube1", shape: PrimitiveShapes.Cube);

        //set target state 
        var state = cube.State;
        state.Position += Vector3.up * 0.7f;
        state.Position += Vector3.right * 2f;
        cube.Controller.TargetState = state;

        //set intentionaly unstable parameters
        cube.Controller.PositionControllerData.Proportional = 20f;
        cube.Controller.PositionControllerData.Derivative = 0.2f;
        cube.Controller.RotationControllerData.Proportional = 5f;
        cube.Controller.RotationControllerData.Derivative = 0.1f;

        _simulation = simulator.CreateSimulation(world);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_simulation.Status == SimulationStatus.Stopped)
                _simulation.Start();
            else if (_simulation.Status == SimulationStatus.Running)
                _simulation.Stop();
            else if (_simulation.Status == SimulationStatus.Paused)
                _simulation.Resume();
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            if (_simulation.Status == SimulationStatus.Running)
                _simulation.Pause();
            else if (_simulation.Status == SimulationStatus.Paused)
                _simulation.Resume();
        }

        _simulation.World.OnGuiUpdate();
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 20), "Status:" + _simulation.Status.ToString());
    }

    void FixedUpdate()
    {
        _simulation.OnPhysicsUpdate();
    }
}
