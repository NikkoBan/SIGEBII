// wwwroot/js/site.js

// Funciones JavaScript personalizadas para la aplicación SIGEBI.Web.
// Este script se carga después de jQuery y Bootstrap.

document.addEventListener('DOMContentLoaded', function () {
    console.log("Scripts personalizados de SIGEBI.Web cargados.");

    var successAlert = document.getElementById('success-alert');
    if (successAlert) {
        setTimeout(function () {
            successAlert.classList.remove('show');
            successAlert.classList.add('fade');
            setTimeout(function () {
                successAlert.remove();
            }, 500); // Esperar que la transición fade termine
        }, 5000); // Desaparece después de 5 segundos
    }
});

if (typeof jQuery != 'undefined') {
    $(function () {
        console.log("jQuery está listo en site.js");
    });
}