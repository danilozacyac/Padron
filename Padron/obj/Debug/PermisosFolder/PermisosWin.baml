   M S B A M L   `   `   `  ����
  Padron'    !Padron.PermisosFolder.PermisosWin   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      m iTelerik.Windows.Controls.Docking, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7e aTelerik.Windows.Controls, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7p lTelerik.Windows.Controls.RibbonView, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7n jTelerik.Windows.Controls.GridView, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7a	 ]Telerik.Windows.Data, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k
 gTelerik.Windows.Controls.Input, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7p lTelerik.Windows.Controls.Navigation, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Ktelerik1http://schemas.telerik.com/2008/xaml/presentation     	 
  5      -   5        $	��500��5      $	��600��5      $��Asignar Permisos��5      (  "Telerik.Windows.Controls.RadWindow    WindowStartupLocation$  CenterScreen=�5
      .��5      �  � 	Resources 5   
      CheckBoxItemTemplate �� ItemsSource �� Orientation d� 	IsChecked �� Mode  TreeViewItemStyle �� 
TargetType.  (Telerik.Windows.Controls.RadTreeViewItem �� Property �� Value��%`  &	        &	 �     �� 5       6D   �� *SeccionesHijo+.R�5      �� $ 
Horizontal=�6   .+�5      �� $��Center=�5      |�.   5      �� *
IsSelected+�� 5      $	��2,0q�6'   ��64   �� *	Seccion+$ OneTime=�5      5      �� 5      "  6.   .�5      ��  �@ 
IsExpanded . 6/   5      5      	 � RowDefinitions	 5   
   �� 5      $��* �6   �� 5       $��50 �6   
 � ColumnDefinitions
 5#   
   �� 5$      $	��.6* �6   �� 5%      $��* �6   .+�5(   
   )  #Telerik.Windows.Controls.RadListBox  -   5          Name$ RLstUsuarios��5)      $��
10,50,10,0q�5*      $��Stretch=�5+      $��Stretch=�5,       �� DisplayMemberPath$ Seccion��5-      >  8Telerik.Windows.Controls.Primitives.ItemsControlSelector   SelectedValuePath$ 	IdSeccion��5/      ��5.      �� 50      *  $Telerik.Windows.Controls.RadTreeView  53   
   -   5          Name$ treePermisos��54      $��1��55      $��
10,50,10,0q�56      $��5q�57      #��[ 58      #��[  59      ��5:      �� �� 5;   
   -   5         ��Name$ 
BtnGuardar��5<      $��1��5=      $��1��5>      $��87��5?      $��	0,15,20,0q�5@      $��Right=�5A      $	��Top=�5B      $��Guardar��5D       �� 	IsDefault .   
BlueButton#��[ 5F      �� 5G   
   -   5        $ BtnCancelar��5H      $��1��5I      $��1��5J      $��82��5K      $��	94,15,0,0q�5L      $
��Left=�5M      $	��Top=�5N      $��Cancelar��5P       �� IsCancel . #��[ 5R      5U      5V      