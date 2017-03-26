using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PadronApi.Dto
{
    public class Organismo : INotifyPropertyChanged
    {
        private int idGrupo;
        private int idOrganismo;
        private string organismoDesc = String.Empty;
        private string organismoStr = String.Empty;
        private int tipoOrganismo ;
        private string tipoOrganismoStr = String.Empty;
        private int circuito;
        private int ordinal;
        private string materia;
        private int ciudad;
        private int estado;
        private string calle = String.Empty;
        private string colonia = String.Empty;
        private string delegacion = String.Empty;
        private string cp = String.Empty;
        private string telefono = String.Empty;
        private string extension = String.Empty;
        private string telefono2 = String.Empty;
        private string extension2 = String.Empty;
        private string telefono3 = String.Empty;
        private string extension3 = String.Empty;
        private string telefono4 = String.Empty;
        private string extension4 = String.Empty;
        private string mail = String.Empty;
        private string observaciones = String.Empty;
        private int activo;
        private int tipoDistr;
        private string distribucion = String.Empty;
        private string abreviado = String.Empty;
        private int orden;
        private int idUsuario;
        private int fecha;
        private ObservableCollection<Adscripcion> adscripciones;
        private int totalAdscritos;
        private int tienePresidente;
        

        

        

        

        public int IdGrupo
        {
            get
            {
                return this.idGrupo;
            }
            set
            {
                this.idGrupo = value;
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

        [Required(AllowEmptyStrings=false,ErrorMessage="Ingresa la descripción del organismos que deseas dar de alta")]
        public string OrganismoDesc
        {
            get
            {
                return this.organismoDesc;
            }
            set
            {
                this.organismoDesc = value;
                this.OnPropertyChanged("OrganismoDesc");
            }
        }

        public string OrganismoStr
        {
            get
            {
                return this.organismoStr;
            }
            set
            {
                this.organismoStr = value;
            }
        }

        [Range(1, 1000, ErrorMessage = "Selecciona el tipo de organismo")]
        public int TipoOrganismo
        {
            get
            {
                return this.tipoOrganismo;
            }
            set
            {
                this.tipoOrganismo = value;
                this.OnPropertyChanged("TipoOrganismo");
            }
        }

        
        public string TipoOrganismoStr
        {
            get
            {
                return this.tipoOrganismoStr;
            }
            set
            {
                this.tipoOrganismoStr = value;
                this.OnPropertyChanged("TipoOrganismoStr");
            }
        }

        public int Circuito
        {
            get
            {
                return this.circuito;
            }
            set
            {
                this.circuito = value;
            }
        }

        public int Ordinal
        {
            get
            {
                return this.ordinal;
            }
            set
            {
                this.ordinal = value;
                this.OnPropertyChanged("Ordinal");
            }
        }

        public string Materia
        {
            get
            {
                return this.materia;
            }
            set
            {
                this.materia = value;
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
                this.OnPropertyChanged("Ciudad");
            }
        }

        [Range(1,int.MaxValue,ErrorMessage="Selecciona el país y estado al que pertenece este organismo")]
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

        [Required(AllowEmptyStrings=false,ErrorMessage="Ingresa la calle donde se encuentra ubicado este organismo")]
        public string Calle
        {
            get
            {
                return this.calle;
            }
            set
            {
                this.calle = value;
            }
        }


        public string Colonia
        {
            get
            {
                return this.colonia;
            }
            set
            {
                this.colonia = value;
            }
        }

        public string Delegacion
        {
            get
            {
                return this.delegacion;
            }
            set
            {
                this.delegacion = value;
            }
        }

        public string Cp
        {
            get
            {
                return this.cp;
            }
            set
            {
                this.cp = value;
            }
        }

        public string Telefono
        {
            get
            {
                return this.telefono;
            }
            set
            {
                this.telefono = value;
            }
        }

        public string Extension
        {
            get
            {
                return this.extension;
            }
            set
            {
                this.extension = value;
            }
        }

        public string Telefono2
        {
            get
            {
                return this.telefono2;
            }
            set
            {
                this.telefono2 = value;
            }
        }

        public string Extension2
        {
            get
            {
                return this.extension2;
            }
            set
            {
                this.extension2 = value;
            }
        }

        public string Telefono3
        {
            get
            {
                return this.telefono3;
            }
            set
            {
                this.telefono3 = value;
            }
        }

        public string Extension3
        {
            get
            {
                return this.extension3;
            }
            set
            {
                this.extension3 = value;
            }
        }

        public string Telefono4
        {
            get
            {
                return this.telefono4;
            }
            set
            {
                this.telefono4 = value;
            }
        }

        public string Extension4
        {
            get
            {
                return this.extension4;
            }
            set
            {
                this.extension4 = value;
            }
        }

        public string Mail
        {
            get
            {
                return this.mail;
            }
            set
            {
                this.mail = value;
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

        public int Activo
        {
            get
            {
                return this.activo;
            }
            set
            {
                this.activo = value;
            }
        }

        [Range(1,10,ErrorMessage="Selecciona el tipo de distribución")]
        public int TipoDistr
        {
            get
            {
                return this.tipoDistr;
            }
            set
            {
                this.tipoDistr = value;
                this.OnPropertyChanged("TipoDistr");
            }
        }

        public string Distribucion
        {
            get
            {
                return this.distribucion;
            }
            set
            {
                this.distribucion = value;
                this.OnPropertyChanged("Distribucion");
            }
        }

        public string Abreviado
        {
            get
            {
                return this.abreviado;
            }
            set
            {
                this.abreviado = value;
            }
        }

        public int Orden
        {
            get
            {
                return this.orden;
            }
            set
            {
                this.orden = value;
            }
        }

        public ObservableCollection<Adscripcion> Adscripciones
        {
            get
            {
                return this.adscripciones;
            }
            set
            {
                this.adscripciones = value;
            }
        }

        public int TienePresidente
        {
            get
            {
                return this.tienePresidente;
            }
            set
            {
                this.tienePresidente = value;
                this.OnPropertyChanged("TienePresidente");
            }
        }

        public int TotalAdscritos
        {
            get
            {
                return this.totalAdscritos;
            }
            set
            {
                this.totalAdscritos = value;
                this.OnPropertyChanged("TotalAdscritos");
            }
        }

        public int IdUsuario
        {
            get
            {
                return this.idUsuario;
            }
            set
            {
                this.idUsuario = value;
            }
        }

        public int Fecha
        {
            get
            {
                return this.fecha;
            }
            set
            {
                this.fecha = value;
            }
        }

        public bool IsEqualTo(Organismo compareTo)
        {
            return (TipoOrganismo == compareTo.TipoOrganismo && OrganismoDesc.Equals(compareTo.organismoDesc) && 
                Circuito == compareTo.Circuito && Ordinal == compareTo.Ordinal && Materia == compareTo.Materia && 
                Ciudad == compareTo.Ciudad && Estado == compareTo.Estado && Calle.Equals(compareTo.Calle) && 
                Colonia.Equals(compareTo.Colonia) && Delegacion.Equals(compareTo.Delegacion) && Cp.Equals(compareTo.Cp) && 
                TipoDistr == compareTo.TipoDistr && Activo == compareTo.Activo && Observaciones.Equals(compareTo.Observaciones));
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
