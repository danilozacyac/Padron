using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PadronApi.Dto
{
    [Serializable]
    public class Obra : INotifyPropertyChanged
    {
        private int idObra;
        private int orden = 0;
        private string titulo = String.Empty;
        private string tituloStr;
        private string sintesis;
        private string sintesisStr;
        private string numMaterial = String.Empty;
        private int anioPublicacion = DateTime.Now.Year;
        private int edicion = 1;
        private string isbn;
        private int pais;
        private int idIdioma;
        private int paginas;
        private int numLibros = 1;
        private string clasificacion;
        private int tipoObra = -1;
        private int medioPublicacion;
        private int idTipoPublicacion;
        private string tipoPublicacionStr;
        private int presentacion = -1;
        private int materia;
        private string descripcion;
        private string descripcionStr;
        private int consecutivo;
        private string precio;
        private string imagePath;
        private int conMay;
        private int nivel;
        private int padre;
        private int idUsuario;
        private DateTime fechaUsuario;
        private int fechaUsuarioInt;
        private int tiraje = 1000;
        private bool agotado;
        private int activo;
        private DateTime? fechaRecibe;
        private int fechaRecibeInt;
        private DateTime? fechaDistribuye;
        private int fechaDistribuyeInt;
        private bool muestraEnKiosko;
        private bool isReadOnly;
        private ObservableCollection<Autor> autores;
        private Obra parentItem;
        private string catalografica;
        private bool esCategoria;

        public Obra() { }

        public Obra(Obra parentItem)
        {
            this.parentItem = parentItem;
        }

        /// <summary>
        /// Indica si la obra será puesta a disposición del personal 
        /// </summary>
        private bool aDisposicion;

        private ObservableCollection<Obra> obraChild;

        

       public Obra ParentItem
        {
            get
            {
                return this.parentItem;
            }
            set
            {
                this.parentItem = value;
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

        [Range(0, Int64.MaxValue, ErrorMessage = "El orden debe de ser mayor a cero y menor a 32000000")]
        public int Orden
        {
            get
            {
                return this.orden;
            }
            set
            {
                this.orden = value;
                this.OnPropertyChanged("Orden");
            }
        }

        [Required(AllowEmptyStrings=false,ErrorMessage="Para dar de alta una obra es necesario ingresar el título de la misma")]
        public string Titulo
        {
            get
            {
                return this.titulo;
            }
            set
            {
                this.titulo = value;
                OnPropertyChanged("Titulo");
            }
        }

        public string TituloStr
        {
            get
            {
                return this.tituloStr;
            }
            set
            {
                this.tituloStr = value;
            }
        }

        [StringLength(10000,MinimumLength=25,ErrorMessage="Ingresa la sintesis de esta publicación")]
        public string Sintesis
        {
            get
            {
                return this.sintesis;
            }
            set
            {
                this.sintesis = value;
                this.OnPropertyChanged("Sintesis");
            }
        }

        public string SintesisStr
        {
            get
            {
                return this.sintesisStr;
            }
            set
            {
                this.sintesisStr = value;
            }
        }

        [Required(AllowEmptyStrings=false,ErrorMessage="Para dar de alta una obra es obligatorio ingresar el número de material")]
        [StringLength(12,MinimumLength=8)]
        public string NumMaterial
        {
            get
            {
                return this.numMaterial;
            }
            set
            {
                this.numMaterial = value;
                OnPropertyChanged("NumMaterial");
            }
        }


        [Range(1870,2017,ErrorMessage="El año de publicación debe estar entre el rango de 1870 y 2017" )]
        public int AnioPublicacion
        {
            get
            {
                return this.anioPublicacion;
            }
            set
            {
                this.anioPublicacion = value;
                OnPropertyChanged("AnioPublicacion");
            }
        }

        [Range(1,50,ErrorMessage="Ingresa la edición de esta obra")]
        public int Edicion
        {
            get
            {
                return this.edicion;
            }
            set
            {
                this.edicion = value;
            }
        }

        public string Isbn
        {
            get
            {
                return this.isbn;
            }
            set
            {
                this.isbn = value;
                OnPropertyChanged("Isbn");
            }
        }

        [Range(1,100,ErrorMessage="Selecciona el país de origen de la publicación")]
        public int Pais
        {
            get
            {
                return this.pais;
            }
            set
            {
                this.pais = value;
            }
        }

        [Range(1,10,ErrorMessage="Selecciona el idioma de publicación")]
        public int IdIdioma
        {
            get
            {
                return this.idIdioma;
            }
            set
            {
                this.idIdioma = value;
            }
        }

        public int Paginas
        {
            get
            {
                return this.paginas;
            }
            set
            {
                this.paginas = value;
                this.OnPropertyChanged("Paginas");
            }
        }

        [Range(1,100,ErrorMessage="Ingresa el número de tomos que integran esa publicación")]
        public int NumLibros
        {
            get
            {
                return this.numLibros;
            }
            set
            {
                this.numLibros = value;
            }
        }

        public string Clasificacion
        {
            get
            {
                return this.clasificacion;
            }
            set
            {
                this.clasificacion = value;
            }
        }

        [Range(1,50,ErrorMessage="Debes de seleccionar el tipo de la obra que estas dando de alta")]
        public int TipoObra
        {
            get
            {
                return this.tipoObra;
            }
            set
            {
                this.tipoObra = value;
            }
        }

        [Range(1,15,ErrorMessage="Selecciona el medio en que esta obra fue publicada")]
        public int MedioPublicacion
        {
            get
            {
                return this.medioPublicacion;
            }
            set
            {
                this.medioPublicacion = value;
            }
        }

        [Range(0,40,ErrorMessage="Selecciona el tipo de publicación de que se trata")]
        public int IdTipoPublicacion
        {
            get
            {
                return this.idTipoPublicacion;
            }
            set
            {
                this.idTipoPublicacion = value;
            }
        }

        public string TipoPublicacionStr
        {
            get
            {
                return this.tipoPublicacionStr;
            }
            set
            {
                this.tipoPublicacionStr = value;
            }
        }

        [Range(1,50,ErrorMessage="Debes de seleccionar la presentación de la obra que estas dando de alta")]
        public int Presentacion
        {
            get
            {
                return this.presentacion;
            }
            set
            {
                this.presentacion = value;
            }
        }

        public int Materia
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

        public string Descripcion
        {
            get
            {
                return this.descripcion;
            }
            set
            {
                this.descripcion = value;
            }
        }

        public string DescripcionStr
        {
            get
            {
                return this.descripcionStr;
            }
            set
            {
                this.descripcionStr = value;
            }
        }

        public int Consecutivo
        {
            get
            {
                return this.consecutivo;
            }
            set
            {
                this.consecutivo = value;
            }
        }

        public string Precio
        {
            get
            {
                return this.precio;
            }
            set
            {
                this.precio = value;
                this.OnPropertyChanged("Precio");
            }
        }

        public string ImagePath
        {
            get
            {
                return this.imagePath;
            }
            set
            {
                this.imagePath = value;
                this.OnPropertyChanged("ImagePath");
            }
        }

        public int ConMay
        {
            get
            {
                return this.conMay;
            }
            set
            {
                this.conMay = value;
            }
        }

        public int Nivel
        {
            get
            {
                return this.nivel;
            }
            set
            {
                this.nivel = value;
            }
        }

        public int Padre
        {
            get
            {
                return this.padre;
            }
            set
            {
                this.padre = value;
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

        public DateTime FechaUsuario
        {
            get
            {
                return this.fechaUsuario;
            }
            set
            {
                this.fechaUsuario = value;
            }
        }

        public int FechaUsuarioInt
        {
            get
            {
                return this.fechaUsuarioInt;
            }
            set
            {
                this.fechaUsuarioInt = value;
            }
        }

        [Required]
        [Range(1,50000,ErrorMessage="Ingresa el tiraje de esta obra")]
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

        public bool Agotado
        {
            get
            {
                return this.agotado;
            }
            set
            {
                this.agotado = value;
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

        public DateTime? FechaRecibe
        {
            get
            {
                return this.fechaRecibe;
            }
            set
            {
                this.fechaRecibe = value;
            }
        }

        public int FechaRecibeInt
        {
            get
            {
                return this.fechaRecibeInt;
            }
            set
            {
                this.fechaRecibeInt = value;
            }
        }

        public DateTime? FechaDistribuye
        {
            get
            {
                return this.fechaDistribuye;
            }
            set
            {
                this.fechaDistribuye = value;
            }
        }

        public int FechaDistribuyeInt
        {
            get
            {
                return this.fechaDistribuyeInt;
            }
            set
            {
                this.fechaDistribuyeInt = value;
            }
        }

        public bool MuestraEnKiosko
        {
            get
            {
                return this.muestraEnKiosko;
            }
            set
            {
                this.muestraEnKiosko = value;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return this.isReadOnly;
            }
            set
            {
                this.isReadOnly = value;
            }
        }

        public ObservableCollection<Autor> Autores
        {
            get
            {
                return this.autores;
            }
            set
            {
                this.autores = value;
                this.OnPropertyChanged("Autores");
            }
        }


        public bool ADisposicion
        {
            get
            {
                return this.aDisposicion;
            }
            set
            {
                this.aDisposicion = value;
            }
        }

        public ObservableCollection<Obra> ObraChild
        {
            get
            {
                return this.obraChild;
            }
            set
            {
                this.obraChild = value;
            }
        }

        public string Catalografica
        {
            get
            {
                return this.catalografica;
            }
            set
            {
                this.catalografica = value;
            }
        }


        public bool EsCategoria
        {
            get
            {
                return this.esCategoria;
            }
            set
            {
                this.esCategoria = value;
                this.OnPropertyChanged("EsCategoria");
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
