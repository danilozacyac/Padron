using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Kiosko.Dto;
using PadronApi.Dto;
using PadronApi.Model;

namespace Kiosko
{
    /// <summary>
    /// Interaction logic for TextoColaboracion.xaml
    /// </summary>
    public partial class TextoColaboracion
    {
        private List<AutorColabora> colaboradores;
        private Obra obra;
        private AutorColabora selectedAutor;

        public TextoColaboracion(Obra obra)
        {
            InitializeComponent();
            this.obra = obra;
        }

        private void RadWindow_Loaded(object sender, RoutedEventArgs e)
        {
            AutorColabora colabora = new AutorColabora();
            colaboradores = colabora.GetAutoresForColaboracion(obra);
            colaboradores.AddRange(colabora.GetInstitucionesForColaboracion(obra));

            GColaboran.DataContext = colaboradores;
        }

        private void GColaboran_CellEditEnded(object sender, Telerik.Windows.Controls.GridViewCellEditEndedEventArgs e)
        {
            if (!String.IsNullOrEmpty(selectedAutor.TextoColabora))
            {
                new AutorModel().UpdateTextoColabora(selectedAutor.IdColaboracion, selectedAutor.TextoColabora);
            }
        }

        private void GColaboran_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            selectedAutor = GColaboran.SelectedItem as AutorColabora;
        }
    }
}
