﻿using UnityEngine;
using Entitas;

public class GameController : MonoBehaviour {

    Systems _systems;

    void Awake() {
        Application.targetFrameRate = 60;
    }

    void Start() {
        _systems = createAllSystems(Pools.core, Pools.input, Pools.bullets);
        _systems.Initialize();
    }

    void Update() {
        _systems.Execute();
    }

    Systems createAllSystems(Pool corePool, Pool inputPool, Pool bulletsPool) {
        return new Feature("Systems")
            .Add(createCoreSystem(corePool))
            .Add(createInputSystem(inputPool, corePool, bulletsPool))
            .Add(createBulletsSystem(bulletsPool));
    }

    Systems createCoreSystem(Pool corePool) {
        return new Feature(corePool.metaData.poolName + " Systems")

            // Initialize
            .Add(corePool.CreateSystem<CreatePlayerSystem>())

            // Update
            .Add(corePool.CreateSystem<VelocitySystem>())
            .Add(corePool.CreateSystem<AddViewSystem>())
            .Add(corePool.CreateSystem<RenderPositionSystem>())

            // Destroy
            .Add(corePool.CreateSystem<DestroyViewSystem>())
            .Add(corePool.CreateSystem<DestroySystem>());
    }
    
    Systems createInputSystem(Pool inputPool, Pool corePool, Pool bulletsPool) {
        return new Feature(inputPool.metaData.poolName + " Systems")

            // Update
            .Add(inputPool.CreateSystem(new ProcessMoveInputSystem(corePool)))
            .Add(inputPool.CreateSystem(new ProcessShootInputSystem(corePool, bulletsPool)));
    }

    Systems createBulletsSystem(Pool bulletsPool) {
        return new Feature(bulletsPool.metaData.poolName + " Systems")

            // Update
            .Add(bulletsPool.CreateSystem<VelocitySystem>())
            .Add(bulletsPool.CreateSystem<DestroyBulletOutOfScreenSystem>())

            // Render
            .Add(bulletsPool.CreateSystem<AddViewFromObjectPoolSystem>())
            .Add(bulletsPool.CreateSystem<RenderPositionSystem>())

            // Destroy
            .Add(bulletsPool.CreateSystem<DestroyViewSystem>())
            .Add(bulletsPool.CreateSystem<DestroySystem>());
    }
}

