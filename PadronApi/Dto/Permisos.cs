using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace PadronApi.Dto
{
    public class Permisos
    {
        private bool isSelected;
        private int idSeccion;
        private int idPadre;
        private string seccion;
        private ObservableCollection<Permisos> seccionesHijo;

        public bool IsSelected
        {
            get
            {
                return this.isSelected;
            }
            set
            {
                this.isSelected = value;
            }
        }

        public int IdSeccion
        {
            get
            {
                return this.idSeccion;
            }
            set
            {
                this.idSeccion = value;
            }
        }

        public int IdPadre
        {
            get
            {
                return this.idPadre;
            }
            set
            {
                this.idPadre = value;
            }
        }

        public string Seccion
        {
            get
            {
                return this.seccion;
            }
            set
            {
                this.seccion = value;
            }
        }

        public ObservableCollection<Permisos> SeccionesHijo
        {
            get
            {
                return this.seccionesHijo;
            }
            set
            {
                this.seccionesHijo = value;
            }
        }

        
    }
}
