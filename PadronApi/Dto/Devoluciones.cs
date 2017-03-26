using System;
using System.Linq;

namespace PadronApi.Dto
{
    public class Devoluciones : PlantillaDto
    {
        private string oficioDevolucion = String.Empty;
        private DateTime? fechaDevolucion;
        private string fechaDevolucionString = String.Empty;
        private string observaciones = String.Empty;
        private int cantidad;
        private int propiedad;
        private int tipoDevolucion;

        

        public string OficioDevolucion
        {
            get
            {
                return this.oficioDevolucion;
            }
            set
            {
                this.oficioDevolucion = value;
            }
        }

        public DateTime? FechaDevolucion
        {
            get
            {
                return this.fechaDevolucion;
            }
            set
            {
                this.fechaDevolucion = value;
            }
        }

        public string FechaDevolucionString
        {
            get
            {
                return this.fechaDevolucionString;
            }
            set
            {
                this.fechaDevolucionString = value;
            }
        }

        public string Observaciones
        {
            get
            {
                return this.observaciones;
            }
            set
            {
                this.observaciones = value;
            }
        }

        public int Cantidad
        {
            get
            {
                return this.cantidad;
            }
            set
            {
                this.cantidad = value;
            }
        }

        public int Propiedad
        {
            get
            {
                return this.propiedad;
            }
            set
            {
                this.propiedad = value;
            }
        }

        public int TipoDevolucion
        {
            get
            {
                return this.tipoDevolucion;
            }
            set
            {
                this.tipoDevolucion = value;
                this.OnPropertyChanged("TipoDevolucion");
            }
        }
    }
}
