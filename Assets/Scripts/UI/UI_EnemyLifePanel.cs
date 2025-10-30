using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_EnemyLifePanel : MonoBehaviour
{
    private SpriteRenderer _healthBarSpriteRenderer;
    private Vector3 _originalScale;
    private Vector3 _originalPosition;

    void Start()
    {
        _healthBarSpriteRenderer = GetComponent<SpriteRenderer>();
        _originalScale = _healthBarSpriteRenderer.transform.localScale;
        _originalPosition = _healthBarSpriteRenderer.transform.localPosition;

        EnemyLifeController enemyLifeController = GetComponentInParent<EnemyLifeController>();
        enemyLifeController.OnHpChanged.AddListener(ResizePanel);
        enemyLifeController.OnEnemyDeath.AddListener(DisablePanel);
    }

    void ResizePanel(int currentHp, int maxHp)
    {
        float healthPercent = (float)currentHp / maxHp;
        Vector2 newScale = new Vector2(_originalScale.x * healthPercent, _originalScale.y);
        _healthBarSpriteRenderer.transform.localScale = newScale;
        float offset = (_originalScale.x - newScale.x) * 2f;
        _healthBarSpriteRenderer.transform.localPosition = _originalPosition - new Vector3(offset, 0, 0);
    }

    void DisablePanel()
    {
        transform.parent.gameObject.SetActive(false);
    }
}
