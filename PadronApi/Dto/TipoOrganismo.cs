using System;
using System.Linq;

namespace PadronApi.Dto
{
    public class TipoOrganismo
    {
        private int idTipoOrganismo;
        private int idGrupo;
        private int orden;
        private string descripcion;
        private string descripcionAbr;

        public int IdTipoOrganismo
        {
            get
            {
                return this.idTipoOrganismo;
            }
            set
            {
                this.idTipoOrganismo = value;
            }
        }

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

        public string DescripcionAbr
        {
            get
            {
                return this.descripcionAbr;
            }
            set
            {
                this.descripcionAbr = value;
            }
        }
    }
}
