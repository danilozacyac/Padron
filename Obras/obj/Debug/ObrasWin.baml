   M S B A M L   `   `   `  ����O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089M4clr-namespace:System.Globalization;assembly=mscorlibSystem.Globalization  5      	 Obras   Obras.ObrasWin   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Gglobal4clr-namespace:System.Globalization;assembly=mscorlib     5      a ]Telerik.Windows.Data, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k gTelerik.Windows.Controls.Input, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7e aTelerik.Windows.Controls, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7n	 jTelerik.Windows.Controls.GridView, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7p
 lTelerik.Windows.Controls.Navigation, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Gtelerik1http://schemas.telerik.com/2008/xaml/presentation    	 
 5      -   5        $	��661��5      $	��415��5      $��Obras��5	      ) 
 #Telerik.Windows.Controls.WindowBase    
ResizeMode$  NoResize=�5      ( 
 "Telerik.Windows.Controls.RadWindow   WindowStartupLocation$ CenterScreen=�5      .��5      �  � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $��35 �6   .+�5   
   � -   5         �Name$ PanelCentral��5      .+�5      �� $��	24,13,0,0q�5      $
��Left=�5      $	��Top=�5      $��Título:��5      �� 5      -   5         ��Name$ 	TxtTitulo��5      $��96��5      $��
25,44,25,0q�5      $��Stretch=�5      $	��Top=�5       �� TextWrapping$
 Wrap=�5"      ��5!      �� *Titulo+5"      �� 5#      $��
25,153,0,0q�5$      $
��Left=�5%      $	��Top=�5&      $��Núm. de material:��5'      �� 5(      -   5        $ TxtNumMaterial��5)      $	��140��5*      $��23��5+      $��
24,183,0,0q�5,      $
��Left=�5-      $	��Top=�5.      $
 Wrap=�53      ��52      �� *NumMaterial+ �� Mode$ TwoWay=�53      �� 54      $��
25,272,0,0q�55      $
��Left=�56      $	��Top=�57      $��Presentación:��58      �� 59      -   5         ��Name$ CbxPresentacion��5:      $	��176��5;      $��
24,298,0,0q�5<      $
��Left=�5=      $	��Top=�5>       �� DisplayMemberPath$ Descripcion��5?      	 �� SelectedValuePath$	 
IdElemento��5A      ��5@      �� 5B      �� 5C      $��192,153,0,0q�5D      $
��Left=�5E      $	��Top=�5F      $��Núm. de volúmenes:��5G      �� 5I      -   5        $ TxtIsbn��5J      $	��140��5K      $��23��5L      $��347,183,0,0q�5M      $
��Left=�5N      $	��Top=�5O      $
 Wrap=�5T      ��5S      �� *Isbn+$ TwoWay=�5T      �� 5U      $��347,153,0,0q�5V      $
��Left=�5W      $	��Top=�5X      $��ISBN:��5Y      �� 5Z      $��273,272,0,0q�5[      $
��Left=�5\      $	��Top=�5]      $��Tipo de obra:��5^      �� 5_      -   5        $ CbxTipoObra��5`      $	��176��5a      $��273,298,0,0q�5b      $
��Left=�5c      $	��Top=�5d      $ Descripcion��5e      $	 
IdElemento��5g      ��5f      �� 5g      �� 5i      $��505,153,0,0q�5j      $
��Left=�5k      $	��Top=�5l      $��Año publicación:��5m      �� 5o      $��29��5p      $��20��5q      $��390,159,0,0q�5r      $
��Left=�5s      $	��Top=�5t      $��Resources/question_64.png��5u      
 � ToolTip$c
 ]El formato de ISBN para 10 digitos es XXX-XXX-XXX-XXX-X para 13 dígitos es 978-XXX-XXX-XXX-X��5v      �� 5w      $��
25,214,0,0q�5x      $
��Left=�5y      $	��Top=�5z      $��Tiraje:��5{      /  )Telerik.Windows.Controls.RadNumericUpDown  5}      -   5          Name$ NudYear��5~      $	��125��5      $��505,183,0,0q�5�      $
��Left=�5�      $	��Top=�5�         	IsInteger .    ValueFormat$ Numeric=�5�      +  %Telerik.Windows.Controls.RadRangeBase   Value 5�      �� *AnioPublicacion+$ TwoWay=�   NumberFormatInfo 5�      +   %System.Globalization.NumberFormatInfo  5�         NumberDecimalDigits$ 0��6.      NumberGroupSeparator$  ��6F   5�        5�      -	   5        $ 	NudTiraje��5�      $	��125��5�      $��
25,244,0,0q�5�      $
��Left=�5�      $	��Top=�5�       .  5�      �� *Tiraje+$ TwoWay=�  5�      $	��125��5�      $��192,183,0,0q�5�      $
��Left=�5�      $	��Top=�5�       .    Maximum$ 15Q�5�         Minimum$ 1Q�5�       5�      �� *	NumLibros+$ TwoWay=��� 5�      $��192,214,0,0q�5�      $
��Left=�5�      $	��Top=�5�      $��Fecha de recepción:��5�      ,  &Telerik.Windows.Controls.RadDatePicker  5�      -
   5          Name$ DtpRecepcion��5�      $	��125��5�      $��192,244,0,0q�5�      $
��Left=�5�      $	��Top=�5�      0  *Telerik.Windows.Controls.RadDateTimePicker   SelectedValue 5�      �� *FechaRecibe+�� 5�      $��347,214,0,0q�5�      $
��Left=�5�      $	��Top=�5�      $��Fecha de distribución:��5�        5�      -   5        $ DtpDistribucion��5�      $	��140��5�      $��347,244,0,0q�5�      $
��Left=�5�      $	��Top=�5�       5�      �� *FechaDistribuye+5�      �� 5�   
   -   5         ��Name$ BtnSalir��5�      $��1��5�      $��80��5�      $��0,7,15,0q�5�      $��Right=�5�      $	��Top=�5�      $��Cancelar��5�       �� IsCancel .    
BlueButton#��[  5�      �� 5�   
   -   5        $ 
BtnGuardar��5�      $��1��5�      $��80��5�      $��	0,7,100,0q�5�      $��Right=�5�      $	��Top=�5�      $��Guardar��5�       �� 	IsDefault . #��[  5�      5�      5�      