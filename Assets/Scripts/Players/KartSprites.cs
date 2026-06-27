using UnityEngine;

public class KartSprites : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Transform kart;
    [SerializeField] private Camera camaraObjetivo;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Sprites en orden")]
    [SerializeField] private Sprite[] sprites = new Sprite[8];

    //lateUpdate se ejecuta despues de cada fotograma como update pero con la diferencia de que lateUpdate se ejecutara despues
    void LateUpdate()
    {
        rostroCamara();
        actualizarDireccionSprite();
    }

    private void rostroCamara() //acomoda el sprite del lado de la camara
    {
        Vector3 posicionCamara = camaraObjetivo.transform.position - transform.position; //calcula la distancia entre el sprite y la camara
        posicionCamara.y = 0f; //como solo necesitamos X y Z, convertimos Y en 0 para evitar fallos en sqrMagnitude

        if(posicionCamara.sqrMagnitude > 0.001f) //revisa si la distancia de la camara esta alejada del sprite
        {
            transform.rotation = Quaternion.LookRotation(posicionCamara); //acomoda el sprite del lado de la camara
        }
    }

    private void actualizarDireccionSprite()
    {
        Vector3 posicionCamara = camaraObjetivo.transform.position - kart.position;
        posicionCamara.y = 0f;
        Vector3 kartForward = kart.forward;
        kartForward.y = 0f;

        if(posicionCamara.sqrMagnitude < 0.001f)
        {
            return;
        }

        float angulo = Vector3.SignedAngle(kartForward, posicionCamara, Vector3.up);
        int index = Mathf.RoundToInt(angulo / 45f);
        if(index < 0)
        {
            index += 8;
        }
        index %= 8;
        if (sprites[index] != null)
        {
            spriteRenderer.sprite = sprites[index];
        }
    }
}
