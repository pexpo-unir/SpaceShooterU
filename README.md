# SpaceShooterU
 [Desarrollo de Videojuegos I] Actividad 1. Space Shooter completo

## Ampliaciones

### Uso de ScriptableObjects
El desarrollo está pensado para que los diseñadores puedan iterar fácilmente sobre el juego, permitiendo crear nuevos tipos de objetos haciendo uso de los ScriptablesObjects.

Veamos el ejemplo de `EnemyRoundData`, que nos permite definir una ronda:
```
        [field: SerializeField]
        public float TimeUntilStartSpawn { get; set; } = 1f;

        [field: SerializeField]
        public EnemyControllerData[] EnemiesControllerData { get; set; }

        [field: SerializeField]
        public float TimeBetweenEnemies { get; set; } = 1f;

        [field: SerializeField]
        public float TimeUntilRoundEnds { get; set; } = 1f;

        [field: SerializeField]
        public bool HasBoss { get; set; } = false;
```

Donde podemos definir el tiempo hasta que una ronda empieza, cuando se considera finalizada, si tiene un jefe y definir qué enemigos participan en la ronda, a través de `EnemiesControllerData`.

Si continuamos por `EnemiesControllerData`, pasamos por las `ShipData`, `WeapoinData`, `LaserData`, etc.

Veamos el ejemplo de `ShipData`:
```
        [field: SerializeField]
        public string Tag { get; set; }

        [field: SerializeField]
        public Sprite Sprite { get; set; }

        [field: SerializeField]
        public Vector2 FacingDirection { get; set; }

        [field: SerializeField]
        public float Speed { get; set; }

        [field: SerializeField]
        public int MaxHealth { get; set; } = 1;

        [field: SerializeField]
        public WeaponData WeaponData { get; set; }

        [field: SerializeField]
        public LaserData LaserData { get; set; }
```

### DamageSystem
Componente que gestiona todo lo relacionado con el daño de naves y torretas, a través de sus métodos: `GetDamage` y `GetHealing`.

Especial atención a los eventos `OnHealthChanged` y `OnDeath`, de modo que el resto del componentes del juego pueda observar y actuar en consecuencia.

Para facilitar el desarrollo se incorpora `HasImmunity`, de modo que el componente ignore el daño recibido. Aparece como opción en el producto final.

### Movement
Componente que gestiona el movimiento de las naves, lasers y powerups, pudiendo ser

Mediante las siguiente propiedades interactuamos con el componente:
```
        [field: SerializeField]
        public float Speed { get; set; } = 10f;

        [field: SerializeField]
        public Vector2 Direction { get; set; }

        [Header("Movement Bounds")]
        [field: SerializeField]
        public bool LimitMovementBounds;

        [field: SerializeField]
        public bool LimitToScreenViewport;

        [field: SerializeField]
        public Bounds MovementBounds { get; set; }
```

Especial atención en `LimitMovementBounds`, limitar el movimiento dentro de los límites establecidos por `MovementBounds`; y `LimitToScreenViewport`, limitar el movimiento al viewport. Esto permite limitar el movimiento de las naves, especialmente la nave del jugador.

### ObjectsPool

```
public abstract class PoolBase<T> : MonoBehaviour where T : Component
```

Permite extender fácilmente la pool, de modo que podamos tener diferentes, en el juego se utilizan: `LaserPool`, `ShipPool` y `ExplosionFXPool`.

### PowerUps
Si bien tienen el nombre powerup, actualmente la implementación se parece más a "objetos consumibles", actualmente hay 3 powerups: `HealingPowerUp`, `SimpleWeaponPowerUp` y `DoubleWeaponPowerUp`.

### Weapons
Definen solo cómo van a ser lanzados los lásers, ratio de disparo, cooldown, además de gestionar el FX correspondiente al disparo.

Para reutilizar la máxima funcionalidad posible, se hace uso de:
```
public abstract class WeaponBase : MonoBehaviour
```

En la que se basan las 3 armas actuales: `SimpleWeapon`, dispara un láser con un bajo, `DoubleWeapon`, dispara dos láser con un ratio mediomedioy `CircularWeapon`, dispara en todas direcciones con un ratio alto.

### Lasers
Definen el daño que aplicarán a la nave cuando se choque y la velocidad de movimiento.

### Enemigos:

- Enemigo "Linear". Se mueve rápido en línea recta.
- Enemigo "Flotante". Se mueve rápido con una función seno/coseno.
- Enemigo "Carguero". Se mueve lento y dispara lasers en todas direcciones.
- Torretas. Tipo de enemigo fijo que lanza lásers rotando aleatoriamente.
- Bosses. Una nave grande que lleva asociada torretas. Para eliminar al jefe hay que eliminar primero sus tres torretas.

### UI
- Menu Principal.
- Menú de opciones (dentro de Menú Principal y Menú de Pausa).
- Menú de Pausa.
- Menú Win/Lost.
