   M S B A M L   `   `   `  ����E  AOrganismos, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:Organismos
Organismos  5      E AUIControls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;,clr-namespace:UIControls;assembly=UIControls
UIControls 5      D @PadronApi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullL4clr-namespace:PadronApi.Converter;assembly=PadronApiPadronApi.Converter 5      A%clr-namespace:Organismos.OrgConverterOrganismos.OrgConverter  5
           Organismos.ListaOrganismos   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      1CustomColumnclr-namespace:Organismos     5      C
UIControls,clr-namespace:UIControls;assembly=UIControls     5      J	converter4clr-namespace:PadronApi.Converter;assembly=PadronApi     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5	      >orgConverter%clr-namespace:Organismos.OrgConverter     5
      a ]Telerik.Windows.Data, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k gTelerik.Windows.Controls.Input, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7e	 aTelerik.Windows.Controls, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7n
 jTelerik.Windows.Controls.GridView, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7p lTelerik.Windows.Controls.Navigation, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Gtelerik1http://schemas.telerik.com/2008/xaml/presentation   	 
  5      -   5        
�������  � 	Resources  5      0   *Organismos.OrgConverter.AdscritosConverter   adsConv+  %PadronApi.Converter.CiudadesConverter  
ciudadConv,  &PadronApi.Converter.OrdinalesConverter  ordinalConv��%W   &	        &	      &	 &       5   
   6*     5   
   6&     5   
   6'   5      .��5      / 	 )Telerik.Windows.Controls.RadBusyIndicator  -   5          Name$ BusyIndicator��5         IsIndeterminate . .��5   
   �  � RowDefinitions 5      �� 5      $��50 �6    �� 5      $��* �6    .+�5      �� $��	10,18,0,0q�5      $
��Left=�5      $	��Top=�5       $��Buscar:��5!      $
��Bold"�5"        UIControls.SearchTextBox  5#      -   5          Name$ TxtBusqueda��5$      $	��256��5%      $��25��5&      $��	85,18,0,0q�5'      $
��Left=�5(      $	��Top=�5)         	LabelText$ Buscar por organismo...��5*      5+      �� 5,      -   5         ��Name$ 
LblTotales��5-      $��	0,10,20,0q�5.      $��Right=�5/      $	��Top=�50      $	��   ��51      $��14(�52      $
��Bold"�53      * 
 $Telerik.Windows.Controls.RadGridView  54      -   5          Name$ GOrganismos��55      $��1��56      $��Stretch=�57      $��Stretch=�58      ; 
 5Telerik.Windows.Controls.GridView.GridViewDataControl   AutoGenerateColumns .  	   EnableRowVirtualization	 . $��14(�5<      
   
IsReadOnly
 .    RowIndicatorVisibility$ 	Collapsed=�5?         ShowGroupPanel .  *  $Telerik.Windows.Controls.DataControl   ItemsSource 5>      ��    Columns 5B      	   Organismos.MyColumnNumber	  5C      -
 
 'Telerik.Windows.Controls.GridViewColumn 
  Width? 
 9Telerik.Windows.Controls.GridView.GridViewLengthConverter$ 30 5D        ColumnaNumerica 
  	CellStyle# [ 5E       
  Header$ #��5F      1 
 +Telerik.Windows.Controls.GridViewDataColumn  5G      $ * 5H      6 
 0Telerik.Windows.Controls.GridViewBoundColumnBase   Header$ 	Organismo��5J       
  IsFilterable .     DataMemberBinding 5I      �� *OrganismoDesc+5K        5L      $	 120 5M      $ 	Tipo Org.��5O       
  ShowFieldFilters .   5N      �� *TipoOrganismoStr+5P        5Q      $	 120 5R      $ Circuito��5T       .   5S      �� *
Circuito+ �� 	Converter# [ 5U        5V      $	 120 5W      $ Ordinal��5Y       .   5X      �� *	Ordinal+# [ 5Z        5[      $	 150 5\      $ Ciudad��5^       .   5]      �� *Ciudad+# [ 5_        5`      $	 120 5a      $ Distribución��5c       .   5b      �� *Distribucion+5d         RowStyle 5f      �� 5g       �� 
TargetType3 
 -Telerik.Windows.Controls.GridView.GridViewRow"  6   .�5h      ��  �� Property � �� �� Value 67   �� *TotalAdscritos+# [  5i      5k      5l      5m      5n      