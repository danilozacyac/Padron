using System;
using System.Linq;

namespace PadronApi.Dto
{
    public class Titulo
    {
        private int idTitulo;
        private string tituloDesc;
        private string tituloAbr;
        private int orden;
        private int idGenero;

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

        public string TituloDesc
        {
            get
            {
                return this.tituloDesc;
            }
            set
            {
                this.tituloDesc = value;
            }
        }

        public string TituloAbr
        {
            get
            {
                return this.tituloAbr;
            }
            set
            {
                this.tituloAbr = value;
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

        public int IdGenero
        {
            get
            {
                return this.idGenero;
            }
            set
            {
                this.idGenero = value;
            }
        }
    }
}
