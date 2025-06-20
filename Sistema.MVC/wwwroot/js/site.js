// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    // Cargar alertas de mantenimiento al inicio
    $('#alerta-camiones').load('/RiesgoFallo/CamionesProximosMantenimiento', function () {
        // Actualizar el contador de alertas
        var alertaCount = $('#alerta-lista .dropdown-item').length;
        if (alertaCount > 0) {
            $('#badge-alerts').text(alertaCount);
        } else {
            $('#badge-alerts').hide();  // Ocultar el badge si no hay alertas
        }
    });

    // Cargar las alertas cuando se hace clic en el ícono de la campanita
    $('#notificaciones').click(function () {
        $.ajax({
            url: '/RiesgoFallo/CamionesProximosMantenimiento',
            method: 'GET',
            success: function (data) {
                $('#alerta-lista').html('');  // Limpiar la lista de alertas
                if (data && data.length > 0) {
                    data.forEach(function (mantenimiento) {
                        var alertClass = mantenimiento.tipo === 'Preventivo' ? 'alert-warning' : 'alert-danger';
                        var alertaHtml = `<li class="dropdown-item">
                                              <div class="alert ${alertClass} mb-1">
                                                  <strong>Atención:</strong> El camión ${mantenimiento.camion.placa} está próximo a necesitar mantenimiento (Descripción: ${mantenimiento.descripcion}).
                                              </div>
                                          </li>`;
                        $('#alerta-lista').append(alertaHtml);
                    });
                    // Actualizar el contador de alertas
                    $('#badge-alerts').text(data.length);
                }
            }
        });
    });
});

