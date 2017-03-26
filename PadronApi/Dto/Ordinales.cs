using System;
using System.Linq;

namespace PadronApi.Dto
{
    public class Ordinales
    {
        private int idOrdinal;
        private string ordinal;
        private string ordinalMay;
        private int idEstado;

        public int IdOrdinal
        {
            get
            {
                return this.idOrdinal;
            }
            set
            {
                this.idOrdinal = value;
            }
        }

        public string Ordinal
        {
            get
            {
                return this.ordinal;
            }
            set
            {
                this.ordinal = value;
            }
        }

        public string OrdinalMay
        {
            get
            {
                return this.ordinalMay;
            }
            set
            {
                this.ordinalMay = value;
            }
        }

        public int IdEstado
        {
            get
            {
                return this.idEstado;
            }
            set
            {
                this.idEstado = value;
            }
        }
    }
}