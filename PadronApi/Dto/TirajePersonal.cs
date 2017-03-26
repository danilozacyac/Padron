using System;
using System.Linq;

namespace PadronApi.Dto
{
    public class TirajePersonal
    {
        private bool isChecked;
        private int idAcuerdo;
        private string acuerdo;

        /// <summary>
        /// Cadena formada por el tiraje y la descripcion es la que se muestra en los combobox
        /// </summary>
        private string acDescripcion;
        private int particular;
        private int oficina;
        private int biblioteca;
        private int resguardo;
        private int personal;
        private int autor;
        private int activo;

        /// <summary>
        /// La descripcion de la plantilla
        /// </summary>
        private string descripcion;
       
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

        public bool IsChecked
        {
            get
            {
                return this.isChecked;
            }
            set
            {
                this.isChecked = value;
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

        public string Acuerdo
        {
            get
            {
                return this.acuerdo;
            }
            set
            {
                this.acuerdo = value;
            }
        }

        public string AcDescripcion
        {
            get
            {
                return this.acDescripcion;
            }
            set
            {
                this.acDescripcion = value;
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
    }
}
