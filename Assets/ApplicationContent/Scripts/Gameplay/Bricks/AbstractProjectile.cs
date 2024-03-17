using UnityEngine;

/// <summary>
/// <para>
/// Abstract class of an object moving from the spawn point to the point of its destruction.
/// The projectile performs an action implemented in the implementation
/// when it comes in contact with <see cref="PlayerBodypart"/>.
/// </para>
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public abstract class AbstractProjectile : MonoBehaviour
{
    /// <summary>
    /// <para><see cref="LevelManager"/> reference.</para>
    /// </summary>
    protected LevelManager levelManager;

    private Vector3 destroyPosition;
    private float movementSpeed = 6f;
    private ProjectileSpawner projectileSpawner;

    private bool isBrickMove;

    /// <summary>
    /// <para>Sets level manager reference.</para>
    /// </summary>
    /// <value>The level manager reference</value>
    public LevelManager LevelManager
    {
        set => levelManager = value;
    }

    /// <summary>
    /// <para>Sets projectile spawner.</para>
    /// </summary>
    /// <value>The projectile spawner</value>
    public ProjectileSpawner ProjectileSpawner
    {
        set => projectileSpawner = value;
    }

    /// <summary>
    /// <para>Sets projectile movement speed.</para>
    /// </summary>
    /// <value>The projectile movement speed</value>
    public float MovementSpeed
    {
        set => movementSpeed = value;
    }

    /// <summary>
    /// <para>Sets position of projectile destruction.</para>
    /// </summary>
    /// <value>The position of projectile destruction</value>
    public Vector3 DestroyPosition
    {
        set => destroyPosition = value;
    }

    private void Start()
    {
        isBrickMove = true;
    }

    private void FixedUpdate()
    {
        if (isBrickMove)
        {
            Move();
        }

        CheckDestination();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerBodypart player) && isBrickMove)
        {
            isBrickMove = false;
            PerformAction();
        }
    }

    private void OnDestroy()
    {
        projectileSpawner.RemoveProjectileFromSpawnedList(this);
    }

    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, destroyPosition, movementSpeed);
    }

    private void CheckDestination()
    {
        if (transform.position == destroyPosition)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// <para>The action that will be performed when a projectile comes into contact with <see cref="PlayerBodypart"/>.</para>
    /// </summary>
    protected abstract void PerformAction();
}