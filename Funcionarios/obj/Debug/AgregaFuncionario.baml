   M S B A M L   `   `   `  ����D  @PadronApi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullL4clr-namespace:PadronApi.Converter;assembly=PadronApiPadronApi.Converter  5       Funcionarios$   Funcionarios.AgregaFuncionario   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      H DMahApps.Metro, Version=1.2.3.0, Culture=neutral, PublicKeyToken=null9Custom,http://metro.mahapps.com/winfx/xaml/controls  5      D	converter4clr-namespace:PadronApi.Converter;assembly=PadronApi  5      k gTelerik.Windows.Controls.Input, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7a ]Telerik.Windows.Data, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7e	 aTelerik.Windows.Controls, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7n
 jTelerik.Windows.Controls.GridView, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7p lTelerik.Windows.Controls.Navigation, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Gtelerik1http://schemas.telerik.com/2008/xaml/presentation   	 
  5      -   5        $	��800��5      $	��710��5	      $��Agregar titular��5
      )  #Telerik.Windows.Controls.WindowBase    
ResizeMode$  NoResize=�5      (  "Telerik.Windows.Controls.RadWindow   WindowStartupLocation$ CenterScreen=�5       � 	Resources 5      *   $PadronApi.Converter.TextValConverter   textConv*   $PadronApi.Converter.FuncionConverter  funcConv��%:   &	        &	        5   
   6%     5   
   6%   5      .��5      �  � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $��50 �6   .+�5   
   � -   5         �Name$ PanelPrincipal��5      .+�5      �� $	��270��5      $��
15,31,15,0q�5      $��Stretch=�5      $	��Top=�5      $��Datos Personales��5      .��5      � .+�5      �� $��
16,107,0,0q�5       $
��Left=�5!      $	��Top=�5"      $��
Apellidos:��5#      �� 5$      -   5         ��Name$ TxtApellidos��5%      $	��437��5&      $��23��5'      $��103,107,-2,0q�5(      $
��Left=�5)      $	��Top=�5*       �� TextWrapping$
 Wrap=�5-      ��5,      �� *	Apellidos+ �� 	Converter# [  5-      �� 5.      $��
16,159,0,0q�5/      $
��Left=�50      $	��Top=�51      $��
Nombre(s):��52      �� 53      -   5        $ 	TxtNombre��54      $	��437��55      $��23��56      $��103,159,-2,0q�57      $
��Left=�58      $	��Top=�59      $
 Wrap=�5<      ��5;      �� *Nombre+# [  5<      *  $Telerik.Windows.Controls.RadComboBox  5=      -   5          Name$ CbxGrado��5>      $	��120��5?      $��
103,68,0,0q�5@      $
��Left=�5A      $	��Top=�5B      	 �� DisplayMemberPath$	 
TituloDesc��5C      
 �� SelectedValuePath$
 IdTitulo��5E      ��5D      �� 5E      �� 5F      $��	35,64,0,0q�5G      $
��Left=�5H      $	��Top=�5I      $��Título:��5J      �� 5K      -   5         ��Name$ 
LblEstatus��5L      $��365,202,0,0q�5M      $
��Left=�5N      $	��Top=�5O      $��Estatus:��5P       J� RenderTransformOrigin$ 0.48,0.5&�5Q      $y�Visible=�5R        5S      -   5        $ 	CbxEstado��5T      $	��120��5U      $��420,203,-2,0q�5V      $
��Left=�5W      $	��Top=�5X      $	 Descripcion��5Y      $
 
IdElemento��5[      $y�Visible=�5\      ��5Z      �� 5\      �� 5]      $��
17,202,0,0q�5^      $
��Left=�5_      $	��Top=�5`      $��Correo:��5a      �� 5b      -   5        $ 	TxtCorreo��5c      $	��235��5d      $��23��5e      $��103,202,0,0q�5f      $
��Left=�5g      $	��Top=�5h      $
 Wrap=�5j      ��5i      �� *Correo+ �� Mode$ TwoWay=�5j      )  #MahApps.Metro.Controls.ToggleSwitch  5k      -	   5          Name$ TbGenero��5l      $��	35,10,0,0q�5m      $
��Left=�5n      $	��Top=�5o      $��Genero       ��5q      $��12(�5r         	IsChecked$
 TrueY�5s      5t      �� 5u      -
   5         ��Name$ ImGenero��5v      $��32��5w      $��35��5x      $��
181,10,0,0q�5y      $
��Left=�5z      $	��Top=�5{      $��Resources/female_128.png��5|      5}      5~      �� 5�      -   5         ��Name$ BtnAgregaAdscripcion��5�      $��38��5�      $��28��5�      $��
0,356,10,0q�5�      $��Right=�5�      $	��Top=�5�      .��5�      �� $��Resources/addsign.png��6   5�      �� 5�      -   5        $ BtnEliminaAdscripcion��5�      $��38��5�      $��28��5�      $��
0,389,10,0q�5�      $��Right=�5�      $	��Top=�5�      .��5�      �� $��Resources/minus_48.png��6   5�      �� 5�      -   5        $ BtnModificaAdscripcion��5�      $��38��5�      $��28��5�      $��
0,425,10,0q�5�      $��Right=�5�      $	��Top=�5�      .��5�      �� $��Resources/pencil_24.png��6   5�      �� 5�      $��
15,488,0,0q�5�      $
��Left=�5�      $	��Top=�5�      $��Observaciones:��5�      �� 5�      -   5        $ TxtObservaciones��5�      $��90��5�      $��15,518,75,0q�5�      $��Stretch=�5�      $	��Top=�5�       �� AcceptsReturn . $
 Wrap=�5�      ��5�      �� *Observaciones+5�      5�      * 
 $Telerik.Windows.Controls.RadGridView  5�   
   -   5          Name$ GOrganismos��5�      $	��150��5�      $��20,330,75,0q�5�      $��Stretch=�5�      $	��Top=�5�      ; 
 5Telerik.Windows.Controls.GridView.GridViewDataControl   AutoGenerateColumns .     IsFilteringAllowed .     
IsReadOnly .    ShowGroupPanel .  *	  $Telerik.Windows.Controls.DataControl 	  ItemsSource 5�      ��    Columns 5�      1
 
 +Telerik.Windows.Controls.GridViewDataColumn
  5�      - 
 'Telerik.Windows.Controls.GridViewColumn   Width? 
 9Telerik.Windows.Controls.GridView.GridViewLengthConverter$ * 5�      6 
 0Telerik.Windows.Controls.GridViewBoundColumnBase   Header$ 	Organismo��5�         DataMemberBinding 5�      �� *Organismo.OrganismoDesc+5�      
  5�      $	 .3* 5�      $ Función��5�       5�      �� *	Funcion+# [ 5�      5�      �� 5�   
   -   5        $ BtnSalir��5�      $��1��5�      $��80��5�      $��30��5�      $��	0,10,20,0q�5�      $��Right=�5�      $	��Top=�5�      $��Cancelar��5�        
BlueButton#��[ 5�      �� 5�   
   -   5        $ 
BtnGuardar��5�      $��1��5�      $��80��5�      $��30��5�      $��
0,10,105,0q�5�      $��Right=�5�      $	��Top=�5�      $��Guardar��5�      #��[ 5�      5�      5�      