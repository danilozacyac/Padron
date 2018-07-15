using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Kiosko.EstructuraKiosko.ViewModels;
using PadronApi.Dto;
using PadronApi.Model;
using Telerik.Windows.Controls;
using Telerik.Windows.DragDrop;

namespace Kiosko.EstructuraKiosko
{
    /// <summary>
    /// Interaction logic for ArbolKiosko.xaml
    /// </summary>
    public partial class ArbolKiosko : UserControl
    {
        private int tipoProceso = 1;


        public ArbolKiosko()
        {
            InitializeComponent();

            worker.DoWork += this.WorkerDoWork;
            worker.RunWorkerCompleted += WorkerRunWorkerCompleted;

            BusyIndicator.BusyContent = "Cargando estructura del quiosco...";
            LaunchBusyIndicator();
        }

        private void OnDrop(object sender, Telerik.Windows.DragDrop.DragEventArgs e)
        {
            Obra data = DragDropPayloadManager.GetDataFromObject(e.Data, "DraggedData") as Obra;
            if (data == null) return;
            if (e.Effects != DragDropEffects.None)
            {
                var destinationItem = (e.OriginalSource as FrameworkElement).ParentOfType<RadTreeViewItem>();

                Obra parentItem = null;

                if (dragItemDropPosition == DropPosition.Before )
                {
                    parentItem = (destinationItem.Item as Obra).ParentItem;
                    data.Padre = parentItem.IdObra;
                    data.Nivel = parentItem.Nivel + 1;
                    data.Orden = (destinationItem.Item as Obra).Orden - 1;
                }
                else if (dragItemDropPosition == DropPosition.After)
                {
                    parentItem = (destinationItem.Item as Obra).ParentItem;
                    data.Padre = parentItem.IdObra;
                    data.Nivel = parentItem.Nivel + 1;
                    data.Orden = (destinationItem.Item as Obra).Orden + 1;
                }
                else
                {
                    parentItem = destinationItem.Item as Obra;
                    data.Padre = parentItem.IdObra;
                    data.Nivel = parentItem.Nivel + 1;
                    data.Orden = parentItem.Orden + 1;
                }

                var dropDetails = DragDropPayloadManager.GetDataFromObject(e.Data, "DropDetails") as DropIndicationDetails;
                (dropDetails.CurrentDraggedItem as Obra).ParentItem = parentItem;

                //int idObra = (dropDetails.CurrentDraggedItem as Obra).IdObra;

                if (destinationItems != null)
                {
                    int dropIndex = dropDetails.DropIndex >= destinationItems.Count ? destinationItems.Count : dropDetails.DropIndex < 0 ? 0 : dropDetails.DropIndex;
                    this.destinationItems.Insert(dropIndex, data);
                    new ObraModel().UpdateObraPadre(data);
                }
            }
        }

        IList destinationItems = null;
        private void OnItemDragOver(object sender, Telerik.Windows.DragDrop.DragEventArgs e)
        {
            var item = (e.OriginalSource as FrameworkElement).ParentOfType<RadTreeViewItem>();
            if (item == null)
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
                return;
            }
            var position = GetPosition(item, e.GetPosition(item));
            if (item.Level == 0 && position != DropPosition.Inside)
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
                return;
            }
            RadTreeView tree = sender as RadTreeView;
            var draggedData = DragDropPayloadManager.GetDataFromObject(e.Data, "DraggedData");
            var dropDetails = DragDropPayloadManager.GetDataFromObject(e.Data, "DropDetails") as DropIndicationDetails;

            if ((draggedData == null && dropDetails == null))
            {
                return;
            }
            if (position != DropPosition.Inside)
            {
                e.Effects = DragDropEffects.All;

                destinationItems = item.Level > 0 ? (IList)item.ParentItem.ItemsSource : (IList)tree.ItemsSource;
                int index = destinationItems.IndexOf(item.Item);
                dropDetails.DropIndex = position == DropPosition.Before ? index : index + 1;
            }
            else
            {
                destinationItems = (IList)item.ItemsSource;

                if (destinationItems == null)
                {
                    e.Effects = DragDropEffects.None;
                }
                else
                {
                    e.Effects = DragDropEffects.All;
                    dropDetails.DropIndex = 0;
                }
            }

            dropDetails.CurrentDraggedOverItem = item.Item;
            dropDetails.CurrentDropPosition = position;

            e.Handled = true;
        }


        DropPosition dragItemDropPosition;
        private DropPosition GetPosition(RadTreeViewItem item, Point point)
        {
            const double TreeViewItemHeight = 24;
            if (point.Y < TreeViewItemHeight / 4)
            {
                dragItemDropPosition = DropPosition.Before;
                return DropPosition.Before;
            }
            else if (point.Y > TreeViewItemHeight * 3 / 4)
            {
                dragItemDropPosition = DropPosition.After;
                return DropPosition.After;
            }

            dragItemDropPosition = DropPosition.Inside;
            return DropPosition.Inside;
        }

        


        public void Reordenar()
        {
            tipoProceso = 3;
            LaunchBusyIndicator();
        }


        private void SearchTextBox_Search(object sender, RoutedEventArgs e)
        {
            tipoProceso = 2;

            String tempString = ((TextBox)sender).Text.ToUpper();

            if (String.IsNullOrEmpty(tempString) || String.IsNullOrWhiteSpace(tempString))
                allProductsView.ItemsSource = arbolObras;
            else
            {
                textoBuscado = tempString;
                LaunchBusyIndicator();
            }
        }




        private Obra selectedObra;
        private void AllProductsViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedObra = allProductsView.SelectedItem as Obra;
        }


        public void EliminaClasifObra()
        {
            if (selectedObra != null)
            {
                MessageBoxResult result = MessageBox.Show(String.Format("¿Estas seguro de eliminar la clasificación de la obra \"{0}\"?", selectedObra.Titulo), "Atención:", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {

                    selectedObra.Orden = 0;
                    selectedObra.Nivel = 0;
                    selectedObra.Padre = 0;

                    new ObraModel().UpdateObraPadre(selectedObra);

                    Obra parentItem = selectedObra.ParentItem;
                    parentItem.ObraChild.Remove(selectedObra);
                    obrasSinPadre.Add(selectedObra);
                }
                //allProductsView.Items.Remove(selectedObra);
            }
            else
            {
                MessageBox.Show("Primero debes de seleccionar la obra a la cual quieres eliminar la clasificación");
            }
        }

        public void EditaObra()
        {
            

            if (selectedObra == null)
            {
                MessageBox.Show("Primero debes de seleccionar la obra que deseas editar");
                return;
            }

            Obra obra = new ObraModel().GetObras(selectedObra.IdObra);

            ObrasKiosko obraWin = new ObrasKiosko(obra, true) { Owner = this };
            obraWin.ShowDialog();
        }

        private void WishlistViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedObra = wishlistView.SelectedItem as Obra;
        }


        #region Background Worker
        
        private BackgroundWorker worker = new BackgroundWorker();
        private void WorkerDoWork(object sender, DoWorkEventArgs e)
        {
            if (tipoProceso == 1)
            {
                arbolObras = new ObraModel().GetArbolKiosko();
                obrasSinPadre = new ObraModel().GetObrasSinPadre();
                

            }
            else if (tipoProceso == 2)
            {
                obrasBuscadas = new ObraModel().GetObrasBuscadas(textoBuscado);
            }
            else if (tipoProceso == 3)
            {
             //   new ObraModel().GetOrdenForUpdate();
                new ObraModel().UpdateOrden(arbolObras);
            }
        }

        ObservableCollection<Obra> arbolObras, obrasSinPadre, obrasBuscadas;
        string textoBuscado;

        void WorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (tipoProceso == 1)
                allProductsView.ItemsSource = arbolObras;
            else if (tipoProceso == 2)
                allProductsView.ItemsSource = obrasBuscadas;


            wishlistView.ItemsSource = obrasSinPadre;

            DragDropManager.AddDragOverHandler(allProductsView, OnItemDragOver);
            DragDropManager.AddDropHandler(allProductsView, OnDrop);

            this.BusyIndicator.IsBusy = false;

        }

        private void LaunchBusyIndicator()
        {
            if (!worker.IsBusy)
            {
                this.BusyIndicator.IsBusy = true;
                worker.RunWorkerAsync();

            }
        }

        #endregion

    }
}
