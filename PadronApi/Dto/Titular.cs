using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PadronApi.Dto
{
    public class Titular : INotifyPropertyChanged
    {
        private int idTitular;
        private string claveTitular;
        private string nombre;
        private string nombreStr;
        private string apellidos;
        private string apellidosStr;
        private string nombreCompleto = String.Empty;
        private int idTitulo = 0;
        private int estado;
        private string observaciones = String.Empty;
        private int quiereDistribucion;
        private string correo = String.Empty;
        private int totalAdscripciones;
        private int genero = 2;
        private bool haPublicado;

        private ObservableCollection<Adscripcion> adscripciones;

       
        public string NombreCompleto
        {
            get
            {
                return this.nombreCompleto;
            }
            set
            {
                this.nombreCompleto = value;
            }
        }

        public int Genero
        {
            get
            {
                return this.genero;
            }
            set
            {
                this.genero = value;
            }
        }

        public int TotalAdscripciones
        {
            get
            {
                return this.totalAdscripciones;
            }
            set
            {
                this.totalAdscripciones = value;
                this.OnPropertyChanged("TotalAdscripciones");
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

        public string Correo
        {
            get
            {
                return this.correo;
            }
            set
            {
                this.correo = value;
                
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

        public string ClaveTitular
        {
            get
            {
                return this.claveTitular;
            }
            set
            {
                this.claveTitular = value;
            }
        }

        [Required(AllowEmptyStrings=false,ErrorMessage="Para dar de alta un titular debes ingresar su(s) nombre(s)")]
        public string Nombre
        {
            get
            {
                return this.nombre;
            }
            set
            {
                this.nombre = value;
                this.OnPropertyChanged("Nombre");
            }
        }

        public string NombreStr
        {
            get
            {
                return this.nombreStr;
            }
            set
            {
                this.nombreStr = value;
            }
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Para dar de alta un titular debes ingresar sus apellidos")]
        public string Apellidos
        {
            get
            {
                return this.apellidos;
            }
            set
            {
                this.apellidos = value;
                this.OnPropertyChanged("Apellidos");
            }
        }

        public string ApellidosStr
        {
            get
            {
                return this.apellidosStr;
            }
            set
            {
                this.apellidosStr = value;
            }
        }

        [Range(1,200,ErrorMessage="Selecciona el título o grado académico")]
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

        public string Observaciones
        {
            get
            {
                return this.observaciones;
            }
            set
            {
                this.observaciones = value;
                this.OnPropertyChanged("Observaciones");
            }
        }

        public int QuiereDistribucion
        {
            get
            {
                return this.quiereDistribucion;
            }
            set
            {
                this.quiereDistribucion = value;
                this.OnPropertyChanged("QuiereDistribucion");
            }
        }


        public bool HaPublicado
        {
            get
            {
                return this.haPublicado;
            }
            set
            {
                this.haPublicado = value;
                this.OnPropertyChanged("HaPublicado");
            }
        }


        public bool IsEqualTo(Titular compareTo)
        {
            return (Estado == compareTo.Estado && Apellidos.Equals(compareTo.apellidos) && Nombre.Equals(compareTo.Nombre));
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
