   M S B A M L   `   `   `  ����E  AOrganismos, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;,clr-namespace:Organismos;assembly=Organismos
Organismos  5      E AUIControls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;,clr-namespace:UIControls;assembly=UIControls
UIControls 5      ;,clr-namespace:UIControls;assembly=UIControls
UIControls 5      D @PadronApi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullL4clr-namespace:PadronApi.Converter;assembly=PadronApiPadronApi.Converter 5      
 Padron   Padron.Cierre.Acuses   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      ECustomColumn,clr-namespace:Organismos;assembly=Organismos     5      C
UIControls,clr-namespace:UIControls;assembly=UIControls     5      DUIControls1,clr-namespace:UIControls;assembly=UIControls     5      J	converter4clr-namespace:PadronApi.Converter;assembly=PadronApi     5      m iTelerik.Windows.Controls.Docking, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7e	 aTelerik.Windows.Controls, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7p
 lTelerik.Windows.Controls.RibbonView, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7n jTelerik.Windows.Controls.GridView, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7a ]Telerik.Windows.Data, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k gTelerik.Windows.Controls.Input, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7p lTelerik.Windows.Controls.Navigation, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Ktelerik1http://schemas.telerik.com/2008/xaml/presentation  	 
     5	      -   5        $
��1000��5
      $	��300��5      $�� ��5      )  #Telerik.Windows.Controls.WindowBase    WindowState$  	Maximized=�5       � 	Resources 5      /  )PadronApi.Converter.DistribucionConverter   
distriConv��%   &	          5   
   6*   5      .��5      / 	 )Telerik.Windows.Controls.RadBusyIndicator  -   5          Name$ BusyIndicator��5         IsIndeterminate . .��5   
   � -   5         �Name$ Layout��5       � RowDefinitions 5      �� 5      $
��Auto �6    �� 5      $��50 �6    �� 5      $��* �6    .+�5      , 
 &Telerik.Windows.Controls.RadRibbonView  "   ApplicationButtonVisibility$ 	Collapsed=�6$      ApplicationName$$ Acuses de entrega de las obras��6L   .@�5      + 
 %Telerik.Windows.Controls.RadRibbonTab  .@�5      - 
 'Telerik.Windows.Controls.RadRibbonGroup  $��Paquetería��6-   .@�5      . 
 (Telerik.Windows.Controls.RadRibbonButton  -   5          Name$ BtnDatosEnvio��5      	   
LargeImage$/	 )/Padron;component/Resources/deliv_128.png��5       
   Size$
 Large=�5!      
 � Tag$ 520001��5"      .��5#      ��  �� Orientation$ Vertical=�6)   .+�5$   "   �� $��Center=�6,   $��Cargar��6I   �� 5%   "   $��Center=�6,   $��datos envio��6I   5&      5'        5(      -   5        $ BtnDatosEntrega��5)      $/	 )/Padron;component/Resources/acuse_128.png��5+      $
 Large=�5,      $ 520002��5-      .��5.      �� $ Vertical=�6)   .+�5/   "   �� $��Center=�6,   $��Cargar��6I   �� 50   "   $��Center=�6,   $��datos entrega��6I   51      52      �� 53      $ Vertical=�6%   .+�54        -   5        $ BtnDatosEnvioPart��55   !   $��Datos envio��57   !   $2	 ,/Padron;component/Resources/delivery_128.png��58   !   $
 Medium=�59   !   $ 520003��5:   !   5;        5<      -   5        $ BtnDescargaComprobante��5=      {�.  $=	 7/Padron;component/Resources/descargaComprobante_128.png��5@      $
 Large=�5A      $ 520008��5B      .��5C      �� $ Vertical=�6)   .+�5D   "   �� $��Center=�6,   $��	Descargar��6I   �� 5E   "   $��Center=�6,   $��comprobantes��6I   5F      5G        5H      -   5        $ BtnComprobantePaq��5I      $2	 ,/Padron;component/Resources/delivery_128.png��5K      $
 Large=�5L      $ 520006��5M      .��5N      �� $ Vertical=�6)   .+�5O   "   �� $��Center=�6,   $��	Ver acuse��6I   �� 5P   "   $��Center=�6,   $��paquetería��6I   5Q      5R      5S        5T      $��Acuses��6-   .@�5U        -	   5        $ BtnDetalleAcuse��5V      $/	 )/Padron;component/Resources/acuse_128.png��5X      $
 Large=�5Y      $ 520010��5Z      .��5[      �� $ Vertical=�6)   .+�5\   "   �� $��Center=�6,   $��Acuse��6I   �� 5]   "   $��Center=�6,   $��oficina��6I   5^      5_        5a      -
   5        $ BtnFechaRecOfic��5b      $2	 ,/Padron;component/Resources/multipleDocs.png��5d      $
 Large=�5e      $ 520011��5f      .��5g      �� $ Vertical=�6)   .+�5h   "   �� $��Center=�6,   $��	Multiples��6I   �� 5i   "   $��Center=�6,   $��acuses��6I   5j      5k        5l      -   5        $ BtnDfMetropol��5m      $0	 */Padron;component/Resources/dfMetroPol.png��5o      $
 Large=�5p      $ 520013��5q      .��5r      �� $ Vertical=�6)   .+�5s   "   �� $��Center=�6,   $��San Lázaro y��6I   �� 5t   "   $��Center=�6,   $��A. Metropolitana��6I   5u      5v        5x      -   5        $ BtnAcuseOficina��5y      $/	 )/Padron;component/Resources/acuse_128.png��5{      $
 Large=�5|      $ 520012��5}      .��5~      �� $ Vertical=�6)   .+�5   "   �� $��Center=�6,   $��	Ver acuse��6I   �� 5�   "   $��Center=�6,   $��oficina��6I   5�      5�      5�      5�      5�        UIControls.SearchTextBox  5�      -   5        $��1��5�      $	��350��5�      $��23��5�      $��	65,10,0,0q�5�      $
��Left=�5�      $	��Top=�5�         	LabelText$N HBuscar por titular, organismo, número de oficio ó número de guía... ��5�      5�      �� 5�      $��1��5�      $��	10,10,0,0q�5�      $
��Left=�5�      $	��Top=�5�      $��Buscar:��5�      $
��Bold"�5�      *	  $Telerik.Windows.Controls.RadGridView	  5�      -   5         	 Name$ 
GPlantilla��5�      $��2��5�      $��Stretch=�5�      $��Stretch=�5�      ;
  5Telerik.Windows.Controls.GridView.GridViewDataControl 
  AutoExpandGroups .  
  AutoGenerateColumns .   
  EnableRowVirtualization . $��14(�5�      8  2Telerik.Windows.Controls.GridView.BaseItemsControl   GroupRenderMode$
 Flat=�5�       
  IsFilteringAllowed .   
  
IsReadOnly .  
  SelectionMode$ Multiple=�5�       
  ShowGroupPanel .  *  $Telerik.Windows.Controls.DataControl   ItemsSource 5�      ��  
  Columns 5�      1  +Telerik.Windows.Controls.GridViewDataColumn  5�      6  0Telerik.Windows.Controls.GridViewBoundColumnBase   Header$ Distribución��5�      -  'Telerik.Windows.Controls.GridViewColumn   	IsVisible .     DataMemberBinding 5�      �� *TipoDistribucion+ �� 	Converter# [  5�        5�      -   5          Name$ ColObra��5�         Width?  9Telerik.Windows.Controls.GridView.GridViewLengthConverter$ * 5�      $
 Obra��5�       5�      �� *
TituloObra+5�        5�      -   5        $ 	ColNombre��5�      $ * 5�      $ Nombre��5�       5�      �� *Nombre+5�        5�      $	 120 5�      $ Num. Oficio��5�       5�      �� *Oficio+5�        5�      $	 120 5�         DataFormatString$  {0:dd/MM/yyyy}��5�      $ F. Envio��5�       5�      �� *
FechaEnvio+5�        5�      $	 120 5�      $ Núm. Guía��5�       5�      �� *	NumGuia+5�        5�      $	 120 5�      $  {0:dd/MM/yyyy}��5�      $ F. Paquetería��5�       5�      �� *FechaRecPaqueteria+5�        5�      $	 120 5�      $ Recibió��5�       5�      �� *QuienRecibe+5�        5�      $	 120 5�      $  {0:dd/MM/yyyy}��5�      $ F.Acuse��5�       5�      �� *FechaRecAcuse+5�      5�      �� 5�      $��1��5�      $��
558,10,0,0q�5�      $
��Left=�5�      $	��Top=�5�      $��Seleccionar año:��5�      $
��Bold"�5�      *  $Telerik.Windows.Controls.RadComboBox  5�      -   5           Name$  CbxAnio��5�      $��1��5�      $��94��5�      $��
660,11,0,0q�5�      $
��Left=�5�      $	��Top=�5�      ! �� SelectedIndex$! 0��5�      ��5�      �� 5�      5�      5�      5�      