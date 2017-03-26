using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace PadronApi.Dto
{
    public class Autor : Titular
    {
        private int idRelObraAutor;
        private int idTipoAutor;
        private bool isAutor;
        private bool isCompilador;
        private bool isTraductor;
        private bool isCoordinador;
        private bool isComentarista;
        private bool isCoedicion;
        private bool isEstudio;
        private bool isPrologo;
        private bool isAdaptador;
        private bool isIlustrador;
        private bool isFotografo;
        private string textoParticipa;
        private string textoParticipaStr;
        private ObservableCollection<Obra> obras;

        public int IdRelObraAutor
        {
            get
            {
                return this.idRelObraAutor;
            }
            set
            {
                this.idRelObraAutor = value;
            }
        }

        public int IdTipoAutor
        {
            get
            {
                return this.idTipoAutor;
            }
            set
            {
                this.idTipoAutor = value;
            }
        }

        public bool IsAutor
        {
            get
            {
                return this.isAutor;
            }
            set
            {
                this.isAutor = value;
            }
        }

        public bool IsCompilador
        {
            get
            {
                return this.isCompilador;
            }
            set
            {
                this.isCompilador = value;
            }
        }

        public bool IsTraductor
        {
            get
            {
                return this.isTraductor;
            }
            set
            {
                this.isTraductor = value;
            }
        }

        public bool IsCoordinador
        {
            get
            {
                return this.isCoordinador;
            }
            set
            {
                this.isCoordinador = value;
            }
        }

        public bool IsComentarista
        {
            get
            {
                return this.isComentarista;
            }
            set
            {
                this.isComentarista = value;
            }
        }

        public bool IsCoedicion
        {
            get
            {
                return this.isCoedicion;
            }
            set
            {
                this.isCoedicion = value;
            }
        }

        public bool IsEstudio
        {
            get
            {
                return this.isEstudio;
            }
            set
            {
                this.isEstudio = value;
            }
        }

        public bool IsPrologo
        {
            get
            {
                return this.isPrologo;
            }
            set
            {
                this.isPrologo = value;
            }
        }

        public bool IsAdaptador
        {
            get
            {
                return this.isAdaptador;
            }
            set
            {
                this.isAdaptador = value;
            }
        }

        public bool IsIlustrador
        {
            get
            {
                return this.isIlustrador;
            }
            set
            {
                this.isIlustrador = value;
            }
        }

        public bool IsFotografo
        {
            get
            {
                return this.isFotografo;
            }
            set
            {
                this.isFotografo = value;
            }
        }

        public string TextoParticipa
        {
            get
            {
                return this.textoParticipa;
            }
            set
            {
                this.textoParticipa = value;
            }
        }

        public string TextoParticipaStr
        {
            get
            {
                return this.textoParticipaStr;
            }
            set
            {
                this.textoParticipaStr = value;
            }
        }

        public ObservableCollection<Obra> Obras
        {
            get
            {
                return this.obras;
            }
            set
            {
                this.obras = value;
            }
        }
    }
}
