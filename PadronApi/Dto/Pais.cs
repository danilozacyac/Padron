using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace PadronApi.Dto
{
    public class Pais : INotifyPropertyChanged
    {
        private int idPais;
        private string pais;
        private string paisStr;
        private ObservableCollection<Estado> estados;

        public int IdPais
        {
            get
            {
                return this.idPais;
            }
            set
            {
                this.idPais = value;
            }
        }

        public string PaisDesc
        {
            get
            {
                return this.pais;
            }
            set
            {
                this.pais = value;
                this.OnPropertyChanged("PaisDesc");
            }
        }

        public string PaisStr
        {
            get
            {
                return this.paisStr;
            }
            set
            {
                this.paisStr = value;
            }
        }

        public ObservableCollection<Estado> Estados
        {
            get
            {
                return this.estados;
            }
            set
            {
                this.estados = value;
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
