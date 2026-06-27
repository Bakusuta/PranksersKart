using UnityEngine;

public class KartCamara : MonoBehaviour
{ 
    [Header("Cámara")]
    [SerializeField] private Transform kart;
    [SerializeField] private Vector3 posicion = new Vector3(0f, 5f, -8f);
    [SerializeField] private float velocidadSeguimiento = 8f;
    [SerializeField] private float velocidadRotacion = 8f;

    private void LateUpdate() //lateUpdate se ejecuta despues de cada fotograma como update pero con la diferencia de que lateUpdate se ejecutara despues
    {

        Vector3 posicionObjetivo = kart.position + kart.TransformDirection(posicion);
        transform.position = Vector3.Lerp(transform.position, posicionObjetivo, velocidadSeguimiento * Time.deltaTime); //movimiento de la camara Vector3.lerp sirve para dar un movimiento fluido de la camara, usando posición inicial, posicion final y la velocidad
        
        Vector3 mirarKart = kart.position + Vector3.up * 1.2f; //guarda la posicion donde debe ir la camara
        Vector3 direccion = mirarKart - transform.position; //calcula la distancia entre el kart y la camara

        if (direccion.sqrMagnitude > 0.01f) //verificamos que la camara no este pegada al personaje
        {
            Quaternion rotacionObjetivo = Quaternion.LookRotation(direccion); //quaternion para no hacer el cuento largo es el calculo en un entorno 3d y en este caso le decimos que la camara mire al kart
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, velocidadRotacion * Time.deltaTime); //Slerp permite rotar con suavidad la posicion de la camara dandole un punto de origen, punto final y a la velocidad a la que girara
        }
    }
}
