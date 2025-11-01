// Hacemos la petición a la URL que acabamos de crear
fetch('/api/usuario')
    .then(response => {
        // Verificamos si la respuesta del servidor es exitosa
        if (!response.ok) {
            throw new Error('Error en la respuesta de la API: ' + response.status);
        }
        // Convertimos la respuesta de JSON a un objeto JavaScript
        return response.json();
    })
    .then(usuarios => {
        // 'usuarios' es el array de objetos 'Usuario'
        console.log("¡Datos recibidos desde la BBDD!");
        console.log(usuarios);

        // Verás que PasswordHash y PasswordSalt son 'null'
    })
    .catch(error => {
        // Manejamos cualquier error que haya ocurrido
        console.error('Hubo un problema con la solicitud fetch:', error);
    });