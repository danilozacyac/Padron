   M S B A M L   `   `   `  ����
  Padron&     Padron.ManttoCatalogos.Funciones   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      m iTelerik.Windows.Controls.Docking, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7e aTelerik.Windows.Controls, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7p lTelerik.Windows.Controls.RibbonView, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7n jTelerik.Windows.Controls.GridView, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7a	 ]Telerik.Windows.Data, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k
 gTelerik.Windows.Controls.Input, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7p lTelerik.Windows.Controls.Navigation, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Ktelerik1http://schemas.telerik.com/2008/xaml/presentation     	 
  5      -   5        $	��500��5      $	��550��5      $��	Funciones��5      (  "Telerik.Windows.Controls.RadWindow    WindowStartupLocation$  CenterScreen=�5	      .��5
      �  � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $��35 �6   .+�5   
   �� -   5         ��Name$ BtnSubir��5      $��65��5      $��49��5      $��407,177,10,0q�5      $��Right=�5      $	��Top=�5      .��5      �� $(��"/Padron;component/Resources/up.png��6   5      �� 5   
   -   5        $ BtnBajar��5      $��65��5      $��49��5      $��407,231,10,0q�5      $��Right=�5      $	��Top=�5      .��5      �� $*��$/Padron;component/Resources/down.png��6   5       )  #Telerik.Windows.Controls.RadListBox  5!   
   -   5          Name$ LstFunciones��5!      $��15,15,90,20q�5"      $��Stretch=�5#      $��Stretch=�5$       �� DisplayMemberPath$ Descripcion��5%      >  8Telerik.Windows.Controls.Primitives.ItemsControlSelector   SelectedValuePath$ 
IdElemento��5'      ��5&      �� 5(      �� 5)   
   -   5        $ 
BtnGuardar��5)      $��1��5*      $��81��5+      $��	389,5,0,0q�5,      $
��Left=�5-      $	��Top=�5.      $��Guardar��50         
BlueButton#��[  51      �� 53   
   -   5        $ BtnModificar��53      $��1��54      $��90��55      $��15,5,0,0q�56      $
��Left=�57      $	��Top=�58      $��	Modificar��59      #��[  5:      65   5<      5=      