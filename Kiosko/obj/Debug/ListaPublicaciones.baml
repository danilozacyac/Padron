   M S B A M L   `   `   `  ����A  =Kiosko, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:Kiosko.ConverterKiosko.Converter  5      clr-namespace:KioskoKiosko  5      E AUIControls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;,clr-namespace:UIControls;assembly=UIControls
UIControls 5          Kiosko.ListaPublicaciones   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      4	Converterclr-namespace:Kiosko.Converter     5      -CustomColumnclr-namespace:Kiosko     5      C
UIControls,clr-namespace:UIControls;assembly=UIControls     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5	      k gTelerik.Windows.Controls.Input, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7m iTelerik.Windows.Controls.Docking, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7a ]Telerik.Windows.Data, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7e	 aTelerik.Windows.Controls, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7n
 jTelerik.Windows.Controls.GridView, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7p lTelerik.Windows.Controls.Navigation, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Itelerik1http://schemas.telerik.com/2008/xaml/presentation    	 
  5
      -   5        
�������  � 	Resources  5      %   Kiosko.Converter.MedioConverter   	medioConv'   !Kiosko.Converter.AutoresConverter  	autorConv)   #Kiosko.Converter.ImagePathConverter  	imageConv��%W   &	        &	      &	 &       5   
   6#     5   
   6%     5   
   6'   5      .��5      / 	 )Telerik.Windows.Controls.RadBusyIndicator  -   5          Name$ BusyIndicator��5         IsIndeterminate . .��5   
   �  � RowDefinitions 5      �� 5      $
��Auto �6    �� 5      $��50 �6    �� 5      $��* �6    .+�5        UIControls.SearchTextBox  -   5        $��1��5      $	��350��5       $��	10,15,0,0q�5!      $
��Left=�5"      $	��Top=�5#         	LabelText$A ;Buscar en síntesis, título, número de material o ISBN...��5$      5%      * 
 $Telerik.Windows.Controls.RadGridView  5'      -   5          Name$ GObras��5(      $��2��5)      $��Stretch=�5*      $��Stretch=�5+      ; 
 5Telerik.Windows.Controls.GridView.GridViewDataControl   AutoGenerateColumns .     EnableRowVirtualization .    
IsReadOnly . 	   RowIndicatorVisibility$	 	Collapsed=�50      
   SelectionMode$
 Single=�52         ShowGroupPanel .     ValidatesOnDataErrors$
 None=�54      *  $Telerik.Windows.Controls.DataControl   ItemsSource 5/      ��    Columns 55      	   Kiosko.MyColumnNumber	  56      -
 
 'Telerik.Windows.Controls.GridViewColumn 
  Width? 
 9Telerik.Windows.Controls.GridView.GridViewLengthConverter$ 30 57        ColumnaNumerica 
  	CellStyle# [ 58       
  Header$ #��59      2 
 ,Telerik.Windows.Controls.GridViewImageColumn  5:      $	 100 5;      6 
 0Telerik.Windows.Controls.GridViewBoundColumnBase   Header$ Portada��5=       
  IsFilterable .     DataMemberBinding 5<      �� *	ImagePath+ �� 	Converter# [  5?      �� 5@       �� 
TargetType4 
 .Telerik.Windows.Controls.GridView.GridViewCell"  6$   .�5A   "   ��  �� Property � �� �� Value$	 Top=�6M   5B      5D      1 
 +Telerik.Windows.Controls.GridViewDataColumn  5E      $ * 5F      $ Título��5G       .   5I      �� 5J      "  6$   .�5K   "   ��  � ��$	 Top=�6M   5L       
  CellTemplate 5N      �� 5O      .R�5P   "   �� $	��Top=�5Q   %   $��12(�5R   %    �� TextAlignment$ Justify=�5T   %   $
��Wrap=�5U   %   ��5S   %   �� *Titulo+5U   %   5V      5Y        5Z      $
 1.5* 5[      $ Sintesis��5\       .   5^      �� 5_      "  6$   .�5`   "   ��  � ��$	 Top=�6M   5a       5c      �� 5d      .R�5e   "   �� $	��150��5f   %   $��12(�5g   %   $ Justify=�5i   %   $
��Wrap=�5j   %   ��5h   %   �� *
Sintesis+5j   %   5k      5m        5n      $	 100 5o      $
 Tipo��5q       5p      �� *TipoPublicacionStr+ 5r      �� 5s      "  6$   .�5t   "   ��  � ��$	 Top=�6M   5u      5w        5x      $	 100 5y      $ Núm. Material��5{       .   5z      �� *NumMaterial+ 5}      �� 5~      "  6$   .�5   "   ��  � ��$	 Top=�6M   5�      5�        5�      $	 100 5�      $ Año Publicación��5�       .  5�      �� *AnioPublicacion+ 5�      �� 5�      "  6$   .�5�   "   ��  � ��$	 Top=�6M   5�      5�        5�      $ 90 5�      $ Precio��5�       
  ShowDistinctFilters .   5�      �� *Precio+ 5�      �� 5�      "  6$   .�5�   "   ��  � ��$	 Top=�6M   5�      5�        5�      $	 150 5�      $
 ISBN��5�       .   5�      �� *Isbn+ 5�      �� 5�      "  6$   .�5�   "   ��  � ��$	 Top=�6M   5�      5�        5�      $ 90 5�      $ Págs.��5�       .  .   5�      �� *	Paginas+ 5�      �� 5�      "  6$   .�5�   "   ��  � ��$	 Top=�6M   5�      5�        5�      $	 .5* 5�      $ Autores��5�       .  5�      �� 5�      .R�5�   "   �� $	��Top=�5�   %   $��12(�5�   %   $ Justify=�5�   %   $
��Wrap=�5�   %   ��5�   %   �� *	Autores+ �� Mode$ TwoWay=�# [ 5�   %   5�      5�         RowStyle 5�      �� 5�      3 
 -Telerik.Windows.Controls.GridView.GridViewRow"  6   .�5�      ��  � �� 67   �� *MedioPublicacion+# [  �� 5�       � ��$	 250��66   5�      5�      5�      5�      5�      