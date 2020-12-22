using Inventory;
using Shapes;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

public class FireExtinguisher : HandHeldItem
{
    [SerializeField] private VisualEffect vfx;
    [SerializeField] private InputAction useAction;

    [SerializeField] private GameObject hud;
    [SerializeField] private TMP_Text capacityPercentText;
    [SerializeField] private Rectangle capacityFillBar;

    private float _capacityPercent = 100f;
    private const float DEPLETION_PER_SECOND = 20;

    private bool _useState;

    private void Update()
    {
        if (!_useState) return;
        
        _capacityPercent -= Time.deltaTime * DEPLETION_PER_SECOND;
        _capacityPercent = Mathf.Max(0, _capacityPercent);
        
        capacityPercentText.text = $"{Mathf.RoundToInt(_capacityPercent)}%";
        capacityFillBar.Width = 1.8f * _capacityPercent;

        SetSpawnRate();
    }

    private void SetSpawnRate()
    {
        if (!_useState || _capacityPercent <= 0)
        {
            vfx.SetFloat("SpawnRate", 0);
        }
        else if (_capacityPercent < 20)
        {
            vfx.SetFloat("SpawnRate", 50);
        }
        else
        {
            vfx.SetFloat("SpawnRate", 200);
        }
    }

    public override void PickUp()
    {
        base.PickUp();
        hud.transform.SetParent(HandHeldItemUI.Instance.hudAnchorPoint, false);
        hud.SetActive(false);
    }

    public override void Drop()
    {
        base.Drop();
        Release();
    }

    public override void Grab()
    {
        base.Grab();
        useAction.performed += OnUse;
        useAction.Enable();
        enabled = true;
        hud.SetActive(true);
    }

    public override void Release()
    {
        base.Release();
        useAction.performed -= OnUse;
        useAction.Disable();
        enabled = false;
        hud.SetActive(false);
    }

    private void OnUse(InputAction.CallbackContext ctx)
    {
        bool pressed = Mathf.Approximately(ctx.ReadValue<float>(), 1);

        _useState = pressed;
        SetSpawnRate();
    }

    private void OnDestroy()
    {
        Destroy(hud);
    }
}