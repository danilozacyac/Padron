using ScjnUtilities;
using System;
using System.ComponentModel;
using System.Linq;

namespace PadronApi.Dto
{
    public class PadronGenerado : INotifyPropertyChanged
    {
        private int idPadron;
        private int numAcuerdo;
        private int anioAcuerdo;
        private int idObra;
        private string tituloObra;
        private readonly string tituloObraStr;
        private DateTime? fechaGenerado;
        private DateTime? fechaDistribucion;
        private int tiraje;
        private int idAcuerdo;
        private int oficioInicial;
        private int oficioFinal;


        public int IdPadron
        {
            get
            {
                return this.idPadron;
            }
            set
            {
                this.idPadron = value;
            }
        }

        public int NumAcuerdo
        {
            get
            {
                return this.numAcuerdo;
            }
            set
            {
                this.numAcuerdo = value;
                this.OnPropertyChanged("NumAcuerdo");
            }
        }

        public int AnioAcuerdo
        {
            get
            {
                return this.anioAcuerdo;
            }
            set
            {
                this.anioAcuerdo = value;
            }
        }

        public int IdObra
        {
            get
            {
                return this.idObra;
            }
            set
            {
                this.idObra = value;
            }
        }

        public string TituloObra
        {
            get
            {
                return this.tituloObra;
            }
            set
            {
                this.tituloObra = value;
            }
        }

        public string TituloObraStr
        {
            get
            {
                return StringUtilities.PrepareToAlphabeticalOrder(this.tituloObra);
            }
        }

        public DateTime? FechaGenerado
        {
            get
            {
                return this.fechaGenerado;
            }
            set
            {
                this.fechaGenerado = value;
            }
        }

        public DateTime? FechaDistribucion
        {
            get
            {
                return this.fechaDistribucion;
            }
            set
            {
                this.fechaDistribucion = value;
            }
        }

        public int Tiraje
        {
            get
            {
                return this.tiraje;
            }
            set
            {
                this.tiraje = value;
            }
        }

        public int IdAcuerdo
        {
            get
            {
                return this.idAcuerdo;
            }
            set
            {
                this.idAcuerdo = value;
            }
        }


        public int OficioInicial
        {
            get
            {
                return this.oficioInicial;
            }
            set
            {
                this.oficioInicial = value;
                this.OnPropertyChanged("OficioInicial");
            }
        }

        public int OficioFinal
        {
            get
            {
                return this.oficioFinal;
            }
            set
            {
                this.oficioFinal = value;
                this.OnPropertyChanged("OficioFinal");
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
