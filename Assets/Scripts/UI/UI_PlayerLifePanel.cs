using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerLifePanel : MonoBehaviour
{
    [SerializeField] private PlayerLifeController _playerLifeController;
    private Image _healthBarImage;
    void Start()
    {
        _healthBarImage = GetComponent<Image>();

        _playerLifeController.OnHpChanged.AddListener(ResizePanel);
    }

    void ResizePanel(int currentHp, int maxHp)
    {
        _healthBarImage.fillAmount = (float)currentHp / maxHp;
    }
}
