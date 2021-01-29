using System;
using System.Linq;

namespace PadronApi.Dto
{
    public class PadConfiguracion
    {
        private static string titular;
        private static string rubricas;
        private static string leyendaOficio;
        private static string numOficio;
        private static string txtAclaraciones;
       
        public static string Titular
        {
            get
            {
                return titular;
            }
            set
            {
                titular = value;
            }
        }

        public static string Rubricas
        {
            get
            {
                return rubricas;
            }
            set
            {
                rubricas = value;
            }
        }

        public static string LeyendaOficio
        {
            get
            {
                return leyendaOficio;
            }
            set
            {
                leyendaOficio = value;
            }
        }

        public static string NumOficio
        {
            get
            {
                return numOficio;
            }
            set
            {
                numOficio = value;
            }
        }

        public static string TxtAclaraciones
        {
            get
            {
                return txtAclaraciones;
            }
            set
            {
                txtAclaraciones = value;
            }
        }
    }
}
