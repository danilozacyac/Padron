using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace PadronApi.Dto
{
    public class Adscripcion : INotifyPropertyChanged
    {
        private Organismo organismo;
        private Titular titular;
        private ObservableCollection<TirajePersonal> tirajes;
        private int funcion;
        private string obrasRecibe;
        private DateTime? fechaAlta;
        private DateTime? fechaBaja;

        public Adscripcion()
        {
            organismo = new Organismo();
            titular = new Titular();
        }
        

        public Organismo Organismo
        {
            get
            {
                return this.organismo;
            }
            set
            {
                this.organismo = value;
            }
        }

        public Titular Titular
        {
            get
            {
                return this.titular;
            }
            set
            {
                this.titular = value;
            }
        }

        public ObservableCollection<TirajePersonal> Tirajes
        {
            get
            {
                return this.tirajes;
            }
            set
            {
                this.tirajes = value;
            }
        }

        public int Funcion
        {
            get
            {
                return this.funcion;
            }
            set
            {
                this.funcion = value;
                this.OnPropertyChanged("Funcion");
            }
        }

        public string ObrasRecibe
        {
            get
            {
                return this.obrasRecibe;
            }
            set
            {
                this.obrasRecibe = value;
            }
        }

        public DateTime? FechaAlta
        {
            get
            {
                return this.fechaAlta;
            }
            set
            {
                this.fechaAlta = value;
            }
        }

        public DateTime? FechaBaja
        {
            get
            {
                return this.fechaBaja;
            }
            set
            {
                this.fechaBaja = value;
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

            //if (propertyName.Equals("QuiereDistribucion"))
            //{
            //    if (quiereDistribucion)
            //        this.Observaciones += "\r\n" + "Con fecha de " + DateTimeUtilities.ToLongDateFormat(DateTime.Now) + " solicito se le vuelva a considerar en la distribución";
            //    else
            //        this.Observaciones += "\r\n" + "Con fecha de " + DateTimeUtilities.ToLongDateFormat(DateTime.Now) + " indico que no desea continuar recibiendo obras";
            //}
        }

        #endregion // INotifyPropertyChanged Members
    }
}
