using System;
using System.ComponentModel;
using System.Linq;

namespace PadronApi.Dto
{
    public class Acuse : PlantillaDto, INotifyPropertyChanged
    {
        private DateTime? fechaEnvio;
        private string numGuia;
        private DateTime? fechaRecPaqueteria;
        private string quienRecibe;
        private string archivoGuia;
        private DateTime? fechaRecAcuse;
        private string archivoAcuse;
       
        public DateTime? FechaEnvio
        {
            get
            {
                return this.fechaEnvio;
            }
            set
            {
                this.fechaEnvio = value;
                this.OnPropertyChanged("FechaEnvio");
            }
        }

        public string NumGuia
        {
            get
            {
                return this.numGuia;
            }
            set
            {
                this.numGuia = value;
                this.OnPropertyChanged("NumGuia");
            }
        }

        public DateTime? FechaRecPaqueteria
        {
            get
            {
                return this.fechaRecPaqueteria;
            }
            set
            {
                this.fechaRecPaqueteria = value;
                this.OnPropertyChanged("FechaRecPaqueteria");
            }
        }

        public string QuienRecibe
        {
            get
            {
                return this.quienRecibe;
            }
            set
            {
                this.quienRecibe = value;
                this.OnPropertyChanged("QuienRecibe");
            }
        }

        public string ArchivoGuia
        {
            get
            {
                return this.archivoGuia;
            }
            set
            {
                this.archivoGuia = value;
                this.OnPropertyChanged("ArchivoGuia");
            }
        }

        public DateTime? FechaRecAcuse
        {
            get
            {
                return this.fechaRecAcuse;
            }
            set
            {
                this.fechaRecAcuse = value;
                this.OnPropertyChanged("FechaRecAcuse");
            }
        }

        public string ArchivoAcuse
        {
            get
            {
                return this.archivoAcuse;
            }
            set
            {
                this.archivoAcuse = value;
                this.OnPropertyChanged("ArchivoAcuse");
            }
        }

        #region INotifyPropertyChanged Members


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

        }

        #endregion // INotifyPropertyChanged Members
    }
}
