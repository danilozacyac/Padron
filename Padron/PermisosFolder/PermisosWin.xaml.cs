using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PadronApi.Dto;
using PadronApi.Model;

namespace Padron.PermisosFolder
{
    /// <summary>
    /// Interaction logic for PermisosWin.xaml
    /// </summary>
    public partial class PermisosWin
    {
        ObservableCollection<Permisos> permisos;
        List<int> permisosUsuario;

        Permisos selectedUsuario;

        public PermisosWin()
        {
            InitializeComponent();
        }

        private void RadWindow_Loaded(object sender, RoutedEventArgs e)
        {
            RLstUsuarios.DataContext = new PermisosModel().GetUsuarios();

            

        }


        private void CargaPermisosUsuario(ObservableCollection<Permisos> permisoCargar)
        {
            foreach (Permisos perm in permisoCargar)
            {
                if (permisosUsuario.Contains(perm.IdSeccion))
                    perm.IsSelected = true;
                else
                    perm.IsSelected = false;

                CargaPermisosUsuario(perm.SeccionesHijo);
            }
        }


        private List<int> permisosAsignados;
        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            permisosAsignados = new List<int>();

            ObtenPermisos(permisos);

            PermisosModel model = new PermisosModel();

            model.EliminaPermisos(selectedUsuario.IdSeccion);

            foreach (int asignado in permisosAsignados)
                model.InsertaPermisos(selectedUsuario.IdSeccion, asignado);
        }

        private void ObtenPermisos(ObservableCollection<Permisos> permisoRevisar)
        {
            foreach (Permisos perm in permisoRevisar)
            {
                if (perm.IsSelected)
                    permisosAsignados.Add(perm.IdSeccion);

                ObtenPermisos(perm.SeccionesHijo);
            }
        }

        private void RLstUsuarios_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedUsuario = RLstUsuarios.SelectedItem as Permisos;

            permisosUsuario = new PermisosModel().GetPermisosByUser(selectedUsuario.IdSeccion);
            permisos = new PermisosModel().GetPermisosTree(0);
            CargaPermisosUsuario(permisos);
            treePermisos.DataContext = permisos;
        }

       
    }
}
