   M S B A M L   `   `   `  ����D  @PadronApi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullL4clr-namespace:PadronApi.Converter;assembly=PadronApiPadronApi.Converter  5       
Organismos#   Organismos.Ciudades.AddCiudad   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      J	converter4clr-namespace:PadronApi.Converter;assembly=PadronApi     5      a ]Telerik.Windows.Data, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k gTelerik.Windows.Controls.Input, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7e aTelerik.Windows.Controls, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7n	 jTelerik.Windows.Controls.GridView, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7p
 lTelerik.Windows.Controls.Navigation, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Gtelerik1http://schemas.telerik.com/2008/xaml/presentation    	 
 5      -   5        $	��315��5      $	��348��5      $��Agregar Ciudad��5      ) 
 #Telerik.Windows.Controls.WindowBase    
ResizeMode$  NoResize=�5      ( 
 "Telerik.Windows.Controls.RadWindow   WindowStartupLocation$ CenterScreen=�5       � 	Resources 5      *   $PadronApi.Converter.TextValConverter   textConv��%   &	          5   
   6%   5      .��5      � .+�5   
   �� $��	27,34,0,0q�6   $
��Left=�5      $	��Top=�5      $��País:��5      �� 5   
   -   5         ��Name$ CbxPais��5      $	��255��5      $��	27,65,0,0q�5      $
��Left=�5      $	��Top=�5       �� DisplayMemberPath$ PaisDesc��5      {�.   �� SelectedValuePath$ IdPais��5      ��5      �� 5      �� 5   
   $��
27,117,0,0q�6   $
��Left=�5       $	��Top=�5!      $��Estado:��5"      �� 5#   
   -   5        $ 	CbxEstado��5#      $	��255��5$      $��
27,148,0,0q�5%      $
��Left=�5&      $	��Top=�5'      $ 
EstadoDesc��5(      {�.  $ IdEstado��5+      ��5*      �� 5+      �� 5,   
   $��
27,196,0,0q�6   $
��Left=�5-      $	��Top=�5.      $��Ciudad��5/      �� 50   
   -   5         ��Name$ 	TxtCiudad��50      $	��255��51      $��23��52      $��
27,227,0,0q�53      $
��Left=�54      $	��Top=�55       �� TextWrapping$
 Wrap=�59      ��57      �� *
CiudadDesc+ �� 	Converter# [  59      �� 5:   
   -   5        	 ��Name$	 BtnSalir��5:      $��80��5;      $��202,277,0,0q�5<      $
��Left=�5=      $	��Top=�5>      $��Cancelar��5@      
 �� IsCancel
 .   
BlueButton#��[ 5B      �� 5C   
   -   5        $	 
BtnGuardar��5C      $��80��5D      $��117,277,0,0q�5E      $
��Left=�5F      $	��Top=�5G      $��Guardar��5I       �� 	IsDefault . #��[ 5K      5M      5N      