   M S B A M L   `   `   `  ����
  Kiosko    Kiosko.TextoColaboracion   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      k gTelerik.Windows.Controls.Input, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7m iTelerik.Windows.Controls.Docking, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7a ]Telerik.Windows.Data, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7e aTelerik.Windows.Controls, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7n	 jTelerik.Windows.Controls.GridView, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7p
 lTelerik.Windows.Controls.Navigation, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Itelerik1http://schemas.telerik.com/2008/xaml/presentation     	 
 5      -   5        $	��755��5      $	��472��5      $��Colaboraciones��5      ( 
 "Telerik.Windows.Controls.RadWindow    WindowStartupLocation$  CenterScreen=�5
      .��5      �  � RowDefinitions 5   
   �� 5      $��50 �6   �� 5      $��* �6   �� 5      $��50 �6   .+�5   
   �� $��	10,14,0,0q�5      $
��Left=�5      $	��Top=�5      $&�� Indica el texto de colaboración��5      $
��Bold"�5      * 	 $Telerik.Windows.Controls.RadGridView  5   
   -   5          Name$ 
GColaboran��5      $��1��5      $��10q�5      $��Stretch=�5      $��Stretch=�5      ; 	 5Telerik.Windows.Controls.GridView.GridViewDataControl   AutoGenerateColumns .     IsFilteringAllowed .     ShowGroupPanel .  *  $Telerik.Windows.Controls.DataControl   ItemsSource 5       ��    Columns 5#      1 	 +Telerik.Windows.Controls.GridViewDataColumn  5$      - 	 'Telerik.Windows.Controls.GridViewColumn   Width? 	 9Telerik.Windows.Controls.GridView.GridViewLengthConverter$	 150 5%      6 	 0Telerik.Windows.Controls.GridViewBoundColumnBase	   Header$	 Nombre Autor��5'      
   
IsReadOnly
 .    DataMemberBinding 5&      �� *NombreAutor+5(        5)      $	 100 5*      $	 Tipo colaboración��5,      
 .  5+      �� *	TipoAutor+5-        5.      $ * 5/      $	 Colaboración��51       50      �� *TextoColabora+51      53      �� 54   
   -   5         ��Name$ BtnSalir��55      $��2��56      $��75��57      $��	0,10,10,0q�58      $��Right=�59      $	��Top=�5:      $��Salir��5;         
BlueButton#��[  5<      5=      5>      