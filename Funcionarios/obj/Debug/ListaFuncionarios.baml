   M S B A M L   `   `   `  ����G  CFuncionarios, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:FuncionariosFuncionarios  5      E AUIControls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;,clr-namespace:UIControls;assembly=UIControls
UIControls 5      D @PadronApi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullL4clr-namespace:PadronApi.Converter;assembly=PadronApiPadronApi.Converter 5      G(clr-namespace:Funcionarios.FuncConverterFuncionarios.FuncConverter  5	      $    Funcionarios.ListaFuncionarios   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      3CustomColumnclr-namespace:Funcionarios     5      C
UIControls,clr-namespace:UIControls;assembly=UIControls     5      J	converter4clr-namespace:PadronApi.Converter;assembly=PadronApi     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      BfuncConverter(clr-namespace:Funcionarios.FuncConverter     5	      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5
      k gTelerik.Windows.Controls.Input, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7a ]Telerik.Windows.Data, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7e	 aTelerik.Windows.Controls, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7n
 jTelerik.Windows.Controls.GridView, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7p lTelerik.Windows.Controls.Navigation, Version=2014.2.729.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Gtelerik1http://schemas.telerik.com/2008/xaml/presentation   	 
  5      -   5        
�������  � 	Resources  5      )  #PadronApi.Converter.TituloConverter   
tituloConv-   'Funcionarios.FuncConverter.AdsConverter 
 totAds1   +Funcionarios.FuncConverter.AuthPicConverter  authConv��%W   &	        &	      &	 &       5   
   6$     5   
   6%     5   
   6)   5      .��5      / 	 )Telerik.Windows.Controls.RadBusyIndicator  -   5          Name$ BusyIndicator��5         IsIndeterminate . .��5   
   �  � RowDefinitions 5      �� 5      $
��Auto �6    �� 5      $��* �6    .+�5        UIControls.SearchTextBox  -   5        $	��382��5      $��23��5       $��	75,15,0,0q�5!      $
��Left=�5"      $	��Top=�5#         	LabelText$% Buscar por nombre o apellido...��5$       �� TextWrapping$
 Wrap=�5&      �� 5(      -   5         ��Name$ 
LblTotales��5)      $��	0,10,20,0q�5*      $��Right=�5+      $	��Top=�5,      $�� ��5-      $��14(�5.      $
��Bold"�5/      �� 51      $��	10,10,0,5q�52      $
��Left=�53      $	��Top=�54      $��Buscar:��55      $��14(�56      $��Black"�57      * 
 $Telerik.Windows.Controls.RadGridView  59      -   5          Name$ 
GTitulares��5:      $��1��5;      $��Stretch=�5<      $��Stretch=�5=      ; 
 5Telerik.Windows.Controls.GridView.GridViewDataControl   AutoGenerateColumns .  	   EnableRowVirtualization	 . $��16(�5A      
   
IsReadOnly
 .    RowIndicatorVisibility$ 	Collapsed=�5D         ShowGroupPanel .  *  $Telerik.Windows.Controls.DataControl   ItemsSource 5C      ��    Columns 5G      !	   Funcionarios.MyColumnNumber	  5H      -
 
 'Telerik.Windows.Controls.GridViewColumn 
  Width? 
 9Telerik.Windows.Controls.GridView.GridViewLengthConverter$ 30 5I        ColumnaNumerica 
  	CellStyle# [ 5J       
  Header$ #��5K      1 
 +Telerik.Windows.Controls.GridViewDataColumn  5L      $ 80 5M      6 
 0Telerik.Windows.Controls.GridViewBoundColumnBase   Header$ Título��5O       
  IsFilterable .    DataMemberBinding 5N      �� *
IdTitulo+ �� 	Converter# [  5P        5Q      $	 .7* 5R      $ Nombre��5T       .   5S      �� *Nombre+5U        5V      $	 .8* 5W      $ 	Apellidos��5Y       .   5X      �� *	Apellidos+5Z      2 
 ,Telerik.Windows.Controls.GridViewImageColumn  5[      $ 32 5\      $  ��5^       .   5]      �� *HaPublicado+# [  5`      �� 5a       �� 
TargetType4 
 .Telerik.Windows.Controls.GridView.GridViewCell"  6$   .�5b   "   ��  �� Property � �� �� Value$	 Top=�6M   5c      5e         RowStyle 5g      �� 5h      3 
 -Telerik.Windows.Controls.GridView.GridViewRow"  6   .�5i      ��  � �� 67   �� *TotalAdscripciones+# [ 5j      5l      5m      5o      5p      