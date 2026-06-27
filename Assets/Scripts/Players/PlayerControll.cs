using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControll : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float aceleracion = 22f;
    [SerializeField] private float velocidadMaxima = 18f;
    [SerializeField] private float velocidadReversa = 8f;
    [SerializeField] private float velocidadDireccion = 120f;

    [Header("Configuración")]
    [SerializeField] private float friccionEstatica = 1.5f;
    [SerializeField] private float friccionMovimiento = 0.4f;
    [SerializeField] float movimientoInput;
    [SerializeField] float direccionInput;
    [SerializeField] Rigidbody rb;

    [Header("Controles")]
    [SerializeField] InputAction accionAceleración;
    [SerializeField] InputAction accionDireccion;

    private void Awake() //configuraciones iniciales
    {

        rb.interpolation = RigidbodyInterpolation.Interpolate; //activa interpolación
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous; //activa la detección de colisiones de manera continua

        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ; //congela X y Z para evitar movimientos bruscos
    }

    private void OnEnable() //para activar los controles
    {
        accionAceleración.Enable();
        accionDireccion.Enable();
    }

    private void OnDisable() //para desactivar los controles
    {
        accionAceleración.Disable();
        accionDireccion.Disable();
    }

    //Actualiza en un intervalo constante de tiempo
    private void FixedUpdate()
    {
        moverKart();
        direccionKart();
        limiteVelocidad();
    }

    // Actualiza en cada frame y lo usamos para leer los movimientos
    void Update()
    {
        //clamp sirve para limitar de -1 a 1 leyendo la entrada del boton
        movimientoInput = Mathf.Clamp(accionAceleración.ReadValue<float>(), -1f, 1f); 
        direccionInput = Mathf.Clamp(accionDireccion.ReadValue<float>(), -1f, 1f); 
    }

    private void moverKart() //para acelerar
    {
        if(Mathf.Abs(movimientoInput) > 0.01f) //calculamos si esta acelerando/retrocediendo o si esta quieto Mathf.Abs es calcular el absoluto
        {
            rb.linearDamping = friccionMovimiento; //linear damping es la asignación para que se sienta la fricción (nos sera util para el hielo)
            rb.AddForce(transform.forward * movimientoInput * aceleracion, ForceMode.Acceleration); //se efectua la acerelación indicandole la posición z, el input de movimiento, aceleración y el tipo de fuerza que se aplicara siendo este aceleración
        } 
        else {
            rb.linearDamping = friccionEstatica; //si deja de moverse va a ir frenando de poco en poco
        }
    }

    private void direccionKart() //para dirigir su posición a la que va
    {
        Vector3 velocidadLateral = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z); //leemos la velocidad actual de X y Z
        float factorVelocidad = Mathf.Clamp01(velocidadLateral.magnitude / 5f); //calculamos la velocidad exacta del vector X para dividirlo en su velocidad maxima y sacarlo en rando de 0 a 1
        
        if(factorVelocidad > 0.05f) 
        {
            float direccion = Vector3.Dot(transform.forward, velocidadLateral.normalized) >= 0 ? 1f : -1f; //compara la direccion en z y la velocidad en el suelo para saber su va adelante o en reversa
            float turn = direccionInput * velocidadDireccion * factorVelocidad * direccion * Time.fixedDeltaTime;

            transform.Rotate(0f, turn, 0f);
        }
    }

    private void limiteVelocidad() 
    {
        Vector3 velocidadLateral = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z); //guardamos la velocidad a la que va x y z
        float velocidadMaximaActual = movimientoInput >= 0 ? velocidadMaxima : velocidadReversa;  //revisamos si va acelerando o va en reversa
        
        if(velocidadLateral.magnitude > velocidadMaximaActual) //medimos la longitud de la velocidad y si esta superandola la disminuimos
        {
            Vector3 velocidadLimitada = velocidadLateral.normalized * velocidadMaximaActual;
            rb.linearVelocity = new Vector3(velocidadLimitada.x, rb.linearVelocity.y, velocidadLimitada.z);
        }
    }

}
