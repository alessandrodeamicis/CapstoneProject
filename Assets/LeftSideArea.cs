using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeftSideArea : MonoBehaviour
{
    [SerializeField] private Transform _camera;
    [SerializeField] private Transform _player;
    [SerializeField] private float playerMoveX = 6f;
    private EdgeCollider2D _edgeCollider;
    private void Start()
    {
        _edgeCollider = GetComponent<EdgeCollider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        float distance = Vector2.Distance(transform.position, _camera.position);
        _camera.position = Vector3.Lerp(_camera.position, new Vector3(_camera.position.x + (distance * 4), _camera.position.y, _camera.position.z), 0.5f);
        _player.position = Vector3.Lerp(_player.position, new Vector3(_player.position.x + playerMoveX, _player.position.y, _player.position.z), 0.5f);

        LevelManager.Instance.SpawnNewEnemies();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _edgeCollider.isTrigger = false;
    }
}
