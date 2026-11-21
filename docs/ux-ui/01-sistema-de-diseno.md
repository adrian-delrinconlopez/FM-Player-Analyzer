# Sistema de diseño del prototipo y del escritorio

Propuesta inicial en píxeles efectivos de Windows (DIP); interpretar CSS px como unidad equivalente en el prototipo a escala de referencia. Colores de referencia en oscuro: fondo #10151C, superficie #19212B, borde #354352, texto principal #EEF3F6, secundario #B5C1CB, acento #69D5C0, advertencia #F7C572 y error #FF9FA2. En alto contraste sustituir por recursos del sistema. Verificar parejas realmente usadas con contraste ≥4,5:1 en texto normal y ≥3:1 en contornos informativos y texto grande.

| Elemento | Especificación inicial |
|---|---|
| Jerarquía | Título 28/34 seminegrita, sección 20/26, cuerpo 14/20, tabla compacta 13/18, cifra destacada 24/30. Unidades visibles. |
| Ritmo | Múltiplos de 4 DIP, márgenes 24 en ancho amplio y 12 en estrecho, separación de secciones 16, radios de panel 8 y controles 4. Filas 36 compactas o 44 cómodas. |
| Marco | ≥1200 DIP: navegación 216, inspector opcional 320. 900–1199: carril lateral de 56, inspector superpuesto. <900: menú accesible en cabecera e inspector/página de detalle completa. |
| Tabla | Encabezado fijo y nombre prioritario, cifras a la derecha. Ajuste de columnas/orden y desbordamiento horizontal localizado para columnas secundarias. |
| Estados | Foco visible continuo de 2 DIP, selección con marca y fondo, estado activo con etiqueta. Diferenciar lesión, dato pendiente y desactualizado mediante icono + texto. |
| Datos deportivos | Atributo exacto «14», rango «12–16», desconocido «Sin observar». DWRS con perfil y cobertura, percentil con cohorte. Ningún cero decorativo para llenar huecos. |
| Movimiento | Transiciones breves sólo para mantener orientación; respetar reducción de movimiento. Evitar desenfoque repetido, animaciones en listas y parpadeo al reordenar. |

Tab y Mayús+Tab recorren acciones en orden visual; flechas navegan listas y marcadores, Intro abre y Esc vuelve al invocador. Diálogos conservan foco dentro y devuelven el foco al control de origen. Con texto ampliado, filtros secundarios pasan al panel y las acciones siguen disponibles. Tooltips complementan nombres accesibles. Diseñar también para 1366×768, 1920×1080 y 2560×1440 con escala de Windows 100–200 %.
