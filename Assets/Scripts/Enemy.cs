using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static event Action OnEnemyKilled;

    private void OnDisable() { OnEnemyKilled?.Invoke(); }
}
