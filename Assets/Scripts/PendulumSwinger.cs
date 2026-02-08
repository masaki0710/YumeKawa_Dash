using UnityEngine;

public class PendulumSwinger : MonoBehaviour
{
    private HingeJoint hinge;
    private JointMotor motor;

    void Start()
    {
        hinge = GetComponent<HingeJoint>();
        motor = hinge.motor;

        float startAngle = Random.Range(-40, 40f);
        transform.localRotation = Quaternion.Euler(startAngle, 0f, 0f);

        motor.targetVelocity = (Random.value > 0.5f) ? 60f : -60f;
        hinge.motor = motor;
    }

    void Update()
    {
        // 現在の角度を取得
        float currentAngle = hinge.angle;

        // 40度を超えていたら、強制的にマイナス方向へ
        if (currentAngle >= 40f)
        {
            motor.targetVelocity = -60f;
        }
        // -40度を下回っていたら、強制的にプラス方向へ
        else if (currentAngle <= -40f)
        {
            motor.targetVelocity = 60f;
        }

        // 常にモーターの状態を更新
        hinge.motor = motor;
    }
}