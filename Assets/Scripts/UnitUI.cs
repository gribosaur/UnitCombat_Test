using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UnitUI : MonoBehaviour
{
    [Header("����")]
    [SerializeField] private SpriteRenderer unitSprite;
    [SerializeField] private Unit unit;

    [Header("������")]
    [SerializeField] private Button attackButton;
    [SerializeField] private Button buffButton;

    [Header("����")]
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider armorBar;
    [SerializeField] private Slider vampirismBar;

    [Header("������ ����������")]
    [SerializeField] private TextMeshProUGUI activeBuffs;

    public static Action OnMoveIsOver;

    private void OnEnable()
    {
        attackButton.onClick.AddListener(() => AttackButtonEvent());
        buffButton.onClick.AddListener(() => BuffButtonEvent());

        unit.OnHealthChanged += OnHealthChangedEvent;
        Unit.OnBuffChanged += OnBuffChangedEvent;
    }

    private void Start()
    {
        UpdateBarInfo(healthBar, unit.Health);
        UpdateBarInfo(armorBar, unit.Armor);
        UpdateBarInfo(vampirismBar, unit.Vampirism);
    }

    private void AttackButtonEvent() //������� ��� ������� ������ �����
    {
        unit.Attack();
        OnMoveIsOver.Invoke();
    }

    private void BuffButtonEvent() //������� ��� ������� ������ �����
    {
        unit.SetBuff();
        buffButton.interactable = false;
    }

    private void OnBuffChangedEvent() //���������� ������ � ���������� � ������
    {
        UpdateBarInfo(armorBar, unit.Armor);
        UpdateBarInfo(vampirismBar, unit.Vampirism);

        var buffText = "";
        foreach(var buff in unit.activeBuffs)
        {
            buffText += $"{buff.Key} ({buff.Value}) \n";
        }
        activeBuffs.text = buffText;
    }

    private void OnHealthChangedEvent() //������� ��� ��������� �����
    {
        if (healthBar.value > unit.Health)
        {
            StartCoroutine(UnitDamaggedAnim());
        }
        UpdateBarInfo(healthBar, unit.Health);
    }

    private void UpdateBarInfo(Slider bar, int currentValue) //���������� ����
    {
        bar.value = currentValue;
        bar.GetComponentInChildren<TextMeshProUGUI>().text = $"{currentValue}";
    }

    public void SetActive(bool value) //���������/���������� ����������� ������� ���
    {
        attackButton.interactable = value;
        buffButton.interactable = unit.activeBuffs.Count > 1 ? false : value;
    }

    IEnumerator UnitDamaggedAnim() //�������� ��� ��������� �����
    {
        float t = 0;
        while (t <= 1)
        {
            unitSprite.color = Color.Lerp(Color.white, Color.red, t);
            t += 0.1f;
            yield return new WaitForSeconds(0.05f);
        }
        while(t >= 0)
        {
            unitSprite.color = Color.Lerp(Color.white, Color.red, t);
            t -= 0.1f;
            yield return new WaitForSeconds(0.05f);
        }
    }

    private void OnDisable()
    {
        attackButton.onClick.RemoveListener(AttackButtonEvent);
        buffButton.onClick.RemoveListener(BuffButtonEvent);

        unit.OnHealthChanged -= OnHealthChangedEvent;
        Unit.OnBuffChanged -= OnBuffChangedEvent;
    }
}
