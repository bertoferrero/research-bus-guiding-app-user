using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BusGuiding.Views.Dev
{
    public partial class DriverDemoTestPage : ContentPage
    {
        public DriverDemoTestPage()
        {
            InitializeComponent();
            //TODO inicializar la vista, descargando la lista de lineas
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //TODO ocultar panel de trabajo
            //TODO mostrar formulario
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            //TODO desubscripcion de eventos
            //TODO desvinculado de usuario y vehiculo
            //TODO desubscripción de eventos locales
            //TODO limpieza de formulario
            //TODO ocultar panel de trabajo
        }

        //Al pinchar en comenzar
        //TODO descargar lista de paradas de la linea, localizar la actual
        //TODO enviar vinculación entre usuario y vehículo
        //Localizar la parada siguiente a la actual y enviar solicitud de parada
        
        //Al pulsar en Stop
        //
    }
}
