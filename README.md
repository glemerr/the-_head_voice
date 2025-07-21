# The Head Voice
![alt text](PortadaLow.png)
🎮 **Visión general:**  
The Head Voice es un videojuego que explora las enfermedades mentales (hiperactividad y depresión) mediante una jugabilidad desafiante y sistemas aleatorios. En el juego controlas a Lucy, una joven que enfrenta sus miedos en entornos sombríos mientras sigue los mensajes de su difunta abuela. Su objetivo es recolectar las *'Lágrimas de la Luz'* ocultas en cada nivel para abrir portales entre mundos y avanzar hacia la sanación.  
⚠️ **Advertencia:** Contiene temáticas sensibles relacionadas con la salud mental.

## Jugabilidad

- 🔄 **Mecánicas aleatorias**: 
  - **Enemigos variables**: Cada vez que aparece un enemigo puede tener habilidades diferentes.
  - **Drops aleatorios**: Al derrotar enemigos, pueden soltar salud, tiempo extra, objetos especiales o nada.
  - **Objetos especiales**: Destruir elementos del escenario otorga poderes al azar (super salto, invisibilidad, mayor velocidad, salud completa, etc.).
- 🎁 **Ítems de apoyo**: 
![alt text](image.png)
  - **🗡️ Doble Daño**: Duplica el daño temporalmente.
  - **⏱️ Reductor de cooldown**: Reduce el tiempo de recarga de habilidades.
  - **🎯 Alcance + Velocidad**: Aumenta el alcance y la velocidad de los proyectiles.
  - **🔄 Reductor de recarga**: Disminuye el tiempo de recarga de armas.
  - **🛡️ Escudo temporal**: Otorga inmunidad breve contra daño.
  - **💣 Explosión radial**: Crea una explosión dañina al impactar.
- 👾 **Enemigos**: 
![alt text](image-1.png)
  - **Devastador**: Dispara proyectiles oscuros desde la boca.
  - **El Vacío**: Ataca cuerpo a cuerpo con fuerza bruta.
  - **El Ahorcado**: Realiza ataques desde la distancia para infligir daño.
- 🔫 **Armas principales**: 
![alt text](image-2.png)
  - **Escopeta (Shotgun)**: Dispara una ráfaga de perdigones, ideal para eliminar enemigos cercanos.
  - **Lanzacohetes**: Lanza misiles explosivos que causan daño en área.
  - **Portal**: Crea un agujero negro que absorbe y aniquila a los enemigos.
  - **Rayo destructor**: Emite un láser continuo de gran daño.
  - **Proyectil buscador (Enemy Search)**: Dispara un proyectil teledirigido que persigue automáticamente a los enemigos.
  - **Lanzallamas**: Emite fuego constante para dañar a los enemigos cercanos.

## Historia

El protagonista, Lucy, es un joven que sufre de hiperactividad y depresión, condiciones que se manifiestan como voces oscuras en su mente. Estas voces la sumergen en un mundo sombrío lleno de criaturas aterradoras que representan sus peores miedos. Lucy recibe mensajes en sueños de su difunta abuela, quien le ofrece sabiduría y consuelo desde más allá de la vida. Con la guía de su abuela, Lucy descubre que para sanar debe recolectar las *'Lágrimas de la Luz'* ocultas en cada zona del mundo. Estas Lágrimas de la Luz son objetos simbólicos que le permitirán abrir portales a nuevas etapas de su subconsciente, acercándola paso a paso a la recuperación.

Cada nivel del juego refleja un capítulo en el proceso de Lucy para enfrentar sus demonios internos. Al avanzar, Lucy debe derrotar manifestaciones físicas de sus traumas más profundos. Al vencer a estos enemigos, reduce el poder de las voces en su cabeza y recupera fragmentos de su verdadera identidad. La atmósfera es opresiva pero cargada de esperanza, donde la luz simboliza la sanación y la oscuridad los desafíos a superar.

> "La oscuridad no es tu enemiga, es el lienzo donde pintarás tu luz."  
> *— Mensaje de la Abuela (Nivel 3)*

## Instalación

1. Clona este repositorio.
2. Abre el proyecto con Unity 2022.3 o superior.
3. Ejecuta la escena principal (`Main.unity`).

## Notas de diseño

- **Aleatoriedad controlada:** Todos los sistemas utilizan `Random.Range` con semillas fijas para garantizar reproducibilidad cuando sea necesario.
- **Feedback visual:** Partículas y efectos de sonido resaltan cada acción importante (ataques, saltos, recolección de objetos, etc.).
- **Dificultad progresiva:** Los enemigos se vuelven más fuertes en zonas avanzadas, incrementando gradualmente la dificultad.

## Recursos

- **Trello:** Tablero de proyecto con tareas y seguimiento de avances.
- **Drive:** Carpeta compartida con los recursos gráficos y sonoros del juego.
- **GitHub:** Repositorio oficial del proyecto (este repositorio).
