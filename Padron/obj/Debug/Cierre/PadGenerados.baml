   M S B A M L   `   `   `  ����E  AOrganismos, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;,clr-namespace:Organismos;assembly=Organismos
Organismos  5      E AUIControls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;,clr-namespace:UIControls;assembly=UIControls
UIControls 5      
 Padron    Padron.Cierre.PadGenerados   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      ECustomColumn,clr-namespace:Organismos;assembly=Organismos     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      m iTelerik.Windows.Controls.Docking, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7e aTelerik.Windows.Controls, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7p	 lTelerik.Windows.Controls.RibbonView, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7n
 jTelerik.Windows.Controls.GridView, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7a ]Telerik.Windows.Data, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k gTelerik.Windows.Controls.Input, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7p lTelerik.Windows.Controls.Navigation, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Ktelerik1http://schemas.telerik.com/2008/xaml/presentation   	 
    5      I
UIControls,clr-namespace:UIControls;assembly=UIControls   	 
    5      -   5        
�������.��5      /  )Telerik.Windows.Controls.RadBusyIndicator  -   5           Name$  BusyIndicator��5         IsIndeterminate . .��5   
   �  � RowDefinitions 5      �� 5      $��50 �6    �� 5      $��* �6    .+�5      * 
 $Telerik.Windows.Controls.RadGridView  -   5          Name$ 	GPadrones��5   "   $��1��5      $��Stretch=�5      $��Stretch=�5      ; 
 5Telerik.Windows.Controls.GridView.GridViewDataControl   AutoGenerateColumns .     EnableRowVirtualization . $��14(�5         
IsReadOnly .    RowIndicatorVisibility$ 	Collapsed=�5          ShowGroupPanel .  *  $Telerik.Windows.Controls.DataControl	   ItemsSource	 5      �� 
   Columns
 5#         Organismos.MyColumnNumber  5$      - 
 'Telerik.Windows.Controls.GridViewColumn   Width? 
 9Telerik.Windows.Controls.GridView.GridViewLengthConverter$ 30 62      ColumnaNumerica   	CellStyle# [  5%         Header$ #��5&      1 
 +Telerik.Windows.Controls.GridViewDataColumn  5'      $ * 61   6	 
 0Telerik.Windows.Controls.GridViewBoundColumnBase 	  Header$
 Obra��5)         IsFilterable .   	  DataMemberBinding 5(      �� *
TituloObra+5*        5+      $	 100 61   $ Acuerdo��5-       5,      �� *
NumAcuerdo+5-        5.      $	 100 61   $
 Año��50       5/      �� *AnioAcuerdo+50        51      $	 100 61   $ Tiraje��53       52      �� *Tiraje+53        54      $	 120 61    	  DataFormatString$  {0:dd/MM/yyyy}��55      $ Generado��57         ShowDistinctFilters .   56      �� *FechaGenerado+58        59      $	 120 61   $  {0:dd/MM/yyyy}��5:      $ Distribuido��5<       .   5;      �� *FechaDistribucion+5=        5>      $	 100 61   $ Of. Inicial��5@       .   5?      �� *OficioInicial+5A        5B      $	 100 61   $ 	Of. Final��5D       .   5C      �� *OficioFinal+5E      5H      
  UIControls.SearchTextBox
  5J      -   5        $
��Left=�5J   '   $��	10,10,0,0q�6B    �� TextWrapping$
 Wrap=�6U    
  	LabelText$  Buscar padrón por obra...��6i   $	��Top=�6�   $	��300��6�   6�   5L      5M      5N      