   M S B A M L   `   `   `  ����E  AUIControls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;,clr-namespace:UIControls;assembly=UIControls
UIControls  5       Funcionarios'   !Funcionarios.SeleccionaOrgAdscrip   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      C
UIControls,clr-namespace:UIControls;assembly=UIControls     5      k gTelerik.Windows.Controls.Input, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7a ]Telerik.Windows.Data, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7e aTelerik.Windows.Controls, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7n	 jTelerik.Windows.Controls.GridView, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7p
 lTelerik.Windows.Controls.Navigation, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Gtelerik1http://schemas.telerik.com/2008/xaml/presentation    	 
 5      -   5        $	��700��5      $	��725��5      $*��$Selecciona organismo de adscripción��5	      ( 
 "Telerik.Windows.Controls.RadWindow    WindowStartupLocation$  CenterScreen=�5      .��5      �  � RowDefinitions 5   
   �� 5      $��35 �6   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $��50 �6   .+�5   
      UIControls.SearchTextBox  -   5          Name$ 	SearchBox��5      $��0��5      $	��250��5      $��25��5      $��25,5,0,0q�5      $
��Left=�5      $	��Top=�5         	LabelText$ Buscar organismo...��5       �� TextWrapping$
 Wrap=�5      � 5   
   -   5         �Name$ PanelOrganismo��5       $��1��5!      $	��290��5"      .+�5#      * 	 $Telerik.Windows.Controls.RadGridView  -   5          Name$ GOrganismos��5$      $��1��5%      $��	25,0,25,0q�5&      $��Stretch=�5'      $��Stretch=�5(      ; 	 5Telerik.Windows.Controls.GridView.GridViewDataControl   AutoGenerateColumns .     EnableRowVirtualization . 	   
IsReadOnly	 . 
   ShowGroupPanel
 .  *  $Telerik.Windows.Controls.DataControl   ItemsSource 5,      ��    Columns 5/      1 	 +Telerik.Windows.Controls.GridViewDataColumn  51      - 	 'Telerik.Windows.Controls.GridViewColumn   Width? 	 9Telerik.Windows.Controls.GridView.GridViewLengthConverter$ * 52      6	 	 0Telerik.Windows.Controls.GridViewBoundColumnBase 	  Header$ 	Organismo��54       	  DataMemberBinding 53      �� *OrganismoDesc+54      56      58      � 5:   
   $��2��6    � ColumnDefinitions 5;      �� 5<      $
��Auto �6#   �� 5=      $��* �6#   .+�5B      �� -   5         ��Name$ LstTipoObra��5C      $��15,55,15,10q�5D      $��Stretch=�5E      $��Stretch=�5F      $y�	Collapsed=�5H      ��5G      �� ��5I      �� 5J      .R�5K      �� ��6#   �� *Descripcion+ d� 	IsChecked 6C   �� *	IsChecked+ �� Mode$ TwoWay=�5L      5N      �� 5O      $��	15,30,0,0q�5P      $
��Left=�5Q      $	��Top=�5R      $��Obras que recibe:��5S      $
��Bold"�5T      $y�	Collapsed=�5U      �� 5Y      $��1��5Z      $��	15,30,0,0q�5[      $
��Left=�5\      $	��Top=�5]      $"��Tiraje en el que se incluye:��5^      $
��Bold"�5_        5a      -   5        $ GTirajes��5b      $��1��5c      $��15,55,15,10q�5d      $��Stretch=�5e      $��Stretch=�5f       .     IsFilteringAllowed .  
 .   5i      ��  5k      5
 	 /Telerik.Windows.Controls.GridViewCheckBoxColumn
  5l      $ 60 5m       
  AutoSelectOnEdit .    EditTriggers$ 	CellClick=�5p      $ Recibe��5q         IsFilterable .   5o      �� *	IsChecked+5r        5t      $ 60 5u      $ Acuerdo��5w       .     
IsReadOnly .  5v      �� *AcDescripcion+5y        5z      $ 
Particular��6Z    61   �� *
Particular+   CellEditTemplate 5{      �� 5|      .R�5}   "   /  )Telerik.Windows.Controls.RadNumericUpDown     	IsInteger . +  %Telerik.Windows.Controls.RadRangeBase   Maximum$
 1000Q�5   %      Minimum$ 0Q�5�   %      UpdateValueEvent$ PropertyChanged=�5�   %      Value 5�   %   �� *
Particular+$ TwoWay=� �� UpdateSourceTrigger$ Explicit=�5�      5�        5�      $ Integrantes��6X    61   �� *
Personal+ 5�      �� 5�      .R�5�   "      . $
 1000Q�5�   %   $ 0Q�5�   %   $ PropertyChanged=�5�   %    5�   %   �� *
Personal+$ TwoWay=�$ Explicit=�5�      5�        5�      $ Oficina��6W    61   �� *	Oficina+ 5�      �� 5�      .R�5�   "      . $
 1000Q�5�   %   $ 0Q�5�   %   $ PropertyChanged=�5�   %    5�   %   �� *	Oficina+$ TwoWay=�$ Explicit=�5�      5�        5�      $ 
Biblioteca��6Z    61   �� *
Biblioteca+ 5�      �� 5�      .R�5�   "      . $
 1000Q�5�   %   $ 0Q�5�   %   $ PropertyChanged=�5�   %    5�   %   �� *
Biblioteca+$ TwoWay=�$ Explicit=�5�      5�        5�      $ 	Resguardo��6Y    61   �� *	Resguardo+ 5�      �� 5�      .R�5�   "      . $
 1000Q�5�   %   $ 0Q�5�   %   $ PropertyChanged=�5�   %    5�   %   �� *	Resguardo+$ TwoWay=�$ Explicit=�5�      5�      5�      *  $Telerik.Windows.Controls.RadComboBox  5�      -   5           Name$  CbxFunciones��5�      $��1��5�      $	��250��5�      $��88,8,0,0q�5�      $
��Left=�5�      $	��Top=�5�      ! �� DisplayMemberPath$! Descripcion��5�      "   
IsEditable" . #   IsFilteringEnabled# . $ �� SelectedValuePath$$ 
IdElemento��5�      %   TextSearchMode$% Contains=�5�      ��5�      �� 5�      �� 5�      $��1��5�      $��15,4,0,0q�5�      $
��Left=�5�      $	��Top=�5�      $��	Función:��5�      5�      �� 5�   
   -   5        & ��Name$& BtnCancelar��5�      $��3��5�      $��80��5�      $��	0,18,10,0q�5�      $��Right=�5�      $	��Top=�5�      $��Cancelar��5�      ' �� IsCancel' .    
BlueButton#��[  5�      �� 5�   
   -	   5        $& 
BtnGuardar��5�      $��3��5�      $��80��5�      $��	0,18,95,0q�5�      $��Right=�5�      $	��Top=�5�      $��Guardar��5�      #��[  5�      5�      5�      