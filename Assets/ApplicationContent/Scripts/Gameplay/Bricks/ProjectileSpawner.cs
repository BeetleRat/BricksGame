using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// <para>Specific spawn speed settings for a particular game speed state.</para>
/// </summary>
[Serializable]
sealed class SpawnerSpeed
{
    public Speed ForSpeed;
    public float MovementSpeed;
    [Range(1, 300)] 
    public float SpawnSpeed;
}

/// <summary>
/// <para>Spawned projectile info.</para>
/// </summary>
[Serializable]
sealed class SpawnedProjectile
{
    public AbstractProjectile SpawnedObject;
    [Range(0, 100)] 
    public int SpawnChance;
    public ObjectMovement[] MovementPoints;
}

/// <summary>
/// <para>The component that performs projectile spawning.</para>
/// It is a <see cref="AbstractSpeedChangingComponent"/>
/// </summary>
public sealed class ProjectileSpawner : AbstractSpeedChangingComponent
{
    private const float SPEED_DIVIDER = 70;

    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private SpawnedProjectile[] _projectiles;

    [SerializeField] private List<SpawnerSpeed> _speedSettings;

    private SpawnerSpeed currentSpeed;
    private bool isSpawnerActive = false;
    private float timeAfterSpawn;
    private List<AbstractProjectile> spawnedProjectiles;

    private void Awake()
    {
        spawnedProjectiles = new List<AbstractProjectile>();
        ValidateSettings(ref _speedSettings);
    }

    private void Start()
    {
        if (_speedSettings.Count == 0)
        {
            CustomLogger.Error(this, "Speed Settings List is empty");
        }
        else
        {
            currentSpeed = _speedSettings[0];
        }

        _levelManager.GameStarted += StartSpawn;
        _levelManager.GameEnded += StopSpawnAndDestroyAllObjects;
        timeAfterSpawn = 200;
    }

    private void Update()
    {
        if (isSpawnerActive)
        {
            PreformSpawn();
        }
    }

    private void OnDestroy()
    {
        _levelManager.GameStarted -= StartSpawn;
        _levelManager.GameEnded -= StopSpawnAndDestroyAllObjects;
    }

    /// <summary>
    /// <para>Remove the specified projectile from the list of tracked projectiles.</para>
    /// </summary>
    /// <param name="abstractProjectile">Removed projectile</param>
    public void RemoveProjectileFromSpawnedList(AbstractProjectile abstractProjectile)
    {
        spawnedProjectiles.Remove(abstractProjectile);
    }

    private void ValidateSettings(ref List<SpawnerSpeed> speedSettings)
    {
        speedSettings.Sort((ss1, ss2) => ss1.ForSpeed - ss2.ForSpeed);
        RemoveDuplicatesFrom(ref speedSettings);
    }

    private void RemoveDuplicatesFrom(ref List<SpawnerSpeed> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            if (list[i].ForSpeed == list[i - 1].ForSpeed)
            {
                list.Remove(list[i]);
            }
        }
    }

    private void StartSpawn()
    {
        isSpawnerActive = true;
    }

    private void StopSpawnAndDestroyAllObjects()
    {
        isSpawnerActive = false;
        for (int i = spawnedProjectiles.Count - 1; i >= 0; i--)
        {
            Destroy(spawnedProjectiles[i].gameObject);
        }
    }

    private void PreformSpawn()
    {
        timeAfterSpawn += Time.deltaTime;
        if (timeAfterSpawn >= (100 / currentSpeed.SpawnSpeed))
        {
            SpawnRandomProjectile();
            timeAfterSpawn = 0;
        }
    }

    private void SpawnRandomProjectile()
    {
        int totalNumbers = 0;

        foreach (SpawnedProjectile brick in _projectiles)
        {
            totalNumbers += brick.SpawnChance;
        }

        int winner = Random.Range(1, totalNumbers);
        foreach (SpawnedProjectile brick in _projectiles)
        {
            totalNumbers -= brick.SpawnChance;
            if (winner >= totalNumbers)
            {
                SpawnProjectile(brick);
                break;
            }
        }
    }

    private void SpawnProjectile(SpawnedProjectile projectile)
    {
        ObjectMovement brickMovement = projectile.MovementPoints[Random.Range(0, projectile.MovementPoints.Length)];

        Transform spawnPoint = brickMovement.StartPoint;
        Vector3 spawnedProjectilePosition = projectile.SpawnedObject.transform.position;

        Vector3 spawnPosition = spawnPoint.position + spawnedProjectilePosition;
        Quaternion spawnRotation = spawnPoint.rotation * projectile.SpawnedObject.transform.rotation;
        Vector3 destroyPosition = brickMovement.EndPoint.position + spawnedProjectilePosition;

        AbstractProjectile spawnProjectile = Instantiate(projectile.SpawnedObject, spawnPosition, spawnRotation);

        spawnProjectile.MovementSpeed = currentSpeed.MovementSpeed / SPEED_DIVIDER;
        spawnProjectile.DestroyPosition = destroyPosition;
        spawnProjectile.LevelManager = _levelManager;
        spawnProjectile.ProjectileSpawner = this;

        spawnedProjectiles.Add(spawnProjectile);
    }

    /// <summary>
    /// <inheritdoc cref="AbstractSpeedChangingComponent.ChangeSpeed"/>
    /// </summary>
    /// <param name="speed"><inheritdoc cref="AbstractSpeedChangingComponent.ChangeSpeed"/></param>
    public override void ChangeSpeed(Speed speed)
    {
        if (speed == Speed.STOP)
        {
            isSpawnerActive = false;
            return;
        }

        if (_speedSettings.Count == 0)
        {
            CustomLogger.Error(this, "Speed Settings List is empty");
            return;
        }

        foreach (SpawnerSpeed spawnerSpeed in _speedSettings)
        {
            if (spawnerSpeed.ForSpeed == speed)
            {
                currentSpeed = spawnerSpeed;
                return;
            }
        }

        LogNoSpeedSettingsFor(speed);
    }
}