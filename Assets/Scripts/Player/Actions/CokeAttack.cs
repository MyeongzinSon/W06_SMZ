using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CokeAttack : ActionInfo
{
    [Header("Coke Gauge")]
    [SerializeField] float gaugeRegen;
    [SerializeField] float gaugeLossOnStop;
    [SerializeField] float regenRollMuliplier;
    [SerializeField] float gaugeCostPerSecond;
    [SerializeField] float maxGauge;
    [SerializeField] float minimumGaugeToShoot;
    [Header("Coke Geyser")]
    [SerializeField] float geyserWidth;
    [SerializeField] float maxLength;
    [SerializeField] float maxAngularSpeed;
    [SerializeField] float geyserDuartionMultiplier;
    [SerializeField] AnimationCurve geyserDurationCurve;
    [SerializeField] AnimationCurve geyserLaunchCurve;
    [SerializeField] LayerMask targetLayer;
    [Header("Super Coke")]
    [SerializeField] float superDurationMultiplier;
    [SerializeField] float superLengthMultipleir;
    [SerializeField] float superWidthMultiplier;
    [SerializeField] float superAngularSpeedMultiplier;

    CokeGeyser geyser;
    CokeGaugeUI ui;

    bool isSuper;
    float _currentGauge;
    float geyserDuration;
    float geyserStartGauge;

    PlayerMove playerMove => player.MoveComponent;
    public override bool CanAction => (CurrentGauge >= minimumGaugeToShoot);
    float CurrentGauge
    {
        get { return _currentGauge; }
        set { 
            var newValue = Mathf.Clamp(value, 0, maxGauge);
            _currentGauge = newValue;
            ui.SetGaugeValue(newValue / maxGauge, isSuper || CanSuper, CanAction || IsShooting);
        }
    }

    bool IsRolling => player.CurrentState == PlayerState.Roll;
    bool CanSuper => CurrentGauge == maxGauge;
    bool IsShooting => player.CurrentState == PlayerState.Action;

    protected override void Awake()
    {
        base.Awake();
        geyser = GetComponentInChildren<CokeGeyser>();
        ui = GetComponentInChildren<CokeGaugeUI>();
    }
    protected override void Start()
    {
        base.Start();
        geyser.SetActive(false);
        ui.gameObject.SetActive(true);
        geyserStartGauge = -1;
        CurrentGauge = 0;
    }
    public override void OnStartAction()
    {
        base.OnStartAction();

        if (CanSuper)
        {
            isSuper = true;
        }
        var angularSpeed = (isSuper ? superAngularSpeedMultiplier : 1) * maxAngularSpeed;
        var width = (isSuper ? superWidthMultiplier : 1) * geyserWidth;
        var duration = (isSuper ? superDurationMultiplier : 1) * geyserDuartionMultiplier;

        geyserStartGauge = CurrentGauge;
        geyserDuration = duration * geyserDurationCurve.Evaluate(CurrentGauge / maxGauge);
        geyser.StartGeyser(targetLayer, transform.position, aimDirection, angularSpeed, width);
    }
    public override void OnUpdateAction()
    {
        if (CurrentGauge > 0)
        {
            CurrentGauge -= (geyserStartGauge / geyserDuration) * Time.deltaTime;
            var gaugeT = (geyserStartGauge - CurrentGauge) / geyserStartGauge;
            var length = (isSuper ? superLengthMultipleir : 1) * geyserLaunchCurve.Evaluate(gaugeT) * maxLength * geyserStartGauge / maxGauge;
            geyser.UpdatePoint(transform.position, aimDirection * length);
        }
        if (CurrentGauge == 0)
        {
            IsActionEnded = true;
        }
    }
    public override void OnUpdateNotAction()
    {
        if (playerMove.IsMoving)
        {
            var multiplier = (IsRolling ? regenRollMuliplier : 1) * playerMove.SpeedModifier;
            CurrentGauge += gaugeRegen * multiplier * Time.deltaTime;
        }
        else
        {
            CurrentGauge -= gaugeLossOnStop * Time.deltaTime;
        }
    }
    public override void OnEndAction()
    {
        base.OnEndAction();
        isSuper = false;
        geyser.EndGeyser();
    }
    public override void OnDeath()
    {
        ui.gameObject.SetActive(false);
    }

}
