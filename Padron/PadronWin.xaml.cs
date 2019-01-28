using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Media.Imaging;
using Funcionarios;
using Kiosko;
using Kiosko.Autores;
using Kiosko.Ayudas;
using Kiosko.EstructuraKiosko;
using Obras.Padron;
using Organismos;
using Organismos.Ciudades;
using Padron.Cierre;
using Padron.Historial;
using Padron.ManttoCatalogos;
using Padron.PermisosFolder;
using Padron.Plantillas;
using PadronApi.Dto;
using PadronApi.Model;
using Telerik.Windows.Controls;
using Padron.Reportes;

namespace Padron
{
    /// <summary>
    /// Interaction logic for PadronWin.xaml
    /// </summary>
    public partial class PadronWin 
    {
        CatalogoObrasPadron obraControl;
        ListaFuncionarios funcionariosControl;
        ListaOrganismos organismosControl;
        List<RadRibbonTab> pestanasBarra;
        Plantilla plantillacontrol;
        PadGenerados padronesGenerados;
        Mantenimiento mantoLocalidades;
        TitulosControl mantoTitulos;
        ListaPublicaciones listadoKiosko;
        ArbolKiosko arbolKiosko;
        DistPorTipo distPorTipo;


        ListadoAutores listadoAutores;
        List<RadRibbonButton> listadoBotones;
        List<RadRibbonDropDownButton> listadoOpciones;

        int codigoColores = 0;

        public PadronWin()
        {
            InitializeComponent();
            this.ShowInTaskbar(this, "Padrón de distribución");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            listadoBotones = new List<RadRibbonButton>()
            {
                BtnCatObras, BtnCatOrganismos, BtnCatTitulares, BtnPlantillas, BtnVerPadrones, BtnCreaPlantilla,BtnLocalidadesMant, BtnFunciones, BtnPermisos,
                VerObra, AgregarObra, EditarObra, EliminarObra, ActivarObra, ObrasDesctivadas, ObrasActivas, InfoTitular, AgregaTitular, ModificaTitular,
                BtnSinDistribucion, BtnTitularesActivos, BtnTitularesJubilados, HistorialTitular, HistorialFuncObras, BtnIncluidosTodo, InfoOrganismo,
                AgregarOrganismo, ModificaOrganismo, BtnActivarOrganismo, DesactivaOrganismo, BtnOrgActivados, BtnOrgDesactivados, BtnLimpiaFiltro, HistorialOrganismo,
                BtnHistOrgObras, BtnTotalSecretarios, IncluyeTitular,ExcluyeTitular, GuardarPadron, BtnGeneraAcuses, BtnGeneraEtiquetas, BtnListadoContraloria,
                BtnVerDistribucion, DetalleAcuses, BtnClonarPlantilla, BtnPadronPorObra, BtnSinNumAcuerdo, BtnKioskoObras, BtnKioskoAutores,BtnVerInfoObraKiosko, BtnEditObraKiosko,
                BtnAddImagen, BtnEstructura, BtnVerAutor,BtnAddAutor, RBtnCadenaControl,BtnAddFunciones, BtnCatTitulos,BtnAddTitulo,BtnExportaKiosko, BtnDeletePadron, BtnTitularEsAutor
            };

            listadoOpciones = new List<RadRibbonDropDownButton>()
            {
                BtnExportObras, BtnImprDetalleOrg, BtnGeneraEtiquetasSelection, BtnListadosEntrega
            };

            pestanasBarra = new List<RadRibbonTab>()
            {
                TabObras,
                TabPadron,
                TabTitulares,
                TabOrganismos,
                TabKiosko,
                TabReportes,
                TabMantenimiento
            };
            this.DeshabilitaPestana();
            this.AplicaPermisos();

            var version = Assembly.GetExecutingAssembly().GetName().Version;
            LblVersion.Content = String.Format("Versión {0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);
        }

        public void ShowInTaskbar(RadWindow control, string title)
        {
            control.Show();
            var window = control.ParentOfType<Window>();
            window.ShowInTaskbar = true;
            window.Title = title;
            var uri = new Uri("pack://application:,,,/Padron;component/Resources/rocket.ico");
            window.Icon = BitmapFrame.Create(uri);
        }

       
        private void RemovePrevControls()
        {
            if (CentralPanel.Children.Count > 0)
                CentralPanel.Children.RemoveAt(0);
        }

        

        private void DeshabilitaPestana()
        {
            foreach (RadRibbonTab tab in pestanasBarra)
            {
                tab.Visibility = Visibility.Collapsed;
            }
        }

        private void AplicaPermisos()
        {
            foreach (RadRibbonButton boton in listadoBotones)
            {
                int tag = Convert.ToInt32(boton.Tag);

                if (!AccesoUsuario.Permisos.Contains(tag))
                    boton.IsEnabled = false;
            }

            foreach (RadRibbonDropDownButton drop in listadoOpciones)
            {
                int tag = Convert.ToInt32(drop.Tag);

                if (!AccesoUsuario.Permisos.Contains(tag))
                    drop.IsEnabled = false;
            }
        }

       

        #region Obras

        private void AgregarObra_Click(object sender, RoutedEventArgs e)
        {
            obraControl.Agregar();
        }

        private void VerObra_Click(object sender, RoutedEventArgs e)
        {
            obraControl.VerInformacion();
        }

        private void EditarObra_Click(object sender, RoutedEventArgs e)
        {
            obraControl.Modificar();
        }

        /// <summary>
        /// Cambia el estatus de una obra activa por el de inactiva
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EliminarObra_Click(object sender, RoutedEventArgs e)
        {
            obraControl.CambiaEstadoObra(0);
        }

        /// <summary>
        /// Cambia el estatus de una obra desactivada por el de activa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ActivarObra_Click(object sender, RoutedEventArgs e)
        {
            obraControl.CambiaEstadoObra(1);
        }

        /// <summary>
        /// Muestra todas las obras que han sido desactivadas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ObrasDesctivadas_Click(object sender, RoutedEventArgs e)
        {
            obraControl.EstadoObras = false;
            ObrasDesctivadas.Visibility = Visibility.Collapsed;
            EliminarObra.Visibility = Visibility.Collapsed;
            ActivarObra.Visibility = Visibility.Visible;
            ObrasActivas.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Muestras las obras activas que forman parte del padrón de distribución
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ObrasActivas_Click(object sender, RoutedEventArgs e)
        {
            obraControl.EstadoObras = true;
            ObrasDesctivadas.Visibility = Visibility.Visible;
            EliminarObra.Visibility = Visibility.Visible;
            ActivarObra.Visibility = Visibility.Collapsed;
            ObrasActivas.Visibility = Visibility.Collapsed;
        }

       

        private void ObraTotExcel_Click(object sender, RoutedEventArgs e)
        {
            obraControl.PrintReporteGeneral(2);
        }

        private void ObraTotPdfClick(object sender, RoutedEventArgs e)
        {
            obraControl.PrintReporteGeneral(3);
        }

        #endregion

       

        #region Organismos

        private void BtnOrgActivados_Click(object sender, RoutedEventArgs e)
        {
            organismosControl.VerActivos();
            BtnOrgDesactivados.Visibility = Visibility.Visible;
            DesactivaOrganismo.Visibility = Visibility.Visible;
            BtnActivarOrganismo.Visibility = Visibility.Collapsed;
            BtnOrgActivados.Visibility = Visibility.Collapsed;
            ModificaOrganismo.IsEnabled = true;
            AgregarOrganismo.IsEnabled = true;
        }

        private void BtnOrgDesactivados_Click(object sender, RoutedEventArgs e)
        {
            organismosControl.VerInactivos();
            BtnOrgDesactivados.Visibility = Visibility.Collapsed;
            DesactivaOrganismo.Visibility = Visibility.Collapsed;
            BtnActivarOrganismo.Visibility = Visibility.Visible;
            BtnOrgActivados.Visibility = Visibility.Visible;
            ModificaOrganismo.IsEnabled = false;
            AgregarOrganismo.IsEnabled = false;
        }

        #endregion



        private void MenuPrincipal(object sender, RoutedEventArgs e)
        {
            
            RadRibbonButton item = sender as RadRibbonButton;
            int switchy = 0;

            if (item == null)
            {
                RadMenuItem menuItem = sender as RadMenuItem;
                switchy = Convert.ToInt32(menuItem.Tag);
            }
            else
                switchy = Convert.ToInt32(item.Tag);


            switch (switchy)
            {
                case 1:
                    CleanCentralPanel();
                    if (obraControl == null)
                        obraControl = new CatalogoObrasPadron();

                    CentralPanel.Children.Add(obraControl);
                    TabObras.Visibility = Visibility.Visible;
                    BarraPrincipal.SelectedItem = TabObras;
                    break;

                case 2: CleanCentralPanel();
                    
                    TabKiosko.Visibility = Visibility.Visible;
                    BarraPrincipal.SelectedItem = TabKiosko;
                    break;

                case 3:
                    CleanCentralPanel();
                    if (funcionariosControl == null)
                        funcionariosControl = new ListaFuncionarios();

                    CentralPanel.Children.Add(funcionariosControl);
                    TabTitulares.Visibility = Visibility.Visible;
                    BarraPrincipal.SelectedItem = TabTitulares;
                    break;
                case 4:
                    CleanCentralPanel();
                    if (organismosControl == null)
                        organismosControl = new ListaOrganismos();

                    CentralPanel.Children.Add(organismosControl);
                    TabOrganismos.Visibility = Visibility.Visible;
                    BarraPrincipal.SelectedItem = TabOrganismos;
                    break;

                case 5:
                    CleanCentralPanel();
                    SeleccionaTiraje nuevoPadron = new SeleccionaTiraje() { Owner = this };
                    nuevoPadron.ShowDialog();

                    if (nuevoPadron.DialogResult == true)
                    {
                        TabPadron.Visibility = Visibility.Visible;
                        BarraPrincipal.SelectedItem = TabPadron;

                        plantillacontrol = new Plantilla(nuevoPadron.SelectedObra, nuevoPadron.Tiraje);
                        CentralPanel.Children.Add(plantillacontrol);
                    }

                    GpoPrepara.Visibility = Visibility.Visible;
                    GpoPapeleria.Visibility = Visibility.Collapsed;
                    GpoRespuesta.Visibility = Visibility.Collapsed;
                    break;

                case 6: break;
                case 7:
                    break;
                case 8:
                    CleanCentralPanel();
                    if (padronesGenerados == null)
                        padronesGenerados = new PadGenerados();

                    CentralPanel.Children.Add(padronesGenerados);
                    TabPadron.Visibility = Visibility.Visible;
                    BarraPrincipal.SelectedItem = TabPadron;

                    GpoPrepara.Visibility = Visibility.Collapsed;
                    GpoPapeleria.Visibility = Visibility.Visible;
                    GpoRespuesta.Visibility = Visibility.Visible;
                    break;
                case 9:
                    break;

                case 201: CleanCentralPanel();
                    if (listadoKiosko == null)
                        listadoKiosko = new ListaPublicaciones();
                    CentralPanel.Children.Add(listadoKiosko);
                    GpObrasKiosko.Visibility = Visibility.Visible;
                    GpAutorKiosko.Visibility = Visibility.Collapsed;
                    GpExportar.Visibility = Visibility.Visible;
                    GpEstructura.Visibility = Visibility.Collapsed;
                    BtnPrintMulSelectK.Visibility = Visibility.Collapsed;
                    BtnPrintOnlySelectK.Visibility = Visibility.Collapsed;
                    BtnPrintSinSintesis.Visibility = Visibility.Collapsed;
                    BtnPrintADisposicion.Visibility = Visibility.Visible;
                    codigoColores = 1;
                    TabKiosko.Visibility = Visibility.Visible;
                    BarraPrincipal.SelectedItem = TabKiosko;
                    break;
                case 202:
                    CleanCentralPanel();
                    if (listadoAutores == null)
                        listadoAutores = new ListadoAutores();
                    GpObrasKiosko.Visibility = Visibility.Collapsed;
                    GpAutorKiosko.Visibility = Visibility.Visible;
                    GpExportar.Visibility = Visibility.Visible;
                    GpEstructura.Visibility = Visibility.Collapsed;
                    CentralPanel.Children.Add(listadoAutores);
                    BtnPrintMulSelectK.Visibility = Visibility.Visible;
                    BtnPrintOnlySelectK.Visibility = Visibility.Collapsed;
                    BtnPrintSinSintesis.Visibility = Visibility.Visible;
                    BtnPrintADisposicion.Visibility = Visibility.Collapsed;
                    codigoColores = 2;
                    TabKiosko.Visibility = Visibility.Visible;
                    BarraPrincipal.SelectedItem = TabKiosko;
                    break;
                case 208:
                    if (codigoColores == 0)
                        return;
                    else if (codigoColores == 1)
                        new ColorObras().ShowDialog();
                    else
                        new ColorAutor().ShowDialog();
                    break;
                case 210: listadoKiosko.VerInformacion();
                    break;
                case 211: listadoKiosko.EditarInformacion();
                    break;
                case 212:
                    CleanCentralPanel();
                    if (arbolKiosko == null)
                        arbolKiosko = new ArbolKiosko();
                    CentralPanel.Children.Add(arbolKiosko);
                    GpEstructura.Visibility = Visibility.Visible;
                    
                    break;
                case 213: listadoKiosko.LimpiarSeleccion();
                    break;
                case 214: listadoKiosko.AgregarImagen();
                    break;
                case 231: listadoAutores.Agregar();
                    break;
                case 240:
                        listadoAutores.ImprimeInformacion(false);
                        listadoAutores.SeleccionSencilla();
                    break;
                case 241: if (codigoColores == 1)
                    {
                        listadoKiosko.ImprimeInformacionObra();
                    }
                    else
                    {
                        listadoAutores.ImprimeInformacion(true);
                        listadoAutores.SeleccionSencilla();
                    }
                    break;
                case 242: listadoAutores.SeleccionMultiple();
                    BtnPrintMulSelectK.Visibility = Visibility.Collapsed;
                    BtnPrintOnlySelectK.Visibility = Visibility.Visible;
                    break;
                case 243: listadoAutores.SeleccionSencilla();
                    BtnPrintMulSelectK.Visibility = Visibility.Visible;
                    BtnPrintOnlySelectK.Visibility = Visibility.Collapsed;
                    break;
                case 244: listadoKiosko.ImprimeObrasADisposicion();
                    break;
                case 301: funcionariosControl.VerInformacion();
                    break;

                case 302: funcionariosControl.Agregar();
                    break;

                case 303: funcionariosControl.Modificar();
                    break;

                case 304: funcionariosControl.NoDistribucion();
                    break;

                case 305: BtnTitularesJubilados.Visibility = Visibility.Visible;
                    BtnTitularesActivos.Visibility = Visibility.Collapsed;
                    funcionariosControl.MuestraTitularesActivos();
                    break;

                case 306: BtnTitularesJubilados.Visibility = Visibility.Collapsed;
                    BtnTitularesActivos.Visibility = Visibility.Visible;
                    funcionariosControl.MuestraTitularesJubilados();
                    break;

                case 310: funcionariosControl.VerTrayectoria();
                    break;

                case 311: Titular titular = funcionariosControl.VerHistorialObras();

                    if (titular != null)
                    {
                        HistorialFuncionarioObras historial = new HistorialFuncionarioObras(titular) { Owner = this };
                        historial.ShowDialog();
                    }
                    break;

                case 320: funcionariosControl.ReporteIncluidosTodos();
                    break;

                case 350: ManageAutor manage = new ManageAutor(null) { Owner = this };
                    manage.ShowDialog();
                    break;

                case 401: organismosControl.VerInformacion();
                    break;

                case 402: organismosControl.Agregar();
                    break;

                case 403: organismosControl.Modificar();
                    break;

                case 404: organismosControl.Activar();
                    break;

                case 405: organismosControl.Desactivar();
                    break;



                case 410: organismosControl.LimpiaFiltros();
                    break;

                case 441: organismosControl.DetalleSeleccion();
                    break;

                case 442: organismosControl.DetalleLista();
                    break;

                case 443: organismosControl.DetalleTodo();
                    break;

                case 445: organismosControl.TotalSecretarios();
                    break;
                case 446: organismosControl.EtiquetaSeleccion();
                    break;
                case 447: organismosControl.EtiquetaListaGrid();
                    break;
                case 448: organismosControl.TotalOrganismos();
                    break;
                case 449: organismosControl.TotalTitulares();
                    break;
                case 501: plantillacontrol.IncluyeTitular();
                    break;

                case 502: plantillacontrol.Excluirtitular();
                    break;

                case 503: 
                    
                    if(plantillacontrol.Guardar())
                        BtnVerPadrones.RaiseEvent(new RoutedEventArgs(RadRibbonButton.ClickEvent));
                    break;

                case 511: padronesGenerados.ImprimeAcusesFirma(1);
                    break;
                case 512: padronesGenerados.ImprimeListado(2);
                    break;
                case 513: padronesGenerados.ImprimeListado(3);
                    break;
                case 514: padronesGenerados.ImprimeListado(4);
                    break;
                case 515: padronesGenerados.ImprimeOficios();
                    break;
                case 516: padronesGenerados.GeneraEtiquetas();
                    break;
                case 517: padronesGenerados.ImprimeContraloria();
                    break;
                case 518: padronesGenerados.VerificaDistribucion();
                    break;
                case 519: padronesGenerados.SetAcuerdoZero();
                    break;
                case 520: padronesGenerados.VerAcuses();
                    break;

                case 521:

                    CleanCentralPanel();
                    PadronGenerado padronAClonar = padronesGenerados.ClonarPadron();

                    SeleccionaTiraje nuevoPadronClonar = new SeleccionaTiraje(padronAClonar) { Owner = this};
                    nuevoPadronClonar.ShowDialog();

                    if (nuevoPadronClonar.DialogResult == true)
                    {
                        TabPadron.Visibility = Visibility.Visible;
                        BarraPrincipal.SelectedItem = TabPadron;

                        plantillacontrol = new Plantilla(nuevoPadronClonar.SelectedObra, nuevoPadronClonar.Tiraje, padronAClonar, false);
                        CentralPanel.Children.Add(plantillacontrol);
                    }


                    break;

                case 522: padronesGenerados.VerAcusesPorObra();
                    break;

                case 523: padronesGenerados.GetCadenaControl();
                    break;

                
                  
                
                case 802:
                    GeneraNuevaPlantilla nuevaPlantilla = new GeneraNuevaPlantilla() { Owner = this };
                    nuevaPlantilla.ShowDialog();
                    break;

                case 999: PermisosWin permisos = new PermisosWin() { Owner = this };
                    permisos.ShowDialog();
                    break;
            }
        }


        private void CleanCentralPanel()
        {
            this.DeshabilitaPestana();
            this.RemovePrevControls();
        }

        

        private void BtnHistOrgObras_Click(object sender, RoutedEventArgs e)
        {
            Organismo organismo = organismosControl.VerHistorial();

            if (organismo != null)
            {
                HistorialOrganismoObra historial = new HistorialOrganismoObra(organismo) { Owner = this };
                historial.ShowDialog();

            }
        }

        private void ObraTotWord_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            obraControl.PrintReporteGeneral(1);
        }

        




        #region Mantenimiento


        private void BtnManteniento_Click(object sender, RoutedEventArgs e)
        {
            CleanCentralPanel();
            TabMantenimiento.Visibility = Visibility.Visible;
            BarraPrincipal.SelectedItem = TabMantenimiento;
        }


        #region Localidades

        private void BtnLocalidadesMant_Click(object sender, RoutedEventArgs e)
        {
            CleanCentralPanel();
            if (mantoLocalidades == null)
                mantoLocalidades = new Mantenimiento();

            CentralPanel.Children.Add(mantoLocalidades);
            TabMantenimiento.Visibility = Visibility.Visible;
            BarraPrincipal.SelectedItem = TabMantenimiento;

        }

        private void BtnAgregaPais_Click(object sender, RoutedEventArgs e)
        {
            mantoLocalidades.AgregaPaisEstado(1);
        }

        private void BtnEditaPais_Click(object sender, RoutedEventArgs e)
        {
            mantoLocalidades.ModificaPaisEstado(1);
        }

        private void BtnEliminaPais_Click(object sender, RoutedEventArgs e)
        {
            mantoLocalidades.EliminaPaisEstado(1);
        }

        private void BtnAgregaEstado_Click(object sender, RoutedEventArgs e)
        {
            mantoLocalidades.AgregaPaisEstado(2);
        }

        private void BtnEditaEstado_Click(object sender, RoutedEventArgs e)
        {
            mantoLocalidades.ModificaPaisEstado(2);
        }

        private void BtnEliminaEstado_Click(object sender, RoutedEventArgs e)
        {
            mantoLocalidades.EliminaPaisEstado(2);
        }

        private void BtnAgregaCiudad_Click(object sender, RoutedEventArgs e)
        {
            mantoLocalidades.AgregaCiudad();
        }

        private void BtnEditaCiudad_Click(object sender, RoutedEventArgs e)
        {
            mantoLocalidades.ModificaCiudad();
        }

        private void BtnEliminaCiudad_Click(object sender, RoutedEventArgs e)
        {
            mantoLocalidades.EliminaCiudad();
        }

        #endregion


        #region Funciones

        private void BtnFunciones_Click(object sender, RoutedEventArgs e)
        {
            Funciones funcionWin = new Funciones() { Owner = this };
            funcionWin.ShowDialog();
        }

        private void BtnAddFunciones_Click(object sender, RoutedEventArgs e)
        {
            DialogParameters dialog = new DialogParameters() { Header = "Agregar función", Content = "Ingresa función que se agregará", Closed = this.OnNewFunctionClosed };

            RadWindow.Prompt(dialog);
        }

        private void OnNewFunctionClosed(object sender, WindowClosedEventArgs e)
        {
            RadWindow win = sender as RadWindow;

            if (win.DialogResult == true && win.PromptResult == null)
            {
                MessageBox.Show("Ingresa la función que deseas agregar o presiona Cancelar");

            }
            else if (win.DialogResult == true)
            {
                if (!new ElementalPropertiesModel().SetFuncion(win.PromptResult))
                {
                    MessageBox.Show("No se pudo completar la operación, favor de volver a intentarlo");
                }
            }
        }

        #endregion

        #region Titulos

        private void BtnCatTitulos_Click(object sender, RoutedEventArgs e)
        {
            CleanCentralPanel();

            if (mantoTitulos == null)
                mantoTitulos = new TitulosControl();

            CentralPanel.Children.Add(mantoTitulos);
        }

        #endregion

        private void BtnExportaKiosko_Click(object sender, RoutedEventArgs e)
        {
            ExportarKiosko exporta = new ExportarKiosko() { Owner = this };
            exporta.ShowDialog();
        }


        #endregion

        private void BtnObraEspecial_Click(object sender, RoutedEventArgs e)
        {
            plantillacontrol.ObrasEspeciales();
        }

        private void BtnDeletePadron_Click(object sender, RoutedEventArgs e)
        {
            padronesGenerados.EliminaPadron();
        }

        private void BtnTitularEsAutor_Click(object sender, RoutedEventArgs e)
        {
            funcionariosControl.MarcarComoAutor();
        }

        private void BtnDelClasifObra_Click(object sender, RoutedEventArgs e)
        {
            arbolKiosko.EliminaClasifObra();
        }

        private void BtnReasOrden_Click(object sender, RoutedEventArgs e)
        {
            arbolKiosko.Reordenar();
        }

        private void BtnEditObraEstructura_Click(object sender, RoutedEventArgs e)
        {
            arbolKiosko.EditaObra();
        }

        private void BtnDistNacional_Click(object sender, RoutedEventArgs e)
        {
            new DistPorTipo().ShowDialog();
        }



    }
}