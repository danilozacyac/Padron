   M S B A M L   `   `   `  ����E  AUIControls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;,clr-namespace:UIControls;assembly=UIControls
UIControls  5   	   D @PadronApi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullL4clr-namespace:PadronApi.Converter;assembly=PadronApiPadronApi.Converter 5   	   
 Padron*   $Padron.Plantillas.IncluyeFuncionario   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   C
UIControls,clr-namespace:UIControls;assembly=UIControls     5   	   J	converter4clr-namespace:PadronApi.Converter;assembly=PadronApi     5   	   m iTelerik.Windows.Controls.Docking, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7e aTelerik.Windows.Controls, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7p	 lTelerik.Windows.Controls.RibbonView, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7n
 jTelerik.Windows.Controls.GridView, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7a ]Telerik.Windows.Data, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k gTelerik.Windows.Controls.Input, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7p lTelerik.Windows.Controls.Navigation, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Ktelerik1http://schemas.telerik.com/2008/xaml/presentation   	 
    5   	   -   5        $��Incluir Funcionario��5   	   $	��600��5   	   $	��600��5	   	   )  #Telerik.Windows.Controls.WindowBase    
ResizeMode$  NoResize=�5   	   (  "Telerik.Windows.Controls.RadWindow   WindowStartupLocation$ CenterScreen=�5   	    � 	Resources 5      )  #PadronApi.Converter.TituloConverter   
tituloConv��%   &	          5   
   6$   5      .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $	��150 �6   �� 5      $��45 �6   .+�5   
      UIControls.SearchTextBox  -   5        $	��382��5   #   $��	75,15,0,0q�5   #   $
��Left=�5   #   $	��Top=�5   #      	LabelText$&  Buscar por nombre y apellidos...��5   #   5   #   �� 5    
   $��	10,10,0,5q�6   $
��Left=�5!      $	��Top=�5"      $��Buscar:��5#      $��14(�5$      $��Black"�5%      * 
 $Telerik.Windows.Controls.RadGridView  5'   
   -   5          Name$ 
GTitulares��5'      $��1��5(      $��Stretch=�5)      $��Stretch=�5*      ; 
 5Telerik.Windows.Controls.GridView.GridViewDataControl   AutoGenerateColumns .     EnableRowVirtualization . $��14(�5-         IsFilteringAllowed .  	   
IsReadOnly	 . 
   ShowGroupPanel
 .  *  $Telerik.Windows.Controls.DataControl   ItemsSource 50      ��    Columns 53      1 
 +Telerik.Windows.Controls.GridViewDataColumn  55      -	 
 'Telerik.Windows.Controls.GridViewColumn 	  Width?
 
 9Telerik.Windows.Controls.GridView.GridViewLengthConverter$ 80
 6-   6 
 0Telerik.Windows.Controls.GridViewBoundColumnBase   Header$ Título��58   -      DataMemberBinding 56   -   �� *
IdTitulo+ �� 	Converter# [  58   -     59      $	 .7*
 6-   $ Nombre��5;   -    5:   -   �� *Nombre+5;   -     5<      $	 .8*
 6-   $ 	Apellidos��5>   -    5=   -   �� *	Apellidos+5>   -   5@      �� 5A   
   $��2��6   $��15q�5B      $��Stretch=�5C      $��Stretch=�5D      $&�� Ejemplares por tipo de propiedad��5E      .��5F      � .+�5G      �� $��	16,21,0,0q�6   $
��Left=�5H      $	��Top=�5I      $��
Particular��5J      /  )Telerik.Windows.Controls.RadNumericUpDown  5K      -   5          Name$ NudParticular��5K   +   $��71��5L   +   $��	16,52,0,0q�5M   +   $
��Left=�5N   +   $	��Top=�5O   +      	IsInteger . +  %Telerik.Windows.Controls.RadRangeBase   Minimum$ 0Q�5Q   +      Value$ 1X�5R   +   �� 5S      $��
108,21,0,0q�6   $
��Left=�5T      $	��Top=�5U      $��Personal��5V        5W      -   5        $ NudPersonal��5W   +   $��71��5X   +   $��
108,52,0,0q�5Y   +   $
��Left=�5Z   +   $	��Top=�5[   +    . $ 0Q�5]   +   $ 0X�5^   +   �� 5_      $��
202,21,0,0q�6   $
��Left=�5`      $	��Top=�5a      $��Oficina��5b        5c      -   5        $ 
NudOficina��5c   +   $��71��5d   +   $��
202,52,0,0q�5e   +   $
��Left=�5f   +   $	��Top=�5g   +    . $ 0Q�5i   +   $ 0X�5j   +   �� 5k      $��
293,21,0,0q�6   $
��Left=�5l      $	��Top=�5m      $��
Biblioteca��5n        5o      -   5        $ NudBiblioteca��5o   +   $��71��5p   +   $��
293,52,0,0q�5q   +   $
��Left=�5r   +   $	��Top=�5s   +    . $ 0Q�5u   +   $ 0X�5v   +   �� 5w      $��
380,21,0,0q�6   $
��Left=�5x      $	��Top=�5y      $��	Resguardo��5z        5{      -   5        $ NudResguardo��5{   +   $��71��5|   +   $��
380,52,0,0q�5}   +   $
��Left=�5~   +   $	��Top=�5   +    . $ 0Q�5�   +   $ 0X�5�   +   �� 5�      $��
472,21,0,0q�6   $
��Left=�5�      $	��Top=�5�      $��Autor��5�        5�      -	   5        $ NudAutor��5�   +   $��71��5�   +   $��
472,52,0,0q�5�   +   $
��Left=�5�   +   $	��Top=�5�   +    . $ 0Q�5�   +   $ 0X�5�   +   5�      5�      �� 5�   
   -
   5         ��Name$ BtnSalir��5�      $��3��5�      $��80��5�      $��
499,11,0,0q�5�      $
��Left=�5�      $	��Top=�5�      $��Cancelar��5�        
BlueButton#��[ 5�      �� 5�   
   -   5        $ 
BtnGuardar��5�      $��3��5�      $��80��5�      $��
414,11,0,0q�5�      $
��Left=�5�      $	��Top=�5�      $��Incluir��5�      #��[ 5�      5�      5�      