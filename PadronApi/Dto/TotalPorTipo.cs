using System;
using System.Linq;

namespace PadronApi.Dto
{
    public class TotalPorTipo
    {
        private string estado;
        private int cd;
        private int dvd;
        private int libro;
        private int ebook;

        /// <summary>
        /// Hace referencia a la distribución que involucra tanto libro como cd de una misma obra
        /// </summary>
        private int ambos;
        private int audioLibro;

        public string Estado
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

        public int Cd
        {
            get
            {
                return this.cd;
            }
            set
            {
                this.cd = value;
            }
        }

        public int Dvd
        {
            get
            {
                return this.dvd;
            }
            set
            {
                this.dvd = value;
            }
        }

        public int Libro
        {
            get
            {
                return this.libro;
            }
            set
            {
                this.libro = value;
            }
        }

        public int Ebook
        {
            get
            {
                return this.ebook;
            }
            set
            {
                this.ebook = value;
            }
        }

        public int Ambos
        {
            get
            {
                return this.ambos;
            }
            set
            {
                this.ambos = value;
            }
        }

        public int AudioLibro
        {
            get
            {
                return this.audioLibro;
            }
            set
            {
                this.audioLibro = value;
            }
        }
    }
}
