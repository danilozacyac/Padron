using System;
using System.ComponentModel;
using System.Linq;

namespace PadronApi.Dto
{
    public class PlantillaDto : INotifyPropertyChanged
    {
        private int idPadron;
        private int idTitulo;
        private string titulo;
        private int idTitular;
        private int idObra;
        private string tituloObra;
        private int idOrganismo;
        private string organismo;
        private string nombre;
        private int ciudad;
        private string ciudadStr;
        private int estado;
        private string estadoStr;
        private int tipoDistribucion;
        private int tipoOrganismo;
        private int grupoOrganismo;
        private int particular;
        private int oficina;
        private int biblioteca;
        private int resguardo;
        private int personal;
        private int autor;
        private int totalDevoluciones;
        private int numAcuerdo;
        private int anioAcuerdo;
        private int oficio;
        private int funcion;
        private string obrasRecibe;
        //private DateTime? fechaRecPaqueteria;
        //private string numGuia;
        //private string archivoGuia;
        //private DateTime? fechaRecAcuse;
        //private string archivoAcuse;
        private DateTime? fechaEnvio;
        private int cancelado;

       

        public int Cancelado
        {
            get
            {
                return this.cancelado;
            }
            set
            {
                this.cancelado = value;
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

        public int TipoOrganismo
        {
            get
            {
                return this.tipoOrganismo;
            }
            set
            {
                this.tipoOrganismo = value;
            }
        }

        public int GrupoOrganismo
        {
            get
            {
                return this.grupoOrganismo;
            }
            set
            {
                this.grupoOrganismo = value;
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

        public int Funcion
        {
            get
            {
                return this.funcion;
            }
            set
            {
                this.funcion = value;
            }
        }

        public int Oficio
        {
            get
            {
                return this.oficio;
            }
            set
            {
                this.oficio = value;
            }
        }

        public DateTime? FechaEnvio
        {
            get
            {
                return this.fechaEnvio;
            }
            set
            {
                this.fechaEnvio = value;
            }
        }

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

        //public DateTime? FechaRecPaqueteria
        //{
        //    get
        //    {
        //        return this.fechaRecPaqueteria;
        //    }
        //    set
        //    {
        //        this.fechaRecPaqueteria = value;
        //        this.OnPropertyChanged("FechaRecPaqueteria");
        //    }
        //}

        //public string NumGuia
        //{
        //    get
        //    {
        //        return this.numGuia;
        //    }
        //    set
        //    {
        //        this.numGuia = value;
        //        this.OnPropertyChanged("NumGuia");
        //    }
        //}

        //public string ArchivoGuia
        //{
        //    get
        //    {
        //        return this.archivoGuia;
        //    }
        //    set
        //    {
        //        this.archivoGuia = value;
        //        this.OnPropertyChanged("ArchivoGuia");
        //    }
        //}

        //public DateTime? FechaRecAcuse
        //{
        //    get
        //    {
        //        return this.fechaRecAcuse;
        //    }
        //    set
        //    {
        //        this.fechaRecAcuse = value;
        //        this.OnPropertyChanged("FechaRecAcuse");
        //    }
        //}

        //public string ArchivoAcuse
        //{
        //    get
        //    {
        //        return this.archivoAcuse;
        //    }
        //    set
        //    {
        //        this.archivoAcuse = value;
        //        this.OnPropertyChanged("ArchivoAcuse");
        //    }
        //}

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

        public string Titulo
        {
            get
            {
                return this.titulo;
            }
            set
            {
                this.titulo = value;
            }
        }

        public int IdTitulo
        {
            get
            {
                return this.idTitulo;
            }
            set
            {
                this.idTitulo = value;
            }
        }

        public int IdTitular
        {
            get
            {
                return this.idTitular;
            }
            set
            {
                this.idTitular = value;
            }
        }

        public int IdOrganismo
        {
            get
            {
                return this.idOrganismo;
            }
            set
            {
                this.idOrganismo = value;
            }
        }

        public string Organismo
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

        public string Nombre
        {
            get
            {
                return this.nombre;
            }
            set
            {
                this.nombre = value;
            }
        }

        public int Ciudad
        {
            get
            {
                return this.ciudad;
            }
            set
            {
                this.ciudad = value;
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

        public int Estado
        {
            get
            {
                return this.estado;
            }
            set
            {
                this.estado = value;
            }
        }

        public string EstadoStr
        {
            get
            {
                return this.estadoStr;
            }
            set
            {
                this.estadoStr = value;
            }
        }

        public int TipoDistribucion
        {
            get
            {
                return this.tipoDistribucion;
            }
            set
            {
                this.tipoDistribucion = value;
            }
        }

        public int Particular
        {
            get
            {
                return this.particular;
            }
            set
            {
                this.particular = value;
            }
        }

        public int Oficina
        {
            get
            {
                return this.oficina;
            }
            set
            {
                this.oficina = value;
            }
        }

        public int Biblioteca
        {
            get
            {
                return this.biblioteca;
            }
            set
            {
                this.biblioteca = value;
            }
        }

        public int Resguardo
        {
            get
            {
                return this.resguardo;
            }
            set
            {
                this.resguardo = value;
            }
        }

        public int Personal
        {
            get
            {
                return this.personal;
            }
            set
            {
                this.personal = value;
            }
        }

        public int Autor
        {
            get
            {
                return this.autor;
            }
            set
            {
                this.autor = value;
            }
        }

        public int TotalDevoluciones
        {
            get
            {
                return this.totalDevoluciones;
            }
            set
            {
                this.totalDevoluciones = value;
                this.OnPropertyChanged("TotalDevoluciones");
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
