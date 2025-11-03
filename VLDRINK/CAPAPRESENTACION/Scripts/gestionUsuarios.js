fetch('/api/usuario')
    .then(response => {
        if (!response.ok) {
            throw new Error('Error en la respuesta de la API: ' + response.status);
        }
        return response.json();
    })
    .then(usuarios => {
        console.log("¡Datos recibidos desde la BBDD!");
        console.log(usuarios);

    })
    .catch(error => {
        console.error('Hubo un problema con la solicitud fetch:', error);
    });