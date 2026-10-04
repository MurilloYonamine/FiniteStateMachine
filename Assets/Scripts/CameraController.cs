using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] float mouseSensitivity = 0.12f;   // graus por pixel
    [SerializeField] float stickSensitivity = 180f;    // graus por segundo
    [SerializeField] float lookSmoothTime = 0.03f;     // suavização (ajuste p/ reduzir tontura)
    [SerializeField] float pitchClampTop = 80f;
    [SerializeField] float pitchClampBottom = -80f;
    [SerializeField] float maxDegPerFrame = 12f;       // teto de variação por frame (anti-spike)
    public GameObject CinemachineCameraTarget;
    float yaw, pitch;           // valores aplicados
    float yawTarget, pitchTarget; // alvos (acumuladores)
    float yawVel, pitchVel;     // vel. interna do SmoothDamp
    private StarterAssetsInputs _input;

    void Start()
    {
        _input = GetComponent<StarterAssetsInputs>();
        // Opcional: trave/oculte cursor conforme o seu jogo
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Inicializa yaw/pitch a partir dos Transforms atuais
        yaw = transform.eulerAngles.y;
        pitch = CinemachineCameraTarget.transform.localEulerAngles.x;
        if (pitch > 180f) pitch -= 360f;

        yawTarget = yaw;
        pitchTarget = pitch;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 look = _input.look; // seu input (pode ser mouse delta + stick)

        // Detecta se o dispositivo atual é mouse (delta por frame) ou analógico (valor contínuo)
        bool usingMouse = UnityEngine.InputSystem.Mouse.current != null
                          && UnityEngine.InputSystem.Mouse.current.delta.IsActuated();

        float dt = usingMouse ? 1f : Time.deltaTime;
        float sens = usingMouse ? mouseSensitivity : stickSensitivity;

        // Acumula alvos
        yawTarget += look.x * sens * dt;
        pitchTarget += (look.y * sens * dt) * /* invertY? */ -1f;

        // Limita pitch com clamp robusto
        pitchTarget = ClampAngle180(pitchTarget, pitchClampBottom, pitchClampTop);

        // Anti-spike: limita variação por frame
        yawTarget = yaw + Mathf.Clamp(yawTarget - yaw, -maxDegPerFrame, maxDegPerFrame);
        pitchTarget = pitch + Mathf.Clamp(pitchTarget - pitch, -maxDegPerFrame, maxDegPerFrame);

        // Suaviza alvo -> valor aplicado
        yaw = Mathf.SmoothDamp(yaw, yawTarget, ref yawVel, lookSmoothTime);
        pitch = Mathf.SmoothDamp(pitch, pitchTarget, ref pitchVel, lookSmoothTime);
    }

    void LateUpdate()
    {
        // Aplica rotação: yaw no player, pitch no alvo do Cinemachine
        transform.rotation = Quaternion.Euler(0f, yaw, 0f);
        CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(-pitch, 0f, 0f);
    }

    static float ClampAngle180(float angle, float min, float max)
    {
        angle = Mathf.Repeat(angle + 180f, 360f) - 180f; // normaliza p/ [-180, 180]
        return Mathf.Clamp(angle, min, max);
    }

}
