using System;
using System.ComponentModel;
using System.Linq;

namespace PadronApi.Dto
{
    public class Ciudad : INotifyPropertyChanged
    {
        private int idCiudad;
        private string ciudad;
        private string ciudadStr;
        private int idEstado;

        public int IdCiudad
        {
            get
            {
                return this.idCiudad;
            }
            set
            {
                this.idCiudad = value;
            }
        }

        public string CiudadDesc
        {
            get
            {
                return this.ciudad;
            }
            set
            {
                this.ciudad = value;
                this.OnPropertyChanged("CiudadDesc");
            }
        }

        public string CiudadStr
        {
            get
            {
                return this.ciudadStr;
            }
            set
            {
                this.ciudadStr = value;
            }
        }

        public int IdEstado
        {
            get
            {
                return this.idEstado;
            }
            set
            {
                this.idEstado = value;
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
