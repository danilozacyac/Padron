   M S B A M L   `   `   `  ����A  =Kiosko, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:Kiosko.ConverterKiosko.Converter  5      clr-namespace:KioskoKiosko  5      E AUIControls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;,clr-namespace:UIControls;assembly=UIControls
UIControls 5      #    Kiosko.Autores.ListadoAutores   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      4	Converterclr-namespace:Kiosko.Converter     5      -CustomColumnclr-namespace:Kiosko     5      C
UIControls,clr-namespace:UIControls;assembly=UIControls     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      k gTelerik.Windows.Controls.Input, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7m iTelerik.Windows.Controls.Docking, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7a ]Telerik.Windows.Data, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7e	 aTelerik.Windows.Controls, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7n
 jTelerik.Windows.Controls.GridView, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7p lTelerik.Windows.Controls.Navigation, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Itelerik1http://schemas.telerik.com/2008/xaml/presentation    	 
  5	      -   5        
�������  � 	Resources  5      )   #Kiosko.Converter.TipoAutorConverter   tipoConv)   #Kiosko.Converter.ImagePathConverter  	imageConv��%:   &	        &	        5   
   6'     5   
   6'   5      .��5      / 	 )Telerik.Windows.Controls.RadBusyIndicator  -   5          Name$ BusyIndicator��5         IsIndeterminate . .��5   
   �  � RowDefinitions 5      �� 5      $
��Auto �6    �� 5      $��* �6     � ColumnDefinitions 5      �� 5      $	��.3* �6#   �� 5      $��* �6#   .+�5        UIControls.SearchTextBox  -   5        $��23��5   '   $��
75,15,10,0q�5    '   $��Stretch=�5!   '   $	��Top=�5"   '      	LabelText$% Buscar por nombre o apellido...��5#   '    �� TextWrapping$
 Wrap=�5%   '   �� 5'      -   5         ��Name$ 
LblTotales��5'      $��	0,10,20,0q�5(      $��Right=�5)      $	��Top=�5*      $�� ��5+      $��14(�5,      $
��Bold"�5-      �� 5/      $��	10,10,0,5q�6   $
��Left=�50      $	��Top=�51      $��Buscar:��52      $��14(�53      $��Black"�54      ,  &Telerik.Windows.Controls.RadTabControl  55      -   5          Name$ 
TabAutores��55   $   $��1��56   $   $��0,0,0,0q�57   $   $��Stretch=�58   $   $��Stretch=�59   $   .@�5:      )  #Telerik.Windows.Controls.RadTabItem  -   5        	  Name$	 RtabPersonas��5:   %   $
��Auto��5;   %   $��30��5<   %   $��Autores��5=   %   .��5>      � 
������ .+�5?      ) 	 #Telerik.Windows.Controls.RadListBox  -   5        
  Name$
 RlstAutores��5?   -   $��2��5@   -   $��10,0,0,0q�5A   -   $��Stretch=�5B   -   $��Stretch=�5C   -    �� DisplayMemberPath$ NombreCompleto��5D   -   ��5E   -   �� 5F   -   5G      5H        5I      -   5        $	 RtabOrganismos��5I   %   $��Instituciones��6=   .��5J      � .+�5K        -	   5        $
 RlstInstituciones��5K   -   $��2��5L   -   $��10,0,0,0q�5M   -   $��Stretch=�5N   -   $��Stretch=�5O   -   $ NombreCompleto��5P   -   ��5Q   -   �� 5R   -   5S      5T      5U      * 
 $Telerik.Windows.Controls.RadGridView  5W      -
   5          Name$ GObras��5W   "   $��1��5X   "   $��1��5Y   "   $��Stretch=�5Z   "   $��Stretch=�5[   "   ;	 
 5Telerik.Windows.Controls.GridView.GridViewDataControl 	  AutoGenerateColumns .   	  EnableRowVirtualization .  	  
IsReadOnly .  	  RowIndicatorVisibility$ 	Collapsed=�5`   "    	  ShowGroupPanel .   	  ValidatesOnDataErrors$
 None=�5b   "   *
  $Telerik.Windows.Controls.DataControl 
  ItemsSource 5_   "   ��  	  Columns 5c         Kiosko.MyColumnNumber  5d      - 
 'Telerik.Windows.Controls.GridViewColumn   Width? 
 9Telerik.Windows.Controls.GridView.GridViewLengthConverter$ 30 62     ColumnaNumerica   	CellStyle# [ 5e   2      Header$ #��5f   2   2 
 ,Telerik.Windows.Controls.GridViewImageColumn  5g      $	 100 62   6 
 0Telerik.Windows.Controls.GridViewBoundColumnBase   Header$ Portada��5i   2      IsFilterable .     DataMemberBinding 5h   2   �� *	ImagePath+ �� 	Converter# [  5k      �� 5l       �� 
TargetType4 
 .Telerik.Windows.Controls.GridView.GridViewCell"  6$   .�5m   "   ��  �� Property � �� �� Value$	 Top=�6M   5n      5p      1 
 +Telerik.Windows.Controls.GridViewDataColumn  5q      $ * 61   $ Título��5r   1    .     CellTemplate 5t      �� 5u      .R�5v   "   �� $	��Top=�6,   $��12(�5w   ,     �� TextAlignment$  Justify=�5y   ,   $
��Wrap=�5z   ,   ��5x   ,   �� *Titulo+5z   ,   5{      5}        5~      $
 1.5* 61   $ Sintesis��5   1    .   5�      �� 5�      .R�5�   "   �� $	��150��6,   $��12(�5�   ,   $  Justify=�5�   ,   $
��Wrap=�5�   ,   ��5�   ,   �� *
Sintesis+5�   ,   5�      5�        5�      $	 100 61   $ Núm. Material��5�   1    .   5�   1   �� *NumMaterial+5�   1     5�      $	 100 61   $ Año Publicación��5�   1    .  5�   1   �� *AnioPublicacion+5�   1     5�      $ 90 61   $ Precio��5�   1   !   ShowDistinctFilters! .   5�   1   �� *Precio+5�   1     5�      $	 150 61   $
 ISBN��5�   1    .   5�   1   �� *Isbn+5�   1     5�      $ 90 61   $ Págs.��5�   1    . ! .   5�   1   �� *	Paginas+5�   1   " 	  RowStyle" 5�      �� 5�      3 
 -Telerik.Windows.Controls.GridView.GridViewRow"  6   .�5�      ��  � �� 67   �� *
IdIdioma+# [  5�      5�      5�      5�      5�      