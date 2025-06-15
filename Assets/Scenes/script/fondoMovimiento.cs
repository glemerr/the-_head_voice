using Unity.VisualScripting;
using UnityEngine;

public class fondoMovimiento : MonoBehaviour
{
    [SerializeField] private Vector3 velocidadMovimiento;
    [SerializeField] private Vector3 velocidadRotacion;
    [SerializeField] private Vector3 velocidadEscalado;
    [SerializeField] private float velocidadEscaladoMaximo = 1.0f;
    [SerializeField] private float velocidadEscaladoMinimo = 0.5f;

    private Vector3 escalaActual;
    private Vector3 direccionEscalado = Vector3.one; // inicia dirección d escalado positiva

    public Material Material { get; private set; }
    public Vector2 TextureOffset { get; private set; } // Cambiado d Vector3 a Vector2  representando el desplazamiento d textura

    private Rigidbody jugadorRB;

    void Start()
    {
        escalaActual = transform.localScale; // Guarda escala inicial del objeto
    }

    private void Awake()
    {
        Material = GetComponent<Renderer>().material; // Asegúrate que el objeto tenga un Renderer con un material                                           
        jugadorRB = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Rigidbody>();
    }

    void Update()
    {
        TextureOffset = new Vector2(velocidadMovimiento.x, velocidadMovimiento.y) * Time.deltaTime;
        Material.mainTextureOffset += TextureOffset; // Desplaza la textura del material
    }
}

