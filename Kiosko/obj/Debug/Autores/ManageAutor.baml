   M S B A M L   `   `   `  ����D  @PadronApi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullL4clr-namespace:PadronApi.Converter;assembly=PadronApiPadronApi.Converter  5      
 Kiosko    Kiosko.Autores.ManageAutor   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      H DMahApps.Metro, Version=1.2.3.0, Culture=neutral, PublicKeyToken=null9Custom,http://metro.mahapps.com/winfx/xaml/controls  5      D	converter4clr-namespace:PadronApi.Converter;assembly=PadronApi  5      k gTelerik.Windows.Controls.Input, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7m iTelerik.Windows.Controls.Docking, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7a	 ]Telerik.Windows.Data, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7e
 aTelerik.Windows.Controls, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7n jTelerik.Windows.Controls.GridView, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7p lTelerik.Windows.Controls.Navigation, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Itelerik1http://schemas.telerik.com/2008/xaml/presentation   	 
   5      -   5        $	��630��5      $	��580��5      $��Autores��5	      )  #Telerik.Windows.Controls.WindowBase    
ResizeMode$  NoResize=�5      (  "Telerik.Windows.Controls.RadWindow   WindowStartupLocation$ CenterScreen=�5       � 	Resources 5      *   $PadronApi.Converter.TextValConverter   textConv��%   &	          5   
   6%   5      .��5      �  � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $��50 �6   .+�5   
   � -   5         �Name$ PanelPrincipal��5      .+�5      �� $	��270��6   $��
15,31,15,0q�5      $��Stretch=�5      $	��Top=�5      $��Datos Personales��5      .��5      � .+�5      �� $��
16,107,0,0q�6   $
��Left=�5      $	��Top=�5      $��
Apellidos:��5      �� 5       -   5         ��Name$ TxtApellidos��5       $	��437��5!      $��23��5"      $��103,107,-2,0q�5#      $
��Left=�5$      $	��Top=�5%       �� TextWrapping$
 Wrap=�5)      ��5'      �� *	Apellidos+ �� 	Converter# [  5)      �� 5*      $��
16,159,0,0q�6   $
��Left=�5+      $	��Top=�5,      $��
Nombre(s):��5-      �� 5.      -   5        $ 	TxtNombre��5.      $	��437��5/      $��23��50      $��103,159,-2,0q�51      $
��Left=�52      $	��Top=�53      $
 Wrap=�57      ��55      �� *Nombre+# [  57      *  $Telerik.Windows.Controls.RadComboBox  58      -   5          Name$ CbxGrado��58   *   $	��120��59   *   $��
103,68,0,0q�5:   *   $
��Left=�5;   *   $	��Top=�5<   *   	 �� DisplayMemberPath$	 
TituloDesc��5=   *   
 �� SelectedValuePath$
 IdTitulo��5?   *   ��5>   *   �� 5?   *   �� 5@      $��	35,64,0,0q�6   $
��Left=�5A      $	��Top=�5B      $��Título:��5C      �� 5D      $��
17,202,0,0q�6   $
��Left=�5E      $	��Top=�5F      $��Correo:��5G      �� 5H      -   5        $ 	TxtCorreo��5H      $	��235��5I      $��23��5J      $��103,202,0,0q�5K      $
��Left=�5L      $	��Top=�5M      $
 Wrap=�5P      ��5N      �� *Correo+ �� Mode$ TwoWay=�5P      )  #MahApps.Metro.Controls.ToggleSwitch  5Q      -   5          Name$ TbGenero��5Q   *   $��	35,10,0,0q�5R   *   $
��Left=�5S   *   $	��Top=�5T   *   $��Genero       ��5V   *   $��12(�5W   *      	IsChecked$
 TrueY�5X   *   5Y   *   �� 5Z      -   5         ��Name$ ImGenero��5Z      $��32��5[      $��35��5\      $��
181,10,0,0q�5]      $
��Left=�5^      $	��Top=�5_      $0��*/Kiosko;component/Resources/female_128.png��5`      5a      5b      �� 5c      $��
15,333,0,0q�6   $
��Left=�5d      $	��Top=�5e      $��Observaciones:��5f      �� 5g      -	   5        $ TxtObservaciones��5g      $��90��5h      $��15,363,15,0q�5i      $��Stretch=�5j      $	��Top=�5k       �� AcceptsReturn . $
 Wrap=�5n      ��5m      �� *Observaciones+5n      5o      �� 5p   
   -
   5         ��Name$ BtnSalir��5p      $��1��5q      $��80��5r      $��30��5s      $��	0,10,20,0q�5t      $��Right=�5u      $	��Top=�5v      $��Cancelar��5x        
BlueButton#��[ 5y      �� 5z   
   -   5        $ 
BtnGuardar��5z      $��1��5{      $��80��5|      $��30��5}      $��
0,10,105,0q�5~      $��Right=�5      $	��Top=�5�      $��Guardar��5�      #��[ 5�      5�      5�      