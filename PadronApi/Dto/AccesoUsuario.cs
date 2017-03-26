using System;
using System.Collections.Generic;
using System.Linq;

namespace PadronApi.Dto
{
    public class AccesoUsuario
    {
        private static string nombre;

        public static string Nombre
        {
            get { return nombre; }
            set
            {
                nombre = value;
            }
        }

        private static string usuario;
        public static string Usuario
        {
            get { return usuario; }
            set
            {
                usuario = value;
            }
        }

        private static string pwd;
        public static string Pwd
        {
            get { return pwd; }
            set
            {
                pwd = value;
            }
        }

        private static int llave;
        public static int Llave
        {
            get { return llave; }
            set
            {
                llave = value;
            }
        }

        private static int grupo;
        public static int Grupo
        {
            get { return grupo; }
            set
            {
                grupo = value;
            }
        }

        private static List<int> permisos;

        public static List<int> Permisos
        {
            get
            {
                return permisos;
            }
            set
            {
                permisos = value;
            }
        }

        
    }
}
